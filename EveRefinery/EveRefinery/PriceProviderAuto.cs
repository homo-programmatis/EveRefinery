﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EveRefinery
{
	class PriceProviderAuto : IPriceProvider
	{
		protected Settings		m_Settings;

		public PriceProviderAuto(Settings a_Settings)
		{
			m_Settings = a_Settings;
		}

		public static IPriceProvider CreateEveCentralProvider(UInt32 a_PriceHistoryDays)
		{
			PriceProviderEveCentral provider = new PriceProviderEveCentral();
			provider.m_PriceHistoryDays = a_PriceHistoryDays;
			return provider;
		}

	    public static IPriceProvider CreateFuzzworkProvider()
	    {
	        return new PriceProviderFuzzwork();
	    }
	    
		public List<PriceRecord> GetPrices(List<UInt32> a_TypeIDs, PriceSettings a_Settings)
		{
			IPriceProvider provider = null;

			switch (a_Settings.Provider)
			{
				case PriceProviders.EveCentral:
					provider = CreateEveCentralProvider(m_Settings.PriceLoad.ItemsHistoryDays);
					break;
			    case PriceProviders.Fuzzwork:
			        provider = CreateFuzzworkProvider();
			        break;
				default:
					Debug.Assert(false, "Invalid price provider");
					return null;
			}

			return provider.GetPrices(a_TypeIDs, a_Settings);
		}
	}
}
