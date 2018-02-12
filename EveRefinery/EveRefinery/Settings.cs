﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SkillDictionary = SpecialFNs.SerializableDictionary<System.UInt32, System.UInt32>;

namespace EveRefinery
{
	[Serializable]
    public class Settings
    {
		public class V1
		{
			[Serializable]
			[XmlType("Key")]
			public class _ApiKey
			{
				public UInt32			KeyID;
				public string			Verification;
			}

			[Serializable]
			[XmlType("Char")]
			public class _ApiChar
			{
				public UInt32			KeyID;
				public UInt32			CharacterID;
				public String			CharacterName;
				public String			CorporationName;
			}

			[Serializable]
			public class _ApiAccess
			{
				public List<_ApiKey>	Keys					= new List<_ApiKey>();
				public List<_ApiChar>	Chars					= new List<_ApiChar>();
			}

			[Serializable]
			[XmlType("Rect")]
			public struct _UIRect
			{
				public Int32			X0;
				public Int32			Y0;
				public Int32			CX;
				public Int32			CY;
			}

			[Serializable]
			[XmlType("Column")]
			public class _UIColumn
			{
				public String			Name;
				public UInt32			Index;
				public bool				Visible;
				public UInt32			Width;
			}

			[Serializable]
			[XmlType("Toolbar")]
			public class _UIToolbar
			{
				public String			Name;
				public _UIRect			Location;
				public UInt32			Panel;
			}

			[Serializable]
			public class _UILocations
			{
				public _UIRect			MainWindow;
				public List<_UIColumn>	Columns					= new List<_UIColumn>();
				public List<_UIToolbar>	Toolbars				= new List<_UIToolbar>();
			}

			[Serializable]
			public struct _PriceSettings
			{
				public PriceProviders	Provider;
				public UInt32			RegionID;
				public UInt32			SolarID;
				public UInt32			StationID;
				public PriceTypes		PriceType;
			}

			[Serializable]
			public class _PriceLoad
			{
				public _PriceSettings	SourceMinerals			= new _PriceSettings{Provider = PriceProviders.EveCentral, RegionID = (UInt32)EveRegions.Forge, PriceType = PriceTypes.SellMedian};
				public _PriceSettings	SourceItems				= new _PriceSettings{Provider = PriceProviders.EveCentral, RegionID = (UInt32)EveRegions.Forge, PriceType = PriceTypes.SellMedian};
				public UInt32			ItemsHistoryDays		= 14;
				public UInt32			ItemsExpiryDays			= 7;
				public UInt32			MineralExpiryDays		= 7;
			}

			[Serializable]
			public class _Appearance
			{
				public double			RedPrice				= 0.50;
				public double			GreenPrice				= 1.00;
				public double			RedIskLoss				= 100000;
				public double			GreenIskLoss			= 10000;
				public bool				UseAssetQuantities		= true;
				public bool				OverrideAssetsColors	= true;
			}

			[Serializable]
			public class _Options
			{
				public String			DBPath					= "EveDatabase.db";
				public bool				CheckUpdates			= true;
			}

			[Serializable]
			public class _Stats
			{
				public DateTime			LastMineralPricesEdit	= DateTime.FromFileTime(0);
			}

			[Serializable]
			public class _Refining
			{
				public SkillDictionary	Skills					= new SkillDictionary();
				public double			BaseYield				= 0.30;
				public double			TaxMultiplier			= 1.00;
				public double			ImplantBonus			= 0;
			}
		}

		public class V2
		{
			[Serializable]
			public class _EveCentralCom
			{
				public UInt32			RegionID				= (UInt32)EveRegions.Forge;
				public UInt32			SolarID					= 0;
				public UInt32			StationID				= 0;
				public PriceTypes		PriceType				= PriceTypes.SellMedian;
				public UInt32			HistoryDays				= 14;
			}

