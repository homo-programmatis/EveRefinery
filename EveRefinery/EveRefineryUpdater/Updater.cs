using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Threading;
using SpecialFNs;
using System.Diagnostics;
using System.Net;
using System.ComponentModel;
using System.IO.Compression;

namespace EveRefineryUpdater
{
	public class Updater
	{
		public enum FileStatus
		{
			Initial,
			AlreadyOk,
			NeedsUpdate,
			Downloading,
			Downloaded,
			Updated,
			Error,
		}
		
		public class UpdatedFile
		{
			public string		FileName;
			public string		FilePath;
			public string		SourcePath;
			public string		DownloadedPath;
			public string		FileHash;
			public string		PackedHash;
			public FileStatus	Status;
			public string		LastError;
			public Exception	LastException;
			public UInt64		BytesDone;
			public UInt64		BytesTotal;
		}
		
		public List<UpdatedFile>	m_Files		= new List<UpdatedFile>();
		public List<Thread>			m_Threads	= new List<Thread>();
		protected string			m_DownloadPath;		// without slash
		protected string			m_BackupPath;		// without slash

		public Updater()
		{
			m_DownloadPath	= Path.GetDirectoryName(Application.ExecutablePath) + "\\Updater";
			m_BackupPath	= Path.GetDirectoryName(Application.ExecutablePath) + "\\Backup";

			ServicePointManager.DefaultConnectionLimit = 20;
		}
		
		public void LoadFileList(string a_UpdateXmlUrl)
		{
			XmlDocument updatesXml = new XmlDocument();
			updatesXml.Load(a_UpdateXmlUrl);
			
			XmlNodeList xmlFiles = updatesXml.GetElementsByTagName("file");
			m_Files.Clear();
			
			foreach (XmlNode currNode in xmlFiles)
			{
				UpdatedFile newFile		= new UpdatedFile();
				newFile.Status			= FileStatus.Initial;
				newFile.FileName		= currNode.Attributes["name"].Value;
				newFile.FilePath		= Path.GetDirectoryName(Application.ExecutablePath) + "\\" + newFile.FileName;
				newFile.SourcePath		= currNode.Attributes["url"].Value;
				newFile.FileHash		= currNode.Attributes["filehash"].Value;
				newFile.PackedHash		= currNode.Attributes["packedhash"].Value;
				newFile.DownloadedPath	= m_DownloadPath + "\\" + newFile.FileName + ".gz";
				
				m_Files.Add(newFile);
			}
		}
		
		static bool CheckFileHash(UpdatedFile a_File, string a_FilePath, bool a_IsPacked)
		{
			try
			{
				string wantedHash		= a_IsPacked ? a_File.PackedHash : a_File.FileHash;
				string actualHash		= Utility.BytesToHex(Utility.Md5File(a_FilePath));
				return (actualHash == wantedHash);
			}
			catch (System.Exception a_Exception)
			{
				a_File.Status			= FileStatus.Error;
				a_File.LastException	= a_Exception;
				return false;
			}
		}

		public void MakeDecisions()
		{
			foreach (UpdatedFile currFile in m_Files)
			{
				// Is already up to date?
				if (File.Exists(currFile.FilePath))
				{
					if (CheckFileHash(currFile, currFile.FilePath, false))
					{
						currFile.Status = FileStatus.AlreadyOk;
						continue;
					}
					else if (currFile.Status == FileStatus.Error)
						continue;
				}
				
				// Is already downloaded?
				if (File.Exists(currFile.DownloadedPath))
				{
					if (CheckFileHash(currFile, currFile.DownloadedPath, true))
					{
						currFile.Status = FileStatus.Downloaded;
						continue;
					}
					else if (currFile.Status == FileStatus.Error)
						continue;
				}

				currFile.Status = FileStatus.NeedsUpdate;
			}
		}

		public void StartThreads()
		{
			foreach (UpdatedFile currFile in m_Files)
			{
				ThreadWithParam threadParam = new ThreadWithParam();
				threadParam.Function	= FileUpdaterThread;
				threadParam.Parameter	= currFile;
				Thread newThread		= threadParam.CreateThread();
				newThread.IsBackground	= true;
				newThread.Start();
				m_Threads.Add(newThread);
			}
		}

		private static void DownloadProgressCB(object a_Sender, DownloadProgressChangedEventArgs a_Args)
		{
			UpdatedFile a_File	= (UpdatedFile)a_Args.UserState;
			a_File.BytesDone	= (UInt64)a_Args.BytesReceived;
			a_File.BytesTotal	= (UInt64)a_Args.TotalBytesToReceive;
		}

		private static void DownloadCompletedCB(object a_Sender, AsyncCompletedEventArgs a_Args)
		{
			UpdatedFile a_File	= (UpdatedFile)a_Args.UserState;
			a_File.Status		= FileStatus.Downloaded;
		}

