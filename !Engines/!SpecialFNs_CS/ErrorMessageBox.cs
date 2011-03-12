using System.Windows.Forms;

namespace SpecialFNs
{
	class ErrorMessageBox
	{
		public static void Show(string a_Message)
		{
			MessageBox.Show(a_Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}
}