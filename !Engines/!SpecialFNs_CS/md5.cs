using System;
using System.Security.Cryptography;
using System.IO;

namespace SpecialFNs
{
	partial class Utility
	{
		public static byte[] Md5String(string a_String)
		{
			byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(a_String);

			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			return md5.ComputeHash(stringBytes);
		}

		public static byte[] Md5File(string a_FilePath)
		{
			using (FileStream fileStream = File.Open(a_FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
				return md5.ComputeHash(fileStream);
			}
		}
	}
}