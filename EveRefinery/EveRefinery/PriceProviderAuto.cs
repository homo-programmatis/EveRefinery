using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EveRefinery
{
	class PriceProviderAuto : IPriceProvider
	{
		private IPriceProvider          m_Provider;

		public							PriceProviderAuto(Settings.V2._PriceSettings a_Settings)
		{
			switch (a_Settings.Provider)
			{
				case PriceProviders.EveCentral:
					m_Provider = new PriceProviderEveCentralCom(a_Settings.EveCentralCom);
					break;
				default:
					Debug.Assert(false, "Invalid price provider");
					break;
			}
		}

		public List<PriceRecord>		GetPrices(List<UInt32> a_TypeIDs)
		{
			return m_Provider.GetPrices(a_TypeIDs);
		}

		public PriceRecord				GetCurrentFilter()
		{
			return m_Provider.GetCurrentFilter();
		}

		public String					GetCurrentFilterHint(EveDatabase a_Database)
		{
			return m_Provider.GetCurrentFilterHint(a_Database);
		}
	}
}
