using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Diagnostics;

namespace SpecialFNs
{
	class ComputerID
	{
		[DllImport("kernel32.dll", SetLastError=true)]
		static extern bool GetVolumeNameForVolumeMountPoint(string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, UInt32 cchBufferLength);

		/// <summary>
		/// Returns Volume GUID for a specified volume
		/// </summary>
		/// <param name="a_Path">Path to any file on the volume</param>
		/// <returns>Volume GUID</returns>
		public static string GetVolumeGuid(string a_Path)
		{
			string mountPoint = a_Path.Substring(0, 3);
		
		    StringBuilder buffer = new StringBuilder(128);
			if (!GetVolumeNameForVolumeMountPoint(mountPoint, buffer, (UInt32)buffer.Capacity))
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			
			return buffer.ToString();
		}

		/// <summary>
		/// Returns Volume GUID for system volume
		/// </summary>
		/// <returns>System Volume GUID</returns>
		public static string GetSystemVolumeGuid()
		{
			string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
			return GetVolumeGuid(systemPath);
		}
		
		/// <summary>
		/// Returns my variant of computer id (based on system Volume GUID)
		/// This id consists of 2 md5 hashes: first is actual id, and second is the hash of first
		/// </summary>
		/// <returns>ComputerID string</returns>
		public static string GetComputerID()
		{
			string volumeGUID = "";

			try
			{
				volumeGUID = ComputerID.GetSystemVolumeGuid();
			}
			catch (System.Exception a_Exception)
			{
				Debug.Assert(false, a_Exception.Message);
			}

			byte[] hash = Utility.Md5String(volumeGUID);
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

			for (int i = 0; i < 64; i++)
			{
				hash = md5.ComputeHash(hash);
			}

			string leftPart = Utility.BytesToHex(hash);
			string rightPart = Utility.BytesToHex(Utility.Md5String(leftPart));

			return leftPart + rightPart;
		}
	}
}