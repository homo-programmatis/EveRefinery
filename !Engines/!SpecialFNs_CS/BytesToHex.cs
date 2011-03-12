using System;
using System.Text;

namespace SpecialFNs
{
	partial class Utility
	{
		public static string BytesToHex(byte[] a_Bytes)
		{
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < a_Bytes.Length; i++)
			{
				result.Append(a_Bytes[i].ToString("x2"));
			}

			return result.ToString();
		}
	}
}