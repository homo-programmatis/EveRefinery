using System;
using System.Windows.Forms;

namespace SpecialFNs
{
	class ListViewEx : System.Windows.Forms.ListView
	{
		public ListViewEx()
		{
			//Activate double buffering
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}
		
		class ColumnHideInfo
		{
			public Int32	RealWidth;
		}
		
		// summary:
		// Hides a column, saving its real with to TAG field
		public static void HideColumn(ColumnHeader a_Column)
		{
			if (0 == a_Column.Width)
				return;
				
			ColumnHideInfo hideInfo = new ColumnHideInfo();
			a_Column.Tag		= hideInfo;
			hideInfo.RealWidth	= a_Column.Width;
			a_Column.Width		= 0;
		}

		// summary:
		// Shows a column previously hidden with HideColumn()
		public static void UnhideColumn(ColumnHeader a_Column)
		{
			if (0 != a_Column.Width)
				return;
		
			ColumnHideInfo hideInfo = (ColumnHideInfo)a_Column.Tag;
			a_Column.Width = hideInfo.RealWidth;
		}
		
		// summary:
		// Gets a column's width, even if it's hidden with HideColumn()
		public static UInt32 GetHideableColumnWidth(ColumnHeader a_Column)
		{
			if (0 != a_Column.Width)
				return (UInt32)a_Column.Width;

			ColumnHideInfo hideInfo = (ColumnHideInfo)a_Column.Tag;
			return (UInt32)hideInfo.RealWidth;
		}

		// summary:
		// Checks whether column is hidden
		public static Boolean IsColumnVisible(ColumnHeader a_Column)
		{
			return (0 != a_Column.Width);
		}
	}
}
