using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using SkillDictionary = SpecialFNs.SerializableDictionary<System.UInt32, System.UInt32>;

namespace EveRefinery
{
	[Serializable]
    public class Settings
    {
		[Serializable]
        public class _ApiAccess
        {
			[Serializable]
            public class Key
            {
                public UInt32       KeyID;
                public string       Verification;
            }

			[Serializable]
            public class Char
            {
                public UInt32       KeyID;
                public UInt32       CharacterID;
                public String       CharacterName;
                public String       CorporationName;
            }

            public List<Key>        Keys                    = new List<Key>();
            public List<Char>       Chars                   = new List<Char>();
        }

		[Serializable]
        public class _UILocations
        {
			[Serializable]
            public struct Rect
            {
                public Int32        X0;
                public Int32        Y0;
                public Int32        CX;
                public Int32        CY;
            }

			[Serializable]
            public class Column
            {
                public String       Name;
                public UInt32       Index;
                public bool         Visible;
                public UInt32       Width;
            }

			[Serializable]
            public class Toolbar
            {
                public String       Name;
                public Rect         Location;
                public UInt32       Panel;
            }

            public Rect             MainWindow;
            public List<Column>     Columns                 = new List<Column>();
            public List<Toolbar>    Toolbars                = new List<Toolbar>();
        }

		[Serializable]
        public class _PriceLoad
        {
            public PriceSettings    SourceMinerals          = new PriceSettings{Provider = PriceProviders.EveCentral, RegionID = (UInt32)EveRegions.Forge, PriceType = PriceTypes.SellMedian};
            public PriceSettings    SourceItems             = new PriceSettings{Provider = PriceProviders.EveCentral, RegionID = (UInt32)EveRegions.Forge, PriceType = PriceTypes.SellMedian};
            public UInt32           ItemsHistoryDays        = 14;
            public UInt32           ItemsExpiryDays         = 7;
            public UInt32           MineralExpiryDays       = 7;
        }

		[Serializable]
        public class _Appearance
        {
            public double           RedPrice                = 0.50;
            public double           GreenPrice              = 1.00;
            public double           RedIskLoss              = 100000;
            public double           GreenIskLoss            = 10000;
            public bool             UseAssetQuantities      = true;
            public bool             OverrideAssetsColors    = true;
        }

		[Serializable]
        public class _Options
        {
            public String           DBPath                  = "EveDatabase.db";
            public bool             CheckUpdates            = true;
        }

		[Serializable]
        public class _Stats
        {
            public DateTime         LastMineralPricesEdit   = DateTime.FromFileTime(0);
        }

		[Serializable]
        public class _Refining
        {
			public SkillDictionary	Skills					= new SkillDictionary();
			public double			BaseYield				= 0.30;
			public double			TaxMultiplier			= 1.00;
			public double			ImplantBonus			= 0;
		}

        public UInt32               Version                 = 1;
        public _Options             Options                 = new _Options();
        public _PriceLoad           PriceLoad               = new _PriceLoad();
        public double[]             MaterialPrices          = new double[(UInt32)Materials.MaxMaterials];
		public _ApiAccess           ApiAccess               = new _ApiAccess();
		public _Stats               Stats                   = new _Stats();
		public _Refining			Refining				= new _Refining();
        public _Appearance          Appearance              = new _Appearance();
        public _UILocations         UILocations             = new _UILocations();

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
                switch (version)
                {
                case 1:
                    return V1.Load(settingsPath);
				case 0:
                default:
                    return new Settings();
                }
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