using System;
using System.Windows.Forms;

namespace EveRefinery
{
	public partial class FrmAddNewApiKey : Form
	{
		public string	m_UserID;
		public string	m_ApiKey;
	
		public FrmAddNewApiKey()
		{
			InitializeComponent();
		}

		private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			m_UserID = TxtUserID.Text;
			m_ApiKey = TxtApiKey.Text;
			
			m_UserID.Trim();
			m_ApiKey.Trim();
			
			if ((m_UserID == "") || (m_ApiKey == ""))
			{
				MessageBox.Show("You must fill all fields");
				return;
			}
			
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
