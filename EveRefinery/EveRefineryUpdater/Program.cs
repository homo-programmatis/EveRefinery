using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SpecialFNs;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace EveRefineryUpdater
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] a_Args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//string	m_UpdateXmlUrl	= @"C:\Users\HOMO_PROGRAMMATIS\Desktop\EveRefineryTask\Package\updates.xml";
			//string	m_UpdateXmlUrl	= @"http://www.homo-programmatis.com/EveRefinery/updates.xml";
			
			string	paranoidString	= "If_you_feel_paranoid_Read=http://www.homo-programmatis.com/EveRefinery/userid.html";

			Updater updater			= new Updater();
			string	updateXmlUrl	= "http://www.homo-programmatis.com/EveRefinery/updates.php?" + paranoidString + "&userid=" + ComputerID.GetComputerID();
			string	updatedApp		= "EveRefinery";
			bool	isSilent		= a_Args.Contains("/silent");
			bool	showForm		= true;
			
			try
			{
				updater.LoadFileList(updateXmlUrl);
				updater.MakeDecisions();
			}
			catch (System.Exception a_Exception)
			{
				if (!isSilent)
				{
					string errorHeader = "Update failed.\nIf it's not clear how to fix the error, please try again later.\n\nThe reported error is:\n";
					ErrorMessageBox.Show(errorHeader + a_Exception.Message);
				}
				
				showForm = false;
			}

			if (isSilent && showForm)
				showForm = updater.IsUpdateNeeded();

			if (showForm)
				Application.Run(new FrmMain(updater, updateXmlUrl, updatedApp));
		}
	}
}
