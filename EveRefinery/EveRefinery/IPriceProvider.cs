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
		public PriceProviders       Provider;
		public UInt32               RegionID;
		public UInt32               SolarID;
		public UInt32               StationID;
		public PriceTypes           PriceType;
		public UInt32				TypeID;
		public double				Price;
		public UInt64				UpdateTime;

		public bool					IsMatchesFilter(PriceRecord a_Filter)
		{
			return
			(
				(this.Provider		== a_Filter.Provider) &&
				(this.RegionID		== a_Filter.RegionID) &&
				(this.SolarID		== a_Filter.SolarID) &&
				(this.StationID		== a_Filter.StationID) &&
				(this.PriceType		== a_Filter.PriceType)
			);
		}
	}

	public interface IPriceProvider
	{
		// Queries and returns prices.
		// Can return more price records then were requested - for example, all PriceType's at once.
		List<PriceRecord>			GetPrices(List<UInt32> a_TypeIDs);

		// Creates a new PriceRecord containing currently requested filter.
		// Used to filter requested records from additionally returned records - see PriceRecord.IsMatchesFilter().
		PriceRecord					GetCurrentFilter();

		// Human-readable hint for currently requested filter.
		String						GetCurrentFilterHint(EveDatabase a_Database);
	}
}
