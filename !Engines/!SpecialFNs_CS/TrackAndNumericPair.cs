using System;
using System.Windows.Forms;

namespace SpecialFNs
{
	public class TrackAndNumericPair
	{
		protected NumericUpDown	m_Numeric;
		protected TrackBar		m_TrackBar;
		protected EventHandler	m_ChangeEventHandler;

		public TrackAndNumericPair(NumericUpDown a_Numeric, TrackBar a_TrackBar, EventHandler a_ChangeEventHandler)
		{
			m_Numeric	= a_Numeric;
			m_TrackBar	= a_TrackBar;
			m_ChangeEventHandler	= a_ChangeEventHandler;

			m_Numeric.ValueChanged	+= NumericUpDown_ValueChanged;
			m_TrackBar.Scroll		+= TrackBar_Scroll;
		}

		private void OnValueChange(decimal a_NewNumericValue)
		{
			// suppress echo
			m_Numeric.ValueChanged	-= NumericUpDown_ValueChanged;
			m_TrackBar.Scroll		-= TrackBar_Scroll;

			if (m_Numeric.Value != a_NewNumericValue)
				m_Numeric.Value = a_NewNumericValue;

			double numericPosition = (double)(m_Numeric.Value - m_Numeric.Minimum) / (double)(m_Numeric.Maximum - m_Numeric.Minimum);
			Int32 newTrackbarValue = (Int32)(m_TrackBar.Minimum + numericPosition * (m_TrackBar.Maximum - m_TrackBar.Minimum));
			if (m_TrackBar.Value != newTrackbarValue)
				m_TrackBar.Value = newTrackbarValue;

			m_ChangeEventHandler(this, new EventArgs());

			m_Numeric.ValueChanged	+= NumericUpDown_ValueChanged;
			m_TrackBar.Scroll		+= TrackBar_Scroll;
		}

		private void NumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			OnValueChange(m_Numeric.Value);
		}

		private void TrackBar_Scroll(object sender, EventArgs e)
		{
			decimal trackPosition = (decimal)(m_TrackBar.Value - m_TrackBar.Minimum) / (decimal)(m_TrackBar.Maximum - m_TrackBar.Minimum);
			decimal newNumericValue = m_Numeric.Minimum + trackPosition * (m_Numeric.Maximum - m_Numeric.Minimum);
			OnValueChange(newNumericValue);
		}
	}
}