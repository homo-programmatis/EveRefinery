using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Xml;
using System.Globalization;
using System.Windows.Forms;
using SpecialFNs;
using System.Diagnostics;

namespace EveRefinery
{
	public class ItemAssets
	{
		public UInt32		TypeID;
		public UInt32		Quantity;
		public bool			IsPacked;
		
		public AssetsMap	NestedPacked;		// TypeID -> Item
		public AssetsList	NestedAssembled;

		public ItemAssets(UInt32 a_TypeID, bool a_IsPacked)
		{
			TypeID		= a_TypeID;
			Quantity	= 0;
			IsPacked	= a_IsPacked;
		}
	}
	
	[Flags]
	public enum AssetFilter
	{
		NoAssembled				= 0x00000001,
		NoAssembledShips		= 0x00000002,
		NoAssembledContainers	= 0x00000004,
	}
	
	public class AssetsMap : Dictionary<UInt32, ItemAssets> { }
	public class AssetsList : List<ItemAssets> { }
	public class ContainerTree : Dictionary<ItemAssets, string> { }

	class EveAssets
	{
		public AssetsMap		Assets = new AssetsMap();	// LocationID -> Item
		public DateTime			m_CacheTime;
		
		protected Engine		m_Engine;
		protected EveDatabase	m_EveDatabase;
		
		public EveAssets(Engine a_Engine, EveDatabase a_EveDatabase)
		{
			m_Engine			= a_Engine;
			m_EveDatabase		= a_EveDatabase;
			m_CacheTime			= DateTime.UtcNow;
		}

		protected void GetAssetsList_AddAsset(ItemAssets a_Asset, ItemAssets a_Target, AssetFilter a_Filter)
		{
			if (!a_Asset.IsPacked && (a_Filter != 0))
			{
				UInt32 itemCategory = m_EveDatabase.GetTypeIdCategory(a_Asset.TypeID);
				if (((a_Filter & AssetFilter.NoAssembledContainers) != 0) && (itemCategory == (UInt32)EveCategories.Celestial))
					return;

				if (((a_Filter & AssetFilter.NoAssembledShips) != 0) && (itemCategory == (UInt32)EveCategories.Ship))
					return;
			}
		
			ItemAssets targetAsset = GetOrInsert_ItemAssets(a_Target, a_Asset.TypeID, true);
			targetAsset.Quantity += a_Asset.Quantity;
			GetAssetsList_FillNested(a_Asset, a_Target, a_Filter);
		}

		protected void GetAssetsList_FillNested(ItemAssets a_Container, ItemAssets a_Target, AssetFilter a_Filter)
		{
			if (a_Container.NestedPacked != null)
			{
				foreach (ItemAssets containedAsset in a_Container.NestedPacked.Values)
				{
					GetAssetsList_AddAsset(containedAsset, a_Target, a_Filter);
				}
			}

			if ((a_Container.NestedAssembled != null) && (0 == (a_Filter & AssetFilter.NoAssembled)))
			{
				foreach (ItemAssets containedAsset in a_Container.NestedAssembled)
				{
					GetAssetsList_AddAsset(containedAsset, a_Target, a_Filter);
				}
			}
		}
		
		/// <summary>
		/// Compiles assets into a list, iterating through any nested ones
		/// </summary>
		/// <param name="a_Container">null or container. If null, all known assets are compiled.</param>
		/// <returns>Compiled assets</returns>
		public AssetsMap GetAssetsList(ItemAssets a_Container, AssetFilter a_Filter)
		{
			ItemAssets container = new ItemAssets(0, false);
			container.NestedPacked = new AssetsMap();
			
			if (a_Container != null)
			{
				GetAssetsList_FillNested(a_Container, container, a_Filter);
				return container.NestedPacked;
			}

			foreach (ItemAssets locationAssets in Assets.Values)
			{
				GetAssetsList_FillNested(locationAssets, container, a_Filter);
			}

			return container.NestedPacked;
		}
		
		protected void FillNestedContainers(ContainerTree a_Result, ItemAssets a_Parent, string a_CurrPath)
		{
			if (a_Parent.NestedAssembled == null)
				return;

			foreach (ItemAssets containedAsset in a_Parent.NestedAssembled)
			{
				if ((containedAsset.NestedPacked == null) && (containedAsset.NestedAssembled == null))
					continue;
				
				string path = a_CurrPath;
				if (path != "")
					path += " \\ ";
				path += m_EveDatabase.GetTypeIdName(containedAsset.TypeID);
				
				a_Result.Add(containedAsset, path);
				FillNestedContainers(a_Result, containedAsset, path);
			}
		}

		public ContainerTree GetContainerTree(ItemAssets a_Location)
		{
			ContainerTree result = new ContainerTree();
			if (a_Location != null)
			{
				FillNestedContainers(result, a_Location, "");
				return result;
			}

			foreach (ItemAssets locationAssets in Assets.Values)
			{
				FillNestedContainers(result, locationAssets, "");
			}

			return result;
		}

