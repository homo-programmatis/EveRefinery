using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using SpecialFNs;

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

		Buy95Pct,
		Sell95Pct,

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

		public static string FormatPrice(double a_Price)
		{
			if (a_Price == ItemPrice.Empty)
				return "";
			else if (a_Price == ItemPrice.Unknown)
				return "Unknown";
			else if (a_Price == ItemPrice.NonMarket)
				return "Non-Market";
			else if (a_Price == ItemPrice.Outdated)
				return "Outdated";
				
			return Engine.FormatDouble(a_Price);
		}
		
		public static bool IsValidPrice(double a_Price)
		{
			return (a_Price > 0);
		}
	}

	public class Engine
	{
		public Settings		m_Settings;

		static CultureInfo	m_DoubleFormat = (CultureInfo)CultureInfo.InvariantCulture.Clone();

		public Engine()
		{
			m_DoubleFormat.NumberFormat.NumberGroupSeparator = " ";

			LoadSettings();
		}

        private bool IsCharacterOrphaned(Settings.V1._ApiChar a_Character)
        {
            return !m_Settings.ApiAccess.Keys.Exists(a => (a.KeyID == a_Character.KeyID));
        }
		
		private void LoadSettings_TestCharacters()
		{
            m_Settings.ApiAccess.Chars.RemoveAll(IsCharacterOrphaned);
		}
		
		protected void LoadSettings_TestLocations()
		{
			Int32 screenCX = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
			Int32 screenCY = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
			
			if (m_Settings.UILocations.MainWindow.CX < 100 ||
				m_Settings.UILocations.MainWindow.CX > screenCX ||
				m_Settings.UILocations.MainWindow.CY < 100 ||
				m_Settings.UILocations.MainWindow.CY > screenCY
				)
			{
                m_Settings.UILocations.MainWindow.CX = 920;
                m_Settings.UILocations.MainWindow.CY = 350;
			}
			
			if (m_Settings.UILocations.MainWindow.X0 < 0 ||
				m_Settings.UILocations.MainWindow.X0 > screenCX - 50 ||
				m_Settings.UILocations.MainWindow.Y0 < 0 ||
				m_Settings.UILocations.MainWindow.Y0 >= screenCY - 50
				)
			{
				m_Settings.UILocations.MainWindow.X0 = (screenCX - m_Settings.UILocations.MainWindow.CX) / 2;
                m_Settings.UILocations.MainWindow.Y0 = (screenCY - m_Settings.UILocations.MainWindow.CY) / 2;
			}
		}

		public void LoadSettings()
		{
            m_Settings = SettingsStorage.Load();

			LoadSettings_TestCharacters();
			LoadSettings_TestLocations();
		}
		
		public void SaveSettings()
		{
			try
			{
				SettingsStorage.Save(m_Settings);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Failed to save settings:\n" + a_Exception.Message);
			}
		}

		public static Settings.V1._ApiKey GetCharacterKey(Settings a_Settings, UInt32 a_CharacterID)
		{
            Settings.V1._ApiChar character = a_Settings.ApiAccess.Chars.FirstOrDefault(a => a.CharacterID == a_CharacterID);
			if (null == character)
				return null;

            Settings.V1._ApiKey key = a_Settings.ApiAccess.Keys.FirstOrDefault(a => a.KeyID == character.KeyID);
			return key;
		}

		public Settings.V1._ApiKey GetCharacterKey(UInt32 a_CharacterID)
		{
			return GetCharacterKey(m_Settings, a_CharacterID);
		}

		private TValue DictionaryGetValue<TKey, TValue>(Dictionary<TKey, TValue> a_Dictionary, TKey a_Key, TValue a_Default)
		{
			if (!a_Dictionary.ContainsKey(a_Key))
				return a_Default;

			return a_Dictionary[a_Key];
		}

		private double GetSkillMultiplier(UInt32 a_SkillID, RefiningMutators a_Mutators)
		{
			UInt32 skillLevel = DictionaryGetValue(m_Settings.Refining.Skills, a_SkillID, (UInt32)0);
			double skillMutator = DictionaryGetValue(a_Mutators, a_SkillID, 0.0);
			return 1 + (skillLevel*skillMutator);
		}

		private double GetRefiningSkillBonus(ItemRecord a_Item, RefiningMutators a_Mutators)
		{
			if (0 == a_Item.RefineSkill)
				return GetSkillMultiplier((UInt32)EveSkills.ScrapmetalProcessing, a_Mutators);

			double result = 1;
			result *= GetSkillMultiplier((UInt32)EveSkills.Reprocessing, a_Mutators);
			result *= GetSkillMultiplier((UInt32)EveSkills.ReprocessingEfficiency, a_Mutators);
			result *= GetSkillMultiplier(a_Item.RefineSkill, a_Mutators);

			return result;
		}

		public double GetEffectiveYield(ItemRecord a_Item, RefiningMutators a_Mutators)
		{
			double skillBonus = GetRefiningSkillBonus(a_Item, a_Mutators);
			return m_Settings.Refining.BaseYield * skillBonus * m_Settings.Refining.TaxMultiplier;
		}

		public double GetEffectiveRefineQuota(ItemRecord a_Item, RefiningMutators a_Mutators, double a_PerfectAmount)
		{
			return a_PerfectAmount * GetEffectiveYield(a_Item, a_Mutators);
		}

		/// <summary>
		/// Gets amount of material after perfect refining
		/// Refines incomplete batches too
		/// </summary>
		public double GetPerfectRefiningQuota(ItemRecord a_Item, UInt32 a_Quantity, Materials a_Material)
		{
			return (a_Quantity * a_Item.MaterialAmount[(UInt32)a_Material]) / a_Item.BatchSize;
		}

		/// <summary>
		/// Gets amount of material after refining with your efficiency and skill
		/// Refines incomplete batches too
		/// </summary>
		public double GetEffectiveRefineQuota(ItemRecord a_Item, RefiningMutators a_Mutators, UInt32 a_Quantity, Materials a_Material)
		{
			double perfectRefinedAmount = GetPerfectRefiningQuota(a_Item, a_Quantity, a_Material);
			return GetEffectiveRefineQuota(a_Item, a_Mutators, perfectRefinedAmount);
		}

		public double GetItemRefinedPrice(ItemRecord a_Item, RefiningMutators a_Mutators, UInt32 a_Quantity)
		{
			double result = 0;

			for (UInt32 i = 0; i < (UInt32)Materials.MaxMaterials; i++)
			{
				double refinedAmount = GetEffectiveRefineQuota(a_Item, a_Mutators, a_Quantity * a_Item.MaterialAmount[i]);
				result += (refinedAmount * m_Settings.MaterialPrices[i]);
			}

			return result / a_Item.BatchSize;
		}

		public Color GetPriceColor(double a_PriceRefine, double a_PriceSell, bool a_QuantityOK)
		{
			double colorGreenPercent = 0;
		
			if (a_QuantityOK && m_Settings.Appearance.OverrideAssetsColors)
			{
				double losses = a_PriceSell - a_PriceRefine;
			
				if (losses <= m_Settings.Appearance.GreenIskLoss)
					return Color.FromArgb(0, 255, 0);
					
				if (losses >= m_Settings.Appearance.RedIskLoss)
					return Color.FromArgb(255, 0, 0);

				colorGreenPercent = (m_Settings.Appearance.RedIskLoss - losses) / (m_Settings.Appearance.RedIskLoss - m_Settings.Appearance.GreenIskLoss);
			}
			else
			{
				double priceRangeLo = a_PriceSell * m_Settings.Appearance.RedPrice;
				double priceRangeHi = a_PriceSell * m_Settings.Appearance.GreenPrice;
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
		public ItemPrice GetItemPrices(ItemRecord a_Item, RefiningMutators a_Mutators, UInt32 a_Quantity)
		{
			ItemPrice result = new ItemPrice();

			result.RefinedCost		= GetItemRefinedPrice(a_Item, a_Mutators, a_Quantity);
			result.MarketPrice		= a_Quantity * a_Item.Price;

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
			else if (!a_Item.IsPricesOk(m_Settings.PriceLoad.Items.ExpiryDays))
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
                    errorHint = " (Did you provide API key with insufficient access?)";

				message += (errorNode.InnerText + errorHint + "\n");
			}

			ErrorMessageBox.Show(message);
		}
		
		public static string GetPriceTypeName(PriceTypes a_PriceType)
		{
			Debug.Assert((UInt32)PriceTypes.MaxPriceTypes == 14);
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
				case PriceTypes.Buy95Pct:
					result += "\tPrice people are buying for, if 5% items were purchased";
					break;
				case PriceTypes.Sell95Pct:
					result += "\tPrice people are selling for, if 5% items were sold";
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
			
			string itemText		= comboBox.Items[a_Args.Index].ToString();
			string[] substrings	= itemText.Split(new Char[]{'\t'}, 2);
			SizeF tabSize		= a_Args.Graphics.MeasureString("SellMedian___", a_Args.Font);

			Rectangle rect1 = Rectangle.FromLTRB(a_Args.Bounds.Left, a_Args.Bounds.Top, a_Args.Bounds.Left + (int)tabSize.Width, a_Args.Bounds.Bottom);
			Rectangle rect2 = Rectangle.FromLTRB(rect1.Right, a_Args.Bounds.Top, a_Args.Bounds.Right, a_Args.Bounds.Bottom);
			a_Args.Graphics.DrawString(substrings[0], a_Args.Font, new SolidBrush(a_Args.ForeColor), rect1);

			bool isEditPart = (a_Args.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
			bool hasHint	= (substrings.Count() > 1);
			if (hasHint && !isEditPart)
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
	    
	    public static JObject LoadJsonWithUserAgent(string a_Url)
	    {
	        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(a_Url);
	        httpRequest.UserAgent = "EveRefinery";

	        using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
	        {
				StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
				JsonTextReader jsonReader = new JsonTextReader(streamReader);
				return (JObject)JToken.ReadFrom(jsonReader);
	        }
	    }
	}
}
