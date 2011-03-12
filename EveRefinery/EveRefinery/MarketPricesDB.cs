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
			connectionString.DataSource = "MarketPrices.db3";
			m_DbConnection.ConnectionString = connectionString.ConnectionString;
			m_DbConnection.Open();

			m_ItemsDB	= a_ItemsDB;
			
			m_Database = new SQLiteDatabase(m_DbConnection, m_DataSet);
			m_Database.CreateTables();
		}

		public void LoadPrices(UInt32 a_RegionID, bool a_Silent, UInt32 a_PriceHistoryDays)
		{
			StopUpdaterThread();
			
			m_ItemsDB.ResetItemPrices();
			
			ItemPrices.PricesDataTable pricesTable = new ItemPrices.PricesDataTable();
			string selectSQL = "Select * from " + pricesTable.TableName + " where " + pricesTable.RegionIDColumn.ColumnName + " = " + a_RegionID.ToString();
			SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectSQL, m_DbConnection);
			adapter.Fill(pricesTable);

			foreach (ItemPrices.PricesRow currRow in pricesTable.Rows)
			{
				ParsePricesRow(currRow);
			}

			TestMarketPrices(a_RegionID, a_Silent, a_PriceHistoryDays);
		}

		public void DropPrices(UInt32 a_RegionID)
		{
			StopUpdaterThread();

			ItemPrices.PricesDataTable pricesTable = new ItemPrices.PricesDataTable();
			string selectSQL = "Delete from " + pricesTable.TableName + " where " + pricesTable.RegionIDColumn.ColumnName + " = " + a_RegionID.ToString();
			SQLiteCommand sqlCommand = new SQLiteCommand(selectSQL, m_DbConnection);
			sqlCommand.ExecuteNonQuery();
		}
		
		protected void ParsePricesRow(ItemPrices.PricesRow a_Row)
		{
			ItemRecord currItem = m_ItemsDB.GetItemByTypeID(a_Row.TypeID);
			if (currItem == null)
				return;
			
			ParsePricesRow(a_Row, currItem);
		}

		public static void ParsePricesRow(ItemPrices.PricesRow a_Row, ItemRecord a_Item)
		{
			lock (a_Item)
			{
				a_Item.PricesDate								= DateTime.FromFileTimeUtc(Convert.ToInt64(a_Row.UpdateTime));
				a_Item.Prices[(UInt32)PriceTypes.AllAvg]		= a_Row.AllAvg;
				a_Item.Prices[(UInt32)PriceTypes.AllMax]		= a_Row.AllMax;
				a_Item.Prices[(UInt32)PriceTypes.AllMin]		= a_Row.AllMin;
				a_Item.Prices[(UInt32)PriceTypes.AllMedian]		= a_Row.AllMedian;
				a_Item.Prices[(UInt32)PriceTypes.BuyAvg]		= a_Row.BuyAvg;
				a_Item.Prices[(UInt32)PriceTypes.BuyMax]		= a_Row.BuyMax;
				a_Item.Prices[(UInt32)PriceTypes.BuyMin]		= a_Row.BuyMin;
				a_Item.Prices[(UInt32)PriceTypes.BuyMedian]		= a_Row.BuyMedian;
				a_Item.Prices[(UInt32)PriceTypes.SellAvg]		= a_Row.SellAvg;
				a_Item.Prices[(UInt32)PriceTypes.SellMax]		= a_Row.SellMax;
				a_Item.Prices[(UInt32)PriceTypes.SellMin]		= a_Row.SellMin;
				a_Item.Prices[(UInt32)PriceTypes.SellMedian]	= a_Row.SellMedian;
			}
		}

		protected class UpdateThreadParam
		{
			public UInt32			RegionID;
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

		protected void TestMarketPrices(UInt32 a_RegionID, bool a_Silent, UInt32 a_PriceHistoryDays)
		{
			ItemFilter filter = new ItemFilter();
			filter.HasMarketGroup = TristateFilter.Yes;
			filter.IsPricesOk = TristateFilter.No;
			
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
			param.RegionID			= a_RegionID;
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

		protected static double ReadInnerDouble(XmlNode a_Node)
		{
			return Convert.ToDouble(a_Node.InnerText, CultureInfo.InvariantCulture);
		}

		protected static UInt64 ReadInnerUInt64(XmlNode a_Node)
		{
			string value = a_Node.InnerText.Replace(".00", "");
			return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
		}

		protected static void ParseEveCentralAllNode(XmlNode a_ItemNode, ItemPrices.PricesRow a_ItemRow)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "volume":
						a_ItemRow.AllVolume = ReadInnerUInt64(childNode);
						break;
					case "avg":
						a_ItemRow.AllAvg = ReadInnerDouble(childNode);
						break;
					case "max":
						a_ItemRow.AllMax = ReadInnerDouble(childNode);
						break;
					case "min":
						a_ItemRow.AllMin = ReadInnerDouble(childNode);
						break;
					case "stddev":
						a_ItemRow.AllStdDev = ReadInnerDouble(childNode);
						break;
					case "median":
						a_ItemRow.AllMedian = ReadInnerDouble(childNode);
						break;
				}
			}
		}

		protected static void ParseEveCentralBuyNode(XmlNode a_ItemNode, ItemPrices.PricesRow a_ItemRow)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "volume":
						a_ItemRow.BuyVolume = ReadInnerUInt64(childNode);
						break;
					case "avg":
						a_ItemRow.BuyAvg = ReadInnerDouble(childNode);
						break;
					case "max":
						a_ItemRow.BuyMax = ReadInnerDouble(childNode);
						break;
					case "min":
						a_ItemRow.BuyMin = ReadInnerDouble(childNode);
						break;
					case "stddev":
						a_ItemRow.BuyStdDev = ReadInnerDouble(childNode);
						break;
					case "median":
						a_ItemRow.BuyMedian = ReadInnerDouble(childNode);
						break;
				}
			}
		}

		protected static void ParseEveCentralSellNode(XmlNode a_ItemNode, ItemPrices.PricesRow a_ItemRow)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "volume":
						a_ItemRow.SellVolume = ReadInnerUInt64(childNode);
						break;
					case "avg":
						a_ItemRow.SellAvg = ReadInnerDouble(childNode);
						break;
					case "max":
						a_ItemRow.SellMax = ReadInnerDouble(childNode);
						break;
					case "min":
						a_ItemRow.SellMin = ReadInnerDouble(childNode);
						break;
					case "stddev":
						a_ItemRow.SellStdDev = ReadInnerDouble(childNode);
						break;
					case "median":
						a_ItemRow.SellMedian = ReadInnerDouble(childNode);
						break;
				}
			}
		}

		protected static void ParseEveCentralItemNode(XmlNode a_ItemNode, ItemPrices.PricesRow a_ItemRow)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "all":
						ParseEveCentralAllNode(childNode, a_ItemRow);
						break;
					case "buy":
						ParseEveCentralBuyNode(childNode, a_ItemRow);
						break;
					case "sell":
						ParseEveCentralSellNode(childNode, a_ItemRow);
						break;
				}
			}
		}

		protected static void ParseEveCentralMarketXML(XmlDocument a_Xml, UInt32 a_RegionID, ItemPrices.PricesDataTable a_PricesTable)
		{
			XmlNodeList xmlItems = a_Xml.GetElementsByTagName("type");

			for (int i = 0; i < xmlItems.Count; i++)
			{
				XmlNode currNode = xmlItems[i];
				ItemPrices.PricesRow currRow = a_PricesTable.NewPricesRow();
				UInt32 itemTypeID = Convert.ToUInt32(currNode.Attributes[0].Value);
				
				currRow.AllAvg		= 0;
				currRow.AllMax		= 0;
				currRow.AllMedian	= 0;
				currRow.AllMin		= 0;
				currRow.AllStdDev	= 0;
				currRow.AllVolume	= 0;
				currRow.BuyAvg		= 0;
				currRow.BuyMax		= 0;
				currRow.BuyMedian	= 0;
				currRow.BuyMin		= 0;
				currRow.BuyStdDev	= 0;
				currRow.BuyVolume	= 0;
				currRow.SellAvg		= 0;
				currRow.SellMax		= 0;
				currRow.SellMedian	= 0;
				currRow.SellMin		= 0;
				currRow.SellStdDev	= 0;
				currRow.SellVolume	= 0;

				ParseEveCentralItemNode(currNode, currRow);
				currRow.UpdateTime	= (UInt64)DateTime.UtcNow.ToFileTimeUtc();
				currRow.TypeID		= itemTypeID;
				currRow.RegionID	= a_RegionID;

				a_PricesTable.Rows.Add(currRow);
			}
		}

		public static ItemPrices.PricesDataTable QueryEveCentralPrices(List<UInt32> a_TypeIDs, UInt32 a_RegionID, UInt32 a_PriceHistoryDays)
		{
			StringBuilder marketXmlUrl = new StringBuilder();
			marketXmlUrl.AppendFormat("http://api.eve-central.com/api/marketstat?hours={0:d}", a_PriceHistoryDays * 24);
			if (0 != a_RegionID)
			{
				marketXmlUrl.Append("&regionlimit=");
				marketXmlUrl.Append(a_RegionID.ToString());
			}
			
			foreach (UInt32 currTypeID in a_TypeIDs)
			{
				marketXmlUrl.AppendFormat("&typeid={0:d}", currTypeID);
			}

			XmlDocument xmlReply = Engine.LoadXmlWithUserAgent(marketXmlUrl.ToString());

			ItemPrices.PricesDataTable result = new ItemPrices.PricesDataTable();
			ParseEveCentralMarketXML(xmlReply, a_RegionID, result);
			
			return result;
		}

		void ApplyPrices(ItemPrices.PricesDataTable a_PricesTable)
		{
			foreach (ItemPrices.PricesRow currRow in a_PricesTable.Rows)
			{
				ParsePricesRow(currRow);
			}

			m_Database.ReplaceRows(a_PricesTable, a_PricesTable.Rows);
		}

		void ThreadQueryMarketPrices(Object a_ParamObj)
		{
			UpdateThreadParam a_Param = (UpdateThreadParam)a_ParamObj;
		
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
					ItemPrices.PricesDataTable newPrices = QueryEveCentralPrices(queriedItems, a_Param.RegionID, a_Param.PriceHistoryDays);
					ApplyPrices(newPrices);
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
			
			Debug.WriteLine(String.Format("Stopped updating prices for region {0:d}", a_Param.RegionID));
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
