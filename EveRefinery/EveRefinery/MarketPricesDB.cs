using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using SQLiteDesign;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Globalization;
using SpecialFNs;

namespace EveRefinery
{
	class MarketPricesDB
	{
		protected SQLiteConnection	m_DbConnection = new SQLiteConnection();
		protected ItemPrices		m_DataSet = new ItemPrices();
		protected ItemsDB			m_ItemsDB;
		protected SQLiteDatabase	m_Database;
		protected Thread			m_UpdateThread;
		protected Boolean			m_EndUpdateThread;
		protected Queue<UInt32>		m_UpdateQueue;

		public MarketPricesDB(ItemsDB a_ItemsDB)
		{
			SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
			connectionString.DataSource		= "MarketPrices_v1.db3";
			m_DbConnection.ConnectionString = connectionString.ConnectionString;
			m_DbConnection.Open();

			m_ItemsDB	= a_ItemsDB;
			
			m_Database = new SQLiteDatabase(m_DbConnection, m_DataSet);
			m_Database.CreateTables();
		}

		public void LoadPrices(PriceSettings a_Settings, UInt32 a_PriceExpiryDays, UInt32 a_PriceHistoryDays, bool a_Silent)
		{
			StopUpdaterThread();
			
			m_ItemsDB.ResetItemPrices();
			
			ItemPrices.PricesDataTable pricesTable = new ItemPrices.PricesDataTable();

			string selectSQL = "Select * from " + pricesTable.TableName + " where " + 
				"(" + pricesTable.ProviderIDColumn.ColumnName	+ " = @ProviderID) AND " +
				"(" + pricesTable.RegionIDColumn.ColumnName		+ " = @RegionID) AND " +
				"(" + pricesTable.SolarIDColumn.ColumnName		+ " = @SolarID) AND " +
				"(" + pricesTable.StationIDColumn.ColumnName	+ " = @StationID) AND" +
				"(" + pricesTable.PriceTypeColumn.ColumnName	+ " = @PriceType)";

			SQLiteCommand sqlCommand = new SQLiteCommand(selectSQL, m_DbConnection);
			sqlCommand.Parameters.AddWithValue("@ProviderID",	(UInt32)a_Settings.Provider);
			sqlCommand.Parameters.AddWithValue("@RegionID",		a_Settings.RegionID);
			sqlCommand.Parameters.AddWithValue("@SolarID",		a_Settings.SolarID);
			sqlCommand.Parameters.AddWithValue("@StationID",	a_Settings.StationID);
			sqlCommand.Parameters.AddWithValue("@PriceType",	(UInt32)a_Settings.PriceType);

			SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlCommand);
			adapter.Fill(pricesTable);

			foreach (ItemPrices.PricesRow currRow in pricesTable.Rows)
			{
				DbRecordToItemRecord(currRow);
			}

			TestMarketPrices(a_Settings, a_PriceExpiryDays, a_PriceHistoryDays, a_Silent);
		}

		public void DropPrices(PriceSettings a_Settings)
		{
			StopUpdaterThread();

			ItemPrices.PricesDataTable pricesTable = new ItemPrices.PricesDataTable();

			string selectSQL = "Delete from " + pricesTable.TableName + " where " + 
				"(" + pricesTable.ProviderIDColumn.ColumnName	+ " = @ProviderID) AND " +
				"(" + pricesTable.RegionIDColumn.ColumnName		+ " = @RegionID) AND " +
				"(" + pricesTable.SolarIDColumn.ColumnName		+ " = @SolarID) AND " +
				"(" + pricesTable.StationIDColumn.ColumnName	+ " = @StationID) AND" +
				"(" + pricesTable.PriceTypeColumn.ColumnName	+ " = @PriceType)";

			SQLiteCommand sqlCommand = new SQLiteCommand(selectSQL, m_DbConnection);
			sqlCommand.Parameters.AddWithValue("@ProviderID",	(UInt32)a_Settings.Provider);
			sqlCommand.Parameters.AddWithValue("@RegionID",		a_Settings.RegionID);
			sqlCommand.Parameters.AddWithValue("@SolarID",		a_Settings.SolarID);
			sqlCommand.Parameters.AddWithValue("@StationID",	a_Settings.StationID);
			sqlCommand.Parameters.AddWithValue("@PriceType",	(UInt32)a_Settings.PriceType);

			sqlCommand.ExecuteNonQuery();
		}
		
		protected void DbRecordToItemRecord(ItemPrices.PricesRow a_DbRecord)
		{
			ItemRecord currItem = m_ItemsDB.GetItemByTypeID(a_DbRecord.TypeID);
			if (currItem == null)
				return;

			DbRecordToItemRecord(a_DbRecord, currItem);
		}

		protected static void DbRecordToItemRecord(ItemPrices.PricesRow a_DbRecord, ItemRecord a_ItemRecord)
		{
			lock (a_ItemRecord)
			{
				a_ItemRecord.PriceDate	= DateTime.FromFileTimeUtc(Convert.ToInt64(a_DbRecord.UpdateTime));
				a_ItemRecord.Price		= a_DbRecord.Price;
			}
		}

