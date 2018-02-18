using System;
using System.Collections.Generic;

namespace EveRefinery
{
	public enum PriceProviders
	{
		EveCentral,
	}

	public class PriceRecord
	{
		public Settings.V1._PriceSettings Settings;
		public UInt32				TypeID;
		public double				Price;
		public UInt64				UpdateTime;
	}

	public interface IPriceProvider
	{
		List<PriceRecord>			GetPrices(List<UInt32> a_TypeIDs, Settings.V1._PriceSettings a_Settings);
	}
}