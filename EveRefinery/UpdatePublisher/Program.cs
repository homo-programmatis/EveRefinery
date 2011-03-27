using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using SpecialFNs;

namespace UpdatePublisher
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				DoWork();
			}
			catch (System.Exception a_Exception)
			{
				Console.WriteLine("EXCEPTION: " + a_Exception.Message);
			}

			Console.WriteLine("Done");
		}
		
		static void DoWork()
		{
			List<string> publishedFiles = new List<string>();
			publishedFiles.Add(@"C:\Program Files (x86)\SQLite.NET\bin\System.Data.SQLite.dll");
			publishedFiles.Add(@"Q:\SourceForge\everefinery\EveRefinery\bin\Release\EveDatabase.db");
			publishedFiles.Add(@"Q:\SourceForge\everefinery\EveRefinery\bin\Release\EveRefinery.exe");
			publishedFiles.Add(@"Q:\SourceForge\everefinery\EveRefinery\bin\Release\EveRefineryUpdater.exe");
			publishedFiles.Add(@"Q:\SourceForge\everefinery\EveRefinery\EveRefinery\whatsnew.txt");

			Publisher publisher = new Publisher();
			foreach (string currFile in publishedFiles)
			{
				publisher.PublishFile(currFile);
			}

			publisher.SaveUpdatesXml();
		}
	}
	
	class Publisher
	{
		public string	m_AttribName_Name		= "name";
		public string	m_AttribName_FileHash	= "filehash";
		public string	m_AttribName_PackHash	= "packedhash";
		public string	m_AttribName_Url		= "url";

		public string		m_7z_Location		= @"C:\Program Files (x86)\7-Zip\7z.exe";
		public string		m_OutputDir			= @"Q:\SourceForge\EveRefinery_Package\";
		public string		m_DownloadUrl		= "http://www.homo-programmatis.com/EveRefinery/Files/";
		public string		m_UpdatesXmlName	= "updates.xml";
		public XmlDocument	m_UpdatesXml		= new XmlDocument();
		public XmlNode		m_RootNode;
		
		public Publisher()
		{
			ReloadUpdatesXml();
			Directory.CreateDirectory(m_OutputDir);
		}
		
		public void ReloadUpdatesXml()
		{
			m_UpdatesXml = new XmlDocument();

			try
			{
				m_UpdatesXml.Load(m_OutputDir + m_UpdatesXmlName);
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
		
		protected XmlNode GetFileNode(string a_FileName)
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
		
		public void PublishFile(string a_FilePath)
		{
			string fileName = Path.GetFileName(a_FilePath);
			string packName = fileName + ".gz";
			string packPath = m_OutputDir + packName;
			
			XmlNode fileNode = GetFileNode(fileName);

			string fileHash = Utility.BytesToHex(Utility.Md5File(a_FilePath));
			if (fileHash == fileNode.Attributes[m_AttribName_FileHash].Value)
			{
				Console.WriteLine("[Old] " + fileName);
				return;
			}

			Console.WriteLine("[Upd] " + fileName);

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
			m_UpdatesXml.Save(m_OutputDir + m_UpdatesXmlName);
		}
	}
}
