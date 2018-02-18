using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace EveRefinery
{
	class PriceProviderEveCentralCom : IPriceProvider
	{
		private Settings.V2._EveCentralCom  m_Settings;

		public							PriceProviderEveCentralCom(Settings.V2._EveCentralCom a_Settings)
		{
			m_Settings = a_Settings;
		}

		public List<PriceRecord>		GetPrices(List<UInt32> a_TypeIDs)
		{
			StringBuilder marketXmlUrl = new StringBuilder();
			marketXmlUrl.AppendFormat("http://api.eve-central.com/api/marketstat?hours={0:d}", m_Settings.HistoryDays * 24);

			if (0 != m_Settings.SolarID)
			{
				marketXmlUrl.Append("&usesystem=");
				marketXmlUrl.Append(m_Settings.SolarID.ToString());
			}
			else if (0 != m_Settings.RegionID)
			{
				marketXmlUrl.Append("&regionlimit=");
				marketXmlUrl.Append(m_Settings.RegionID.ToString());
			}

			foreach (UInt32 currTypeID in a_TypeIDs)
			{
				marketXmlUrl.AppendFormat("&typeid={0:d}", currTypeID);
			}

			XmlDocument xmlReply = Engine.LoadXmlWithUserAgent(marketXmlUrl.ToString());
			return ParseReplyXML(xmlReply);
		}

		public PriceRecord				GetCurrentFilter()
		{
			PriceRecord filter	= new PriceRecord();
			filter.Provider		= PriceProviders.EveCentral;
			filter.RegionID		= m_Settings.RegionID;
			filter.SolarID		= m_Settings.SolarID;
			filter.StationID	= m_Settings.StationID;
			filter.PriceType	= m_Settings.PriceType;
			filter.TypeID       = 0;
			filter.Price        = 0;
			filter.UpdateTime   = 0;

			return filter;
		}

		public String					GetCurrentFilterHint(EveDatabase a_Database)
		{
			return "eve-central.com - " + m_Settings.PriceType.ToString() + " - " + a_Database.GetLocationName(m_Settings.RegionID, m_Settings.SolarID, m_Settings.StationID);
		}

		public UInt32					GetRequestBlockSize()
		{
			// Historically used value, not sure if it's still good
			return 32;
		}

		public static PriceTypes[]		GetSupportedPriceTypes()
		{
			PriceTypes[] result =
			{
				PriceTypes.AllAvg,
				PriceTypes.AllMax,
				PriceTypes.AllMin,
				PriceTypes.AllMedian,
				PriceTypes.BuyAvg,
				PriceTypes.BuyMax,
				PriceTypes.BuyMin,
				PriceTypes.BuyMedian,
				PriceTypes.SellAvg,
				PriceTypes.SellMax,
				PriceTypes.SellMin,
				PriceTypes.SellMedian,
			};

			return result;
		}

		private static double			ReadInnerDouble(XmlNode a_Node)
		{
			return Convert.ToDouble(a_Node.InnerText, CultureInfo.InvariantCulture);
		}

		private static UInt64			ReadInnerUInt64(XmlNode a_Node)
		{
			string value = a_Node.InnerText.Replace(".00", "");
			return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
		}

		private PriceRecord				ComposePrice(XmlNode a_ItemNode, UInt32 a_TypeID, PriceTypes a_PriceType)
		{
			PriceRecord result	= GetCurrentFilter();
			result.PriceType	= a_PriceType;
			result.Price		= ReadInnerDouble(a_ItemNode);
			result.TypeID		= a_TypeID;
			result.UpdateTime	= (UInt64)DateTime.UtcNow.ToFileTimeUtc();

			return result;
		}

		private void					ParsePriceNode(XmlNode a_ItemNode, UInt32 a_TypeID, List<PriceRecord> a_Result, PriceTypes a_MaxType, PriceTypes a_MinType, PriceTypes a_AvgType, PriceTypes a_MedType)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "volume":
					case "stddev":
						break;
					case "avg":
						a_Result.Add(ComposePrice(childNode, a_TypeID, a_AvgType));
						break;
					case "max":
						a_Result.Add(ComposePrice(childNode, a_TypeID, a_MaxType));
						break;
					case "min":
						a_Result.Add(ComposePrice(childNode, a_TypeID, a_MinType));
						break;
					case "median":
						a_Result.Add(ComposePrice(childNode, a_TypeID, a_MedType));
						break;
				}
			}
		}

		private void					ParseItemNode(XmlNode a_ItemNode, UInt32 a_TypeID, List<PriceRecord> a_Result)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "all":
						ParsePriceNode(childNode, a_TypeID, a_Result, PriceTypes.AllMax, PriceTypes.AllMin, PriceTypes.AllAvg, PriceTypes.AllMedian);
						break;
					case "buy":
						ParsePriceNode(childNode, a_TypeID, a_Result, PriceTypes.BuyMax, PriceTypes.BuyMin, PriceTypes.BuyAvg, PriceTypes.BuyMedian);
						break;
					case "sell":
						ParsePriceNode(childNode, a_TypeID, a_Result, PriceTypes.SellMax, PriceTypes.SellMin, PriceTypes.SellAvg, PriceTypes.SellMedian);
						break;
				}
			}
		}

		private List<PriceRecord>		ParseReplyXML(XmlDocument a_Xml)
		{
			XmlNodeList xmlItems = a_Xml.GetElementsByTagName("type");
			List<PriceRecord> result = new List<PriceRecord>();

			for (int i = 0; i < xmlItems.Count; i++)
			{
				XmlNode currNode = xmlItems[i];
				UInt32 itemTypeID = Convert.ToUInt32(currNode.Attributes[0].Value);

				ParseItemNode(currNode, itemTypeID, result);
			}

			return result;
		}
	}
}