using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Collections;
using System.Windows.Forms;
using SpecialFNs;

namespace EveRefinery
{
	public class EveRegion
	{
		public UInt32	RegionID;
		public string	Name;
	}
	
	public enum EveTypeIDs
	{
		Tritanium	= 34,
		Pyerite		= 35,
		Mexallon	= 36,
		Isogen		= 37,
		Noxcium		= 38,
		Zydrine		= 39,
		Megacyte	= 40,
		Morphite	= 11399,
	}
	
	public enum EveCategories
	{
		Ship		= 6,
		Celestial	= 2,
	}
	
	public enum EveRegions
	{
		Forge		= 10000002,
	}

	public enum EveAttributes
	{
		MetaLevel = 633,
	}

	public class EveDatabase
	{
		protected SQLiteConnection	m_DbConnection;

		protected enum Tables
		{
			dgmTypeAttributes,
			invCategories,
			invGroups,
			invTypeMaterials,
			invTypes,
			mapSolarSystems,
			mapRegions,
			staStations
		}

		private static Materials LookupMaterial(UInt32 a_MaterialTypeID)
		{
			switch ((EveTypeIDs)a_MaterialTypeID)
			{
				case EveTypeIDs.Tritanium:
					return Materials.Tritanium;
				case EveTypeIDs.Pyerite:
					return Materials.Pyerite;
				case EveTypeIDs.Mexallon:
					return Materials.Mexallon;
				case EveTypeIDs.Isogen:
					return Materials.Isogen;
				case EveTypeIDs.Noxcium:
					return Materials.Noxcium;
				case EveTypeIDs.Zydrine:
					return Materials.Zydrine;
				case EveTypeIDs.Megacyte:
					return Materials.Megacyte;
				case EveTypeIDs.Morphite:
					return Materials.Morphite;
			}

			return Materials.Unknown;
		}

		private void LoadMineralCompositions(Hashtable a_Items)
		{
			string sqlText = "SELECT * FROM " + Tables.invTypeMaterials;
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

			// Optimization: accessing columns by index (saves over 1 sec)
			bool idx_Inited = false;
			int idx_typeID = 0, idx_materialTypeID = 0, idx_Quantity = 0;

			while (dataReader.Read())
			{
				if (!idx_Inited)
				{
					idx_Inited			= true;
					idx_typeID			= dataReader.GetOrdinal("typeID");
					idx_materialTypeID	= dataReader.GetOrdinal("materialTypeID");
					idx_Quantity		= dataReader.GetOrdinal("Quantity");
				}

				UInt32 currTypeID		= (UInt32)dataReader.GetInt32(idx_typeID);
				UInt32 currMaterialID	= (UInt32)dataReader.GetInt32(idx_materialTypeID);
				UInt32 currQuantity		= (UInt32)dataReader.GetInt32(idx_Quantity);

				if (!a_Items.ContainsKey(currTypeID))
					a_Items.Add(currTypeID, new ItemRecord(currTypeID));
				ItemRecord currItem = (ItemRecord)a_Items[currTypeID];

				Materials currMaterial = LookupMaterial(currMaterialID);
				if (Materials.Unknown == currMaterial)
					currItem.HasUnknownMaterials = true;
				else
					currItem.MaterialAmount[(UInt32)currMaterial] = currQuantity;
			}
		}

