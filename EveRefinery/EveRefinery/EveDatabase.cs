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
		public UInt32	ID;
		public String	Name;
	}

	public class EveSolarSystem
	{
		public UInt32	ID;
		public String	Name;
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
		RefiningMutator		= 379,
		MetaLevel			= 633,
		ReprocessingSkill	= 790,
	}

	public enum EveSkills
	{
		Reprocessing			= 3385,
		ReprocessingEfficiency	= 3389,
		ArkonorProcessing		= 12180,
		BistotProcessing		= 12181,
		CrokiteProcessing		= 12182,
		DarkOchreProcessing		= 12183,
		GneissProcessing		= 12184,
		HedbergiteProcessing	= 12185,
		HemorphiteProcessing	= 12186,
		JaspetProcessing		= 12187,
		KerniteProcessing		= 12188,
		MercoxitProcessing		= 12189,
		OmberProcessing			= 12190,
		PlagioclaseProcessing	= 12191,
		PyroxeresProcessing		= 12192,
		ScorditeProcessing		= 12193,
		SpodumainProcessing		= 12194,
		VeldsparProcessing		= 12195,
		ScrapmetalProcessing	= 12196,
		IceProcessing			= 18025,
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
			String sqlText = "SELECT * FROM " + Tables.invTypeMaterials;
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
			String[] typeIDs		= new String[a_Items.Count];
			int typeIdIndex			= 0;
			foreach (Object currKey in a_Items.Keys)
			{
				typeIDs[typeIdIndex++] = ((UInt32)currKey).ToString();
			}
			
			String typeIdList		= "(" + String.Join(", ", typeIDs) + ")";
			//////////////////////////////////////////////////////////////////////////

            // Load .ItemName, .IsPublished, .GroupID, .MarketGroupID, .BatchSize, .Volume
			{
				String sqlText				= "SELECT * FROM " + Tables.invTypes + " WHERE typeID in " + typeIdList;
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

					currItem.ItemName		= (String)dataReader[idx_typeName];
					currItem.IsPublished	= (0 != dataReader.GetInt32(idx_published));
					currItem.GroupID		= (UInt32)dataReader.GetInt32(idx_groupID);
					currItem.MarketGroupID	= dataReader.IsDBNull(idx_marketGroupID) ? 0 : (UInt32)dataReader.GetInt32(idx_marketGroupID);
					currItem.BatchSize		= (UInt32)dataReader.GetInt32(idx_portionSize);
					currItem.Volume			= dataReader.GetDouble(idx_volume);
				}
			}

            // Load Category and Group names for composing sort string
			{
				String sqlText = 
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

					String categoryName		= dataReader.GetString(1);
					String groupName		= dataReader.GetString(2);
					currItem.TypeSortString = categoryName + " " + groupName + " " + currItem.ItemName;
				}
			}

            // Load .MetaLevel
            {
                String sqlText = 
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

			String sqlText = "SELECT regionID, regionName FROM " + Tables.mapRegions;
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();
			
			while (dataReader.Read())
			{
				EveRegion newRegion = new EveRegion();
				newRegion.ID		= (UInt32)dataReader.GetInt32(0);
				newRegion.Name		= dataReader.GetString(1);

				// There's a bunch of "Unknown" regions in DB, just skip them
				if (newRegion.Name == "Unknown")
					continue;
				
				result.Add(newRegion);
			}
			
			return result;
		}

		public List<EveSolarSystem> GetSolarSystems(UInt32 a_RegionID)
		{
			List<EveSolarSystem> result = new List<EveSolarSystem>();

			String sqlText = "SELECT solarSystemID, solarSystemName FROM " + Tables.mapSolarSystems + " WHERE regionID=@RegionID";
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			sqlCommand.Parameters.AddWithValue("@RegionID", a_RegionID);
			
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();
			while (dataReader.Read())
			{
				EveSolarSystem newSystem = new EveSolarSystem();
				newSystem.ID		= (UInt32)dataReader.GetInt32(0);
				newSystem.Name		= dataReader.GetString(1);

				result.Add(newSystem);
			}

			return result;
		}

		public String GetRegionName(UInt32 a_ID)
		{
			String sqlText = "SELECT regionName FROM " + Tables.mapRegions + " WHERE regionID=@ID";
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			sqlCommand.Parameters.AddWithValue("@ID", a_ID);

			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();
			if (dataReader.Read())
				return Convert.ToString(dataReader.GetString(0));

			return null;
		}

		public String GetSolarName(UInt32 a_ID)
		{
			String sqlText = "SELECT solarSystemName FROM " + Tables.mapSolarSystems + " WHERE solarSystemID=@ID";
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			sqlCommand.Parameters.AddWithValue("@ID", a_ID);

			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();
			if (dataReader.Read())
				return Convert.ToString(dataReader.GetString(0));

			return null;
		}

		public String GetStationName(UInt32 a_ID)
		{
			String sqlText = "SELECT stationName FROM " + Tables.staStations + " WHERE stationID=@ID";
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			sqlCommand.Parameters.AddWithValue("@ID", a_ID);

			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();
			if (dataReader.Read())
				return Convert.ToString(dataReader.GetString(0));

			return null;
		}

		public String GetLocationName(UInt32 a_LocationID)
		{
			String result = GetStationName(a_LocationID);
			if (result != null)
				return result;

			result = GetSolarName(a_LocationID);
			if (result != null)
				return result;

			return String.Format("Unknown location {0:d}", a_LocationID);
		}

		public String GetLocationName(UInt32 a_RegionID, UInt32 a_SolarID, UInt32 a_StationID)
		{
			if (a_StationID != 0)
			{
				String result = GetStationName(a_StationID);
				if (result != null)
					return result;

				return String.Format("Station {0:d}", a_StationID);
			}

			if (a_SolarID != 0)
			{
				String result = GetSolarName(a_SolarID);
				if (result != null)
					return result;

				return String.Format("System {0:d}", a_SolarID);
			}

			if (a_RegionID != 0)
			{
				String result = GetRegionName(a_RegionID);
				if (result != null)
					return result;

				return String.Format("Region {0:d}", a_RegionID);
			}

			return "All locations";
		}

		public String GetTypeIdName(UInt32 a_TypeID)
		{
			String sqlText = String.Format("SELECT typeName FROM " + Tables.invTypes + " WHERE typeID = {0:d}", a_TypeID);
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, m_DbConnection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

			if (dataReader.Read())
				return dataReader.GetString(0);

			return String.Format("TypeID_{0:d}", a_TypeID);
		}

		public UInt32 GetTypeIdCategory(UInt32 a_TypeID)
		{
			String sqlText = 
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

		private static List<String> GetDatabaseTables(SQLiteConnection a_Connection)
		{
			List<String> result = new List<String>();

			String sqlText = "SELECT name FROM sqlite_master WHERE type='table';";
			SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, a_Connection);
			SQLiteDataReader dataReader = sqlCommand.ExecuteReader();

			while (dataReader.Read())
			{
				result.Add(Convert.ToString(dataReader["name"]));
			}

			return result;
		}

		private Boolean TestEveDatabaseTables()
		{
			List<String> tableList;

			try
			{
				tableList = GetDatabaseTables(m_DbConnection);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Database error:\n" + a_Exception.Message);
				return false;
			}

			String errorMessage = "Your database is missing the following tables:\n";
			bool hasAllTables = true;

			foreach (String requiredTable in GetUsedTableNames())
			{
				if (!tableList.Contains(requiredTable))
				{
					hasAllTables = false;
					errorMessage += (requiredTable + "\n");
				}
			}
			
			if (!hasAllTables)
			{
				ErrorMessageBox.Show(errorMessage);
				return false;
			}

			return true;
		}
		
		public Hashtable LoadDatabase(String a_DBPath)
		{
			Hashtable items = new Hashtable();
		
			try
			{
				SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
				connBuilder.FailIfMissing = true;
				connBuilder.DataSource = a_DBPath;
				connBuilder.ReadOnly = true;

				m_DbConnection = new SQLiteConnection();
				m_DbConnection.ConnectionString = connBuilder.ToString();
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

		private static List<String> GetUsedTableNames()
		{
			List<String> result = new List<String>();

			foreach (Tables requiredTable in Enum.GetValues(typeof(Tables)))
			{
				String tableName = requiredTable.ToString();
				result.Add(tableName);
			}

			return result;
		}

		private static String MakeUsefulAttributeList()
		{
			StringBuilder result = new StringBuilder();

			EveAttributes[] attributeIDs = (EveAttributes[])Enum.GetValues(typeof(EveAttributes));
			foreach (EveAttributes currAttribute in attributeIDs)
			{
				if (0 != result.Length)
					result.Append(',');

				result.Append((int)currAttribute);
			}

			return result.ToString();
		}

		public static void StripDatabase(String a_DBPath)
		{
			SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
			connBuilder.FailIfMissing = true;
			connBuilder.DataSource = a_DBPath;
			connBuilder.JournalMode = SQLiteJournalModeEnum.Off;

			SQLiteConnection connection = new SQLiteConnection();
			connection.ConnectionString = connBuilder.ToString();
			connection.Open();

			List<String> allTables = GetDatabaseTables(connection);
			List<String> neededTables = GetUsedTableNames();

			// Drop unwanted tables
			foreach (String currentTable in allTables)
			{
				if (neededTables.Contains(currentTable))
					continue;

				String sqlText = "DROP TABLE " + currentTable;
				SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, connection);
				sqlCommand.ExecuteNonQuery();
			}

			// Leave only MetaLevel in attributes
			{
				String sqlText = "DELETE FROM " + Tables.dgmTypeAttributes + " WHERE attributeID NOT IN (" + MakeUsefulAttributeList() + ")";
				SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, connection);
				sqlCommand.ExecuteNonQuery();
			}

			// Free emptied space in database
			{
				String sqlText = "VACUUM";
				SQLiteCommand sqlCommand = new SQLiteCommand(sqlText, connection);
				sqlCommand.ExecuteNonQuery();
			}
		}
	}
}
