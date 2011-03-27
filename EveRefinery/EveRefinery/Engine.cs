using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using SpecialFNs;
using System.Xml;
using System.Diagnostics;
using System.Net;
using System.Globalization;

namespace EveRefinery
{
	public enum PriceTypes
	{
		AllAvg,
		AllMax,
		AllMin,
		AllMedian,
		BuyAvg,
		BuyMax,
		BuyMin,
		BuyMedian,
		SellAvg,
		SellMax,
		SellMin,
		SellMedian,
		
		MaxPriceTypes,
	}

	public class ItemPrice
	{
		public static double	Empty			= -1;
		public static double	Unknown			= -2;
		public static double	NonMarket		= -3;
		public static double	Outdated		= -4;

		public double			RefinedCost;
		public double			MarketPrice;
		public double			PriceDelta;

		public static string FormatPrice(double a_Price, bool a_IsPatial)
		{
			if (a_Price == ItemPrice.Empty)
				return "";
			else if (a_Price == ItemPrice.Unknown)
				return "Unknown";
			else if (a_Price == ItemPrice.NonMarket)
				return "Non-Market";
			else if (a_Price == ItemPrice.Outdated)
				return "Outdated";
				
			if (a_IsPatial)
				return String.Format("Unknown (known part is {0:s})", Engine.FormatDouble(a_Price));

			return Engine.FormatDouble(a_Price);
		}
		
		public static bool IsValidPrice(double a_Price)
		{
			return (a_Price > 0);
		}
	}

	// Optimization: Cache popular settings to counter insane DataTable overhead (saves 8 sec per sort request)	
	public class OptionsCache
	{
		public double		RefineryTax;
		public double		RefineryEfficiency;
		public UInt32		PriceType;
		public UInt32		PriceExpiryDays;
	}

	public class Engine
	{
		public double[]		m_MaterialPrices = new double[(UInt32)Materials.MaxMaterials];
		public Settings		m_Settings;

		public OptionsCache	m_OptionsCache = new OptionsCache();

		static CultureInfo	m_DoubleFormat = (CultureInfo)CultureInfo.InvariantCulture.Clone();

		public Engine()
		{
			m_DoubleFormat.NumberFormat.NumberGroupSeparator = " ";

			LoadSettings();
		}
		
		void LoadSettingsXml()
		{
			m_Settings = new Settings();
			
			try
			{
				m_Settings.ReadXml("Settings.xml", System.Data.XmlReadMode.IgnoreSchema);
			}
			catch (System.Exception a_Exception)
			{
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
			}
		}
		
		private void LoadSettings_TestOptions()
		{
			if (m_Settings.Options.Count == 0)
				m_Settings.Options.Rows.Add();

			Settings.OptionsRow options = m_Settings.Options[0];
			if (options.IsDBPathNull())
				options.DBPath = "EveDatabase.db";
				
			if (options.IsPriceTypeNull())
				options.PriceType = (UInt32)PriceTypes.SellMedian;
				
			if (options.IsPricesRegionNull())
				options.PricesRegion = (UInt32)EveRegions.Forge;
				
			if (options.IsRedPriceNull())
				options.RedPrice = 0.50;
				
			if (options.IsGreenPriceNull())
				options.GreenPrice = 1.00;
				
			if (options.IsCheckUpdatesNull())
				options.CheckUpdates = true;
				
			if (options.IsRefineryEfficiencyNull())
				options.RefineryEfficiency = 1.00;

			if (options.RefineryEfficiency < 0)
				options.RefineryEfficiency = 0;

			if (options.RefineryEfficiency > 1.00)
				options.RefineryEfficiency = 1.00;
				
			if (options.IsRefineryTaxNull())
				options.RefineryTax = 0.00;

			if (options.RefineryTax < 0)
				options.RefineryTax = 0;

			if (options.RefineryTax > 1.00)
				options.RefineryTax = 1.00;
				
			if (options.IsUseAssetQuantitiesNull())
				options.UseAssetQuantities = true;
				
			if (options.IsOverrideAssetsColorsNull())
				options.OverrideAssetsColors = true;
				
			if (options.IsGreenIskLossNull())
				options.GreenIskLoss = 10000;
				
			if (options.IsRedIskLossNull())
				options.RedIskLoss = 100000;
				
			if (options.IsPriceHistoryDaysNull())
				options.PriceHistoryDays = 14;

			if (options.IsPriceExpiryDaysNull())
				options.PriceExpiryDays = 7;

			if (options.IsMineralPriceExpiryDaysNull())
				options.MineralPriceExpiryDays = 7;

			if (options.IsMineralPricesRegionNull())
				options.MineralPricesRegion = (UInt32)EveRegions.Forge;

			if (options.IsMineralPricesTypeNull())
				options.MineralPricesType = (UInt32)PriceTypes.SellMedian;
		}