		void DownloadFile(UpdatedFile a_File)
		{
			Directory.CreateDirectory(m_DownloadPath);
			File.Delete(a_File.DownloadedPath);
		
			WebClient webClient = new WebClient();
			webClient.DownloadFileCompleted		+= new AsyncCompletedEventHandler(DownloadCompletedCB);
			webClient.DownloadProgressChanged	+= new DownloadProgressChangedEventHandler(DownloadProgressCB);
			webClient.DownloadFileAsync(new Uri(a_File.SourcePath), a_File.DownloadedPath, a_File);
			a_File.Status = FileStatus.Downloading;
		}
		
		void ExtractGZipFile(string a_Packed, string a_Destination)
		{
			FileStream packedFile = File.Open(a_Packed, FileMode.Open);
			FileStream destFile = File.Create(a_Destination);
			GZipStream gzipStream = new GZipStream(packedFile, CompressionMode.Decompress);

			byte[] buffer = new byte[4096];
			for (;;)
			{
				int bytesRead = gzipStream.Read(buffer, 0, buffer.Length);
				if (bytesRead == 0)
					break;

				destFile.Write(buffer, 0, bytesRead);
			}

			gzipStream.Dispose();
			destFile.Dispose();
			packedFile.Dispose();
		}

		void MoveDownloadedFile(UpdatedFile a_File)
		{
			if (!CheckFileHash(a_File, a_File.DownloadedPath, true))
			{
				a_File.Status			= FileStatus.Error;
				a_File.LastError		= "Downloaded file is damaged";
				a_File.LastException	= null;
				return;
			}
		
			string backupPath = m_BackupPath + "\\" + a_File.FileName;
			Directory.CreateDirectory(m_BackupPath);

			File.Delete(backupPath);
			
			if (File.Exists(a_File.FilePath))
				File.Move(a_File.FilePath, backupPath);
			
			ExtractGZipFile(a_File.DownloadedPath, a_File.FilePath);
			
			a_File.Status = FileStatus.Updated;
			File.Delete(a_File.DownloadedPath);
		}

		// summary:
		// Takes a single necessary file action
		// Returns false if no more actions needed
		bool TakeFileAction(UpdatedFile a_File)
		{
			Utility.AssertEnumCount(typeof(FileStatus), 7);
		
			switch (a_File.Status)
			{
			case FileStatus.Initial:
				return false;
			case FileStatus.AlreadyOk:
				return false;
			case FileStatus.NeedsUpdate:
				DownloadFile(a_File);
				return true;
			case FileStatus.Downloading:
				Thread.Sleep(1000);
				return true;
			case FileStatus.Downloaded:
				return false;
			case FileStatus.Updated:
				return false;
			case FileStatus.Error:
				return false;
			}
			
			return false;
		}
		
		void FileUpdaterThread(Object a_Parameter)
		{
			UpdatedFile a_File = (UpdatedFile)a_Parameter;
		
			try
			{
				for (;;)
				{
					if (!TakeFileAction(a_File))
						break;
				}
			}
			catch (System.Exception a_Exception)
			{
				a_File.Status			= FileStatus.Error;
				a_File.LastException	= a_Exception;
			}
		}
		
		void PurgeFinishedThreads()
		{
			for (int i = 0; i < m_Threads.Count; i++)
			{
				Thread currThread = m_Threads[i];
				if (!currThread.IsAlive)
				{
					m_Threads.RemoveAt(i);
					i--;
				}
			}
		}
		
		public UInt32 GetRunningThreadCount()
		{
			PurgeFinishedThreads();
			return (UInt32)m_Threads.Count;
		}
		
		public bool IsUpdateNeeded()
		{
			Utility.AssertEnumCount(typeof(FileStatus), 7);

			foreach (UpdatedFile currFile in m_Files)
			{
				switch (currFile.Status)
				{
					case FileStatus.NeedsUpdate:
					case FileStatus.Downloaded:
						return true;
				}
			}

			return false;
		}
		
		bool IsReadyToUpdate()
		{
			Utility.AssertEnumCount(typeof(FileStatus), 7);

			foreach (UpdatedFile currFile in m_Files)
			{
				switch (currFile.Status)
				{
				case FileStatus.Initial:
					return false;
				case FileStatus.NeedsUpdate:
					return false;
				case FileStatus.Downloading:
					return false;
				case FileStatus.Error:
					return false;
				}
			}
			
			return true;
		}
		
		public void FinalizeUpdate()
		{
			if (!IsReadyToUpdate())
				return;
		
			foreach (UpdatedFile currFile in m_Files)
			{
				try
				{
					if (currFile.Status == FileStatus.Downloaded)
						MoveDownloadedFile(currFile);
				}
				catch (System.Exception a_Exception)
				{
					currFile.Status			= FileStatus.Error;
					currFile.LastException	= a_Exception;
				}
			}
		}
	}
}
