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
	public partial class FrmRefineCalc : Form
	{
		public double	m_RefineryEfficiency;
		public double	m_TaxesTaken;

		protected TrackAndNumericPair m_StationEquipment;
		protected TrackAndNumericPair m_RefiningSkill;
		protected TrackAndNumericPair m_RefineryEfficiencySkill;
		protected TrackAndNumericPair m_SpecialtySkill;
		protected TrackAndNumericPair m_StationStandings;
	
		public FrmRefineCalc()
		{
			InitializeComponent();
		}

		private void FrmRefineCalc_Load(object sender, EventArgs e)
		{
			m_StationEquipment			= new TrackAndNumericPair(TxtStationEquipment, TrkStationEquipment, OnValueChange);
			m_RefiningSkill				= new TrackAndNumericPair(TxtRefiningSkill, TrkRefiningSkill, OnValueChange);
			m_RefineryEfficiencySkill	= new TrackAndNumericPair(TxtRefineryEfficiencySkill, TrkRefineryEfficiencySkill, OnValueChange);
			m_SpecialtySkill			= new TrackAndNumericPair(TxtSpecialtySkill, TrkSpecialtySkill, OnValueChange);
			m_StationStandings			= new TrackAndNumericPair(TxtStationStandings, TrkStationStandings, OnValueChange);
		
			Recalculate();
		}

		private void Recalculate()
		{
			double stationEquipment		= (double)TxtStationEquipment.Value / 100;
			double refining				= 1.00 + (double)TxtRefiningSkill.Value * 0.02;
			double refineryEfficiency	= 1.00 + (double)TxtRefineryEfficiencySkill.Value * 0.04;
			double specialty			= 1.00 + (double)TxtSpecialtySkill.Value * 0.05;
			double standings			= (double)TxtStationStandings.Value;

			m_RefineryEfficiency		= stationEquipment + (0.375 * refining * refineryEfficiency * specialty);
			m_TaxesTaken				= 0.05 * (6.6667 - standings) / 6.6667;

			if (m_RefineryEfficiency < 0)
				m_RefineryEfficiency = 0;
			else if (m_RefineryEfficiency > 1.00)
				m_RefineryEfficiency = 1.00;

			if (m_TaxesTaken < 0)
				m_TaxesTaken = 0;
			else if (m_TaxesTaken > 0.05)
				m_TaxesTaken = 0.05;

			LblEfficiency.Text	= String.Format("{0:00.00}%", m_RefineryEfficiency * 100);
			LblTaxes.Text		= String.Format("{0:0.00}%", m_TaxesTaken * 100);
		}
		
		private void OnValueChange(object sender, EventArgs e)
		{
			Recalculate();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
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
