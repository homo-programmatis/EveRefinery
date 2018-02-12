using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EveRefinery
{
    static class Program
    {
		public static String GetProgramFolder()
		{
			return System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";
		}

		public static String GetCacheFolder()
		{
			return GetProgramFolder() + "Cache\\";
		}

		static void Upgrade_MarketPrices()
		{
			// Delete old format market prices
			File.Delete(GetProgramFolder() + "MarketPrices.db3");
		}

		// Move files previously residing in program folder into cache folder
		static void Upgrade_MoveToCacheFolder()
		{
			String programFolder = GetProgramFolder();
			String cacheFolder = GetCacheFolder();

			List<String> movedFiles = new List<String>();
			movedFiles.AddRange(Directory.GetFiles(programFolder, "Assets_*.xml"));
			movedFiles.Add(programFolder + "MarketPrices_v1.db3");

			foreach (String filePath in movedFiles)
			{
				try
				{
					String cachedPath = cacheFolder + Path.GetFileName(filePath);
					File.Move(filePath, cachedPath);
				}
				catch (System.Exception a_Exception)
				{
					// Ignore any exceptions
					System.Diagnostics.Debug.WriteLine(a_Exception.Message);
				}
			}
		}

		static void UpgradeFromOldVersion()
		{
			Upgrade_MarketPrices();
			Upgrade_MoveToCacheFolder();
		}
		
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			Directory.CreateDirectory(GetCacheFolder());
			UpgradeFromOldVersion();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FrmMain());
        }
    }
}
