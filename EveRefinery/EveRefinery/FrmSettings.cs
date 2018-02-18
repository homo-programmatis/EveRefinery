﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SpecialFNs;

namespace EveRefinery
{
	public partial class FrmSettings : Form
	{
		protected Settings		m_Settings;
		protected EveDatabase	m_EveDatabase;
		protected Pages			m_StartPage;
		protected ListView.ColumnHeaderCollection m_ListColumns;
		
		public enum Pages
		{
			Minerals,
			Refining,
			ApiKeys,
			Appearance,
			Other,
			Developer,
		}

		public FrmSettings(Pages a_StartPage, Settings a_Settings, EveDatabase a_EveDatabase, ListView.ColumnHeaderCollection a_ListColumns)
		{
			m_Settings		= Utility.CloneUsingBinary(a_Settings);
			m_EveDatabase	= a_EveDatabase;
			m_StartPage		= a_StartPage;
			m_ListColumns	= a_ListColumns;
			
			InitializeComponent();
		}

		public Settings GetSettings()
		{
			return m_Settings;
		}

		private void FrmSettings_Load(object sender, EventArgs e)
		{
			InitPage_ApiKeys();
			InitPage_Minerals();
			InitPage_Refining();
			InitPage_Appearance();
			InitPage_Other();
			InitPage_Developer();
			
			TabMain.SelectedIndex = (Int32)m_StartPage;
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			if (!SavePage_ApiKeys())
				return;
			
			if (!SavePage_Minerals())
				return;

			if (!SavePage_Refining())
				return;

			if (!SavePage_Appearance())
				return;

			if (!SavePage_Other())
				return;
				
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void TabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (TabMain.SelectedTab == TbpRefining)
			{
				SavePage_ApiKeys();
				UpdateCmbLoadSkills();
			}
		}