			[Serializable]
			public class _EveMarketdataCom
			{
				public UInt32           RegionID                = (UInt32)EveRegions.Forge;
				public UInt32           SolarID                 = (UInt32)EveSolars.Jita;
				public UInt32           StationID               = (UInt32)EveStations.Jita_4_4;
				public PriceTypes       PriceType               = PriceTypes.Sell95Pct;
			}

			[Serializable]
			public class _FuzzworkCoUk
			{
				public bool             IsRegion				= false;
				public UInt32           RegionID                = (UInt32)EveRegions.Forge;
				public UInt32           StationID               = (UInt32)EveStations.Jita_4_4;
				public PriceTypes       PriceType               = PriceTypes.SellMedian;
			}

			[Serializable]
			public class _PriceSettings
			{
				public PriceProviders	Provider				= PriceProviders.FuzzworkCoUk;
				public _EveCentralCom	EveCentralCom			= new _EveCentralCom();
				public _EveMarketdataCom EveMarketdataCom       = new _EveMarketdataCom();
				public _FuzzworkCoUk    FuzzworkCoUk            = new _FuzzworkCoUk();
				public UInt32			ExpiryDays				= 7;
			}

			[Serializable]
			public class _PriceLoad
			{
				public _PriceSettings	Minerals				= new _PriceSettings();
				public _PriceSettings	Items					= new _PriceSettings();
			}
		}

		public UInt32				Version					= 2;
        public V1._Options			Options					= new V1._Options();
        public V2._PriceLoad		PriceLoad				= new V2._PriceLoad();
        public double[]				MaterialPrices			= new double[(UInt32)Materials.MaxMaterials];
		public V1._ApiAccess		ApiAccess				= new V1._ApiAccess();
		public V1._Stats			Stats					= new V1._Stats();
		public V1._Refining			Refining				= new V1._Refining();
        public V1._Appearance		Appearance				= new V1._Appearance();
        public V1._UILocations		UILocations				= new V1._UILocations();

		public Settings				Clone()
		{
			return SpecialFNs.Utility.CloneUsingBinary(this);
		}
    }

	public class SettingsStorage
    {
        public const UInt32         LAST_VERSION_FORMAT     = 2;

