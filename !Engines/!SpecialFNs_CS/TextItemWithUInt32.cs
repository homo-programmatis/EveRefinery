using System;

namespace SpecialFNs
{
	class TextItemWithUInt32
	{
		public string	Caption;
		public UInt32	Data;

		public TextItemWithUInt32(string a_Caption, UInt32 a_Data)
		{
			Caption		= a_Caption;
			Data		= a_Data;
		}
		
		public override string ToString()
		{
			return Caption;
		}
		
		public static UInt32 GetData(Object a_Item)
		{
			return ((TextItemWithUInt32)a_Item).Data;
		}
	}
}