		private DateTime GetCacheTime(XmlDocument a_AssetsXml)
		{
			try
			{
				XmlNodeList cacheTimeNode = a_AssetsXml.GetElementsByTagName("cachedUntil");
				if (0 == cacheTimeNode.Count)
					return new DateTime();

				string cachedUntilStr = cacheTimeNode[0].InnerText;
				DateTime result = DateTime.ParseExact(cachedUntilStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
				return result;
			}
			catch (System.Exception a_Exception)
			{
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
				return new DateTime();
			}
		}

		ItemAssets GetOrInsert_LocationAssets(UInt32 a_LocationID)
		{
			if (Assets.ContainsKey(a_LocationID))
				return Assets[a_LocationID];

			ItemAssets newLocation = new ItemAssets(a_LocationID, false);
			Assets.Add(a_LocationID, newLocation);
			return newLocation;
		}

		ItemAssets GetOrInsert_ItemAssets(ItemAssets a_Parent, UInt32 a_TypeID, bool a_IsPacked)
		{
			if (a_IsPacked && (a_Parent.NestedPacked != null) && a_Parent.NestedPacked.ContainsKey(a_TypeID))
				return a_Parent.NestedPacked[a_TypeID];
		
			ItemAssets newAsset = new ItemAssets(a_TypeID, a_IsPacked);

			if (a_IsPacked)
			{
				if (a_Parent.NestedPacked == null)
					a_Parent.NestedPacked = new AssetsMap();

				a_Parent.NestedPacked.Add(a_TypeID, newAsset);
			}
			else
			{
				if (a_Parent.NestedAssembled == null)
					a_Parent.NestedAssembled = new AssetsList();

				a_Parent.NestedAssembled.Add(newAsset);
			}

			return newAsset;
		}

		private void ParseNestedAssets(XmlNode a_ParentNode, ItemAssets a_Parent)
		{
			XmlNodeList nestedNodes = a_ParentNode.SelectNodes("rowset/row");
			foreach (XmlNode currNode in nestedNodes)
			{
				UInt32	typeID		= Convert.ToUInt32(currNode.Attributes["typeID"].Value);
				UInt32	quantity	= Convert.ToUInt32(currNode.Attributes["quantity"].Value);
				bool	isPacked	= (0 != Convert.ToUInt32(currNode.Attributes["singleton"].Value));

				ItemAssets currAsset = GetOrInsert_ItemAssets(a_Parent, typeID, isPacked);
				currAsset.Quantity += quantity;

				if (currNode.HasChildNodes)
					ParseNestedAssets(currNode, currAsset);
			}
		}

		private void ParseAssetsXML(XmlDocument a_AssetsXml)
		{
			UnloadAssets();
			
			XmlNodeList globalNodes = a_AssetsXml.SelectNodes("//eveapi/result/rowset/row");
			foreach (XmlNode currNode in globalNodes)
			{
				UInt32	locationID	= Convert.ToUInt32(currNode.Attributes["locationID"].Value);
				UInt32	typeID		= Convert.ToUInt32(currNode.Attributes["typeID"].Value);
				UInt32	quantity	= Convert.ToUInt32(currNode.Attributes["quantity"].Value);
				bool	isPacked	= (0 == Convert.ToUInt32(currNode.Attributes["singleton"].Value));

				ItemAssets currLocAssets = GetOrInsert_LocationAssets(locationID);
				ItemAssets currAsset = GetOrInsert_ItemAssets(currLocAssets, typeID, isPacked);
				currAsset.Quantity += quantity;
				
				if (currNode.HasChildNodes)
					ParseNestedAssets(currNode, currAsset);
			}
			
			m_CacheTime = GetCacheTime(a_AssetsXml);
		}
		
		private Boolean LoadAssetsXml(string a_FilePath, bool a_TestCacheDate)
		{
			if (!File.Exists(a_FilePath))
				return false;
			
			XmlDocument assetsXml = new XmlDocument();
			assetsXml.Load(a_FilePath);

			if (a_TestCacheDate && (GetCacheTime(assetsXml) < DateTime.UtcNow))
			{
				if (DialogResult.No != MessageBox.Show("Your assets information expired. Would you like to update now?\nWARNING: Due to CCP limitations, you can only update assets once in 24 hours.", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
					return false;
			}

			ParseAssetsXML(assetsXml);
			return true;
		}
		
		public void UnloadAssets()
		{
			Assets.Clear();
			m_CacheTime = DateTime.UtcNow;
		}
		
		public Boolean LoadAssets(UInt32 a_CharID)
		{
			string cacheFilename = "Assets_" + a_CharID.ToString() + ".xml";

			try
			{
				if (LoadAssetsXml(cacheFilename, true))
					return true;
			}
			catch (System.Exception a_Exception)
			{
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
			}

			Settings.AccountsRow account = m_Engine.GetCharacterAccount(a_CharID);
			if (null == account)
				return false;
				
			string requestUrl = String.Format("http://api.eve-online.com/char/AssetList.xml.aspx?userID={0:d}&characterID={1:d}&version=2&apiKey=", account.UserID, a_CharID);
			requestUrl += account.FullKey;
			
			string errorHeader = "Failed to update assets:\n";
			XmlDocument assetsXml = new XmlDocument();

			try
			{
				assetsXml = Engine.LoadXmlWithUserAgent(requestUrl);

				XmlNodeList errorNodes = assetsXml.GetElementsByTagName("error");
				if (0 != errorNodes.Count)
				{
					Engine.ShowXmlRequestErrors(errorHeader, errorNodes);
					return false;
				}

				ParseAssetsXML(assetsXml);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show(errorHeader + a_Exception.Message);
				return false;
			}

			// Ignore saving errors
			try
			{
				assetsXml.Save(cacheFilename);
			}
			catch (System.Exception a_Exception)
			{
				System.Diagnostics.Debug.WriteLine(a_Exception.Message);
			}

			return true;
		}
	}
}
