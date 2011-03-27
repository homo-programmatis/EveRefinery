using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace EveRefinery
{
	class Exporter
	{
		public class ExportedRow
		{
			public Color			Color;
			public String[]			Cells;
		}

		public class ExportedData
		{
			public HorizontalAlignment[]	Aligns;
			public ExportedRow[]	Rows;
		}

		public static String CsvEncode(string a_Text)
		{
			StringBuilder sb = new StringBuilder(a_Text.Length);
			sb.Append('"');

			for (int i = 0; i < a_Text.Length; i++)
			{
				switch (a_Text[i])
				{
					case '\\':
						sb.Append("\\\\");
						break;
					case '"':
						sb.Append("\\\"");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					default:
						sb.Append(a_Text[i]);
						break;
				}
			}

			sb.Append('"');
			return sb.ToString();
		}

		public static void ExportAsCsv(String a_FilePath, ExportedData a_Data)
		{
			StreamWriter file = new StreamWriter(a_FilePath);

			for (int i = 0; i < a_Data.Rows.Length; i++)
			{
				ExportedRow currRow = a_Data.Rows[i];
				StringBuilder sb = new StringBuilder();

				for (int j = 0; j < currRow.Cells.Length; j++)
				{
					if (j != 0)
						sb.Append(",");

					String text = currRow.Cells[j];
					sb.Append(CsvEncode(text));
				}

				file.WriteLine(sb.ToString());
			}

			file.Close();
		}

		public static String HtmlEncode(string a_Text)
		{
			if (a_Text == "")
				return " ";

			StringBuilder sb = new StringBuilder(a_Text.Length);

			for (int i = 0; i < a_Text.Length; i++)
			{
				switch (a_Text[i])
				{
					case '<':
						sb.Append("&lt;");
						break;
					case '>':
						sb.Append("&gt;");
						break;
					case '"':
						sb.Append("&quot;");
						break;
					case '&':
						sb.Append("&amp;");
						break;
					default:
						if (a_Text[i] > 159)
							sb.AppendFormat("&#{0};", (int)a_Text[i]);
						else
							sb.Append(a_Text[i]);
						break;
				}
			}
			return sb.ToString();
		}

		public static String GetHtmlAlignName(HorizontalAlignment a_Align)
		{
			switch(a_Align)
			{
				case HorizontalAlignment.Right:
					return "right";
				case HorizontalAlignment.Left:
					return "left";
			}

			return "center";
		}

		public static void ExportAsHtml(String a_FilePath, ExportedData a_Data)
		{
			StreamWriter file = new StreamWriter(a_FilePath);
			file.WriteLine("<html>");
				file.WriteLine("\t<style type=\"text/css\">");
				file.WriteLine("\ttable {");
				file.WriteLine("\tborder-bottom-color: #c3c3c3;");
				file.WriteLine("\tborder-bottom-style: solid;");
				file.WriteLine("\tborder-bottom-width: 1px;");
				file.WriteLine("\tborder-collapse: collapse;");
				file.WriteLine("\tborder-left-color: #c3c3c3;");
				file.WriteLine("\tborder-left-style: solid;");
				file.WriteLine("\tborder-left-width: 1px;");
				file.WriteLine("\tborder-right-color: #c3c3c3;");
				file.WriteLine("\tborder-right-style: solid;");
				file.WriteLine("\tborder-right-width: 1px;");
				file.WriteLine("\tborder-top-color: #c3c3c3;");
				file.WriteLine("\tborder-top-style: solid;");
				file.WriteLine("\tborder-top-width: 1px;");
				file.WriteLine("</style>");
			file.WriteLine("</head>");

			file.WriteLine("<body>");
			file.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\" border=\"1\">");

			for (int i = 0; i < a_Data.Rows.Length; i++)
			{
				ExportedRow currRow = a_Data.Rows[i];
				file.WriteLine("\t<tr bgcolor=\"#{0:X2}{1:X2}{2:X2}\">", currRow.Color.R, currRow.Color.G, currRow.Color.B);

				for (int j = 0; j < currRow.Cells.Length; j++)
				{
					HorizontalAlignment align = a_Data.Aligns[j];
					String text = currRow.Cells[j];
					file.WriteLine("\t\t<td align=\"{0}\">{1}</td>", GetHtmlAlignName(align), HtmlEncode(text));
				}

				file.WriteLine("\t</tr>");
			}

			file.WriteLine("</table>");
			file.WriteLine("</body>");
			file.WriteLine("</html>");

			file.Close();
		}
	}
}
