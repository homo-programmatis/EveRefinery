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
		protected Settings		m_Settings;
		protected EveDatabase	m_EveDatabase;
		protected Pages			m_StartPage;
		protected ListView.ColumnHeaderCollection m_ListColumns;
		protected PriceSettings	m_MineralPriceSettings;
		
		public enum Pages
		{
			Minerals,
			ApiKeys,
		}

		public FrmSettings(Pages a_StartPage, Settings a_Settings, EveDatabase a_EveDatabase, ListView.ColumnHeaderCollection a_ListColumns)
		{
			m_Settings		= a_Settings;
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

		private void InitPage_ApiKeys()
		{
			foreach (Settings._ApiAccess.Key currKey in m_Settings.ApiAccess.Keys)
			{
				ListViewItem newItem = LstUsers.Items.Add(currKey.KeyID.ToString());
				newItem.SubItems.Add(currKey.Verification);
			}

            foreach (Settings._ApiAccess.Char currCharacter in m_Settings.ApiAccess.Chars)
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

				Settings._ApiAccess.Key newRow = new Settings._ApiAccess.Key();
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

				Settings._ApiAccess.Char newRow = new Settings._ApiAccess.Char();
				newRow.KeyID			= Convert.ToUInt32(keyID);
				newRow.CharacterID		= Convert.ToUInt32(charID);
				newRow.CharacterName	= charName;
				newRow.CorporationName	= charCorp;
				m_Settings.ApiAccess.Chars.Add(newRow);
			}

			return true;
		}

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
			
			m_MineralPriceSettings		= m_Settings.PriceLoad.SourceMinerals;
			
            // @@@@ Remove controls
            TxtRefineryEfficiency.Value = 100;
			TxtRefineryTax.Value		= 0;

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

			m_Settings.PriceLoad.SourceMinerals = m_MineralPriceSettings;

			HandleMineralPricesChange(oldMaterialPrices);
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
		
		private void InitPage_Other()
		{
			ChkCheckUpdates.Checked			= m_Settings.Options.CheckUpdates;
			TxtPriceHistory.Value			= m_Settings.PriceLoad.ItemsHistoryDays;
			TxtPricesExpiryDays.Value		= m_Settings.PriceLoad.ItemsExpiryDays;
			TxtMineralPricesExpiryDays.Value = m_Settings.PriceLoad.MineralExpiryDays;
		}
		
		private bool SavePage_Other()
		{
			m_Settings.Options.CheckUpdates		= ChkCheckUpdates.Checked;
			m_Settings.PriceLoad.ItemsHistoryDays	= (UInt32)TxtPriceHistory.Value;
			m_Settings.PriceLoad.ItemsExpiryDays	= (UInt32)TxtPricesExpiryDays.Value;
			m_Settings.PriceLoad.MineralExpiryDays	= (UInt32)TxtMineralPricesExpiryDays.Value;
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

		private void UpdateSingleUser(string a_KeyID, string a_Verification)
		{
			string xmlQueryUrl = "http://api.eve-online.com/account/Characters.xml.aspx?keyID=" + a_KeyID + "&vCode=" + a_Verification;
			XmlDocument xmlReply = new XmlDocument();

			string errorHeader = "Failed to update user " + a_KeyID + ":\n";

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

			IPriceProvider provider = new PriceProviderAuto(m_Settings);

			// @@@@ Check for exceptions?
			List<PriceRecord> prices = provider.GetPrices(loadPricesFor, m_MineralPriceSettings);
			foreach (PriceRecord currRecord in prices)
			{
				if (!currRecord.Settings.Matches(m_MineralPriceSettings))
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

			try
			{
				EveDatabase.StripDatabase(fileDialog.FileName);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Failed to strip database:\n" + a_Exception.Message);
			}
		}

		private void UpdateMineralPricesTypeLabel()
		{
			BtnMineralPricesType.Text = m_MineralPriceSettings.GetHintText(m_EveDatabase);
		}

		private void BtnMineralPricesType_Click(object sender, EventArgs e)
		{
			FrmPriceType dialog = new FrmPriceType(m_EveDatabase);
			dialog.m_Settings = m_MineralPriceSettings;
			if (DialogResult.OK != dialog.ShowDialog(this))
				return;

			m_MineralPriceSettings = dialog.m_Settings;
			UpdateMineralPricesTypeLabel();
		}
	}
}
