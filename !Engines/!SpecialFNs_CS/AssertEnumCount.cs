using System;
using System.Diagnostics;

namespace SpecialFNs
{
	partial class Utility
	{
		/// <summary>
		/// Tests the specified enum to contain exact number of elements
		/// </summary>
		/// <returns>Always returns true (this way you can write static bool foo = AssertEnumCount() to do only once)</returns>
		public static bool AssertEnumCount(Type a_Enum, UInt32 a_ExpectedCount)
		{
			Debug.Assert(Enum.GetValues(a_Enum).GetLength(0) == a_ExpectedCount);
			return true;
		}
	}
}