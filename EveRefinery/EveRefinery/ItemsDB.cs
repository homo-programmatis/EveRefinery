using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Xml;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Collections;

namespace EveRefinery
{
    public enum Materials
    {
		Tritanium,
        Pyerite,
        Mexallon,
        Isogen,
        Noxcium,
        Zydrine,
        Megacyte,
        Morphite,

		MaxMaterials,
		Unknown,
	}
	
    public class ItemRecord
    {
		public ItemRecord(UInt32 a_TypeID)
		{
			TypeID = a_TypeID;
		}
		
		public Boolean IsPricesOk(UInt32 a_PriceExpiryDays)
		{
			DateTime limitTime = PriceDate.AddDays(a_PriceExpiryDays);
			if (limitTime < DateTime.UtcNow)
				return false;
			
			return true;
		}
		
		public void ResetPrice()
		{
			PriceDate	= new DateTime();
			Price		= 0;
		}
		
		public Boolean IsListItem()
		{
			if (HasUnknownMaterials)
				return false;

			if (!IsPublished)
				return false;
				
			return true;
		}

		public UInt32		TypeID;
		public string		ItemName;
		public string		TypeSortString;
		public Boolean		IsPublished;
		public UInt32		GroupID;
		public UInt32		MarketGroupID;
		public Boolean		HasUnknownMaterials;
		public double[]		MaterialAmount	= new double[(UInt32)Materials.MaxMaterials];
		public UInt32		BatchSize;
		public double		Volume;
		public DateTime		PriceDate;
		public double		Price			= 0;
        public UInt32       MetaLevel       = 0;
    }
    
    public enum TristateFilter
    {
		Yes,
		No,
		Any
    }
    
    public class ItemFilter
    {
		public TristateFilter	Published		= TristateFilter.Any;
		public TristateFilter	PlainMaterials	= TristateFilter.Any;
		public TristateFilter	HasMarketGroup	= TristateFilter.Any;
		public TristateFilter	IsPricesOk		= TristateFilter.Any;
		public AssetsMap		AssetsFilter;
		public UInt32			PriceExpiryDays;
		
		private Boolean TestTristate(TristateFilter a_Filter, Boolean a_Value)
		{
			if (a_Filter == TristateFilter.Any)
				return true;
				
			if (a_Filter == TristateFilter.Yes)
				return (a_Value == true);

			return (a_Value == false);
		}
		
		public Boolean TestItem(ItemRecord a_Item)
		{
			if (!TestTristate(Published, a_Item.IsPublished))
				return false;

			if (!TestTristate(PlainMaterials, !a_Item.HasUnknownMaterials))
				return false;

			if (!TestTristate(HasMarketGroup, (0 != a_Item.MarketGroupID)))
				return false;

			if (!TestTristate(IsPricesOk, a_Item.IsPricesOk(PriceExpiryDays)))
				return false;

			if ((AssetsFilter != null) && !AssetsFilter.ContainsKey(a_Item.TypeID))
				return false;

			return true;
		}
    }

    public class ItemsDB
    {
        Hashtable			m_Items = new Hashtable();

		public void ResetItemPrices()
		{
			IDictionaryEnumerator mapItem = m_Items.GetEnumerator();
			while (mapItem.MoveNext())
			{
				ItemRecord currItem = (ItemRecord)mapItem.Value;
				currItem.ResetPrice();
			}
		}

		public UInt32[] FilterItems(ItemFilter a_Filter)
		{
			UInt32 skeptItems = 0;
			IDictionaryEnumerator mapItem = m_Items.GetEnumerator();
			while (mapItem.MoveNext())
			{
				ItemRecord currItem = (ItemRecord)mapItem.Value;
				if (!a_Filter.TestItem(currItem))
					skeptItems++;
			}

			UInt32[] result = new UInt32[m_Items.Count - skeptItems];

			UInt32 arrayIndex = 0;
			mapItem	= m_Items.GetEnumerator();
			while (mapItem.MoveNext())
			{
				ItemRecord currItem = (ItemRecord)mapItem.Value;
				if (!a_Filter.TestItem(currItem))
					continue;

				result[arrayIndex] = (UInt32)mapItem.Key;
				arrayIndex++;
			}
			
			return result;
		}

		public Boolean LoadEveDatabase(EveDatabase a_Database, string a_DBPath)
		{
			m_Items = a_Database.LoadDatabase(a_DBPath);
			return (m_Items != null);
		}
		
		public ItemRecord GetItemByTypeID(UInt32 a_TypeID)
		{
			if (!m_Items.ContainsKey(a_TypeID))
				return null;

			return (ItemRecord)m_Items[a_TypeID];
		}
	}

	public class MaterialsInfo
	{
		public static double GetMaterialVolume(Materials a_Material)
		{
			return 0.01;
		}
	}
}
