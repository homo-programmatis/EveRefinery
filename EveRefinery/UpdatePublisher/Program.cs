using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SpecialFNs;

namespace UpdatePublisher
{
	class Program
	{
		static void Main(string[] a_CommandLineArgs)
		{
            if (a_CommandLineArgs.Length < 2)
            {
                String exeFileName = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location);
                Console.WriteLine("Usage:");
                Console.WriteLine("{0} PathToEveRefinerySln OutputPath", exeFileName);
                Console.WriteLine("----");
                Console.WriteLine("PathToEveRefinerySln - Path to EveRefinery.sln file");
                Console.WriteLine("OutputPath           - Folder where to store results");
            }

            try
			{
				DoWork(a_CommandLineArgs[0], a_CommandLineArgs[1]);
			}
			catch (System.Exception a_Exception)
			{
				Console.WriteLine("EXCEPTION: " + a_Exception.Message);
			}

			Console.WriteLine("Done");
		}
		
		static void DoWork(String a_PathToEveRefinerySln, String a_OutputDir)
		{
            if (0 != String.Compare("EveRefinery.sln", Path.GetFileName(a_PathToEveRefinerySln), true))
                throw new System.ArgumentException("You must specify path to EveRefinery.sln");

            if (!File.Exists(a_PathToEveRefinerySln))
                throw new System.ArgumentException("Specified EveRefinery.sln does not exist");

            ////////////////////////////////////////////////////////////////////////////////

            String folderSolution = Path.GetDirectoryName(a_PathToEveRefinerySln);

            ////////////////////////////////////////////////////////////////////////////////

            Publisher publisher = new Publisher();
            publisher.SetOutputDir(a_OutputDir);
            publisher.SetDownloadUrl("http://www.homo-programmatis.com/EveRefinery/Files/");

			publisher.PublishFile(Path.Combine(folderSolution, @"..\ThirdParty\Newtonsoft.Json\Newtonsoft.Json.dll"));
			publisher.PublishFile(Path.Combine(folderSolution, @"..\ThirdParty\System.Data.SQLite\System.Data.SQLite.dll"));
			publisher.PublishFile(Path.Combine(folderSolution, @"bin\Release\EveDatabase.db"));
			publisher.PublishFile(Path.Combine(folderSolution, @"bin\Release\EveRefinery.exe"));
			publisher.PublishFile(Path.Combine(folderSolution, @"bin\Release\EveRefineryUpdater.exe"));
            publisher.PublishFile(Path.Combine(folderSolution, @"EveRefinery\whatsnew.txt"));

			publisher.SaveUpdatesXml();
		}
	}
	
	class Publisher
	{
		public string	m_AttribName_Name		= "name";
		public string	m_AttribName_FileHash	= "filehash";
		public string	m_AttribName_PackHash	= "packedhash";
		public string	m_AttribName_Url		= "url";

		public string		m_7z_Location;
		public string		m_OutputDir;
		public string		m_DownloadUrl;
		public string		m_UpdatesXmlName	= "updates.xml";
		public XmlDocument	m_UpdatesXml		= new XmlDocument();
		public XmlNode		m_RootNode;
		
		public          Publisher()
		{
            Locate7zip();
		}

        public void     SetDownloadUrl(String a_Url)
        {
            m_DownloadUrl = a_Url;
        }

        public void     SetOutputDir(String a_OutputDir)
        {
            m_OutputDir = a_OutputDir;
            Directory.CreateDirectory(m_OutputDir);
            ReloadUpdatesXml();
        }

        public void     PublishFile(string a_FilePath)
		{
			string fileName = Path.GetFileName(a_FilePath);
			string packName = fileName + ".gz";
			string packPath = Path.Combine(m_OutputDir, packName);
			
			XmlNode fileNode = GetFileNode(fileName);

			string fileHash = Utility.BytesToHex(Utility.Md5File(a_FilePath));
			if (fileHash == fileNode.Attributes[m_AttribName_FileHash].Value)
			{
				Console.WriteLine("[Old] " + fileName);
				return;
			}

			Console.WriteLine("[Upd] " + fileName);
			if (!GetUserConfirm("Update this file?"))
				return;

			Process packer = new Process();
			packer.StartInfo.FileName	= m_7z_Location;
			packer.StartInfo.Arguments	= "a \"" + packPath + "\" \"" + a_FilePath + "\" -tgzip -mx9";
			packer.Start();
			packer.WaitForExit();
			if (0 != packer.ExitCode)
			{
				string message = String.Format("Failed to pack file {0:s} with code {1:d}", fileName, packer.ExitCode);
				throw new System.IO.IOException(message);
			}

			string packHash = Utility.BytesToHex(Utility.Md5File(packPath));

			fileNode.Attributes[m_AttribName_Name].Value		= fileName;
			fileNode.Attributes[m_AttribName_FileHash].Value	= fileHash;
			fileNode.Attributes[m_AttribName_PackHash].Value	= packHash;
			fileNode.Attributes[m_AttribName_Url].Value			= m_DownloadUrl + packName;
		}

        public void SaveUpdatesXml()
        {
            m_UpdatesXml.Save(ComposeUpdatesXmlPath());
        }

        #region Private functions
        private void    Locate7zip()
        {
            String[] possibleLocations =
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),    @"7-Zip\7z.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"7-Zip\7z.exe"),
            };

            foreach (String possibleLocation in possibleLocations)
            {
                if (File.Exists(possibleLocation))
                {
                    m_7z_Location = possibleLocation;
                    return;
                }
            }

            throw new System.Exception("7z.exe not found");
        }

        private String  ComposeUpdatesXmlPath()
        {
            return Path.Combine(m_OutputDir, m_UpdatesXmlName);
        }

        private void    ReloadUpdatesXml()
        {
            m_UpdatesXml = new XmlDocument();

            try
            {
                m_UpdatesXml.Load(ComposeUpdatesXmlPath());
            }
            catch (System.Exception a_Exception)
            {
                Debug.WriteLine(a_Exception.Message);
            }

            XmlNodeList rootNodes = m_UpdatesXml.GetElementsByTagName("files");
            if (0 != rootNodes.Count)
                m_RootNode = rootNodes[0];
            else
            {
                m_RootNode = m_UpdatesXml.CreateNode("element", "files", "");
                m_UpdatesXml.AppendChild(m_RootNode);
            }
        }

        private XmlNode GetFileNode(string a_FileName)
        {
            string fileXPath = "/files/file[@" + m_AttribName_Name + "='" + a_FileName + "']";
            XmlNode fileNode = m_UpdatesXml.SelectSingleNode(fileXPath);
            if (null == fileNode)
            {
                fileNode = m_UpdatesXml.CreateNode("element", "file", "");
                fileNode.Attributes.Append(m_UpdatesXml.CreateAttribute(m_AttribName_Name));
                fileNode.Attributes.Append(m_UpdatesXml.CreateAttribute(m_AttribName_FileHash));
                fileNode.Attributes.Append(m_UpdatesXml.CreateAttribute(m_AttribName_PackHash));
                fileNode.Attributes.Append(m_UpdatesXml.CreateAttribute(m_AttribName_Url));

                m_RootNode.AppendChild(fileNode);
            }

            return fileNode;
        }

        private bool    GetUserConfirm(String a_Question)
        {
            Console.WriteLine(a_Question + " (Y/N)");

            for (;;)
            {
                Char reply = Console.ReadKey(true).KeyChar;
                switch (reply)
                {
                    case 'Y':
                    case 'y':
                        return true;
                    case 'N':
                    case 'n':
                        return false;
                }
            }
        }
        #endregion
    }
}