		protected static void PriceRecordToDbRecord(PriceRecord a_PriceRecord, ItemPrices.PricesRow a_DbRecord)
		{
			a_DbRecord.ProviderID	= (UInt32)a_PriceRecord.Settings.Provider;
			a_DbRecord.RegionID		= a_PriceRecord.Settings.RegionID;
			a_DbRecord.SolarID		= a_PriceRecord.Settings.SolarID;
			a_DbRecord.StationID	= a_PriceRecord.Settings.StationID;
			a_DbRecord.PriceType	= (UInt32)a_PriceRecord.Settings.PriceType;
			a_DbRecord.TypeID		= a_PriceRecord.TypeID;
			a_DbRecord.Price		= a_PriceRecord.Price;
			a_DbRecord.UpdateTime	= a_PriceRecord.UpdateTime;
		}

		void ApplyPrices(List<PriceRecord> a_Prices, PriceSettings a_Filter)
		{
			ItemPrices.PricesDataTable newDbData = new ItemPrices.PricesDataTable();

			foreach (PriceRecord currRecord in a_Prices)
			{
				ItemPrices.PricesRow dbRecord = newDbData.NewPricesRow();
				PriceRecordToDbRecord(currRecord, dbRecord);
				newDbData.Rows.Add(dbRecord);

				if (currRecord.Settings.Matches(a_Filter))
					DbRecordToItemRecord(dbRecord);
			}

			m_Database.ReplaceRows(newDbData, newDbData.Rows);
		}

		protected class UpdateThreadParam
		{
			public PriceSettings	PriceSettings;
			public UInt32			PriceHistoryDays;
			public Queue<UInt32>	UpdateQueue;
		}
		
		protected void StopUpdaterThread()
		{
			if (m_UpdateThread == null)
				return;

			m_EndUpdateThread = true;
			m_UpdateThread.Join(new TimeSpan(0, 0, 10));
			if (m_UpdateThread.IsAlive)
			{
				m_UpdateThread.Abort();
				m_UpdateThread.Join();
			}
			
			m_UpdateThread	= null;
			m_UpdateQueue	= null;
		}

		protected void TestMarketPrices(PriceSettings a_Settings, UInt32 a_PriceExpiryDays, UInt32 a_PriceHistoryDays, bool a_Silent)
		{
			ItemFilter filter		= new ItemFilter();
			filter.HasMarketGroup	= TristateFilter.Yes;
			filter.IsPricesOk		= TristateFilter.No;
			filter.PriceExpiryDays	= a_PriceExpiryDays;
			
			UInt32[] badItems = m_ItemsDB.FilterItems(filter);
			if (0 == badItems.Count())
				return;

			if (!a_Silent)
			{
				if (DialogResult.Yes != MessageBox.Show("You have outdated market prices. Would you like to update them now?", Application.ProductName, MessageBoxButtons.YesNo))
					return;
			}

			Queue<UInt32> pricesQueue = new Queue<UInt32>();
			foreach (UInt32 currItem in badItems)
			{
				pricesQueue.Enqueue(currItem);
			}
			
			StopUpdaterThread();

			UpdateThreadParam param = new UpdateThreadParam();
			param.PriceSettings		= a_Settings;
			param.PriceHistoryDays	= a_PriceHistoryDays;
			param.UpdateQueue		= pricesQueue;

			ThreadWithParam paramThread = new ThreadWithParam();
			paramThread.Function	= ThreadQueryMarketPrices;
			paramThread.Parameter	= param;
			
			m_EndUpdateThread		= false;
			m_UpdateQueue			= pricesQueue;

			m_UpdateThread = paramThread.CreateThread();
			m_UpdateThread.IsBackground = true;
			m_UpdateThread.Start();
		}

		public static IPriceProvider CreateEveCentralProvider(UInt32 a_PriceHistoryDays)
		{
			PriceProviderEveCentral provider = new PriceProviderEveCentral();
			provider.m_PriceHistoryDays = a_PriceHistoryDays;
			return provider;
		}

		protected void ThreadQueryMarketPrices(Object a_ParamObj)
		{
			UpdateThreadParam a_Param = (UpdateThreadParam)a_ParamObj;
			IPriceProvider provider = CreateEveCentralProvider(a_Param.PriceHistoryDays);

			for (; !m_EndUpdateThread; )
			{
				const int blockSize = 32;
				List<UInt32> queriedItems = new List<UInt32>(blockSize);

				lock (a_Param.UpdateQueue)
				{
					if (0 == a_Param.UpdateQueue.Count)
						return;

					for (int i = 0; i < blockSize; i++)
					{
						if (0 == a_Param.UpdateQueue.Count)
							break;

						UInt32 currItem = a_Param.UpdateQueue.Dequeue();
						queriedItems.Add(currItem);
					}
				}

				try
				{
					List<PriceRecord> newPrices = provider.GetPrices(queriedItems, a_Param.PriceSettings);
					ApplyPrices(newPrices, a_Param.PriceSettings);
				}
				catch (System.Exception a_Exception)
				{
					Debug.Assert(false, a_Exception.Message);
					System.Diagnostics.Debug.WriteLine(a_Exception.Message);

					// Return unqueried items back
					lock (a_Param.UpdateQueue)
					{
						foreach (UInt32 typeID in queriedItems)
						{
							a_Param.UpdateQueue.Enqueue(typeID);
						}
					}
				}
			}
			
			Debug.WriteLine(String.Format("Stopped updating prices for region {0:d}", a_Param.PriceSettings.RegionID));
		}

		public UInt32 GetQueueSize()
		{
			if (null == m_UpdateQueue)
				return 0;
		
			lock (m_UpdateQueue)
			{
				return (UInt32)m_UpdateQueue.Count;
			}
		}
	}
}