		private void LoadSettings_TestPrices()
		{
			if (m_Settings.Prices.Count == 0)
				m_Settings.Prices.Rows.Add();

			Settings.PricesRow prices = m_Settings.Prices[0];

			if (prices.IsTritaniumNull())
				prices.Tritanium = 0;

			if (prices.IsPyeriteNull())
				prices.Pyerite = 0;

			if (prices.IsTritaniumNull())
				prices.Tritanium = 0;

			if (prices.IsMexallonNull())
				prices.Mexallon = 0;

			if (prices.IsIsogenNull())
				prices.Isogen = 0;

			if (prices.IsNoxciumNull())
				prices.Noxcium = 0;

			if (prices.IsZydrineNull())
				prices.Zydrine = 0;

			if (prices.IsMegacyteNull())
				prices.Megacyte = 0;

			if (prices.IsMorphiteNull())
				prices.Morphite = 0;
		}
		
		private void LoadSettings_TestAccounts()
		{
			// Eliminate everything that contains null's
			for (int i = m_Settings.Accounts.Count - 1; i >= 0; i--)
			{
				Settings.AccountsRow currAccount = m_Settings.Accounts[i];
				if (currAccount.IsFullKeyNull())
					m_Settings.Accounts.Rows.RemoveAt(i);
			}		
		}

		private void LoadSettings_TestCharacters()
		{
			// Eliminate everything that contains null's
			for (int i = m_Settings.Characters.Count - 1; i >= 0; i--)
			{
				Settings.CharactersRow currCharacter = m_Settings.Characters[i];
				if (currCharacter.IsCharacterNameNull() ||
					currCharacter.IsUserIDNull())
				{
					m_Settings.Characters.Rows.RemoveAt(i);
					continue;
				}
				
				if (currCharacter.IsCorporationNameNull())
					currCharacter.CorporationName = "(Unknown)";
			}			
		
			// Eliminate orphaned characters
			for (int i = m_Settings.Characters.Count - 1; i >= 0; i--)
			{
				Settings.CharactersRow currCharacter = m_Settings.Characters[i];
				
				if (null == m_Settings.Accounts.FindByUserID(currCharacter.UserID))
					m_Settings.Characters.Rows.RemoveAt(i);
			}
		}
		
		protected void LoadSettings_TestViewColumns()
		{
			for (int i = m_Settings.ViewColumns.Rows.Count - 1; i >= 0; i--)
			{
				Settings.ViewColumnsRow currRow = m_Settings.ViewColumns[i];
			
				if (currRow.IsIndexNull() ||
					currRow.IsVisibleNull() ||
					currRow.IsWidthNull())
				{
					m_Settings.ViewColumns.Rows.RemoveAt(i);
				}
			}
		}

		protected void LoadSettings_TestLocations()
		{
			if (m_Settings.Locations.Count == 0)
				m_Settings.Locations.Rows.Add();

			Settings.LocationsRow locations = m_Settings.Locations[0];
			
			Int32 screenCX = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			Int32 screenCY = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
			
			if (locations.IsFormSizeNull() ||
				locations.FormSize.Width < 100 ||
				locations.FormSize.Width > screenCX ||
				locations.FormSize.Height < 100 ||
				locations.FormSize.Height > screenCY
				)
			{
				locations.FormSize = new Size(918, 350);
			}
			
			if (locations.IsFormLocationNull() ||
				locations.FormLocation.X < 0 ||
				locations.FormLocation.X > screenCX - 50 ||
				locations.FormLocation.Y < 0 ||
				locations.FormLocation.Y >= screenCY - 50
				)
			{
				int x = (screenCX - locations.FormSize.Width) / 2;
				int y = (screenCY - locations.FormSize.Height) / 2;
				locations.FormLocation = new Point(x, y);
			}
		}

