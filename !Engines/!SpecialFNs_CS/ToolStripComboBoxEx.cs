using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace SpecialFNs
{
	public class ToolStripComboBoxEx : ToolStripComboBox
	{
		public event CancelEventHandler SelectedIndexChanging;
		protected int m_LastIndex;

		public ToolStripComboBoxEx()
		{
			m_LastIndex = -1;
		}

		protected void OnSelectedIndexChanging(CancelEventArgs e)
		{
			if (SelectedIndexChanging != null)
				SelectedIndexChanging(this, e);
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (m_LastIndex != SelectedIndex)
			{
				var cancelEventArgs = new CancelEventArgs();
				OnSelectedIndexChanging(cancelEventArgs);

				if (!cancelEventArgs.Cancel)
				{
					m_LastIndex = SelectedIndex;
					base.OnSelectedIndexChanged(e);
				}
				else
					SelectedIndex = m_LastIndex;
			}
		}

	}
}
