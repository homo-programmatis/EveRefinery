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
            public PriceSettings    SourceMinerals          = new PriceSettings{Provider = PriceProviders.Fuzzwork, RegionID = (UInt32)EveRegions.Forge, PriceType = PriceTypes.SellMedian};
            public PriceSettings    SourceItems             = new PriceSettings{Provider = PriceProviders.Fuzzwork, RegionID = (UInt32)EveRegions.Forge, PriceType = PriceTypes.SellMedian};
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

        public class V0
        {
            private static void DoPriceSettings(ref PriceSettings a_Result, XmlNode a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                a_Result.Provider = PriceProviders.EveCentral;
                TryImportSetting(ref a_Result.RegionID,     a_Xml.SelectSingleNode("x:RegionID", a_Namespace));
                TryImportSetting(ref a_Result.SolarID,      a_Xml.SelectSingleNode("x:SolarID", a_Namespace));
                TryImportSetting(ref a_Result.StationID,    a_Xml.SelectSingleNode("x:StationID", a_Namespace));
                TryImportSetting(ref a_Result.PriceType,    a_Xml.SelectSingleNode("x:PriceType", a_Namespace));
            }

            private static void DoOptions(Settings a_Settings, XmlNode a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                TryImportSetting(ref a_Settings.Options.DBPath,                 a_Xml.SelectSingleNode("x:DBPath", a_Namespace));
                TryImportSetting(ref a_Settings.Options.CheckUpdates,           a_Xml.SelectSingleNode("x:CheckUpdates", a_Namespace));

                DoPriceSettings(ref a_Settings.PriceLoad.SourceMinerals,        a_Xml.SelectSingleNode("x:PriceSettings_Minerals", a_Namespace), a_Namespace);
                DoPriceSettings(ref a_Settings.PriceLoad.SourceItems,           a_Xml.SelectSingleNode("x:PriceSettings_Items", a_Namespace), a_Namespace);
                TryImportSetting(ref a_Settings.PriceLoad.ItemsHistoryDays,     a_Xml.SelectSingleNode("x:PriceHistoryDays", a_Namespace));
                TryImportSetting(ref a_Settings.PriceLoad.ItemsExpiryDays,      a_Xml.SelectSingleNode("x:PriceExpiryDays", a_Namespace));
                TryImportSetting(ref a_Settings.PriceLoad.MineralExpiryDays,    a_Xml.SelectSingleNode("x:MineralPriceExpiryDays", a_Namespace));

                TryImportSetting(ref a_Settings.Appearance.RedPrice,              a_Xml.SelectSingleNode("x:RedPrice", a_Namespace));
                TryImportSetting(ref a_Settings.Appearance.GreenPrice,            a_Xml.SelectSingleNode("x:GreenPrice", a_Namespace));
                TryImportSetting(ref a_Settings.Appearance.UseAssetQuantities,    a_Xml.SelectSingleNode("x:UseAssetQuantities", a_Namespace));
                TryImportSetting(ref a_Settings.Appearance.OverrideAssetsColors,  a_Xml.SelectSingleNode("x:OverrideAssetsColors", a_Namespace));
                TryImportSetting(ref a_Settings.Appearance.RedIskLoss,            a_Xml.SelectSingleNode("x:RedIskLoss", a_Namespace));
                TryImportSetting(ref a_Settings.Appearance.GreenIskLoss,          a_Xml.SelectSingleNode("x:GreenIskLoss", a_Namespace));
            }

            private static void DoApiCharacters(Settings a_Settings, XmlNodeList a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                foreach (XmlNode currNode in a_Xml)
                {
                    Settings._ApiAccess.Char currItem = new Settings._ApiAccess.Char();
                    TryImportSetting(ref currItem.KeyID,            currNode.SelectSingleNode("x:KeyID", a_Namespace));
                    TryImportSetting(ref currItem.CharacterID,      currNode.SelectSingleNode("x:CharacterID", a_Namespace));
                    TryImportSetting(ref currItem.CharacterName,    currNode.SelectSingleNode("x:CharacterName", a_Namespace));
                    TryImportSetting(ref currItem.CorporationName,  currNode.SelectSingleNode("x:CorporationName", a_Namespace));
                    a_Settings.ApiAccess.Chars.Add(currItem);
                }
            }

            private static void DoApiKeys(Settings a_Settings, XmlNodeList a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                foreach (XmlNode currNode in a_Xml)
                {
                    Settings._ApiAccess.Key currItem = new Settings._ApiAccess.Key();
                    TryImportSetting(ref currItem.KeyID,            currNode.SelectSingleNode("x:KeyID", a_Namespace));
                    TryImportSetting(ref currItem.Verification,     currNode.SelectSingleNode("x:Verification", a_Namespace));
                    a_Settings.ApiAccess.Keys.Add(currItem);
                }
            }

            private static void DoFormLocation(Settings a_Settings, XmlNode a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                TryImportSetting(ref a_Settings.UILocations.MainWindow.X0,      a_Xml.SelectSingleNode("x:FormLocation/x:X", a_Namespace));
                TryImportSetting(ref a_Settings.UILocations.MainWindow.Y0,      a_Xml.SelectSingleNode("x:FormLocation/x:Y", a_Namespace));
                TryImportSetting(ref a_Settings.UILocations.MainWindow.CX,      a_Xml.SelectSingleNode("x:FormSize/x:Width", a_Namespace));
                TryImportSetting(ref a_Settings.UILocations.MainWindow.CY,      a_Xml.SelectSingleNode("x:FormSize/x:Height", a_Namespace));
            }

            private static void DoViewColumns(Settings a_Settings, XmlNodeList a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                foreach (XmlNode currNode in a_Xml)
                {
                    Settings._UILocations.Column currItem = new Settings._UILocations.Column();
                    TryImportSetting(ref currItem.Name,             currNode.SelectSingleNode("x:Name", a_Namespace));
                    TryImportSetting(ref currItem.Index,            currNode.SelectSingleNode("x:Index", a_Namespace));
                    TryImportSetting(ref currItem.Visible,          currNode.SelectSingleNode("x:Visible", a_Namespace));
                    TryImportSetting(ref currItem.Width,            currNode.SelectSingleNode("x:Width", a_Namespace));
                    a_Settings.UILocations.Columns.Add(currItem);
                }
            }

            private static void DoToolbars(Settings a_Settings, XmlNodeList a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                foreach (XmlNode currNode in a_Xml)
                {
                    Settings._UILocations.Toolbar currItem = new Settings._UILocations.Toolbar();
                    TryImportSetting(ref currItem.Name,             currNode.SelectSingleNode("x:Name", a_Namespace));
                    TryImportSetting(ref currItem.Location.X0,      currNode.SelectSingleNode("x:Location/x:X", a_Namespace));
                    TryImportSetting(ref currItem.Location.Y0,      currNode.SelectSingleNode("x:Location/x:Y", a_Namespace));
                    TryImportSetting(ref currItem.Location.CX,      currNode.SelectSingleNode("x:Size/x:Width", a_Namespace));
                    TryImportSetting(ref currItem.Location.CY,      currNode.SelectSingleNode("x:Size/x:Height", a_Namespace));
                    TryImportSetting(ref currItem.Panel,            currNode.SelectSingleNode("x:Panel", a_Namespace));
                    a_Settings.UILocations.Toolbars.Add(currItem);
                }
            }

            private static void DoSettings(Settings a_Settings, XmlNode a_Xml, XmlNamespaceManager a_Namespace)
            {
                if (null == a_Xml)
                    return;

                DoOptions(a_Settings,       a_Xml.SelectSingleNode("x:Options", a_Namespace), a_Namespace);
                DoApiCharacters(a_Settings, a_Xml.SelectNodes("x:ApiCharacters", a_Namespace), a_Namespace);
                DoApiKeys(a_Settings,       a_Xml.SelectNodes("x:ApiKeys", a_Namespace), a_Namespace);
                DoFormLocation(a_Settings,  a_Xml.SelectSingleNode("x:Locations", a_Namespace), a_Namespace);
                DoViewColumns(a_Settings,   a_Xml.SelectNodes("x:ViewColumns", a_Namespace), a_Namespace);
                DoToolbars(a_Settings,      a_Xml.SelectNodes("x:Toolbars", a_Namespace), a_Namespace);
            }

            public static Settings Load(String a_Path)
            {
                Settings result = new Settings();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(a_Path);

                XmlNamespaceManager xmlNamespace = new XmlNamespaceManager(xmlDocument.NameTable);
                xmlNamespace.AddNamespace("x", "http://tempuri.org/Settings.xsd");

                XmlNode xmlSettingsNode = xmlDocument.SelectSingleNode("/x:Settings", xmlNamespace);
                DoSettings(result, xmlSettingsNode, xmlNamespace);

                return result;
            }
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
                case 0:
                    return V0.Load(settingsPath);
                case 1:
                    return V1.Load(settingsPath);
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