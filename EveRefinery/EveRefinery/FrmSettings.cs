using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpecialFNs;
using System.Xml;

namespace EveRefinery
{
	public partial class FrmSettings : Form
	{
		protected Engine		m_Engine;
		protected EveDatabase	m_EveDatabase;
		protected Pages			m_StartPage;
		protected ListView.ColumnHeaderCollection m_ListColumns;
		
		public enum Pages
		{
			Minerals,
			ApiKeys,
		}

		public FrmSettings(Pages a_StartPage, Engine a_Engine, EveDatabase a_EveDatabase, ListView.ColumnHeaderCollection a_ListColumns)
		{
			m_Engine		= a_Engine;
			m_EveDatabase	= a_EveDatabase;
			m_StartPage		= a_StartPage;
			m_ListColumns	= a_ListColumns;
			
			InitializeComponent();
		}

		private void FrmSettings_Load(object sender, EventArgs e)
		{
			InitPage_ApiKeys();
			InitPage_Minerals();
			InitPage_Appearance();
			InitPage_Other();
			InitPage_Developer();
			
			CmbMineralPriceType.DrawItem += Engine.DrawPriceTypeItem;
			TabMain.SelectedIndex = (Int32)m_StartPage;
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			if (!SavePage_ApiKeys())
				return;
			
			if (!SavePage_Minerals())
				return;

			if (!SavePage_Appearance())
				return;

			if (!SavePage_Other())
				return;
				
			m_Engine.UpdateSettingsCache();

			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void InitPage_ApiKeys()
		{
			for (int i = 0; i < m_Engine.m_Settings.Accounts.Count; i++)
			{
				Settings.AccountsRow currAccount = m_Engine.m_Settings.Accounts[i];
				ListViewItem newItem = LstUsers.Items.Add(currAccount.UserID.ToString());
				newItem.SubItems.Add(currAccount.FullKey);
			}

			for (int i = 0; i < m_Engine.m_Settings.Characters.Count; i++)
			{
				Settings.CharactersRow currCharacter = m_Engine.m_Settings.Characters[i];
				ListViewItem newItem = LstCharacters.Items.Add(currCharacter.UserID.ToString());
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

			m_Engine.m_Settings.Accounts.Clear();
			for (int i = 0; i < LstUsers.Items.Count; i++)
			{
				ListViewItem currItem = LstUsers.Items[i];
				string userID	= currItem.Text;
				string apiKey	= currItem.SubItems[1].Text;

				Settings.AccountsRow newRow = m_Engine.m_Settings.Accounts.NewAccountsRow();
				newRow.UserID	= Convert.ToUInt32(userID);
				newRow.FullKey	= apiKey;
				m_Engine.m_Settings.Accounts.AddAccountsRow(newRow);
			}

			m_Engine.m_Settings.Characters.Clear();
			for (int i = 0; i < LstCharacters.Items.Count; i++)
			{
				ListViewItem currItem = LstCharacters.Items[i];
				string userID	= currItem.Text;
				string charID	= currItem.SubItems[1].Text;
				string charName = currItem.SubItems[2].Text;
				string charCorp = currItem.SubItems[3].Text;

				Settings.CharactersRow newRow = m_Engine.m_Settings.Characters.NewCharactersRow();
				newRow.UserID			= Convert.ToUInt32(userID);
				newRow.CharacterID		= Convert.ToUInt32(charID);
				newRow.CharacterName	= charName;
				newRow.CorporationName	= charCorp;
				m_Engine.m_Settings.Characters.AddCharactersRow(newRow);
			}

			return true;
		}

		private void InitPage_Minerals()
		{
			TxtTritanium.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Tritanium];
			TxtPyerite.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Pyerite];
			TxtMexallon.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Mexallon];
			TxtIsogen.Value		= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Isogen];
			TxtNoxcium.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Noxcium];
			TxtZydrine.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Zydrine];
			TxtMegacyte.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Megacyte];
			TxtMorphite.Value	= (decimal)m_Engine.m_MaterialPrices[(UInt32)Materials.Morphite];
			
			Init_CmbMineralPriceType();
			Init_CmbMineralRegion();

			TxtRefineryEfficiency.Value = (decimal)m_Engine.m_Settings.Options[0].RefineryEfficiency * 100;
			TxtRefineryTax.Value		= (decimal)m_Engine.m_Settings.Options[0].RefineryTax * 100;
		}

		private bool SavePage_Minerals()
		{
			m_Engine.m_MaterialPrices[(UInt32)Materials.Tritanium]	= (double)TxtTritanium.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Pyerite]	= (double)TxtPyerite.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Mexallon]	= (double)TxtMexallon.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Isogen]		= (double)TxtIsogen.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Noxcium]	= (double)TxtNoxcium.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Zydrine]	= (double)TxtZydrine.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Megacyte]	= (double)TxtMegacyte.Value;
			m_Engine.m_MaterialPrices[(UInt32)Materials.Morphite]	= (double)TxtMorphite.Value;

			m_Engine.m_Settings.Options[0].RefineryEfficiency		= (double)(TxtRefineryEfficiency.Value / 100);
			m_Engine.m_Settings.Options[0].RefineryTax				= (double)(TxtRefineryTax.Value / 100);
			return true;
		}
		
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
			
			TxtRedPrice.Value	= (int)(m_Engine.m_Settings.Options[0].RedPrice * 100);
			TxtGreenPrice.Value = (int)(m_Engine.m_Settings.Options[0].GreenPrice * 100);
			
			ChkOverrideColorsISK.Checked	= m_Engine.m_Settings.Options[0].OverrideAssetsColors;
			TxtGreenIskLoss.Value			= (decimal)m_Engine.m_Settings.Options[0].GreenIskLoss;
			TxtRedIskLoss.Value				= (decimal)m_Engine.m_Settings.Options[0].RedIskLoss;
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
			
			m_Engine.m_Settings.Options[0].RedPrice				= ((double)TxtRedPrice.Value) / 100;
			m_Engine.m_Settings.Options[0].GreenPrice			= ((double)TxtGreenPrice.Value) / 100;
			
			m_Engine.m_Settings.Options[0].OverrideAssetsColors	= ChkOverrideColorsISK.Checked;
			m_Engine.m_Settings.Options[0].GreenIskLoss			= (double)TxtGreenIskLoss.Value;
			m_Engine.m_Settings.Options[0].RedIskLoss			= (double)TxtRedIskLoss.Value;
			
			return true;
		}
		
		private void InitPage_Other()
		{
			ChkCheckUpdates.Checked = m_Engine.m_Settings.Options[0].CheckUpdates;
			TxtPriceHistory.Value	= m_Engine.m_Settings.Options[0].PriceHistoryDays;
		}
		
		private bool SavePage_Other()
		{
			m_Engine.m_Settings.Options[0].CheckUpdates			= ChkCheckUpdates.Checked;
			m_Engine.m_Settings.Options[0].PriceHistoryDays		= (UInt32)TxtPriceHistory.Value;
			return true;
		}

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

		private void UpdateSingleUser(string a_UserID, string a_ApiKey)
		{
			string xmlQueryUrl = "http://api.eve-online.com//account/Characters.xml.aspx?userID=" + a_UserID + "&apiKey=" + a_ApiKey;
			XmlDocument xmlReply = new XmlDocument();

			string errorHeader = "Failed to update user " + a_UserID + ":\n";

			try
			{
				xmlReply = Engine.LoadXmlWithUserAgent(xmlQueryUrl);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show(errorHeader + a_Exception.Message);
				return;
			}

			XmlNodeList errorNodes = xmlReply.GetElementsByTagName("error");
			if (0 != errorNodes.Count)
			{
				Engine.ShowXmlRequestErrors(errorHeader, errorNodes);
				return;
			}

			XmlNodeList characterNodes = xmlReply.GetElementsByTagName("row");
			if (0 == characterNodes.Count)
			{
				ErrorMessageBox.Show(errorHeader + "No characters found");
				return;
			}

			DeleteCharactersFromUser(a_UserID);

			foreach (XmlNode characterNode in characterNodes)
			{
				ListViewItem newItem = LstCharacters.Items.Add(a_UserID);
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

		private void Init_CmbMineralPriceType()
		{
			ComboBox currCombo = CmbMineralPriceType;
			currCombo.Items.Clear();

			for (UInt32 i = 0; i < (UInt32)PriceTypes.MaxPriceTypes; i++)
			{
				string enumName = Engine.GetPriceTypeName((PriceTypes)i);
				TextItemWithUInt32 newItem = new TextItemWithUInt32(enumName, i);
				currCombo.Items.Add(newItem);
				
				if (i == (UInt32)PriceTypes.SellMedian)
					currCombo.SelectedItem = newItem;
			}
		}

		private void Init_CmbMineralRegion()
		{
			ComboBox currCombo = CmbMineralRegion;
			currCombo.Items.Clear();

			List<EveRegion> regions = m_EveDatabase.GetRegions();
			foreach (EveRegion currRegion in regions)
			{
				TextItemWithUInt32 newItem = new TextItemWithUInt32(currRegion.Name, currRegion.RegionID);
				currCombo.Items.Add(newItem);

				if (currRegion.RegionID == (UInt32)EveRegions.Forge)
					currCombo.SelectedItem = newItem;
			}
		}

		private void BtnLoadMineralPrices_Click(object sender, EventArgs e)
		{
			UInt32 regionID		= TextItemWithUInt32.GetData(CmbMineralRegion.SelectedItem);
			UInt32 priceType	= TextItemWithUInt32.GetData(CmbMineralPriceType.SelectedItem);
		
			List<UInt32> loadPricesFor = new List<UInt32>();
			loadPricesFor.Add((UInt32)EveTypeIDs.Tritanium);
			loadPricesFor.Add((UInt32)EveTypeIDs.Pyerite);
			loadPricesFor.Add((UInt32)EveTypeIDs.Mexallon);
			loadPricesFor.Add((UInt32)EveTypeIDs.Isogen);
			loadPricesFor.Add((UInt32)EveTypeIDs.Noxcium);
			loadPricesFor.Add((UInt32)EveTypeIDs.Zydrine);
			loadPricesFor.Add((UInt32)EveTypeIDs.Megacyte);
			loadPricesFor.Add((UInt32)EveTypeIDs.Morphite);

			ItemPrices.PricesDataTable prices = MarketPricesDB.QueryEveCentralPrices(loadPricesFor, regionID, m_Engine.m_Settings.Options[0].PriceHistoryDays);
			foreach (ItemPrices.PricesRow currRow in prices.Rows)
			{
				ItemRecord currItem = new ItemRecord(currRow.TypeID);
				MarketPricesDB.ParsePricesRow(currRow, currItem);
			
				switch ((EveTypeIDs)currRow.TypeID)
				{
				case EveTypeIDs.Tritanium:
					TxtTritanium.Value	= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Pyerite:
					TxtPyerite.Value	= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Mexallon:
					TxtMexallon.Value	= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Isogen:
					TxtIsogen.Value		= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Noxcium:
					TxtNoxcium.Value	= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Zydrine:
					TxtZydrine.Value	= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Megacyte:
					TxtMegacyte.Value	= (decimal)currItem.Prices[priceType];
					break;
				case EveTypeIDs.Morphite:
					TxtMorphite.Value	= (decimal)currItem.Prices[priceType];
					break;
				}
			}
		}

		private void BtnRefineryCalculator_Click(object sender, EventArgs e)
		{
			FrmRefineCalc frmRefineCalc = new FrmRefineCalc();
			if (DialogResult.OK != frmRefineCalc.ShowDialog(this))
				return;
				
			TxtRefineryEfficiency.Value = (decimal)frmRefineCalc.m_RefineryEfficiency * 100;
			TxtRefineryTax.Value		= (decimal)frmRefineCalc.m_TaxesTaken * 100;
		}

		private void LnkGetApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
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

		private void BtnStripDatabase_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Title = "Browse for EVE database";
			fileDialog.Filter = "All files (*.*)|*.*|Databases (*.db)|*.db";
			fileDialog.FilterIndex = 2;
			fileDialog.RestoreDirectory = true;
			if (DialogResult.OK != fileDialog.ShowDialog())
				return;

			EveDatabase.StripDatabase(fileDialog.FileName);
		}
	}
}
