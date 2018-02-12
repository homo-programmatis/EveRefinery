using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpecialFNs;

namespace EveRefineryUpdater
{
	public partial class FrmMain : Form
	{
		private Updater m_Updater;
		private string	m_UpdateXmlUrl;
		private string	m_UpdatedApp;

		public FrmMain(Updater a_Updater, string a_UpdateXmlUrl, string a_UpdatedApp)
		{
			m_Updater		= a_Updater;
			m_UpdateXmlUrl	= a_UpdateXmlUrl;
			m_UpdatedApp	= a_UpdatedApp;
		
			InitializeComponent();
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			UpdateList();
		}

		private void BtnUpdate_Click(object sender, EventArgs e)
		{
			DoUpdates();
		}

		private void DoUpdates()
		{
			try
			{
				BtnUpdate.Enabled = false;
				TmrUpdateList.Enabled = true;

				m_Updater.LoadFileList(m_UpdateXmlUrl);
				m_Updater.MakeDecisions();
				UpdateList();
				
				m_Updater.StartThreads();
			}
			catch (System.Exception a_Exception)
			{
				string errorHeader = "Update failed.\nIf it's not clear how to fix the error, please try again later.\n\nThe reported error is:\n";
				ErrorMessageBox.Show(errorHeader + a_Exception.Message);
			}
		}

		private void SetItemStatus(ListViewItem a_Item, Updater.UpdatedFile a_File)
		{
			Debug.Assert(Enum.GetValues(typeof(Updater.FileStatus)).GetLength(0) == 7);
			
			Color colorGood = Color.FromArgb(0, 192, 0);
			Color colorWork = Color.FromArgb(192, 192, 0);
			Color colorBad	= Color.FromArgb(192, 0, 0);
			
			switch (a_File.Status)
			{
				case Updater.FileStatus.Initial:
					a_Item.BackColor = colorWork;
					a_Item.SubItems.Add("Not processed yet");
					break;
				case Updater.FileStatus.AlreadyOk:
					a_Item.BackColor = colorGood;
					a_Item.SubItems.Add("Up to date");
					break;
				case Updater.FileStatus.NeedsUpdate:
					a_Item.BackColor = colorBad;
					a_Item.SubItems.Add("Needs update");
					break;
				case Updater.FileStatus.Downloading:
					a_Item.BackColor = colorWork;
					if (a_File.BytesTotal == 0)
						a_Item.SubItems.Add("Download pending...");
					else
						a_Item.SubItems.Add(String.Format(new FileSizeFormatProvider(), "Downloading: {0:fs} / {1:fs}", a_File.BytesDone, a_File.BytesTotal));
					break;
				case Updater.FileStatus.Downloaded:
					a_Item.BackColor = colorWork;
					a_Item.SubItems.Add("Downloaded");
					break;
				case Updater.FileStatus.Updated:
					a_Item.BackColor = colorGood;
					a_Item.SubItems.Add(String.Format("Updated (restart {0:s} to take effect)", m_UpdatedApp));
					break;
				case Updater.FileStatus.Error:
					a_Item.BackColor = colorBad;

					if (null == a_File.LastException)
						a_Item.SubItems.Add(String.Format("Error: {0:s}", a_File.LastError));
					else if (a_File.LastException is System.IO.IOException)
					{
						a_Item.BackColor = colorWork;
						a_Item.SubItems.Add(String.Format("Close {1:s} and press Update again (Error: {0:s})", a_File.LastException.Message, m_UpdatedApp));
					}
					else
						a_Item.SubItems.Add(String.Format("Error: {0:s}", a_File.LastException.Message));
				
					break;
				default:
					Debug.Assert(false, "Unknown FileStatus value");
					a_Item.SubItems.Add("Unknown");
					a_Item.BackColor = colorBad;
					break;
			}
		}

		
		private void UpdateList()
		{
			int[] oldSelection = new int[LstFiles.SelectedIndices.Count];
			LstFiles.SelectedIndices.CopyTo(oldSelection, 0);
			LstFiles.Items.Clear();
		
			for (int i = 0; i < m_Updater.m_Files.Count; i++)
			{
				Updater.UpdatedFile currFile = m_Updater.m_Files[i];
			
				ListViewItem newItem = new ListViewItem(currFile.FileName);
				SetItemStatus(newItem, currFile);
				if (oldSelection.Contains(i))
					newItem.Selected = true;

				LstFiles.Items.Add(newItem);
			}
		}

		private void TmrUpdateList_Tick(object sender, EventArgs e)
		{
			if (0 == m_Updater.GetRunningThreadCount())
			{
				TmrUpdateList.Enabled = false;
				m_Updater.FinalizeUpdate();
			}

			BtnUpdate.Enabled = !TmrUpdateList.Enabled;
			UpdateList();
		}
	}
}
