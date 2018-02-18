﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SpecialFNs;
using System.Diagnostics;

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
			switch (m_Settings.Provider)
			{
				case PriceProviders.EveCentral:
					TabProviders.SelectedIndex = 0;
					break;
				case PriceProviders.FuzzworkCoUk:
					TabProviders.SelectedIndex = 1;
					break;
				case PriceProviders.EveMarketdataCom:
					TabProviders.SelectedIndex = 2;
					break;
				default:
					Debug.Assert(false, "Wrong price provider");
					break;
			}

			Cmb_EveCentralCom_PriceType.DrawItem += Engine.DrawPriceTypeItem;
			Init_EveCentralCom_CmbRegion();
			Init_EveCentralCom_CmbPriceType();
			Txt_EveCentralCom_PriceHistory.Value = m_Settings.EveCentralCom.HistoryDays;

			Cmb_FuzzworkCoUk_PriceType.DrawItem += Engine.DrawPriceTypeItem;
			Init_FuzzworkCoUk_RadSourceType();
			Init_FuzzworkCoUk_CmbRegion();
			Init_FuzzworkCoUk_CmbStation();
			Init_FuzzworkCoUk_CmbPriceType();

			Cmb_EveMarketdataCom_PriceType.DrawItem += Engine.DrawPriceTypeItem;
			EveMarketdataCom_Init_CmbRegion();
			// EveMarketdataCom_Init_CmbSolar();	// Will be called by SelectedIndexChanged handler
			// EveMarketdataCom_Init_CmbStation();	// Will be called by SelectedIndexChanged handler
			EveMarketdataCom_Init_CmbPriceType();
		}

		private void PopulateCombo(ComboBox a_Combo, List<TextItemWithUInt32> a_Items, UInt32 a_SelectedData)
		{
			a_Combo.Items.Clear();

			TextItemWithUInt32 selectedItem = null;
			foreach (TextItemWithUInt32 currItem in a_Items)
			{
				a_Combo.Items.Add(currItem);

				if (currItem.Data == a_SelectedData)
					selectedItem = currItem;
			}

			// Force items to be sorted right now
			a_Combo.Sorted = false;
			a_Combo.Sorted = true;

			// Choose selected item only after sorting
			if (null != selectedItem)
				a_Combo.SelectedItem = selectedItem;
			else
				a_Combo.SelectedIndex = 0;
		}

		#region EveCentralCom
		private void Init_EveCentralCom_CmbRegion()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			items.Add(new TextItemWithUInt32("[All regions]", 0));

			foreach (EveRegion currRegion in m_EveDatabase.GetRegions())
			{
				items.Add(new TextItemWithUInt32(currRegion.Name, currRegion.ID));
			}

			PopulateCombo(Cmb_EveCentralCom_Region, items, m_Settings.EveCentralCom.RegionID);
		}

		private void Init_EveCentralCom_CmbSolar()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			items.Add(new TextItemWithUInt32("[All solar systems]", 0));

			UInt32 regionID = TextItemWithUInt32.GetData(Cmb_EveCentralCom_Region.SelectedItem);
			foreach (EveSolarSystem currSystem in m_EveDatabase.GetSolarSystems(regionID))
			{
				items.Add(new TextItemWithUInt32(currSystem.Name, currSystem.ID));
			}

			PopulateCombo(Cmb_EveCentralCom_Solar, items, m_Settings.EveCentralCom.SolarID);
		}

		private void Init_EveCentralCom_CmbPriceType()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			foreach (PriceTypes priceType in PriceProviderEveCentralCom.GetSupportedPriceTypes())
			{
				String enumName = Engine.GetPriceTypeName(priceType);
				items.Add(new TextItemWithUInt32(enumName, (UInt32)priceType));
			}

			PopulateCombo(Cmb_EveCentralCom_PriceType, items, (UInt32)m_Settings.EveCentralCom.PriceType);
		}

		private void CmbRegion_SelectedIndexChanged(object sender, EventArgs e)
		{
			Init_EveCentralCom_CmbSolar();
		}
		#endregion

		#region FuzzworkCoUk
		private void Init_FuzzworkCoUk_RadSourceType()
		{
			if (m_Settings.FuzzworkCoUk.IsRegion)
				Rad_FuzzworkCoUk_Region.Checked = true;
			else
				Rad_FuzzworkCoUk_Station.Checked = true;

			// Enable_FuzzworkCoUk_SourceCombo();	// Will be called by 'Checked = true'
		}

		private void Enable_FuzzworkCoUk_SourceCombo()
		{
			Cmb_FuzzworkCoUk_Region.Enabled		= Rad_FuzzworkCoUk_Region.Checked;
			Cmb_FuzzworkCoUk_Station.Enabled	= Rad_FuzzworkCoUk_Station.Checked;
		}

		private void Init_FuzzworkCoUk_CmbRegion()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			foreach (EveRegion currRegion in m_EveDatabase.GetRegions())
			{
				items.Add(new TextItemWithUInt32(currRegion.Name, currRegion.ID));
			}

			PopulateCombo(Cmb_FuzzworkCoUk_Region, items, m_Settings.FuzzworkCoUk.RegionID);
		}

		private void Init_FuzzworkCoUk_CmbStation()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			foreach (UInt32 stationID in PriceProviderFuzzworkCoUk.GetSupportedStations())
			{
				String stationName = m_EveDatabase.GetLocationName(0, 0, stationID);
				items.Add(new TextItemWithUInt32(stationName, stationID));
			}

			PopulateCombo(Cmb_FuzzworkCoUk_Station, items, m_Settings.FuzzworkCoUk.StationID);
		}

		private void Init_FuzzworkCoUk_CmbPriceType()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			foreach (PriceTypes priceType in PriceProviderFuzzworkCoUk.GetSupportedPriceTypes())
			{
				String enumName = Engine.GetPriceTypeName(priceType);
				items.Add(new TextItemWithUInt32(enumName, (UInt32)priceType));
			}

			PopulateCombo(Cmb_FuzzworkCoUk_PriceType, items, (UInt32)m_Settings.FuzzworkCoUk.PriceType);
		}

		private void FuzzworkCoUk_RadSource_CheckedChanged(object sender, EventArgs e)
		{
			Enable_FuzzworkCoUk_SourceCombo();
		}
		#endregion

		#region EveMarketdataCom
		private void EveMarketdataCom_Init_CmbRegion()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			foreach (EveRegion currRegion in m_EveDatabase.GetRegions())
			{
				items.Add(new TextItemWithUInt32(currRegion.Name, currRegion.ID));
			}

			PopulateCombo(Cmb_EveMarketdataCom_Region, items, m_Settings.EveMarketdataCom.RegionID);
		}

		private void EveMarketdataCom_Init_CmbSolar()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			items.Add(new TextItemWithUInt32("[All solar systems]", 0));

			if (null == Cmb_EveMarketdataCom_Region.SelectedItem)
				return; // Workaround for premature call from PopulateCombo()

			UInt32 regionID = TextItemWithUInt32.GetData(Cmb_EveMarketdataCom_Region.SelectedItem);
			foreach (EveSolarSystem currSystem in m_EveDatabase.GetSolarSystems(regionID))
			{
				items.Add(new TextItemWithUInt32(currSystem.Name, currSystem.ID));
			}

			PopulateCombo(Cmb_EveMarketdataCom_Solar, items, m_Settings.EveMarketdataCom.SolarID);
		}

		private void EveMarketdataCom_Init_CmbStation()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			items.Add(new TextItemWithUInt32("[All stations]", 0));

			if (null == Cmb_EveMarketdataCom_Solar.SelectedItem)
				return; // Workaround for premature call from PopulateCombo()

			UInt32 solarID = TextItemWithUInt32.GetData(Cmb_EveMarketdataCom_Solar.SelectedItem);
			foreach (EveStation currStation in m_EveDatabase.GetStations(solarID))
			{
				items.Add(new TextItemWithUInt32(currStation.Name, currStation.ID));
			}

			PopulateCombo(Cmb_EveMarketdataCom_Station, items, m_Settings.EveMarketdataCom.StationID);
		}

		private void EveMarketdataCom_Init_CmbPriceType()
		{
			List<TextItemWithUInt32> items = new List<TextItemWithUInt32>();
			foreach (PriceTypes priceType in PriceProviderEveMarketdataCom.GetSupportedPriceTypes())
			{
				String enumName = Engine.GetPriceTypeName(priceType);
				items.Add(new TextItemWithUInt32(enumName, (UInt32)priceType));
			}

			PopulateCombo(Cmb_EveMarketdataCom_PriceType, items, (UInt32)m_Settings.EveMarketdataCom.PriceType);
		}

		private void Cmb_EveMarketdataCom_Region_SelectedIndexChanged(object sender, EventArgs e)
		{
			EveMarketdataCom_Init_CmbSolar();
		}

		private void Cmb_EveMarketdataCom_Solar_SelectedIndexChanged(object sender, EventArgs e)
		{
			EveMarketdataCom_Init_CmbStation();
		}
		#endregion

		private void BtnOk_Click(object sender, EventArgs e)
		{
			switch (TabProviders.SelectedIndex)
			{
				case 0:
					m_Settings.Provider				= PriceProviders.EveCentral;
					break;
				case 1:
					m_Settings.Provider             = PriceProviders.FuzzworkCoUk;
					break;
				case 2:
					m_Settings.Provider             = PriceProviders.EveMarketdataCom;
					break;
				default:
					Debug.Assert(false, "Wrong tab page selected");
					break;
			}

			m_Settings.EveCentralCom.RegionID		= TextItemWithUInt32.GetData(Cmb_EveCentralCom_Region.SelectedItem);
			m_Settings.EveCentralCom.SolarID		= TextItemWithUInt32.GetData(Cmb_EveCentralCom_Solar.SelectedItem);
			m_Settings.EveCentralCom.PriceType		= (PriceTypes)TextItemWithUInt32.GetData(Cmb_EveCentralCom_PriceType.SelectedItem);
			m_Settings.EveCentralCom.HistoryDays	= (UInt32)Txt_EveCentralCom_PriceHistory.Value;

			m_Settings.FuzzworkCoUk.IsRegion        = Rad_FuzzworkCoUk_Region.Checked;
			m_Settings.FuzzworkCoUk.RegionID		= TextItemWithUInt32.GetData(Cmb_FuzzworkCoUk_Region.SelectedItem);
			m_Settings.FuzzworkCoUk.StationID		= TextItemWithUInt32.GetData(Cmb_FuzzworkCoUk_Station.SelectedItem);
			m_Settings.FuzzworkCoUk.PriceType		= (PriceTypes)TextItemWithUInt32.GetData(Cmb_FuzzworkCoUk_PriceType.SelectedItem);

			m_Settings.EveMarketdataCom.RegionID	= TextItemWithUInt32.GetData(Cmb_EveMarketdataCom_Region.SelectedItem);
			m_Settings.EveMarketdataCom.SolarID		= TextItemWithUInt32.GetData(Cmb_EveMarketdataCom_Solar.SelectedItem);
			m_Settings.EveMarketdataCom.StationID	= TextItemWithUInt32.GetData(Cmb_EveMarketdataCom_Station.SelectedItem);
			m_Settings.EveMarketdataCom.PriceType	= (PriceTypes)TextItemWithUInt32.GetData(Cmb_EveMarketdataCom_PriceType.SelectedItem);

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
