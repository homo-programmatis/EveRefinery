﻿using System;
using SQLiteDesign;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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

		#region public
		public						MarketPricesDB(ItemsDB a_ItemsDB)
		{
			SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
			connectionString.DataSource		= Program.GetCacheFolder() + "MarketPrices_v1.db3";
			m_DbConnection.ConnectionString = connectionString.ConnectionString;
			m_DbConnection.Open();

			m_ItemsDB	= a_ItemsDB;
			
			m_Database = new SQLiteDatabase(m_DbConnection, m_DataSet);
			m_Database.CreateTables();
		}

		public void					LoadPrices(IPriceProvider a_PriceProvider, Settings.V2._PriceSettings a_Settings, bool a_Silent)
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
			PriceRecord priceFilter = a_PriceProvider.GetCurrentFilter();
			sqlCommand.Parameters.AddWithValue("@ProviderID",	(UInt32)priceFilter.Provider);
			sqlCommand.Parameters.AddWithValue("@RegionID",		priceFilter.RegionID);
			sqlCommand.Parameters.AddWithValue("@SolarID",		priceFilter.SolarID);
			sqlCommand.Parameters.AddWithValue("@StationID",	priceFilter.StationID);
			sqlCommand.Parameters.AddWithValue("@PriceType",	(UInt32)priceFilter.PriceType);

			SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlCommand);
			adapter.Fill(pricesTable);

			foreach (ItemPrices.PricesRow currRow in pricesTable.Rows)
			{
				DbRecordToItemRecord(currRow);
			}

			TestMarketPrices(a_PriceProvider, a_Settings, a_Silent);
		}

		public void					DropPrices(IPriceProvider a_PriceProvider)
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
			PriceRecord priceFilter = a_PriceProvider.GetCurrentFilter();
			sqlCommand.Parameters.AddWithValue("@ProviderID",	(UInt32)priceFilter.Provider);
			sqlCommand.Parameters.AddWithValue("@RegionID",		priceFilter.RegionID);
			sqlCommand.Parameters.AddWithValue("@SolarID",		priceFilter.SolarID);
			sqlCommand.Parameters.AddWithValue("@StationID",	priceFilter.StationID);
			sqlCommand.Parameters.AddWithValue("@PriceType",	(UInt32)priceFilter.PriceType);

			sqlCommand.ExecuteNonQuery();
		}

		public UInt32				GetQueueSize()
		{
			if (null == m_UpdateQueue)
				return 0;

			lock (m_UpdateQueue)
			{
				return (UInt32)m_UpdateQueue.Count;
			}
		}
		#endregion

		#region private
		private void				DbRecordToItemRecord(ItemPrices.PricesRow a_DbRecord)
		{
			ItemRecord currItem = m_ItemsDB.GetItemByTypeID(a_DbRecord.TypeID);
			if (currItem == null)
				return;

			DbRecordToItemRecord(a_DbRecord, currItem);
		}

		private static void			DbRecordToItemRecord(ItemPrices.PricesRow a_DbRecord, ItemRecord a_ItemRecord)
		{
			lock (a_ItemRecord)
			{
				a_ItemRecord.PriceDate	= DateTime.FromFileTimeUtc(Convert.ToInt64(a_DbRecord.UpdateTime));
				a_ItemRecord.Price		= a_DbRecord.Price;
			}
		}

		private static void			PriceRecordToDbRecord(PriceRecord a_PriceRecord, ItemPrices.PricesRow a_DbRecord)
		{
			a_DbRecord.ProviderID	= (UInt32)a_PriceRecord.Provider;
			a_DbRecord.RegionID		= a_PriceRecord.RegionID;
			a_DbRecord.SolarID		= a_PriceRecord.SolarID;
			a_DbRecord.StationID	= a_PriceRecord.StationID;
			a_DbRecord.PriceType	= (UInt32)a_PriceRecord.PriceType;
			a_DbRecord.TypeID		= a_PriceRecord.TypeID;
			a_DbRecord.Price		= a_PriceRecord.Price;
			a_DbRecord.UpdateTime	= a_PriceRecord.UpdateTime;
		}

		private void				ApplyPrices(List<PriceRecord> a_Prices, PriceRecord a_Filter)
		{
			ItemPrices.PricesDataTable newDbData = new ItemPrices.PricesDataTable();

			foreach (PriceRecord currRecord in a_Prices)
			{
				ItemPrices.PricesRow dbRecord = newDbData.NewPricesRow();
				PriceRecordToDbRecord(currRecord, dbRecord);
				newDbData.Rows.Add(dbRecord);

				// Price provider can return more data then requested (for example, all price types at once, or all regions at once).
				// If data matches currently selected filter, apply it to loaded items.
				if (currRecord.IsMatchesFilter(a_Filter))
					DbRecordToItemRecord(dbRecord);
			}

			// Store everything in database
			m_Database.ReplaceRows(newDbData, newDbData.Rows);
		}

		private class				UpdateThreadParam
		{
			public IPriceProvider	PriceProvider;
			public Queue<UInt32>	UpdateQueue;
		}

		private void				StopUpdaterThread()
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

		private void				TestMarketPrices(IPriceProvider a_PriceProvider, Settings.V2._PriceSettings a_Settings, bool a_Silent)
		{
			ItemFilter filter		= new ItemFilter();
			filter.HasMarketGroup	= TristateFilter.Yes;
			filter.IsPricesOk		= TristateFilter.No;
			filter.PriceExpiryDays	= a_Settings.ExpiryDays;
			
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
			param.PriceProvider		= a_PriceProvider;
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

		private void				ThreadQueryMarketPrices(Object a_ParamObj)
		{
			UpdateThreadParam a_Param = (UpdateThreadParam)a_ParamObj;

			for (; !m_EndUpdateThread; )
			{
				UInt32 blockSize = a_Param.PriceProvider.GetRequestBlockSize();
				List<UInt32> queriedItems = new List<UInt32>(a_Param.UpdateQueue.Count);

				lock (a_Param.UpdateQueue)
				{
					if (0 == a_Param.UpdateQueue.Count)
						return;

					for (UInt32 i = 0; i < blockSize; i++)
					{
						if (0 == a_Param.UpdateQueue.Count)
							break;

						UInt32 currItem = a_Param.UpdateQueue.Dequeue();
						queriedItems.Add(currItem);
					}
				}

				try
				{
					List<PriceRecord> newPrices = a_Param.PriceProvider.GetPrices(queriedItems);
					ApplyPrices(newPrices, a_Param.PriceProvider.GetCurrentFilter());
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
			
			Debug.WriteLine(String.Format("ThreadQueryMarketPrices: Stopped updating prices"));
		}
		#endregion
	}
}
