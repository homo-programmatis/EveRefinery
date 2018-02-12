using System;
using System.Collections.Generic;

namespace EveRefinery
{
	public enum PriceProviders
	{
		EveCentral,
	    Fuzzwork,

	    MaxPriceProviders,
	}

	[Serializable]
	public struct PriceSettings
	{
		public PriceProviders		Provider;
		public UInt32				RegionID;
		public UInt32				SolarID;
		public UInt32				StationID;
		public PriceTypes			PriceType;

		public bool Matches(PriceSettings a_Rhs)
		{
			return
				(Provider	== a_Rhs.Provider) && 
				(RegionID	== a_Rhs.RegionID) && 
				(SolarID	== a_Rhs.SolarID) &&
				(StationID	== a_Rhs.StationID) &&
				(PriceType	== a_Rhs.PriceType);
		}

		public String GetHintText(EveDatabase a_Database)
		{
			return PriceType.ToString() + " - " + a_Database.GetLocationName(RegionID, SolarID, StationID);
		}
	}

	public class PriceRecord
	{
		public PriceSettings		Settings;
		public UInt32				TypeID;
		public double				Price;
		public UInt64				UpdateTime;
	}

	public interface IPriceProvider
	{
		List<PriceRecord>			GetPrices(List<UInt32> a_TypeIDs, PriceSettings a_Settings);
	}
}