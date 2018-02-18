using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace EveRefinery
{
    public class PriceProviderFuzzworkCoUk : IPriceProvider
    {
		Settings.V2._FuzzworkCoUk		m_Settings;

		public							PriceProviderFuzzworkCoUk(Settings.V2._FuzzworkCoUk a_Settings)
		{
			m_Settings = a_Settings;
		}

		public List<PriceRecord>		GetPrices(List<UInt32> a_TypeIDs)
        {
            StringBuilder marketUrl = new StringBuilder();
            marketUrl.Append("https://market.fuzzwork.co.uk/aggregates/");
            if (m_Settings.IsRegion)
				marketUrl.AppendFormat("?region={0}", m_Settings.RegionID);
			else
                marketUrl.AppendFormat("?station={0}", m_Settings.StationID);

            marketUrl.Append("&types=");
            for (int i = 0; i < a_TypeIDs.Count; i++)
			{
				UInt32 currTypeID = a_TypeIDs[i];

				if (0 != i)
					marketUrl.Append(",");

				marketUrl.AppendFormat("{0:d}", currTypeID);
			}
            
            JObject jsonReply = Engine.LoadJsonWithUserAgent(marketUrl.ToString());
            return ParseReplyJSON(jsonReply);
		}

		public PriceRecord				GetCurrentFilter()
		{
			PriceRecord filter  = new PriceRecord();
			filter.Provider     = PriceProviders.FuzzworkCoUk;
			filter.RegionID     = m_Settings.IsRegion ? m_Settings.RegionID : 0;
			filter.SolarID      = 0;
			filter.StationID    = m_Settings.IsRegion ? 0 : m_Settings.StationID;
			filter.PriceType    = m_Settings.PriceType;
			filter.TypeID       = 0;
			filter.Price        = 0;
			filter.UpdateTime   = 0;

			return filter;
		}

		public String					GetCurrentFilterHint(EveDatabase a_Database)
		{
			if (m_Settings.IsRegion)
				return "market.fuzzwork.co.uk - " + m_Settings.PriceType.ToString() + " - " + a_Database.GetLocationName(m_Settings.RegionID, 0, 0);
			else
				return "market.fuzzwork.co.uk - " + m_Settings.PriceType.ToString() + " - " + a_Database.GetLocationName(0, 0, m_Settings.StationID);
		}

		public UInt32					GetRequestBlockSize()
		{
			// From https://market.fuzzwork.co.uk/api/
			// "I highly recommend pulling all the aggregates into one sheet, and then using vlookup to retrieve them. It's more efficient for both you and me"
			// However, trying to fit all item in one request results in "(414) Request-URI Too Large."
			return 1024;
		}

		public static PriceTypes[]		GetSupportedPriceTypes()
		{
			PriceTypes[] result =
			{
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

		public static UInt32[]			GetSupportedStations()
		{
			UInt32[] result =
			{
				(UInt32)EveStations.Amarr_8,
				(UInt32)EveStations.Dodixie_9_20,
				(UInt32)EveStations.Jita_4_4,
				(UInt32)EveStations.Hek_8_12,
				(UInt32)EveStations.Rens_6_8,
			};

			return result;
		}

		protected PriceRecord			ComposePrice(JToken a_Value, UInt32 a_TypeID, PriceTypes a_PriceType)
		{
			PriceRecord result	= GetCurrentFilter();
			result.PriceType	= a_PriceType;
			result.Price		= a_Value.Value<double>();
			result.TypeID		= a_TypeID;
			result.UpdateTime	= (UInt64)DateTime.UtcNow.ToFileTimeUtc();

			return result;
		}

		protected void					ParsePriceNode(JToken a_ItemNode, UInt32 a_TypeID, List<PriceRecord> a_Result, PriceTypes a_MaxType, PriceTypes a_MinType, PriceTypes a_AvgType, PriceTypes a_MedType)
		{
			foreach (JProperty childNode in (a_ItemNode as JObject).Properties())
			{
				switch (childNode.Name)
				{
					case "weightedAverage":
						a_Result.Add(ComposePrice(childNode.Value, a_TypeID, a_AvgType));
						break;
					case "max":
						a_Result.Add(ComposePrice(childNode.Value, a_TypeID, a_MaxType));
						break;
					case "min":
						a_Result.Add(ComposePrice(childNode.Value, a_TypeID, a_MinType));
						break;
					case "median":
						a_Result.Add(ComposePrice(childNode.Value, a_TypeID, a_MedType));
						break;
				}
			}
		}

		protected void					ParseItemNode(JToken a_ItemNode, UInt32 a_TypeID, List<PriceRecord> a_Result)
		{
			foreach (JProperty childNode in (a_ItemNode as JObject).Properties())
			{
				switch (childNode.Name)
				{
					case "buy":
						ParsePriceNode(childNode.Value, a_TypeID, a_Result, PriceTypes.BuyMax, PriceTypes.BuyMin, PriceTypes.BuyAvg, PriceTypes.BuyMedian);
						break;
					case "sell":
						ParsePriceNode(childNode.Value, a_TypeID, a_Result, PriceTypes.SellMax, PriceTypes.SellMin, PriceTypes.SellAvg, PriceTypes.SellMedian);
						break;
				}
			}
		}

		protected List<PriceRecord>		ParseReplyJSON(JObject a_Json)
		{
			List<PriceRecord> result = new List<PriceRecord>();

			foreach (JProperty currNode in a_Json.Properties())
			{
				UInt32 itemTypeID	= Convert.ToUInt32(currNode.Name);
			    ParseItemNode(currNode.Value, itemTypeID, result);
			}

			return result;
		}
	}
}
