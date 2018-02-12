using System;
using System.Windows.Forms;

namespace EveRefinery
{
	public partial class FrmInvalidDB : Form
	{
		public string m_SelectedDbPath;

		private string m_CurrDbPath;
	
		public FrmInvalidDB(string a_CurrDbPath)
		{
			m_CurrDbPath = a_CurrDbPath;
			InitializeComponent();
		}

		private void FrmInvalidDB_Load(object sender, EventArgs e)
		{
			LblBadDatabase.Text = m_CurrDbPath;
		}

		private void BtnBrowseForDB_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Title = "Browse for EVE database";
			fileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			fileDialog.Filter = "All files (*.*)|*.*|Databases (*.db)|*.db";
			fileDialog.FilterIndex = 2;
			fileDialog.RestoreDirectory = true;
			if (DialogResult.OK != fileDialog.ShowDialog())
				return;
				
			m_SelectedDbPath = fileDialog.FileName;
			Close();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			m_SelectedDbPath = null;
			Close();
		}
	}
}