        public static void TryImportSetting<T>(ref T a_Value, XmlNode a_Xml)
        {
            if (null == a_Xml)
                return;

            if (null == a_Xml.FirstChild)
                return;

            if (null == a_Xml.FirstChild.Value)
                return;

            System.Type targetType = typeof(T);
            String value = a_Xml.FirstChild.Value;

            try
            {
                if (targetType.IsEnum)
                    a_Value = (T)Enum.Parse(typeof(T), value);
                else
                    a_Value = (T)System.Convert.ChangeType(value, typeof(T));
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            {
            }
            catch (InvalidCastException)
            {
            }
            catch (OverflowException)
            {
            }
        }

        public static UInt32 DetectSettingsVersion(String a_Path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(a_Path);

            UInt32 version = 0;
            TryImportSetting(ref version, xmlDoc.SelectSingleNode("/Settings/Version"));

            return version;
        }

        public class V1
        {
			public static Settings		Load(String a_Path)
			{
				SettingsV1 oldSettings	= LoadV1(a_Path);

				Settings result			= new Settings();
				result.Options			= oldSettings.Options;
				result.PriceLoad        = Convert_PriceLoad(oldSettings.PriceLoad);
				result.MaterialPrices   = oldSettings.MaterialPrices;
				result.ApiAccess        = oldSettings.ApiAccess;
				result.Stats            = oldSettings.Stats;
				result.Refining         = oldSettings.Refining;
				result.Appearance       = oldSettings.Appearance;
				result.UILocations      = oldSettings.UILocations;

				return result;
			}

			[Serializable]
			[XmlRoot("Settings")]
			public class SettingsV1
			{
				public UInt32						Version			= 1;
				public Settings.V1._Options         Options			= new Settings.V1._Options();
				public Settings.V1._PriceLoad       PriceLoad		= new Settings.V1._PriceLoad();
				public double[]						MaterialPrices	= new double[(UInt32)Materials.MaxMaterials];
				public Settings.V1._ApiAccess       ApiAccess		= new Settings.V1._ApiAccess();
				public Settings.V1._Stats           Stats			= new Settings.V1._Stats();
				public Settings.V1._Refining        Refining		= new Settings.V1._Refining();
				public Settings.V1._Appearance      Appearance		= new Settings.V1._Appearance();
				public Settings.V1._UILocations     UILocations		= new Settings.V1._UILocations();
			}

			private static SettingsV1					LoadV1(String a_Path)
            {
				using (TextReader stream = new StreamReader(a_Path))
                {
					XmlSerializer serializer = new XmlSerializer(typeof(SettingsV1));
					return (SettingsV1)serializer.Deserialize(stream);
                }
            }

			private static Settings.V2._PriceSettings	Convert_PriceSettings(Settings.V1._PriceSettings a_OldPriceSettings)
			{
				Settings.V2._PriceSettings result	= new Settings.V2._PriceSettings();
				result.EveCentralCom.RegionID		= a_OldPriceSettings.RegionID;
				result.EveCentralCom.SolarID		= a_OldPriceSettings.SolarID;
				result.EveCentralCom.StationID		= a_OldPriceSettings.StationID;
				result.EveCentralCom.PriceType		= a_OldPriceSettings.PriceType;

				return result;
			}

			private static Settings.V2._PriceLoad		Convert_PriceLoad(Settings.V1._PriceLoad a_OldPriceLoad)
			{
				Settings.V2._PriceLoad result   = new Settings.V2._PriceLoad();

				result.Items = Convert_PriceSettings(a_OldPriceLoad.SourceItems);
				result.Items.ExpiryDays = a_OldPriceLoad.ItemsExpiryDays;
				result.Items.EveCentralCom.HistoryDays = a_OldPriceLoad.ItemsHistoryDays;

				result.Minerals = Convert_PriceSettings(a_OldPriceLoad.SourceMinerals);
				result.Minerals.ExpiryDays = a_OldPriceLoad.MineralExpiryDays;
				result.Minerals.EveCentralCom.HistoryDays = a_OldPriceLoad.ItemsHistoryDays;

				return result;
			}
		}

		public class V2
		{
			public static Settings		Load(String a_Path)
			{
				using (TextReader stream = new StreamReader(a_Path))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Settings));
					return (Settings)serializer.Deserialize(stream);
				}
			}

			public static void			Save(String a_Path, Settings a_Settings)
			{
				using (TextWriter stream = new StreamWriter(a_Path))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Settings));
					serializer.Serialize(stream, a_Settings);
				}
			}
		}

		private static String GetSettingsPath()
        {
            return "settings.xml";
        }

        public static Settings Load()
        {
            try
            {
                String settingsPath = GetSettingsPath();
                if (!File.Exists(settingsPath))
                    return new Settings();

                UInt32 version = DetectSettingsVersion(settingsPath);
				Settings result = null;
                switch (version)
                {
					case 1:
						result = V1.Load(settingsPath);
						break;
					case 2:
						result = V2.Load(settingsPath);
						break;
					case 0:
					default:
						result = new Settings();
						break;
                }

				Debug.Assert(result.Version == LAST_VERSION_FORMAT);
				return result;
			}
            catch (System.Exception a_Exception)
            {
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
            }

            return new Settings();
        }

        private static void BackupOldSettings()
        {
            try
            {
                String settingsPath = GetSettingsPath();
                if (!File.Exists(settingsPath))
                    return;

                UInt32 oldVersion = DetectSettingsVersion(settingsPath);
                if (oldVersion == LAST_VERSION_FORMAT)
                    return;

                String backupPath = settingsPath + String.Format(".old.{0}", oldVersion);
                File.Move(settingsPath, backupPath);
            }
            catch (System.Exception)
            {

            }
        }

        public static void Save(Settings a_Settings)
        {
            BackupOldSettings();
            V2.Save(GetSettingsPath(), a_Settings);
        }
    }
}