using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace EveRefinery
{
	class PriceProviderEveMarketdataCom : IPriceProvider
	{
		private Settings.V2._EveMarketdataCom  m_Settings;

		public							PriceProviderEveMarketdataCom(Settings.V2._EveMarketdataCom a_Settings)
		{
			m_Settings = a_Settings;
		}

		public List<PriceRecord>		GetPrices(List<UInt32> a_TypeIDs)
		{
			StringBuilder marketUrl = new StringBuilder();
			marketUrl.Append("http://eve-marketdata.com/api/item_prices2.xml?char_name=EveRefinery_Program");

			if (0 != m_Settings.StationID)
				marketUrl.AppendFormat("&station_ids={0}", m_Settings.StationID);
			else if (0 != m_Settings.SolarID)
				marketUrl.AppendFormat("&solarsystem_ids={0}", m_Settings.SolarID);
			else if (0 != m_Settings.RegionID)
				marketUrl.AppendFormat("&region_ids={0}", m_Settings.RegionID);

			marketUrl.Append("&buysell=a&type_ids=");
			for (int i = 0; i < a_TypeIDs.Count; i++)
			{
				UInt32 currTypeID = a_TypeIDs[i];

				if (0 != i)
					marketUrl.Append(",");

				marketUrl.AppendFormat("{0:d}", currTypeID);
			}

			XmlDocument xmlReply = Engine.LoadXmlWithUserAgent(marketUrl.ToString());
			return ParseReplyXML(xmlReply);
		}

		public PriceRecord				GetCurrentFilter()
		{
			PriceRecord filter	= new PriceRecord();
			filter.Provider		= PriceProviders.EveMarketdataCom;
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
			return "eve-marketdata.com - " + m_Settings.PriceType.ToString() + " - " + a_Database.GetLocationName(m_Settings.RegionID, m_Settings.SolarID, m_Settings.StationID);
		}

		public UInt32					GetRequestBlockSize()
		{
			// From documentation: "a max of 10,000 rows will be returned".
			// 1024 should be a good number.
			return 1024;
		}

		public static PriceTypes[]		GetSupportedPriceTypes()
		{
			PriceTypes[] result =
			{
				PriceTypes.Buy95Pct,
				PriceTypes.Sell95Pct,
			};

			return result;
		}

		private List<PriceRecord>		ParseReplyXML(XmlDocument a_Xml)
		{
			XmlNodeList xmlItems = a_Xml.GetElementsByTagName("row");
			List<PriceRecord> result = new List<PriceRecord>();

			foreach (XmlNode currNode in xmlItems)
			{
				PriceRecord newRecord = GetCurrentFilter();

				String attr_buysell = currNode.Attributes["buysell"].Value;
				if ("b" == attr_buysell)
					newRecord.PriceType = PriceTypes.Buy95Pct;
				else if ("s" == attr_buysell)
					newRecord.PriceType = PriceTypes.Sell95Pct;
				else
					throw new FormatException("Invalid response xml format");

				newRecord.TypeID		= Convert.ToUInt32(currNode.Attributes["typeID"].Value);
				newRecord.Price			= Convert.ToDouble(currNode.Attributes["price"].Value, CultureInfo.InvariantCulture);
				newRecord.UpdateTime	= (UInt64)DateTime.UtcNow.ToFileTimeUtc();

				result.Add(newRecord);
			}

			return result;
		}
	}
}