		protected void LoadSettings_TestToolbars()
		{
			for (int i = m_Settings.Toolbars.Rows.Count - 1; i >= 0; i--)
			{
				Settings.ToolbarsRow currRow = m_Settings.Toolbars[i];
				
				if (currRow.IsLocationNull() ||
					currRow.IsSizeNull() ||
					currRow.IsPanelNull())
				{
					m_Settings.Toolbars.Rows.RemoveAt(i);
				}
			}
		}

		private void LoadSettings_TestStats()
		{
			if (m_Settings.Stats.Count == 0)
				m_Settings.Stats.Rows.Add();

			Settings.StatsRow stats = m_Settings.Stats[0];

			if (stats.IsLastMineralPricesEditNull())
				stats.LastMineralPricesEdit = DateTime.FromFileTime(0);
		}
		
		public void UpdateSettingsCache()
		{
			m_OptionsCache.PriceType			= m_Settings.Options[0].PriceType;
			m_OptionsCache.RefineryEfficiency	= m_Settings.Options[0].RefineryEfficiency;
			m_OptionsCache.RefineryTax			= m_Settings.Options[0].RefineryTax;
			m_OptionsCache.PriceExpiryDays		= m_Settings.Options[0].PriceExpiryDays;
		}
		
		protected void OnUpdateOptionsRow(object sender, Settings.OptionsRowChangeEvent e)
		{
			UpdateSettingsCache();
		}

		public void LoadSettings()
		{
			LoadSettingsXml();
			LoadSettings_TestOptions();
			LoadSettings_TestPrices();
			LoadSettings_TestAccounts();
			LoadSettings_TestCharacters();	// Must go AFTER accounts
			LoadSettings_TestLocations();
			LoadSettings_TestViewColumns();
			LoadSettings_TestToolbars();
			LoadSettings_TestStats();
			
			UpdateSettingsCache();
			m_Settings.Options.OptionsRowChanged += OnUpdateOptionsRow;

			m_MaterialPrices[(UInt32)Materials.Tritanium]	= m_Settings.Prices[0].Tritanium;
			m_MaterialPrices[(UInt32)Materials.Pyerite]		= m_Settings.Prices[0].Pyerite;
			m_MaterialPrices[(UInt32)Materials.Mexallon]	= m_Settings.Prices[0].Mexallon;
			m_MaterialPrices[(UInt32)Materials.Isogen]		= m_Settings.Prices[0].Isogen;
			m_MaterialPrices[(UInt32)Materials.Noxcium]		= m_Settings.Prices[0].Noxcium;
			m_MaterialPrices[(UInt32)Materials.Zydrine]		= m_Settings.Prices[0].Zydrine;
			m_MaterialPrices[(UInt32)Materials.Megacyte]	= m_Settings.Prices[0].Megacyte;
			m_MaterialPrices[(UInt32)Materials.Morphite]	= m_Settings.Prices[0].Morphite;
		}
		
		public void SaveSettings()
		{
			m_Settings.Prices[0].Tritanium	= m_MaterialPrices[(UInt32)Materials.Tritanium];
			m_Settings.Prices[0].Pyerite	= m_MaterialPrices[(UInt32)Materials.Pyerite];
			m_Settings.Prices[0].Mexallon	= m_MaterialPrices[(UInt32)Materials.Mexallon];
			m_Settings.Prices[0].Isogen		= m_MaterialPrices[(UInt32)Materials.Isogen];
			m_Settings.Prices[0].Noxcium	= m_MaterialPrices[(UInt32)Materials.Noxcium];
			m_Settings.Prices[0].Zydrine	= m_MaterialPrices[(UInt32)Materials.Zydrine];
			m_Settings.Prices[0].Megacyte	= m_MaterialPrices[(UInt32)Materials.Megacyte];
			m_Settings.Prices[0].Morphite	= m_MaterialPrices[(UInt32)Materials.Morphite];
			
			try
			{
				m_Settings.WriteXml("Settings.xml", System.Data.XmlWriteMode.IgnoreSchema);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Failed to save settings:\n" + a_Exception.Message);
			}
		}
		
