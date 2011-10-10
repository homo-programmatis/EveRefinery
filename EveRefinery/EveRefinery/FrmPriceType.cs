using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpecialFNs;

namespace EveRefinery
{
	public partial class FrmPriceType : Form
	{
		protected	EveDatabase		m_EveDatabase;
		public		PriceSettings	m_Settings;

		public FrmPriceType(EveDatabase a_EveDatabase)
		{
			m_EveDatabase			= a_EveDatabase;

			InitializeComponent();
		}

		private void FrmPriceType_Load(object sender, EventArgs e)
		{
			CmbPriceType.DrawItem += Engine.DrawPriceTypeItem;

			Init_CmbRegion();
			Init_CmbPriceType();
		}

		private void Init_CmbRegion()
		{
			ComboBox currCombo = CmbRegion;
			currCombo.Items.Clear();

			TextItemWithUInt32 allItem = new TextItemWithUInt32("[All regions]", 0);
			currCombo.Items.Add(allItem);

			List<EveRegion> regions = m_EveDatabase.GetRegions();
			foreach (EveRegion currRegion in regions)
			{
				TextItemWithUInt32 newItem = new TextItemWithUInt32(currRegion.Name, currRegion.ID);
				currCombo.Items.Add(newItem);

				if (currRegion.ID == m_Settings.RegionID)
					currCombo.SelectedItem = newItem;
			}

			if (currCombo.SelectedItem == null)
				currCombo.SelectedItem = allItem;
		}

		private void Init_CmbSolar()
		{
			UInt32 regionID		= TextItemWithUInt32.GetData(CmbRegion.SelectedItem);

			ComboBox currCombo = CmbSolar;
			currCombo.Items.Clear();

			TextItemWithUInt32 allItem = new TextItemWithUInt32("[All solar systems]", 0);
			currCombo.Items.Add(allItem);

			List<EveSolarSystem> systems = m_EveDatabase.GetSolarSystems(regionID);
			foreach (EveSolarSystem currSystem in systems)
			{
				TextItemWithUInt32 newItem = new TextItemWithUInt32(currSystem.Name, currSystem.ID);
				currCombo.Items.Add(newItem);

				if (currSystem.ID == m_Settings.SolarID)
					currCombo.SelectedItem = newItem;
			}

			if (currCombo.SelectedItem == null)
				currCombo.SelectedItem = allItem;
		}

		private void Init_CmbPriceType()
		{
			ComboBox currCombo = CmbPriceType;
			currCombo.Items.Clear();

			for (UInt32 i = 0; i < (UInt32)PriceTypes.MaxPriceTypes; i++)
			{
				string enumName = Engine.GetPriceTypeName((PriceTypes)i);
				TextItemWithUInt32 newItem = new TextItemWithUInt32(enumName, i);
				currCombo.Items.Add(newItem);

				if ((PriceTypes)newItem.Data == m_Settings.PriceType)
					currCombo.SelectedItem = newItem;
			}
		}

		private void CmbRegion_SelectedIndexChanged(object sender, EventArgs e)
		{
			Init_CmbSolar();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			m_Settings.RegionID		= TextItemWithUInt32.GetData(CmbRegion.SelectedItem);
			m_Settings.SolarID		= TextItemWithUInt32.GetData(CmbSolar.SelectedItem);
			m_Settings.PriceType	= (PriceTypes)TextItemWithUInt32.GetData(CmbPriceType.SelectedItem);

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
