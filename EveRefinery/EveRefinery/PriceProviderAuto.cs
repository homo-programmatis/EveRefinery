﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EveRefinery
{
	class PriceProviderAuto
	{
		public static IPriceProvider	GetPriceProvider(Settings.V2._PriceSettings a_Settings)
		{
			switch (a_Settings.Provider)
			{
				case PriceProviders.EveCentral:
					return new PriceProviderEveCentralCom(a_Settings.EveCentralCom);
			    case PriceProviders.FuzzworkCoUk:
					return new PriceProviderFuzzworkCoUk(a_Settings.FuzzworkCoUk);
				case PriceProviders.EveMarketdataCom:
					return new PriceProviderEveMarketdataCom(a_Settings.EveMarketdataCom);
			}

			Debug.Assert(false, "Invalid price provider");
			return null;
		}
	}
}
