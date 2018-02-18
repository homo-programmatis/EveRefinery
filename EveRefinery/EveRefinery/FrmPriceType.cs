using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SpecialFNs;

namespace EveRefinery
{
	public partial class FrmPriceType : Form
	{
		private	EveDatabase					m_EveDatabase;
		private	Settings.V2._PriceSettings  m_Settings;

		public FrmPriceType(EveDatabase a_EveDatabase, Settings.V2._PriceSettings a_Settings)
		{
			m_EveDatabase			= a_EveDatabase;
			m_Settings              = a_Settings;

			InitializeComponent();
		}

		private void FrmPriceType_Load(object sender, EventArgs e)
		{
			Cmb_EveCentralCom_PriceType.DrawItem += Engine.DrawPriceTypeItem;

			Init_EveCentralCom_CmbRegion();
			Init_EveCentralCom_CmbPriceType();

			Txt_EveCentralCom_PriceHistory.Value = m_Settings.EveCentralCom.HistoryDays;
		}

		private void Init_EveCentralCom_CmbRegion()
		{
			ComboBox currCombo = Cmb_EveCentralCom_Region;
			currCombo.Items.Clear();

			TextItemWithUInt32 allItem = new TextItemWithUInt32("[All regions]", 0);
			currCombo.Items.Add(allItem);

			List<EveRegion> regions = m_EveDatabase.GetRegions();
			foreach (EveRegion currRegion in regions)
			{
				TextItemWithUInt32 newItem = new TextItemWithUInt32(currRegion.Name, currRegion.ID);
				currCombo.Items.Add(newItem);

				if (currRegion.ID == m_Settings.EveCentralCom.RegionID)
					currCombo.SelectedItem = newItem;
			}

			if (currCombo.SelectedItem == null)
				currCombo.SelectedItem = allItem;
		}

		private void Init_EveCentralCom_CmbSolar()
		{
			UInt32 regionID		= TextItemWithUInt32.GetData(Cmb_EveCentralCom_Region.SelectedItem);

			ComboBox currCombo = Cmb_EveCentralCom_Solar;
			currCombo.Items.Clear();

			TextItemWithUInt32 allItem = new TextItemWithUInt32("[All solar systems]", 0);
			currCombo.Items.Add(allItem);

			List<EveSolarSystem> systems = m_EveDatabase.GetSolarSystems(regionID);
			foreach (EveSolarSystem currSystem in systems)
			{
				TextItemWithUInt32 newItem = new TextItemWithUInt32(currSystem.Name, currSystem.ID);
				currCombo.Items.Add(newItem);

				if (currSystem.ID == m_Settings.EveCentralCom.SolarID)
					currCombo.SelectedItem = newItem;
			}

			if (currCombo.SelectedItem == null)
				currCombo.SelectedItem = allItem;
		}

		private void Init_EveCentralCom_CmbPriceType()
		{
			ComboBox currCombo = Cmb_EveCentralCom_PriceType;
			currCombo.Items.Clear();

			foreach (PriceTypes priceType in PriceProviderEveCentral.GetSupportedPriceTypes())
			{
				string enumName = Engine.GetPriceTypeName(priceType);
				TextItemWithUInt32 newItem = new TextItemWithUInt32(enumName, (UInt32)priceType);
				currCombo.Items.Add(newItem);

				if ((PriceTypes)newItem.Data == m_Settings.EveCentralCom.PriceType)
					currCombo.SelectedItem = newItem;
			}
		}

		private void CmbRegion_SelectedIndexChanged(object sender, EventArgs e)
		{
			Init_EveCentralCom_CmbSolar();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			m_Settings.Provider                     = PriceProviders.EveCentral;

			m_Settings.EveCentralCom.RegionID		= TextItemWithUInt32.GetData(Cmb_EveCentralCom_Region.SelectedItem);
			m_Settings.EveCentralCom.SolarID		= TextItemWithUInt32.GetData(Cmb_EveCentralCom_Solar.SelectedItem);
			m_Settings.EveCentralCom.PriceType		= (PriceTypes)TextItemWithUInt32.GetData(Cmb_EveCentralCom_PriceType.SelectedItem);
			m_Settings.EveCentralCom.HistoryDays	= (UInt32)Txt_EveCentralCom_PriceHistory.Value;

			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
