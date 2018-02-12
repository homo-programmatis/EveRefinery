using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

namespace EveRefinery
{
	public class EveOutposts
	{
		private Dictionary<UInt32, String>		m_LocationIdToName = new Dictionary<UInt32, String>();
		private Thread							m_ThreadUpdateFromApi;

		public					EveOutposts()
		{
			Load();
		}

		public String			GetLocationName(UInt32 a_Location)
		{
			lock (m_LocationIdToName)
			{
				if (!m_LocationIdToName.ContainsKey(a_Location))
					return null;

				return m_LocationIdToName[a_Location];
			}
		}

		private static String	GetCacheFilePath()
		{
			return Program.GetCacheFolder() + "ConquerableStationList.xml";
		}

		private void			Load()
		{
			if (!LoadCached())
				StartThread_UpdateFromApi();
		}

		private bool			LoadCached()
		{
			String cacheFilePath = GetCacheFilePath();
			if (!File.Exists(cacheFilePath))
				return false;

			XmlDocument xml = new XmlDocument();

			try
			{
				xml.Load(cacheFilePath);
				ParseXml(xml);
			}
			catch (System.Exception a_Exception)
			{
				// Don't nag user if cached file is bad
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
				return false;
			}

			// Cached data is always loaded. That helps if update fails, and while it's still working.
			// Outposts don't change so often, and even if they do, it's not too important.
			return !EveApi.IsCacheExpired(xml);
		}

		private void			ParseXml(XmlDocument a_Xml)
		{
			lock (m_LocationIdToName)
			{
				XmlNodeList rows = a_Xml.SelectNodes("//eveapi/result/rowset/row");
				foreach (XmlNode currNode in rows)
				{
					UInt32	stationID	= Convert.ToUInt32(currNode.Attributes["stationID"].Value);
					String	stationName	= currNode.Attributes["stationName"].Value;
					m_LocationIdToName[stationID] = stationName;
				}
			}
		}

		private void			StartThread_UpdateFromApi()
		{
			m_ThreadUpdateFromApi = new Thread(new ThreadStart(UpdateFromApi));
			m_ThreadUpdateFromApi.IsBackground = true;
			m_ThreadUpdateFromApi.Start();
		}

		private void			UpdateFromApi()
		{
			string errorHeader = "Failed to load outpost information from EVE API";
			XmlDocument xml = EveApi.MakeRequest("eve/ConquerableStationList.xml.aspx", null, 0, errorHeader);
			if (null == xml)
				return;

			try
			{
				xml.Save(GetCacheFilePath());
				ParseXml(xml);
			}
			catch (System.Exception a_Exception)
			{
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
			}
		}
	}
}
