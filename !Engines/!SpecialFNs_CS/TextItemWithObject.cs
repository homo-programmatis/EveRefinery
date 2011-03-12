using System;

namespace SpecialFNs
{
	class TextItemWithObject
	{
		public string	Caption;
		public Object	Data;

		public TextItemWithObject(string a_Caption, Object a_Data)
		{
			Caption		= a_Caption;
			Data		= a_Data;
		}
		
		public override string ToString()
		{
			return Caption;
		}

		public static Object GetData(Object a_Item)
		{
			return ((TextItemWithObject)a_Item).Data;
		}
	}
}