		private void LoadItemsProperties(Hashtable a_Items)
		{
			//////////////////////////////////////////////////////////////////////////
			// Optimization: do everything in one super-query (saves over 2.5 sec)
			// unfortunately Data.SQLite has tremendous per-query overhead
			string[] typeIDs		= new string[a_Items.Count];
			int typeIdIndex			= 0;
			foreach (Object currKey in a_Items.Keys)
			{
				typeIDs[typeIdIndex++] = ((UInt32)currKey).ToString();
			}
			
			string typeIdList		= "(" + String.Join(", ", typeIDs) + ")";
			//////////////////////////////////////////////////////////////////////////

            // Load .ItemName, .IsPublished, .GroupID, .MarketGroupID, .BatchSize, .Volume
			{
				string sqlText				= "SELECT * FROM " + Tables.invTypes + " WHERE typeID in " + typeIdList;
				SQLiteCommand sqlCommand	= new SQLiteCommand(sqlText, m_DbConnection);
				SQLiteDataReader dataReader	= sqlCommand.ExecuteReader();

				// Optimization: accessing columns by index (saves over 1 sec)
				bool idx_Inited = false;
				int idx_typeID = 0, idx_typeName = 0, idx_published = 0, idx_groupID = 0, idx_marketGroupID = 0, idx_portionSize = 0, idx_volume = 0;

				while (dataReader.Read())
				{
					if (!idx_Inited)
					{
						idx_Inited			= true;
						idx_typeID			= dataReader.GetOrdinal("typeID");
						idx_typeName		= dataReader.GetOrdinal("typeName");
						idx_published		= dataReader.GetOrdinal("published");
						idx_groupID			= dataReader.GetOrdinal("groupID");
						idx_marketGroupID	= dataReader.GetOrdinal("marketGroupID");
						idx_portionSize		= dataReader.GetOrdinal("portionSize");
						idx_volume			= dataReader.GetOrdinal("volume");
					}
				
					UInt32 currTypeID		= (UInt32)dataReader.GetInt32(idx_typeID);
					ItemRecord currItem		= (ItemRecord)a_Items[currTypeID];

					currItem.ItemName		= (string)dataReader[idx_typeName];
					currItem.IsPublished	= (0 != dataReader.GetInt32(idx_published));
					currItem.GroupID		= (UInt32)dataReader.GetInt32(idx_groupID);
					currItem.MarketGroupID	= dataReader.IsDBNull(idx_marketGroupID) ? 0 : (UInt32)dataReader.GetInt32(idx_marketGroupID);
					currItem.BatchSize		= (UInt32)dataReader.GetInt32(idx_portionSize);
					currItem.Volume			= dataReader.GetDouble(idx_volume);
				}
			}

            // Load Category and Group names for composing sort string
			{
				string sqlText = 
					"SELECT " + Tables.invTypes + ".typeID, " + Tables.invCategories + ".CategoryName, " + Tables.invGroups + ".GroupName FROM " +
					Tables.invTypes + ", " + Tables.invCategories + ", " + Tables.invGroups + " where " +
					"(" + Tables.invCategories + ".CategoryID = " + Tables.invGroups + ".CategoryID) and" +
					"(" + Tables.invGroups + ".GroupID = " + Tables.invTypes + ".GroupID) and " +
					"(" + Tables.invTypes + ".TypeID in " + typeIdList + ")";

				SQLiteCommand sqlCommand	= new SQLiteCommand(sqlText, m_DbConnection);
				SQLiteDataReader dataReader	= sqlCommand.ExecuteReader();

				while (dataReader.Read())
				{
					UInt32 currTypeID		= (UInt32)dataReader.GetInt32(0);
					ItemRecord currItem		= (ItemRecord)a_Items[currTypeID];

					string categoryName		= dataReader.GetString(1);
					string groupName		= dataReader.GetString(2);
					currItem.TypeSortString = categoryName + " " + groupName + " " + currItem.ItemName;
				}
			}

            // Load .MetaLevel
            {
                string sqlText = 
					"SELECT typeID, valueInt, valueFloat FROM " + 
					Tables.dgmTypeAttributes + " WHERE " +
					"(attributeID = " + (int)EveAttributes.MetaLevel + ") and " +
					"(TypeID in " + typeIdList + ")";

                SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
                SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    UInt32 currTypeID   = (UInt32)dataReader.GetInt32(0);
                    ItemRecord currItem = (ItemRecord)a_Items[currTypeID];

                    if (!dataReader.IsDBNull(1))
                        currItem.MetaLevel  = (UInt32)dataReader.GetInt32(1);
                    else if (!dataReader.IsDBNull(2))
                        currItem.MetaLevel  = (UInt32)dataReader.GetFloat(2);
                }
            }
		}
		