		public Settings.AccountsRow GetCharacterAccount(UInt32 a_CharacterID)
		{
			Settings.CharactersRow character = m_Settings.Characters.FindByCharacterID(a_CharacterID);
			if (null == character)
				return null;
			
			Settings.AccountsRow account = m_Settings.Accounts.FindByUserID(character.UserID);
			return account;
		}

		public double GetRefineQuota(double a_OriginalAmount)
		{
			double result = a_OriginalAmount * m_OptionsCache.RefineryEfficiency;
			result *= (1 - m_OptionsCache.RefineryTax);
			return result;
		}

		/// <summary>
		/// Gets amount of material after perfect refining
		/// Refines incomplete batches too
		/// </summary>
		public double GetItemRefinedMaterial(ItemRecord a_Item, UInt32 a_Quantity, Materials a_Material)
		{
			return (a_Quantity * a_Item.MaterialAmount[(UInt32)a_Material]) / a_Item.BatchSize;
		}

		/// <summary>
		/// Gets amount of material after refining with your efficiency and skill
		/// Refines incomplete batches too
		/// </summary>
		public double GetItemRefinedQuota(ItemRecord a_Item, UInt32 a_Quantity, Materials a_Material)
		{
			return GetRefineQuota(GetItemRefinedMaterial(a_Item, a_Quantity, a_Material));
		}

		public double GetItemRefinedPrice(ItemRecord a_Item, UInt32 a_Quantity)
		{
			double result = 0;

			for (UInt32 i = 0; i < (UInt32)Materials.MaxMaterials; i++)
			{
				result += (GetRefineQuota(a_Quantity * a_Item.MaterialAmount[i]) * m_MaterialPrices[i]);
			}

			return result / a_Item.BatchSize;
		}

		public Color GetPriceColor(double a_PriceRefine, double a_PriceSell, bool a_QuantityOK)
		{
			double colorGreenPercent = 0;
		
			if (a_QuantityOK && m_Settings.Options[0].OverrideAssetsColors)
			{
				double losses = a_PriceSell - a_PriceRefine;
			
				if (losses <= m_Settings.Options[0].GreenIskLoss)
					return Color.FromArgb(0, 255, 0);
					
				if (losses >= m_Settings.Options[0].RedIskLoss)
					return Color.FromArgb(255, 0, 0);

				colorGreenPercent = (m_Settings.Options[0].RedIskLoss - losses) / (m_Settings.Options[0].RedIskLoss - m_Settings.Options[0].GreenIskLoss);
			}
			else
			{
				double priceRangeLo = a_PriceSell * m_Settings.Options[0].RedPrice;
				double priceRangeHi = a_PriceSell * m_Settings.Options[0].GreenPrice;
				if (priceRangeLo == priceRangeHi)
					return Color.FromArgb(255, 255, 255);

				if (a_PriceRefine < priceRangeLo)
					return Color.FromArgb(255, 0, 0);

				if (priceRangeHi < a_PriceRefine)
					return Color.FromArgb(0, 255, 0);

				colorGreenPercent = (a_PriceRefine - priceRangeLo) / (priceRangeHi - priceRangeLo);
			}

			Int32 colorStep = Convert.ToInt32(colorGreenPercent * (2*256 - 1));

			if (colorStep < 256)
				return Color.FromArgb(255, colorStep, 0);
			
			colorStep -= 256;
			return Color.FromArgb(255 - colorStep, 255, 0);
		}
		
		public static string FormatDouble(double a_Value)
		{
			return String.Format(m_DoubleFormat, "{0:0,0.00}", a_Value);
		}

		/// <summary>
		/// Gets various prices and colors for an item
		/// </summary>
		/// <param name="a_Item">Item in question</param>
		/// <param name="a_Quantity">Quantity of the item</param>
		/// <param name="a_QuantityOK">true means that quantity is valid (ie assets mode + use quantities enabled)</param>
		/// <returns>Prices and color</returns>
		public ItemPrice GetItemPrices(ItemRecord a_Item, UInt32 a_Quantity)
		{
			ItemPrice result = new ItemPrice();

			result.RefinedCost		= GetItemRefinedPrice(a_Item, a_Quantity);
			result.MarketPrice		= a_Quantity * a_Item.Prices[m_OptionsCache.PriceType];

			bool isError = false;

			if (0 == a_Item.MarketGroupID)
			{
				result.MarketPrice	= ItemPrice.NonMarket;
				isError = true;
			}
			else if (0 == result.MarketPrice)
			{
				result.MarketPrice	= ItemPrice.Unknown;
				isError = true;
			}
			else if (!a_Item.IsPricesOk(m_OptionsCache.PriceExpiryDays))
			{
				result.MarketPrice	= ItemPrice.Outdated;
				isError = true;
			}
			else
				result.PriceDelta	= result.RefinedCost - result.MarketPrice;

			if (isError)
				result.PriceDelta	= ItemPrice.Empty;

			return result;
		}
		
