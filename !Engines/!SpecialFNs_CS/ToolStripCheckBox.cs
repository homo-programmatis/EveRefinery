using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace SpecialFNs
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripCheckBox : ToolStripControlHost
	{
		public ToolStripCheckBox()
			: base(new CheckBox())
		{

		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public CheckBox CheckBox
		{
			get
			{
				return (CheckBox)this.Control;
			}
		}

		public event EventHandler CheckedChanged;

		protected void OnCheckedChanged(object sender, EventArgs e)
		{
			if (CheckedChanged != null)
			{
				CheckedChanged(this, e);
			}
		}

		protected override void OnSubscribeControlEvents(Control control)
		{
			base.OnSubscribeControlEvents(control);
			(control as CheckBox).CheckedChanged += OnCheckedChanged;
		}

		protected override void OnUnsubscribeControlEvents(Control control)
		{
			base.OnUnsubscribeControlEvents(control);
			(control as CheckBox).CheckedChanged -= OnCheckedChanged;
		}
	}
}