using System;
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

				public bool Matches(_PriceSettings a_Rhs)
				{
					return
						(Provider   == a_Rhs.Provider) &&
						(RegionID   == a_Rhs.RegionID) &&
						(SolarID    == a_Rhs.SolarID) &&
						(StationID  == a_Rhs.StationID) &&
						(PriceType  == a_Rhs.PriceType);
				}

				public String GetHintText(EveDatabase a_Database)
				{
					return PriceType.ToString() + " - " + a_Database.GetLocationName(RegionID, SolarID, StationID);
				}
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

        public UInt32				Version					= 1;
        public V1._Options			Options					= new V1._Options();
        public V1._PriceLoad		PriceLoad				= new V1._PriceLoad();
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

    class SettingsStorage
    {
        public const UInt32         LAST_VERSION_FORMAT     = 1;

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
            public static Settings Load(String a_Path)
            {
                using (TextReader stream = new StreamReader(a_Path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    return (Settings)serializer.Deserialize(stream);
                }
            }

            public static void Save(String a_Path, Settings a_Settings)
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
            V1.Save(GetSettingsPath(), a_Settings);
        }
    }
}