		public static void ShowXmlRequestErrors(string a_ErrorHeader, XmlNodeList a_ErrorNodes)
		{
			string message = a_ErrorHeader;
			foreach (XmlNode errorNode in a_ErrorNodes)
			{
                string errorHint = "";
                if (errorNode.InnerText == "Current security level not high enough.")
                    errorHint = " (Did you provide Limited API key instead of Full one?)";

				message += (errorNode.InnerText + errorHint + "\n");
			}

			ErrorMessageBox.Show(message);
		}
		
		public static string GetPriceTypeName(PriceTypes a_PriceType)
		{
			Debug.Assert((UInt32)PriceTypes.MaxPriceTypes == 12);
			string result = a_PriceType.ToString();
		
			switch (a_PriceType)
			{
				case PriceTypes.AllAvg:
					result += "\tAverage price of both sell and buy orders";
					break;
				case PriceTypes.AllMax:
					result += "\tMaximum price in both sell and buy orders";
					break;
				case PriceTypes.AllMin:
					result += "\tMinimum price in both sell and buy orders";
					break;
				case PriceTypes.AllMedian:
					result += "\tPopular price in both sell and buy orders";
					break;
				case PriceTypes.BuyAvg:
					result += "\tAverage price people are buying for";
					break;
				case PriceTypes.BuyMax:
					result += "\tMaximum price people are buying for";
					break;
				case PriceTypes.BuyMin:
					result += "\tMinimum price people are buying for";
					break;
				case PriceTypes.BuyMedian:
					result += "\tPopular price people are buying for";
					break;
				case PriceTypes.SellAvg:
					result += "\tAverage price people are selling for";
					break;
				case PriceTypes.SellMax:
					result += "\tMaximum price people are selling for";
					break;
				case PriceTypes.SellMin:
					result += "\tMinimum price people are selling for";
					break;
				case PriceTypes.SellMedian:
					result += "\tPopular price people are selling for";
					break;
			}

			return result;
		}

		public static void DrawPriceTypeItem(object a_Sender, System.Windows.Forms.DrawItemEventArgs a_Args)
		{
			// Fill in the background
			a_Args.DrawBackground();
			if (a_Args.Index < 0)
				return;
				
			ComboBox comboBox = null;
			if (a_Sender is ToolStripComboBox)
				comboBox = ((ToolStripComboBox)a_Sender).ComboBox;
			else if (a_Sender is ComboBox)
				comboBox = (ComboBox)a_Sender;
			else
			{
				Debug.Assert(false, "Invalid a_Sender type");
				return;
			}
			
			string itemText = comboBox.Items[a_Args.Index].ToString();
			string[] substrings = itemText.Split(new Char[]{'\t'}, 2);

			Rectangle rect1 = Rectangle.FromLTRB(a_Args.Bounds.Left, a_Args.Bounds.Top, a_Args.Bounds.Left + comboBox.Width, a_Args.Bounds.Bottom);
			Rectangle rect2 = Rectangle.FromLTRB(rect1.Right, a_Args.Bounds.Top, a_Args.Bounds.Right, a_Args.Bounds.Bottom);

			a_Args.Graphics.DrawString(substrings[0], a_Args.Font, new SolidBrush(a_Args.ForeColor), rect1);
			
			if (substrings.Count() > 1)
				a_Args.Graphics.DrawString(substrings[1], a_Args.Font, new SolidBrush(a_Args.ForeColor), rect2);
		}
		
		public static XmlDocument LoadXmlWithUserAgent(string a_Url)
		{
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(a_Url);
			httpRequest.UserAgent = "EveRefinery";

			using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(httpResponse.GetResponseStream());
				return xmlDocument;
			}
		}
	}
}