		#region Page Minerals
		private void InitPage_Minerals()
		{
			TxtTritanium.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Tritanium];
			TxtPyerite.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Pyerite];
			TxtMexallon.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Mexallon];
			TxtIsogen.Value		= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Isogen];
			TxtNoxcium.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Noxcium];
			TxtZydrine.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Zydrine];
			TxtMegacyte.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Megacyte];
			TxtMorphite.Value	= (decimal)m_Settings.MaterialPrices[(UInt32)Materials.Morphite];
			
			UpdateMineralPricesTypeLabel();
		}

		private void HandleMineralPricesChange(double[] a_OldPrices)
		{
			Boolean mineralPricesChanged = false;
			for (int i = 0; i < a_OldPrices.Length; i++)
			{
				if (a_OldPrices[i] != m_Settings.MaterialPrices[i])
				{
					mineralPricesChanged = true;
					break;
				}
			}

			if (mineralPricesChanged)
				m_Settings.Stats.LastMineralPricesEdit = DateTime.UtcNow;
		}

		private bool SavePage_Minerals()
		{
			double[] oldMaterialPrices = (double[])m_Settings.MaterialPrices.Clone();

			m_Settings.MaterialPrices[(UInt32)Materials.Tritanium]	= (double)TxtTritanium.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Pyerite]	= (double)TxtPyerite.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Mexallon]	= (double)TxtMexallon.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Isogen]		= (double)TxtIsogen.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Noxcium]	= (double)TxtNoxcium.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Zydrine]	= (double)TxtZydrine.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Megacyte]	= (double)TxtMegacyte.Value;
			m_Settings.MaterialPrices[(UInt32)Materials.Morphite]	= (double)TxtMorphite.Value;

			HandleMineralPricesChange(oldMaterialPrices);
			return true;
		}

		private void BtnLoadMineralPrices_Click(object sender, EventArgs e)
		{
			List<UInt32> loadPricesFor = new List<UInt32>();
			loadPricesFor.Add((UInt32)EveTypeIDs.Tritanium);
			loadPricesFor.Add((UInt32)EveTypeIDs.Pyerite);
			loadPricesFor.Add((UInt32)EveTypeIDs.Mexallon);
			loadPricesFor.Add((UInt32)EveTypeIDs.Isogen);
			loadPricesFor.Add((UInt32)EveTypeIDs.Noxcium);
			loadPricesFor.Add((UInt32)EveTypeIDs.Zydrine);
			loadPricesFor.Add((UInt32)EveTypeIDs.Megacyte);
			loadPricesFor.Add((UInt32)EveTypeIDs.Morphite);

			IPriceProvider provider = PriceProviderAuto.GetPriceProvider(m_Settings.PriceLoad.Minerals);

			// @@@@ Check for exceptions?
			List<PriceRecord> prices = provider.GetPrices(loadPricesFor);
			PriceRecord priceFilter = provider.GetCurrentFilter();
			foreach (PriceRecord currRecord in prices)
			{
				if (!currRecord.IsMatchesFilter(priceFilter))
					continue;

				switch ((EveTypeIDs)currRecord.TypeID)
				{
				case EveTypeIDs.Tritanium:
					TxtTritanium.Value	= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Pyerite:
					TxtPyerite.Value	= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Mexallon:
					TxtMexallon.Value	= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Isogen:
					TxtIsogen.Value		= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Noxcium:
					TxtNoxcium.Value	= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Zydrine:
					TxtZydrine.Value	= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Megacyte:
					TxtMegacyte.Value	= (decimal)currRecord.Price;
					break;
				case EveTypeIDs.Morphite:
					TxtMorphite.Value	= (decimal)currRecord.Price;
					break;
				}
			}
		}

		private void UpdateMineralPricesTypeLabel()
		{
			IPriceProvider priceProvider = PriceProviderAuto.GetPriceProvider(m_Settings.PriceLoad.Minerals);
			LblMineralPricesType.Text = priceProvider.GetCurrentFilterHint(m_EveDatabase);
		}

		private void BtnMineralPricesType_Click(object sender, EventArgs e)
		{
			FrmPriceType dialog = new FrmPriceType(m_EveDatabase, m_Settings.PriceLoad.Minerals);
			if (DialogResult.OK != dialog.ShowDialog(this))
				return;

			UpdateMineralPricesTypeLabel();
		}
		#endregion

		#region Page Refining
		private void InitPage_Refining()
		{
			InitializeSkillValues();
			UpdateCmbLoadSkills();
			PrpRefining.SelectedObject = new RefiningSettings(m_Settings);
		}

		private bool SavePage_Refining()
		{
			return true;
		}

		private void UpdateCmbLoadSkills()
		{
			CmbLoadSkills.Items.Clear();

			foreach (Settings.V1._ApiChar currChar in m_Settings.ApiAccess.Chars)
			{
				TextItemWithUInt32 newItem = new TextItemWithUInt32(currChar.CharacterName, currChar.CharacterID);
				CmbLoadSkills.Items.Add(newItem);
			}

			if (0 != CmbLoadSkills.Items.Count)
				CmbLoadSkills.SelectedIndex = 0;
		}

		private void InitializeSkillValues()
		{
			EveSkills[] skillIDs = (EveSkills[])Enum.GetValues(typeof(EveSkills));
			foreach (EveSkills currSkill in skillIDs)
			{
				if (!m_Settings.Refining.Skills.ContainsKey((UInt32)currSkill))
					m_Settings.Refining.Skills[(UInt32)currSkill] = 0;
			}
		}

		#region PropertyGrid
		static UInt32[] m_SkillLevels = new UInt32[]{0, 1, 2, 3, 4, 5};

		class StringOnlyConverter : TypeConverter
		{
			public override bool CanConvertFrom(ITypeDescriptorContext a_Context, Type a_Type)
			{
				return a_Type == typeof(string);
			}

			public override bool CanConvertTo(ITypeDescriptorContext a_Context, Type a_Type)
			{
				return a_Type == typeof(string);
			}
		}

		class SkillConverter : StringOnlyConverter
		{
			public override bool GetStandardValuesSupported(ITypeDescriptorContext a_Context)
			{
				return true;
			}

			public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext a_Context)
			{
				return new StandardValuesCollection(m_SkillLevels);
			}

			public override object ConvertFrom(ITypeDescriptorContext a_Context, CultureInfo a_Culture, object a_Value)
			{
				UInt32 value = UInt32.Parse((String)a_Value);
				if (value > 5)
					return (UInt32)5;

				return value;
			}
		}

		public static double ParseDouble(String a_String)
		{
			StringBuilder cleaned = new StringBuilder();
			foreach (char currChar in a_String)
			{
				if (Char.IsDigit(currChar))
				{
					cleaned.Append(currChar);
					continue;
				}

				if ((currChar == '.') || (currChar == ','))
				{
					cleaned.Append('.');
					continue;
				}
			}

			return double.Parse(cleaned.ToString(), CultureInfo.InvariantCulture);
		}

		class PercentConverter : StringOnlyConverter
		{
			public override object ConvertFrom(ITypeDescriptorContext a_Context, CultureInfo a_Culture, object a_Value)
			{
				return ParseDouble((String)a_Value) / 100.0;
			}

			public override object ConvertTo(ITypeDescriptorContext a_Context, CultureInfo a_Culture, object a_Value, Type a_Type)
			{
				return String.Format(CultureInfo.InvariantCulture, "{0:0%}", a_Value);
			}
		}

		class MultiplierConverter : StringOnlyConverter
		{
			public override object ConvertFrom(ITypeDescriptorContext a_Context, CultureInfo a_Culture, object a_Value)
			{
				return ParseDouble((String)a_Value);
			}

			public override object ConvertTo(ITypeDescriptorContext a_Context, CultureInfo a_Culture, object a_Value, Type a_Type)
			{
				return String.Format(CultureInfo.InvariantCulture, "x{0:0.00####}", (double)a_Value);
			}
		}

		class RefiningSettings
		{
			#region Yield
			[Category("1. Station equipment")]
			[Description("Your station's [base yield]. See tooltip on refining meter in EVE.")]
			[TypeConverter(typeof(PercentConverter))]
			public double BaseYield
			{
				get {return m_Settings.Refining.BaseYield;}
				set {m_Settings.Refining.BaseYield = value;}
			}

			[Category("1. Station equipment")]
			[Description("Your station's [reduction from station owner tax]. See tooltip on refining meter in EVE.")]
			[TypeConverter(typeof(MultiplierConverter))]
			public double TaxMultiplier
			{
				get { return m_Settings.Refining.TaxMultiplier; }
				set { m_Settings.Refining.TaxMultiplier = value; }
			}
			#endregion

			#region Skills
			[Category("2. Non-Ore refining")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 ScrapmetalProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.ScrapmetalProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.ScrapmetalProcessing] = value; }
			}

			#region Ore refining
			[Category("3. Ore refining - generic")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 Reprocessing
			{
				get {return m_Settings.Refining.Skills[(UInt32)EveSkills.Reprocessing];}
				set {m_Settings.Refining.Skills[(UInt32)EveSkills.Reprocessing] = value;}
			}

			[Category("3. Ore refining - generic")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 ReprocessingEfficiency
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.ReprocessingEfficiency]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.ReprocessingEfficiency] = value; }
			}

			[Category("3. Ore refining - generic")]
			[Description("Bonus from implants, if any.")]
			[TypeConverter(typeof(PercentConverter))]
			public double ImplantBonus
			{
				get { return m_Settings.Refining.ImplantBonus; }
				set { m_Settings.Refining.ImplantBonus = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 ArkonorProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.ArkonorProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.ArkonorProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 BistotProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.BistotProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.BistotProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 CrokiteProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.CrokiteProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.CrokiteProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 DarkOchreProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.DarkOchreProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.DarkOchreProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 GneissProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.GneissProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.GneissProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 HedbergiteProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.HedbergiteProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.HedbergiteProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 HemorphiteProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.HemorphiteProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.HemorphiteProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 JaspetProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.JaspetProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.JaspetProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 KerniteProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.KerniteProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.KerniteProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 MercoxitProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.MercoxitProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.MercoxitProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 OmberProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.OmberProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.OmberProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 PlagioclaseProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.PlagioclaseProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.PlagioclaseProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 PyroxeresProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.PyroxeresProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.PyroxeresProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 ScorditeProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.ScorditeProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.ScorditeProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 SpodumainProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.SpodumainProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.SpodumainProcessing] = value; }
			}

			[Category("4. Ore refining - specific")]
			[Description("Relevant skill level.")]
			[TypeConverter(typeof(SkillConverter))]
			public UInt32 VeldsparProcessing
			{
				get { return m_Settings.Refining.Skills[(UInt32)EveSkills.VeldsparProcessing]; }
				set { m_Settings.Refining.Skills[(UInt32)EveSkills.VeldsparProcessing] = value; }
			}
			#endregion
			#endregion

			#region Settings reference
			private Settings m_Settings;

			public RefiningSettings(Settings a_Settings)
			{
				m_Settings = a_Settings;
			}
			#endregion
		}
		#endregion

		private bool LoadSkills(Settings.V1._ApiKey a_ApiKey, UInt32 a_UserID, Dictionary<UInt32, UInt32> a_Result)
		{
			String errorHeader = "Failed to load skills";
			XmlDocument xmlReply = EveApi.MakeRequest("char/CharacterSheet.xml.aspx", a_ApiKey, a_UserID, errorHeader);
			if (null == xmlReply)
				return false;

			try
			{
				XmlNodeList skillNodes = xmlReply.SelectNodes("/eveapi/result/rowset[@name='skills']/row");
				foreach (XmlNode currNode in skillNodes)
				{
					UInt32 typeID	= UInt32.Parse(currNode.Attributes["typeID"].Value);
					UInt32 level	= UInt32.Parse(currNode.Attributes["level"].Value);
					a_Result[typeID]= level;
				}
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show(errorHeader + ":\n" + "Failed to parse EVE API reply:\n" + a_Exception.Message);
				return false;
			}

			// Safety check for possible XML format change
			if (0 == a_Result.Count)
				return false;

			return true;
		}

		private void BtnLoadSkills_Click(object sender, EventArgs e)
		{
			if (null == CmbLoadSkills.SelectedItem)
				return;

			UInt32 userID = TextItemWithUInt32.GetData(CmbLoadSkills.SelectedItem);
			Settings.V1._ApiKey apiKey = Engine.GetCharacterKey(m_Settings, userID);
			if (null == apiKey)
			{
				ErrorMessageBox.Show("Can't find API key for selected character");
				return;
			}

			Dictionary<UInt32, UInt32> skills = new Dictionary<UInt32, UInt32>();
			if (!LoadSkills(apiKey, userID, skills))
				return;

			EveSkills[] skillIDs = (EveSkills[])Enum.GetValues(typeof(EveSkills));
			foreach (EveSkills currSkill in skillIDs)
			{
				UInt32 skillLevel = 0;
				if (skills.ContainsKey((UInt32)currSkill))
					skillLevel = skills[(UInt32)currSkill];

				m_Settings.Refining.Skills[(UInt32)currSkill] = skillLevel;
			}

			PrpRefining.Refresh();
		}
		#endregion

		#region Page API
		private void InitPage_ApiKeys()
		{
			foreach (Settings.V1._ApiKey currKey in m_Settings.ApiAccess.Keys)
			{
				ListViewItem newItem = LstUsers.Items.Add(currKey.KeyID.ToString());
				newItem.SubItems.Add(currKey.Verification);
			}

            foreach (Settings.V1._ApiChar currCharacter in m_Settings.ApiAccess.Chars)
			{
				ListViewItem newItem = LstCharacters.Items.Add(currCharacter.KeyID.ToString());
				newItem.SubItems.Add(currCharacter.CharacterID.ToString());
				newItem.SubItems.Add(currCharacter.CharacterName);
				newItem.SubItems.Add(currCharacter.CorporationName);
			}
		}

		private bool SavePage_ApiKeys()
		{
			if (HasOrphanedUsers())
			{
				if (DialogResult.Yes != MessageBox.Show("You entered some users, but did not [Load characters]. If you continue now, the corresponding characters will not be available.\nContinue anyway?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
					return false;
			}

			m_Settings.ApiAccess.Keys.Clear();
			for (int i = 0; i < LstUsers.Items.Count; i++)
			{
				ListViewItem currItem = LstUsers.Items[i];
				string userID	= currItem.Text;
				string apiKey	= currItem.SubItems[1].Text;

				Settings.V1._ApiKey newRow = new Settings.V1._ApiKey();
				newRow.KeyID		= Convert.ToUInt32(userID);
				newRow.Verification	= apiKey;
				m_Settings.ApiAccess.Keys.Add(newRow);
			}

			m_Settings.ApiAccess.Chars.Clear();
			for (int i = 0; i < LstCharacters.Items.Count; i++)
			{
				ListViewItem currItem = LstCharacters.Items[i];
				string keyID	= currItem.Text;
				string charID	= currItem.SubItems[1].Text;
				string charName = currItem.SubItems[2].Text;
				string charCorp = currItem.SubItems[3].Text;

				Settings.V1._ApiChar newRow = new Settings.V1._ApiChar();
				newRow.KeyID			= Convert.ToUInt32(keyID);
				newRow.CharacterID		= Convert.ToUInt32(charID);
				newRow.CharacterName	= charName;
				newRow.CorporationName	= charCorp;
				m_Settings.ApiAccess.Chars.Add(newRow);
			}

			return true;
		}

		private void BtnAddApiKey_Click(object sender, EventArgs e)
		{
			FrmAddNewApiKey frmAddNewApiKey = new FrmAddNewApiKey();
			if (DialogResult.OK != frmAddNewApiKey.ShowDialog())
				return;

			for (int i = 0; i < LstUsers.Items.Count; i++)
			{
				ListViewItem currItem = LstUsers.Items[i];
				if (currItem.Text == frmAddNewApiKey.m_UserID)
				{
					ErrorMessageBox.Show("This UserID is already present");
					return;
				}
			}

			ListViewItem newItem = LstUsers.Items.Add(frmAddNewApiKey.m_UserID);
			newItem.SubItems.Add(frmAddNewApiKey.m_ApiKey);
		}

		private void DeleteCharactersFromUser(string a_UserID)
		{
			for (int i = 0; i < LstCharacters.Items.Count; i++)
			{
				ListViewItem currItem = LstCharacters.Items[i];

				if (currItem.Text == a_UserID)
				{
					LstCharacters.Items.RemoveAt(i);
					i--;
				}
			}
		}

		private void BtnDeleteApiKey_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in LstUsers.SelectedItems)
			{
				DeleteCharactersFromUser(selectedItem.Text);
				LstUsers.Items.Remove(selectedItem);
			}
		}

		private void UpdateSingleUser(string a_KeyID, string a_Verification)
		{
			Settings.V1._ApiKey tempKey = new Settings.V1._ApiKey();
			UInt32.TryParse(a_KeyID, out tempKey.KeyID);
			tempKey.Verification = a_Verification;

			string errorHeader = "Failed to update user " + a_KeyID;
			XmlDocument xmlReply = EveApi.MakeRequest("account/Characters.xml.aspx", tempKey, 0, errorHeader);
			if (null == xmlReply)
				return;

			XmlNodeList characterNodes = xmlReply.GetElementsByTagName("row");
			if (0 == characterNodes.Count)
			{
				ErrorMessageBox.Show(errorHeader + "No characters found");
				return;
			}

			DeleteCharactersFromUser(a_KeyID);

			foreach (XmlNode characterNode in characterNodes)
			{
				ListViewItem newItem = LstCharacters.Items.Add(a_KeyID);
				newItem.SubItems.Add(characterNode.Attributes["characterID"].Value);
				newItem.SubItems.Add(characterNode.Attributes["name"].Value);
				newItem.SubItems.Add(characterNode.Attributes["corporationName"].Value);
			}
		}

		private void BtnUpdateChars_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < LstUsers.Items.Count; i++)
			{
				ListViewItem currItem = LstUsers.Items[i];
				string userID = currItem.Text;
				string apiKey = currItem.SubItems[1].Text;

				UpdateSingleUser(userID, apiKey);
			}
		}

		private Boolean HasOrphanedUsers()
		{
			foreach (ListViewItem currUser in LstUsers.Items)
			{
				string userUserID = currUser.Text;
				bool isFound = false;

				foreach (ListViewItem currChar in LstCharacters.Items)
				{
					string charUserID = currChar.Text;

					if (charUserID == userUserID)
					{
						isFound = true;
						break;
					}
				}

				if (!isFound)
					return true;
			}

			return false;
		}

		private void LnkGetApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
		}
		#endregion

		#region Page Appearance
		private void InitPage_Appearance()
		{
			foreach (ColumnHeader column in m_ListColumns)
			{
				bool isColumnVisible = (0 != column.Width);
				ListViewItem newItem = new ListViewItem(column.Text);
				newItem.Checked = isColumnVisible;
				newItem.Tag = (Object)column.Index;
				LstColumns.Items.Add(newItem);
			}

			TxtRedPrice.Value				= (int)(m_Settings.Appearance.RedPrice * 100);
			TxtGreenPrice.Value				= (int)(m_Settings.Appearance.GreenPrice * 100);
			
			ChkOverrideColorsISK.Checked	= m_Settings.Appearance.OverrideAssetsColors;
			TxtGreenIskLoss.Value			= (decimal)m_Settings.Appearance.GreenIskLoss;
			TxtRedIskLoss.Value				= (decimal)m_Settings.Appearance.RedIskLoss;
			Update_OverrideColorsControls_Enabled();
		}
		
		private bool SavePage_Appearance()
		{
			for (int i = 0; i < LstColumns.Items.Count; i++)
			{
				ListViewItem currItem = LstColumns.Items[i];
				Int32 columnIndex = (Int32)currItem.Tag;
				
				if (currItem.Checked)
					ListViewEx.UnhideColumn(m_ListColumns[columnIndex]);
				else
					ListViewEx.HideColumn(m_ListColumns[columnIndex]);
			}

			m_Settings.Appearance.RedPrice				= ((double)TxtRedPrice.Value) / 100;
			m_Settings.Appearance.GreenPrice			= ((double)TxtGreenPrice.Value) / 100;
			
			m_Settings.Appearance.OverrideAssetsColors	= ChkOverrideColorsISK.Checked;
			m_Settings.Appearance.GreenIskLoss			= (double)TxtGreenIskLoss.Value;
			m_Settings.Appearance.RedIskLoss			= (double)TxtRedIskLoss.Value;
			
			return true;
		}

		private void OnGreenPriceChange(Int32 a_NewValue)
		{
			if (TrkGreenPrice.Value != a_NewValue)
				TrkGreenPrice.Value = a_NewValue;

			if (TxtGreenPrice.Value != a_NewValue)
				TxtGreenPrice.Value = a_NewValue;

			if (TxtRedPrice.Value > a_NewValue)
				TxtRedPrice.Value = a_NewValue;
		}

		private void OnRedPriceChange(Int32 a_NewValue)
		{
			if (TrkRedPrice.Value != a_NewValue)
				TrkRedPrice.Value = a_NewValue;

			if (TxtRedPrice.Value != a_NewValue)
				TxtRedPrice.Value = a_NewValue;

			if (TxtGreenPrice.Value < a_NewValue)
				TxtGreenPrice.Value = a_NewValue;
		}

		private void TxtRedPrice_ValueChanged(object sender, EventArgs e)
		{
			OnRedPriceChange((Int32)TxtRedPrice.Value);
		}

		private void TrkRedPrice_ValueChanged(object sender, EventArgs e)
		{
			OnRedPriceChange(TrkRedPrice.Value);
		}

		private void TxtGreenPrice_ValueChanged(object sender, EventArgs e)
		{
			OnGreenPriceChange((Int32)TxtGreenPrice.Value);
		}

		private void TrkGreenPrice_ValueChanged(object sender, EventArgs e)
		{
			OnGreenPriceChange(TrkGreenPrice.Value);
		}

		private void Update_OverrideColorsControls_Enabled()
		{
			TxtGreenIskLoss.Enabled = ChkOverrideColorsISK.Checked;
			TxtRedIskLoss.Enabled	= ChkOverrideColorsISK.Checked;
		}

		private void ChkOverrideColorsISK_CheckedChanged(object sender, EventArgs e)
		{
			Update_OverrideColorsControls_Enabled();
		}
		#endregion

		#region Page Other
		private void InitPage_Other()
		{
			ChkCheckUpdates.Checked				= m_Settings.Options.CheckUpdates;
			TxtPricesExpiryDays.Value			= m_Settings.PriceLoad.Items.ExpiryDays;
			TxtMineralPricesExpiryDays.Value	= m_Settings.PriceLoad.Minerals.ExpiryDays;
		}
		
		private bool SavePage_Other()
		{
			m_Settings.Options.CheckUpdates				= ChkCheckUpdates.Checked;
			m_Settings.PriceLoad.Items.ExpiryDays		= (UInt32)TxtPricesExpiryDays.Value;
			m_Settings.PriceLoad.Minerals.ExpiryDays	= (UInt32)TxtMineralPricesExpiryDays.Value;
			return true;
		}
		#endregion

		#region Page Developer
		private void InitPage_Developer()
		{
			Boolean isPageShown = false;

			#if (DEBUG)
				isPageShown = true;
			#endif

			if (Environment.GetCommandLineArgs().Contains("/dev"))
				isPageShown = true;

			if (!isPageShown)
				TabMain.TabPages.Remove(TabDeveloper);
		}

		private void BtnStripDatabase_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Title = "Browse for EVE database";
			fileDialog.Filter = "All files (*.*)|*.*|Databases (*.db)|*.db";
			fileDialog.FilterIndex = 2;
			fileDialog.RestoreDirectory = true;
			if (DialogResult.OK != fileDialog.ShowDialog())
				return;

			try
			{
				EveDatabase.StripDatabase(fileDialog.FileName);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Failed to strip database:\n" + a_Exception.Message);
			}
		}
		#endregion
	}
}
