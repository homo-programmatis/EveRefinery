﻿﻿﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace EveRefinery
{
    public class PriceProviderFuzzwork : IPriceProvider
    {
        List<PriceRecord> IPriceProvider.GetPrices(List<UInt32> a_TypeIDs, PriceSettings a_Settings)
        {
            StringBuilder marketUrl = new StringBuilder();
            marketUrl.Append("https://market.fuzzwork.co.uk/aggregates/?region=");
            if (0 != a_Settings.RegionID)
            {
                marketUrl.Append(a_Settings.RegionID.ToString());
            }
            else
            {
                //use Jita region id if not defined
                marketUrl.Append(EveRegions.Forge);
            }

            marketUrl.Append("&types=");
            foreach (UInt32 currTypeID in a_TypeIDs)
			{
			    marketUrl.AppendFormat("{0:d},", currTypeID);
			}
            
            XmlDocument xmlReply = Engine.LoadXmlFromJsonWithUserAgent(marketUrl.ToString());
            return ParseReplyXML(xmlReply, a_Settings);
		}

		protected static double ReadInnerDouble(XmlNode a_Node)
		{
			return Convert.ToDouble(a_Node.InnerText, CultureInfo.InvariantCulture);
		}

		protected static PriceRecord ComposePrice(XmlNode a_ItemNode, PriceRecord a_Template, PriceTypes a_Type)
		{
			PriceRecord result			= new PriceRecord();
			result.Settings				= a_Template.Settings;
			result.Settings.PriceType	= a_Type;
			result.Price				= ReadInnerDouble(a_ItemNode);
			result.TypeID				= a_Template.TypeID;
			result.UpdateTime			= a_Template.UpdateTime;

			return result;
		}

		protected static void ParsePriceNode(XmlNode a_ItemNode, PriceRecord a_Template, List<PriceRecord> a_Result, PriceTypes a_MaxType, PriceTypes a_MinType, PriceTypes a_AvgType, PriceTypes a_MedType)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "volume":
					case "stddev":
						break;
					case "weightedAverage":
						a_Result.Add(ComposePrice(childNode, a_Template, a_AvgType));
						break;
					case "max":
						a_Result.Add(ComposePrice(childNode, a_Template, a_MaxType));
						break;
					case "min":
						a_Result.Add(ComposePrice(childNode, a_Template, a_MinType));
						break;
					case "median":
						a_Result.Add(ComposePrice(childNode, a_Template, a_MedType));
						break;
				}
			}
		}

		protected static void ParseItemNode(XmlNode a_ItemNode, PriceRecord a_Template, List<PriceRecord> a_Result)
		{
			foreach (XmlNode childNode in a_ItemNode.ChildNodes)
			{
				switch (childNode.Name)
				{
					case "buy":
						ParsePriceNode(childNode, a_Template, a_Result, PriceTypes.BuyMax, PriceTypes.BuyMin, PriceTypes.BuyAvg, PriceTypes.BuyMedian);
						break;
					case "sell":
						ParsePriceNode(childNode, a_Template, a_Result, PriceTypes.SellMax, PriceTypes.SellMin, PriceTypes.SellAvg, PriceTypes.SellMedian);
						break;
				}
			}
		}

		protected static List<PriceRecord> ParseReplyXML(XmlDocument a_Xml, PriceSettings a_Filter)
		{
			XmlNodeList xmlItems = a_Xml.GetElementsByTagName("root")[0].ChildNodes;
			List<PriceRecord> result = new List<PriceRecord>();

			for (int i = 0; i < xmlItems.Count; i++)
			{
			    XmlNode currNode = xmlItems[i];
			    //clear symbol '_' which was added to 'type' name
			    UInt32 itemTypeID = Convert.ToUInt32(currNode.Name.Replace("_", ""));

			    PriceRecord template = new PriceRecord();
			    template.Settings		= a_Filter;
			    template.TypeID		= itemTypeID;
			    template.UpdateTime	= (UInt64)DateTime.UtcNow.ToFileTimeUtc();

			    ParseItemNode(currNode, template, result);
			}

			return result;
		}
	}
}