		public List<EveRegion> GetRegions()
		{
			List<EveRegion> result = new List<EveRegion>();

			string sqlText = "SELECT regionID, regionName FROM " + Tables.mapRegions;
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();
			
			while (dataReader.Read())
			{
				EveRegion newRegion = new EveRegion();
				newRegion.RegionID	= Convert.ToUInt32(dataReader["regionID"]);
				newRegion.Name		= Convert.ToString(dataReader["regionName"]);

				// There's a bunch of "Unknown" regions in DB, just skip them
				if (newRegion.Name == "Unknown")
					continue;
				
				result.Add(newRegion);
			}
			
			return result;
		}

		public string GetLocationName(UInt32 a_LocationID)
		{
			string sqlText = String.Format("SELECT stationName FROM " + Tables.staStations + " WHERE stationID = {0:d}", a_LocationID);
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

			if (dataReader.Read())
				return Convert.ToString(dataReader["stationName"]);

			sqlText		= String.Format("SELECT solarSystemName FROM " + Tables.mapSolarSystems + " WHERE solarSystemID = {0:d}", a_LocationID);
			sqlCommand	= new SQLiteCommand(sqlText, m_DbConnection);
			dataReader	= sqlCommand.ExecuteReader();

			if (dataReader.Read())
				return Convert.ToString(dataReader["solarSystemName"] + " (space)");
			
			return String.Format("Unknown location {0:d}", a_LocationID);
		}

		public string GetTypeIdName(UInt32 a_TypeID)
		{
			string sqlText = String.Format("SELECT typeName FROM " + Tables.invTypes + " WHERE typeID = {0:d}", a_TypeID);
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

			if (dataReader.Read())
				return dataReader.GetString(0);

			return String.Format("TypeID_{0:d}", a_TypeID);
		}

		public UInt32 GetTypeIdCategory(UInt32 a_TypeID)
		{
			string sqlText = 
				"SELECT " + Tables.invGroups + ".CategoryID FROM " +
				Tables.invGroups + ", " + Tables.invTypes + " where " +
				"(" + Tables.invGroups + ".GroupID = " + Tables.invTypes + ".GroupID) and " +
				"(" + Tables.invTypes + ".TypeID = " + a_TypeID + ")";

			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

			if (dataReader.Read())
				return (UInt32)dataReader.GetInt32(0);

			return 0;
		}

		private Boolean TestEveDatabaseTables()
		{
			List<String> tableList = new List<String>();

			try
			{
				string sqlText = "SELECT name FROM sqlite_master WHERE type='table';";
				SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
				SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

				while (dataReader.Read())
				{
					tableList.Add(Convert.ToString(dataReader["name"]));
				}
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Database error:\n" + a_Exception.Message);
				return false;
			}

			string errorMessage = "Your database is missing the following tables:\n";
			bool hasAllTables = true;

			foreach (Tables requiredTable in Enum.GetValues(typeof(Tables)))
			{
				String tableName = requiredTable.ToString();

				if (!tableList.Contains(tableName))
				{
					hasAllTables = false;
					errorMessage += (tableName + "\n");
				}
			}
			
			if (!hasAllTables)
			{
				ErrorMessageBox.Show(errorMessage);
				return false;
			}

			return true;
		}
		
		public Hashtable LoadDatabase(string a_DBPath)
		{
			Hashtable items = new Hashtable();
		
			try
			{
				m_DbConnection = new SQLiteConnection();
				m_DbConnection.ConnectionString = "FailIfMissing=True;Read Only=True;Data Source=" + a_DBPath;
				m_DbConnection.Open();
				
				if (!TestEveDatabaseTables())
					return null;

				LoadMineralCompositions(items);
				LoadItemsProperties(items);
			}
			catch (System.Exception a_Exception)
			{
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
				return null;
			}

			return items;
		}
	}
}
