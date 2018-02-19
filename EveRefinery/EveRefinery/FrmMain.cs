using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SpecialFNs;

namespace EveRefinery
{
	enum Columns
	{
		Name,
        MetaLevel,
		Quantity,
		LossPercent,
		RefinedCost,
		MarketPrice,
		PriceDelta,
		Yield,
		Tritanium,
		Pyerite,
		Mexallon,
		Isogen,
		Noxcium,
		Zydrine,
		Megacyte,
		Morphite,
		Type,
		Volume,
		RefinedVolume,
		
		MaxColumns,
	}
	
	enum ToolbarLocations
	{
		Top,
		Left,
		Right,
		Bottom,
	}
	
	[Flags]
	enum ListUpdates
	{
		Prices			= 0x00000001,
	}
	
    public partial class FrmMain : Form
    {
		ItemsDB			m_ItemsDB		= new ItemsDB();
		Engine			m_Engine		= new Engine();
		MarketPricesDB	m_MarketPrices;
		EveDatabase		m_EveDatabase	= new EveDatabase();
		EveAssets		m_Assets;
		
		Columns			m_SortType		= Columns.Name;
		Int32			m_SortDirection	= 1;
		MainListItem[]	m_ItemList;
		MainListItem	m_TotalsItem;
		AssetsMap		m_SelectedAssets;
		ListUpdates		m_RunningListUpdates;
		
		const UInt32	character_AllItems		= 0x00000000;
		const UInt32	character_EditApiKeys	= 0xFFFFFFFF;
		const UInt32	SpecialTypeID_Totals	= 0xFFFFFFFF;

		public FrmMain()
        {
			m_Assets				= new EveAssets(m_Engine, m_EveDatabase);
			m_MarketPrices			= new MarketPricesDB(m_ItemsDB);
			m_RunningListUpdates	= 0;
        
            InitializeComponent();
		}
		
		abstract class CompareItemBase : IComparer
		{
			protected Int32			m_SortDirection;

			public void SetParameters(Int32 a_SortDirection)
			{
				m_SortDirection = a_SortDirection;
			}

			protected abstract int CompareItems(MainListItem a_Left, MainListItem a_Right);

			int IComparer.Compare(Object a_Left, Object a_Right)
			{
				MainListItem lhs = (MainListItem)a_Left;
				MainListItem rhs = (MainListItem)a_Right;
				
				bool isLhsTotals = (lhs.TypeID == SpecialTypeID_Totals);
				bool isRhsTotals = (rhs.TypeID == SpecialTypeID_Totals);

				if (isLhsTotals && isRhsTotals)
					return 0;
				else if (isLhsTotals)
					return -1;
				else if (isRhsTotals)
					return 1;
			
				return m_SortDirection * CompareItems(lhs, rhs);
			}
		}

		class CompareColumn : CompareItemBase
		{
			protected Columns		m_Column;
			protected Hashtable		m_ColumnData;	// UInt32(TypeID) -> Object[](column data)

			public CompareColumn(Columns a_Column, Hashtable a_ColumnData)
			{
				m_Column		= a_Column;
				m_ColumnData	= a_ColumnData;
			}
		
			protected override int CompareItems(MainListItem a_Left, MainListItem a_Right)
			{
				Object[] leftColumns	= (Object[])m_ColumnData[a_Left.TypeID];
				Object[] rightColumns	= (Object[])m_ColumnData[a_Right.TypeID];
				
				Object lhs = leftColumns[(int)m_Column];
				Object rhs = rightColumns[(int)m_Column];
				if (lhs is String)
					return String.Compare((string)lhs, (string)rhs, StringComparison.OrdinalIgnoreCase);

				return Comparer.DefaultInvariant.Compare(leftColumns[(int)m_Column], rightColumns[(int)m_Column]);
			}
		}
	
		ItemRecord CreateSpecialItem_Totals()
		{
			ItemRecord itemRecord		= new ItemRecord(m_TotalsItem.TypeID);
			itemRecord.ItemName			= "[Totals] -- known part";
			itemRecord.TypeSortString	= itemRecord.ItemName;
			itemRecord.IsPublished		= true;
			itemRecord.GroupID			= 1;
			itemRecord.MarketGroupID	= 1;
			itemRecord.HasUnknownMaterials = false;
			itemRecord.BatchSize		= 1;
			itemRecord.PriceDate		= DateTime.UtcNow;
			
			return itemRecord;
		}

		/// <summary>
		/// Fills m_ItemList with data
		/// </summary>
		/// <param name="a_ListTypeIDs">TypeIDs of items to be filled into list</param>
		/// <param name="a_Assets">Null or Assets to be listed</param>
		private void SetupListItemsData(UInt32[] a_ListTypeIDs, AssetsMap a_Assets)
		{
			m_TotalsItem	= null;
			
			List<MainListItem> specialItems = new List<MainListItem>();
			if (a_Assets != null)
			{
				m_TotalsItem = new MainListItem();
				m_TotalsItem.TypeID		= SpecialTypeID_Totals;
				m_TotalsItem.ItemData	= CreateSpecialItem_Totals();
				m_TotalsItem.Quantity	= 0;

				specialItems.Add(m_TotalsItem);
			}

			m_ItemList = new MainListItem[specialItems.Count + a_ListTypeIDs.Count()];
			for (int i = 0; i < specialItems.Count; i++)
			{
				m_ItemList[i] = specialItems[i];
			}

			for (int i = 0; i < a_ListTypeIDs.Count(); i++)
			{
				UInt32 currTypeID		= a_ListTypeIDs[i];
				MainListItem currItem	= new MainListItem();
				m_ItemList[specialItems.Count + i] = currItem;
				
				ItemAssets currAssets	= null;
				if ((a_Assets != null) && a_Assets.ContainsKey(currTypeID))
					currAssets			= (ItemAssets)a_Assets[currTypeID];

				currItem.TypeID			= currTypeID;
				currItem.ItemData		= m_ItemsDB.GetItemByTypeID(currTypeID);
				
				if (currAssets == null)
					currItem.Quantity	= 1;
				else
					currItem.Quantity	= currAssets.Quantity;
			}
		}

		private void MakeRefineryItemList()
		{
			UInt32 characterID		= ((TextItemWithUInt32)TlbCmbCharacter.SelectedItem).Data;
			ItemAssets container	= (ItemAssets)TextItemWithObject.GetData(TlbCmbContainer.SelectedItem);
			m_SelectedAssets		= null;
			if (character_AllItems != characterID)
			{
				AssetFilter assetFilter	= 0;
				if (TlbChkIgnoreContainers.Checked)
					assetFilter		|= AssetFilter.NoAssembledContainers;

				if (TlbChkIgnoreShips.Checked)
					assetFilter		|= AssetFilter.NoAssembledShips;

				if (TlbChkIgnoreAssembled.Checked)
					assetFilter		|= AssetFilter.NoAssembled;

				m_SelectedAssets	= m_Assets.GetAssetsList(container, assetFilter);
			}
		
			ItemFilter filter		= new ItemFilter();
			filter.Published		= TristateFilter.Yes;
			filter.PlainMaterials	= TristateFilter.Yes;
			if (m_SelectedAssets != null)
				filter.AssetsFilter = m_SelectedAssets;

			SetupListItemsData(m_ItemsDB.FilterItems(filter), m_SelectedAssets);
			UpdateTotalsRow();
			
			// Optimization: pre-fill all column data (saves around 1 sec)
			Hashtable columnData = new Hashtable();
			foreach (MainListItem listItem in m_ItemList)
			{
				Object[] columnValues = GetListItemColumnData(listItem);
				columnData.Add(listItem.TypeID, columnValues);
			}

			CompareItemBase comparer = new CompareColumn(m_SortType, columnData);
			comparer.SetParameters(m_SortDirection);
			Array.Sort(m_ItemList, comparer);

			LstRefinery.VirtualListSize = m_ItemList.Count();
			UpdateLstRefinery();
		}
		
		private Object[] GetListItemColumnData(MainListItem a_ListItem)
		{
			Object[] result			= new Object[(int)Columns.MaxColumns];

			bool isQuantityOk		= (m_SelectedAssets != null) && m_Engine.m_Settings.Appearance.UseAssetQuantities;
			UInt32 quantity			= isQuantityOk ? a_ListItem.Quantity : 1;

			bool isTotals = (a_ListItem.ItemData.TypeID == SpecialTypeID_Totals);
			if (isTotals)
				quantity = 1;

			lock (a_ListItem.ItemData)
			{
				ItemPrice prices = m_Engine.GetItemPrices(a_ListItem.ItemData, m_ItemsDB.GetMutators(), quantity);

				result[(int)Columns.Name]			= a_ListItem.ItemData.ItemName;
                result[(int)Columns.MetaLevel]		= a_ListItem.ItemData.MetaLevel;
                result[(int)Columns.RefinedCost]	= prices.RefinedCost;
				result[(int)Columns.MarketPrice]	= prices.MarketPrice;
				result[(int)Columns.PriceDelta]		= prices.PriceDelta;
				result[(int)Columns.Yield]			= m_Engine.GetEffectiveYield(a_ListItem.ItemData, m_ItemsDB.GetMutators());

				double		refinedVolume	= 0;
				Columns[]	materialColumns = new Columns[] { Columns.Tritanium, Columns.Pyerite, Columns.Mexallon, Columns.Isogen, Columns.Noxcium, Columns.Zydrine, Columns.Megacyte, Columns.Morphite };
				Materials[] columnMaterials = new Materials[] { Materials.Tritanium, Materials.Pyerite, Materials.Mexallon, Materials.Isogen, Materials.Noxcium, Materials.Zydrine, Materials.Megacyte, Materials.Morphite };
				for (int i = 0; i < materialColumns.Length; i++)
				{
					Columns currColumn = materialColumns[i];
					Materials currMaterial = columnMaterials[i];
					double materialAmount = 0;

					if (isTotals)
						materialAmount = a_ListItem.ItemData.MaterialAmount[(UInt32)currMaterial];
					else
						materialAmount = m_Engine.GetEffectiveRefineQuota(a_ListItem.ItemData, m_ItemsDB.GetMutators(), quantity, currMaterial);

					refinedVolume += materialAmount * MaterialsInfo.GetMaterialVolume(currMaterial);
					result[(int)currColumn] = materialAmount;
				}

				result[(int)Columns.Quantity]		= a_ListItem.Quantity;
				result[(int)Columns.Type]			= a_ListItem.ItemData.TypeSortString;
				
				double lossPercent = 0;
				if (!ItemPrice.IsValidPrice(prices.MarketPrice))
					lossPercent = double.PositiveInfinity;
				else
					lossPercent = (prices.MarketPrice - prices.RefinedCost) / prices.MarketPrice;
				
				result[(int)Columns.LossPercent]	= lossPercent;
				result[(int)Columns.Volume]			= quantity * a_ListItem.ItemData.Volume;
				result[(int)Columns.RefinedVolume]	= refinedVolume;
			}
			
			return result;
		}
		
		private void Refinery_RetrieveVirtualItem(object a_Sender, RetrieveVirtualItemEventArgs a_QueryArgs)
		{
			MainListItem listItem	= m_ItemList[a_QueryArgs.ItemIndex];
			Object[] columnData		= GetListItemColumnData(listItem);

			a_QueryArgs.Item		= new ListViewItem();
			ListViewItem.ListViewSubItemCollection subitems = a_QueryArgs.Item.SubItems;
			bool isQuantityOk		= (m_SelectedAssets != null) && m_Engine.m_Settings.Appearance.UseAssetQuantities;
		
			for (int i = 0; i < (int)Columns.MaxColumns; i++)
			{
				subitems.Add(new ListViewItem.ListViewSubItem());
			}
			
			if (!ItemPrice.IsValidPrice((double)columnData[(int)Columns.MarketPrice]))
				a_QueryArgs.Item.BackColor = Color.White;
			else
				a_QueryArgs.Item.BackColor = m_Engine.GetPriceColor((double)columnData[(int)Columns.RefinedCost], (double)columnData[(int)Columns.MarketPrice], isQuantityOk);
			
			subitems[(int)Columns.Name].Text		= (string)columnData[(int)Columns.Name];
            subitems[(int)Columns.MetaLevel].Text	= columnData[(int)Columns.MetaLevel].ToString();
			subitems[(int)Columns.RefinedCost].Text	= ItemPrice.FormatPrice((double)columnData[(int)Columns.RefinedCost]);
			subitems[(int)Columns.MarketPrice].Text	= ItemPrice.FormatPrice((double)columnData[(int)Columns.MarketPrice]);
			subitems[(int)Columns.PriceDelta].Text	= ItemPrice.FormatPrice((double)columnData[(int)Columns.PriceDelta]);
			subitems[(int)Columns.Yield].Text		= String.Format("{0:0.00%}", (double)columnData[(int)Columns.Yield]);

			subitems[(int)Columns.Tritanium].Text	= Engine.FormatDouble((double)columnData[(int)Columns.Tritanium]);
			subitems[(int)Columns.Pyerite].Text		= Engine.FormatDouble((double)columnData[(int)Columns.Pyerite]);
			subitems[(int)Columns.Mexallon].Text	= Engine.FormatDouble((double)columnData[(int)Columns.Mexallon]);
			subitems[(int)Columns.Isogen].Text		= Engine.FormatDouble((double)columnData[(int)Columns.Isogen]);
			subitems[(int)Columns.Noxcium].Text		= Engine.FormatDouble((double)columnData[(int)Columns.Noxcium]);
			subitems[(int)Columns.Zydrine].Text		= Engine.FormatDouble((double)columnData[(int)Columns.Zydrine]);
			subitems[(int)Columns.Megacyte].Text	= Engine.FormatDouble((double)columnData[(int)Columns.Megacyte]);
			subitems[(int)Columns.Morphite].Text	= Engine.FormatDouble((double)columnData[(int)Columns.Morphite]);
			subitems[(int)Columns.Quantity].Text	= String.Format("{0:#,0}", columnData[(int)Columns.Quantity]);
			subitems[(int)Columns.Type].Text		= (string)columnData[(int)Columns.Type];
			
			double lossPercent						= (double)columnData[(int)Columns.LossPercent];
			bool isInvalidLossPercent				= double.IsInfinity(lossPercent);
			subitems[(int)Columns.LossPercent].Text	= isInvalidLossPercent ? "" : String.Format("{0:d}%", (int)(100 * lossPercent));
			subitems[(int)Columns.Volume].Text		= Engine.FormatDouble((double)columnData[(int)Columns.Volume]);
			subitems[(int)Columns.RefinedVolume].Text = Engine.FormatDouble((double)columnData[(int)Columns.RefinedVolume]);
		}
		
		private void SilentSetSelectedItem(ToolStripComboBox a_Combo, Object a_Item, EventHandler a_EventChanged)
		{
			// Disable notifications
			a_Combo.SelectedIndexChanged -= a_EventChanged;

			// Set selection
			a_Combo.SelectedItem = a_Item;

			// Enable notifications
			a_Combo.SelectedIndexChanged += a_EventChanged;
		}

		private void SilentSetSelectedItem(ToolStripComboBoxEx a_Combo, Object a_Item, EventHandler a_EventChanged, CancelEventHandler a_EventChanging)
		{
			// Disable notifications
			a_Combo.SelectedIndexChanged	-= a_EventChanged;
			a_Combo.SelectedIndexChanging	-= a_EventChanging;

			// Set selection
			a_Combo.SelectedItem = a_Item;

			// Enable notifications
			a_Combo.SelectedIndexChanged	+= a_EventChanged;
			a_Combo.SelectedIndexChanging	+= a_EventChanging;
		}

		private void Init_TlbCmbCharacter(bool a_IsSilent)
		{
			EventHandler		myHandler1	= new EventHandler(TlbCmbCharacter_SelectedIndexChanged);
			CancelEventHandler	myHandler2	= new CancelEventHandler(TlbCmbCharacter_SelectedIndexChanging);
			ToolStripComboBoxEx	currCombo	= TlbCmbCharacter;

			UInt32 oldCharID = character_AllItems;
			if (null != currCombo.SelectedItem)
				oldCharID = ((TextItemWithUInt32)currCombo.SelectedItem).Data;

			currCombo.Items.Clear();

			TextItemWithUInt32 newItem = new TextItemWithUInt32("[All items]", character_AllItems);
			TextItemWithUInt32 selectItem = newItem;
			currCombo.Items.Add(newItem);

			newItem = new TextItemWithUInt32("[Edit list...]", character_EditApiKeys);
			currCombo.Items.Add(newItem);

			foreach (Settings.V1._ApiChar currChar in m_Engine.m_Settings.ApiAccess.Chars)
			{
				newItem = new TextItemWithUInt32(currChar.CharacterName, currChar.CharacterID);
				currCombo.Items.Add(newItem);

				if (newItem.Data == oldCharID)
					selectItem = newItem;
			}

			if (a_IsSilent || (oldCharID == selectItem.Data))
				SilentSetSelectedItem(currCombo, selectItem, myHandler1, myHandler2);
			else
				currCombo.SelectedItem = selectItem;
		}
		
		private void Init_TlbCmbLocation(bool a_IsSilent)
		{
			EventHandler myHandler = new EventHandler(TlbCmbLocation_SelectedIndexChanged);
			ToolStripComboBox currCombo = TlbCmbLocation;

			currCombo.Items.Clear();

			TextItemWithObject newItem = new TextItemWithObject("[All items]", null);
			TextItemWithObject selectItem = newItem;
			currCombo.Items.Add(newItem);

			foreach (KeyValuePair<UInt32, ItemAssets> keyValuePair in m_Assets.Assets)
			{
				UInt32 locationID = keyValuePair.Key;
				string locationName = m_EveDatabase.GetLocationName(locationID);

				newItem = new TextItemWithObject(locationName, keyValuePair.Value);
				currCombo.Items.Add(newItem);
			}

			if (a_IsSilent)
				SilentSetSelectedItem(currCombo, selectItem, myHandler);
			else
				currCombo.SelectedItem = selectItem;
		}

		private void Init_TlbCmbContainer(bool a_IsSilent)
		{
			EventHandler myHandler = new EventHandler(TlbCmbContainer_SelectedIndexChanged);
			ToolStripComboBox currCombo = TlbCmbContainer;
			currCombo.Items.Clear();

			ItemAssets locationAssets	= (ItemAssets)TextItemWithObject.GetData(TlbCmbLocation.SelectedItem);
			ContainerTree containers	= m_Assets.GetContainerTree(locationAssets);

			TextItemWithObject newItem	= new TextItemWithObject("[All items]", locationAssets);
			TextItemWithObject selectItem = newItem;
			currCombo.Items.Add(newItem);

			foreach (KeyValuePair<ItemAssets, string> keyValuePair in containers)
			{
				newItem = new TextItemWithObject(keyValuePair.Value, keyValuePair.Key);
				currCombo.Items.Add(newItem);
			}

			if (a_IsSilent)
				SilentSetSelectedItem(currCombo, selectItem, myHandler);
			else
				currCombo.SelectedItem = selectItem;
		}

		private void CheckUpdates(bool a_IsSilent)
		{
			try
			{
				Process updater = new Process();
				updater.StartInfo.FileName  = "EveRefineryUpdater.exe";
				updater.StartInfo.Arguments = "/silent";
				updater.Start();
			}
			catch (System.Exception a_Exception)
			{
				if (!a_IsSilent)
					ErrorMessageBox.Show(a_Exception.Message);
			}
		}

        private void ShowBuildWarnings()
        {

		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this.Text = this.Text + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
	
			#if (!DEBUG)
				if (m_Engine.m_Settings.Options.CheckUpdates)
					CheckUpdates(true);
			#endif

            ShowBuildWarnings();
		
			while (!m_ItemsDB.LoadEveDatabase(m_EveDatabase, m_Engine.m_Settings.Options.DBPath))
			{
				FrmInvalidDB frmInvalidDB = new FrmInvalidDB(m_Engine.m_Settings.Options.DBPath);
				frmInvalidDB.ShowDialog(this);
				if (frmInvalidDB.m_SelectedDbPath == null)
				{
					Close();
					return;
				}

				m_Engine.m_Settings.Options.DBPath = frmInvalidDB.m_SelectedDbPath;
			}
			
			Init_TlbCmbCharacter(true);
			Init_TlbCmbLocation(true);
			Init_TlbCmbContainer(true);
			UpdateToolbarIcons();

			LoadSettings_Locations();
			LoadSettings_Columns();
			LoadSettings_Toolbars();
			LoadSettings_Generic();

			UpdatePricesSettingsHint();
			LoadMarketPrices(false, false);
			CheckMineralPricesExpiry();
			MakeRefineryItemList();
			UpdateStatus();
			
			TmrUpdate.Enabled = true;
		}
		
		private void LoadMarketPrices(bool a_Silent, bool a_DeleteOld)
		{
			try
			{
				Settings.V2._PriceSettings settings	= m_Engine.m_Settings.PriceLoad.Items;
				IPriceProvider provider = PriceProviderAuto.GetPriceProvider(settings);

				if (a_DeleteOld)
					m_MarketPrices.DropPrices(provider);

				m_MarketPrices.LoadPrices(provider, settings, a_Silent);
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Failed to load market prices:\n" + a_Exception.Message);
			}

			UpdateLstRefinery();
			UpdateStatus();
			
			if (0 != m_MarketPrices.GetQueueSize())
				m_RunningListUpdates = ListUpdates.Prices;
		}

		private void CheckMineralPricesExpiry()
		{
			DateTime expiryDate = m_Engine.m_Settings.Stats.LastMineralPricesEdit.AddDays(m_Engine.m_Settings.PriceLoad.Minerals.ExpiryDays);
			if (DateTime.UtcNow < expiryDate)
				return;

			MessageBox.Show("Your mineral prices are outdated.\nThis can make all calculations made by this program incorrect.\nPlease go to settings and edit mineral prices.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		
		private Columns Columns_EnumFromString(string a_ItemName)
		{
			for (int i = 0; i < (int)Columns.MaxColumns; i++)
			{
				Columns enumItem = (Columns)i;
				if (enumItem.ToString() == a_ItemName)
					return enumItem;
			}
			
			return Columns.MaxColumns;
		}

		private void LoadSettings_Columns()
		{
			foreach (Settings.V1._UIColumn currRow in m_Engine.m_Settings.UILocations.Columns)
			{
				Columns currColumnID = Columns_EnumFromString(currRow.Name);
				if (Columns.MaxColumns == currColumnID)
					continue;

				ColumnHeader currColumn = LstRefinery.Columns[(int)currColumnID];
				currColumn.DisplayIndex = (Int32)currRow.Index;
				currColumn.Width = (Int32)currRow.Width;
				
				if (currRow.Visible)
					ListViewEx.UnhideColumn(currColumn);
				else
					ListViewEx.HideColumn(currColumn);
			}
		}

		private void LoadSettings_Locations()
		{
			this.Location	= new Point(m_Engine.m_Settings.UILocations.MainWindow.X0, m_Engine.m_Settings.UILocations.MainWindow.Y0);
			this.Size		= new Size(m_Engine.m_Settings.UILocations.MainWindow.CX, m_Engine.m_Settings.UILocations.MainWindow.CY);
		}

		private ToolStripPanel GetToolPanel(ToolbarLocations a_Panel)
		{
			switch (a_Panel)
			{
			case ToolbarLocations.Top:
				return TlcToolContainer.TopToolStripPanel;
			case ToolbarLocations.Left:
				return TlcToolContainer.LeftToolStripPanel;
			case ToolbarLocations.Right:
				return TlcToolContainer.RightToolStripPanel;
			case ToolbarLocations.Bottom:
				return TlcToolContainer.BottomToolStripPanel;
			}
			
			Debug.Assert(false, "Invalid panel location");
			return TlcToolContainer.TopToolStripPanel;
		}
		
		private Control RemoveToolbarTool(string a_Name)
		{
			ToolStripPanel[] panels = new ToolStripPanel[]{TlcToolContainer.TopToolStripPanel, TlcToolContainer.LeftToolStripPanel, TlcToolContainer.RightToolStripPanel, TlcToolContainer.BottomToolStripPanel};

			foreach (ToolStripPanel currPanel in panels)
			{
				foreach (Control currControl in currPanel.Controls)
				{
					if (currControl.Name == a_Name)
					{
						currPanel.Controls.Remove(currControl);
						return currControl;
					}
				}
			}
		
			return null;
		}
		
		private void LoadSettings_SingleToolbar(Settings.V1._UIToolbar a_Toolbar)
		{
			Control currTool	= RemoveToolbarTool(a_Toolbar.Name);
			if (currTool == null)
				return;
						
			currTool.Location	= new Point(a_Toolbar.Location.X0, a_Toolbar.Location.Y0);
			currTool.Size		= new Size(a_Toolbar.Location.CX, a_Toolbar.Location.CY);

			GetToolPanel((ToolbarLocations)a_Toolbar.Panel).Controls.Add(currTool);
		}
		
		private void LoadSettings_Toolbars()
		{
			Control[] suspendList = new Control[]
			{
				TlcToolContainer,
				TlcToolContainer.LeftToolStripPanel,
				TlcToolContainer.TopToolStripPanel,
				TlcToolContainer.RightToolStripPanel,
				TlcToolContainer.BottomToolStripPanel,
			};

			foreach (Control currControl in suspendList)
				currControl.SuspendLayout();
				
			foreach (Settings.V1._UIToolbar currRow in m_Engine.m_Settings.UILocations.Toolbars)
			{
				LoadSettings_SingleToolbar(currRow);
			}

			foreach (Control currControl in suspendList)
				currControl.ResumeLayout();
		}

		private void LoadSettings_Generic()
		{
			TlbChkUseQuantities.Checked = m_Engine.m_Settings.Appearance.UseAssetQuantities;
		}

		private void UpdateSettings_Locations()
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				m_Engine.m_Settings.UILocations.MainWindow.X0 = this.Location.X;
                m_Engine.m_Settings.UILocations.MainWindow.Y0 = this.Location.Y;
                m_Engine.m_Settings.UILocations.MainWindow.CX = this.Size.Width;
                m_Engine.m_Settings.UILocations.MainWindow.CY = this.Size.Height;
			}
		}
		
		private void UpdateSettings_SingleToolbar(Control a_ToolControl, ToolbarLocations a_Panel)
		{
			Settings.V1._UIToolbar currToolbar = new Settings.V1._UIToolbar();
			currToolbar.Name		= a_ToolControl.Name;
			currToolbar.Location.X0	= a_ToolControl.Location.X;
            currToolbar.Location.Y0	= a_ToolControl.Location.Y;
            currToolbar.Location.CX	= a_ToolControl.Size.Width;
            currToolbar.Location.CY	= a_ToolControl.Size.Height;
			currToolbar.Panel	    = (UInt32)a_Panel;

			m_Engine.m_Settings.UILocations.Toolbars.Add(currToolbar);
		}

		private void UpdateSettings_Toolbars()
		{
			m_Engine.m_Settings.UILocations.Toolbars.Clear();
		
			foreach (Control currControl in TlcToolContainer.TopToolStripPanel.Controls)
			{
				UpdateSettings_SingleToolbar(currControl, ToolbarLocations.Top);
			}

			foreach (Control currControl in TlcToolContainer.LeftToolStripPanel.Controls)
			{
				UpdateSettings_SingleToolbar(currControl, ToolbarLocations.Left);
			}

			foreach (Control currControl in TlcToolContainer.RightToolStripPanel.Controls)
			{
				UpdateSettings_SingleToolbar(currControl, ToolbarLocations.Right);
			}

			foreach (Control currControl in TlcToolContainer.BottomToolStripPanel.Controls)
			{
				UpdateSettings_SingleToolbar(currControl, ToolbarLocations.Bottom);
			}
		}

		private void UpdateSettings_Columns()
		{
			m_Engine.m_Settings.UILocations.Columns.Clear();
			
			for (int i = 0; i < LstRefinery.Columns.Count; i++)
			{
				ColumnHeader currColumnHdr = LstRefinery.Columns[i];
				Columns currColumnID = (Columns)i;

				Settings.V1._UIColumn currColumn = new Settings.V1._UIColumn();
				currColumn.Name		= currColumnID.ToString();
				currColumn.Index	= (UInt32)currColumnHdr.DisplayIndex;
				currColumn.Visible	= ListViewEx.IsColumnVisible(currColumnHdr);
				currColumn.Width	= ListViewEx.GetHideableColumnWidth(currColumnHdr);
				m_Engine.m_Settings.UILocations.Columns.Add(currColumn);
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			UpdateSettings_Locations();
			UpdateSettings_Columns();
			UpdateSettings_Toolbars();
			m_Engine.SaveSettings();
		}

		private void UpdateStatus()
		{
			LblPriceQueue.Text = String.Format("{0:d}", m_MarketPrices.GetQueueSize());
			
			if (m_Assets.m_CacheTime <= DateTime.UtcNow)
			{
				LblAssetsCache.Text			= "Not locked";
				LblAssetsCache.ForeColor	= Color.DarkGreen;
			}
			else
			{
				TimeSpan timeRemaining		= m_Assets.m_CacheTime - DateTime.UtcNow;
				LblAssetsCache.Text			= String.Format ("{0:00}h {1:00}m {2:00}s", timeRemaining.TotalHours, timeRemaining.Minutes, timeRemaining.Seconds);
				LblAssetsCache.ForeColor	= Color.DarkRed;
			}
		}
		
		private void UpdateLstRefinery()
		{
			if (0 == LstRefinery.Items.Count)
				return;

			UpdateTotalsRow();
			LstRefinery.RedrawItems(0, LstRefinery.Items.Count - 1, true);
		}

		private void TmrUpdate_Tick(object sender, EventArgs e)
		{
			if (m_RunningListUpdates != 0)
				UpdateLstRefinery();
				
			UpdateStatus();

			if (0 == m_MarketPrices.GetQueueSize())
				m_RunningListUpdates &= ~ListUpdates.Prices;
		}

		private void TlbBtnAbout_Click(object sender, EventArgs e)
		{
			FrmAboutBox form = new FrmAboutBox();
			form.ShowDialog(this);
		}

		private void ShowSettings(FrmSettings.Pages a_Page)
		{
			FrmSettings frmSettings = new FrmSettings(a_Page, (Settings)m_Engine.m_Settings.Clone(), m_EveDatabase, LstRefinery.Columns);
			if (DialogResult.OK != frmSettings.ShowDialog(this))
				return;

			m_Engine.m_Settings = frmSettings.GetSettings();
			Init_TlbCmbCharacter(false);
			UpdateLstRefinery();
		}

		private void TlbBtnSettings_Click(object sender, EventArgs e)
		{
			ShowSettings(FrmSettings.Pages.Minerals);
		}

		private void SetColumnSortMark(Int32 a_Column)
		{
		/*
			for (int i = 0; i < LstRefinery.Columns.Count; i++)
			{
				ColumnHeader currColumn = LstRefinery.Columns[i];
			
				if (i != e.Column)
					currColumn.ImageIndex = -2;
				else
					currColumn.ImageIndex = (1 == m_SortDirection) ? 0 : 1;
			}

			 LstRefinery.Update();
		*/
		}

		private void LstRefinery_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (m_SortType == (Columns)e.Column)
			{
				m_SortDirection	= -m_SortDirection;
			}
			else
			{
				m_SortType = (Columns)e.Column;
				m_SortDirection	= 1;
			}

			SetColumnSortMark(e.Column);
			MakeRefineryItemList();
		}
		
		private void TlbCmbCharacter_SelectedIndexChanging(object sender, CancelEventArgs e)
		{
			UInt32 charID = ((TextItemWithUInt32)TlbCmbCharacter.SelectedItem).Data;

			switch (charID)
			{
				case character_AllItems:
					m_Assets.UnloadAssets();
					break;
				case character_EditApiKeys:
					ShowSettings(FrmSettings.Pages.ApiKeys);
					e.Cancel = true;
					break;
				default:
					e.Cancel = !m_Assets.LoadAssets(charID);
					break;
			}
			
			UpdateStatus();
		}

		private void TlbCmbCharacter_SelectedIndexChanged(object sender, EventArgs e)
		{
			Init_TlbCmbLocation(false);
		}

		private void TlbCmbLocation_SelectedIndexChanged(object sender, EventArgs e)
		{
			Init_TlbCmbContainer(false);
		}

		private void TlbChkUseQuantities_CheckedChanged(object sender, EventArgs e)
		{
			m_Engine.m_Settings.Appearance.UseAssetQuantities = TlbChkUseQuantities.Checked;
			UpdateToolbarIcons();
			UpdateLstRefinery();
		}
		
		private void UpdateTotalsRow()
		{
			if (m_TotalsItem == null)
				return;
			
			m_TotalsItem.ItemData	= CreateSpecialItem_Totals();
			m_TotalsItem.Quantity	= 0;
			
			ItemRecord totalRecord	= m_TotalsItem.ItemData;
			
			foreach (MainListItem listItem in m_ItemList)
			{
				if (listItem.TypeID == SpecialTypeID_Totals)
					continue;
			
				ItemRecord currRecord = listItem.ItemData;
			
				lock (currRecord)
				{
					m_TotalsItem.Quantity			+= listItem.Quantity;
					m_TotalsItem.ItemData.Volume	+= listItem.Quantity * currRecord.Volume;

					bool isPriceExpired	= !currRecord.IsPricesOk(m_Engine.m_Settings.PriceLoad.Items.ExpiryDays);
					bool isZeroPrice	= (currRecord.Price == 0);

					if (!isPriceExpired && !isZeroPrice)
						totalRecord.Price += listItem.Quantity * currRecord.Price;
					
					for (int i = 0; i < currRecord.MaterialAmount.Count(); i++)
					{
						double currAmount = m_Engine.GetPerfectRefiningQuota(currRecord, listItem.Quantity, (Materials)i);
						totalRecord.MaterialAmount[i] += currAmount;
					}
				}
			}
		}

		private void TlbBtnUpdatePrices_Click(object sender, EventArgs e)
		{
			if (DialogResult.Yes != MessageBox.Show("Do you really want to reset and update prices for that region?", Application.ProductName, MessageBoxButtons.YesNo))
				return;
		
			LoadMarketPrices(true, true);
		}

		private void TlbCmbContainer_SelectedIndexChanged(object sender, EventArgs e)
		{
			MakeRefineryItemList();
		}
		
		private Image GetImageWithNoSign(Image a_Image, bool a_AddNoSign)
		{
			if (!a_AddNoSign)
				return a_Image;
				
			Image result = new Bitmap(a_Image.Width, a_Image.Height);

			using (System.Drawing.Graphics canvas = Graphics.FromImage(result))
			{
				Image noSign = ImlToolbarButtons.Images["NoSign"];
				
				canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
				canvas.DrawImage(a_Image, new Rectangle(0, 0, a_Image.Width, a_Image.Height), new Rectangle(0, 0, a_Image.Width, a_Image.Height), GraphicsUnit.Pixel);
				canvas.DrawImage(noSign, new Rectangle(0, 0, a_Image.Width, a_Image.Height), new Rectangle(0, 0, noSign.Width, noSign.Height), GraphicsUnit.Pixel);
				canvas.Save();
			}
			
			return result;
		}
		
		private void UpdateToolbarIcons()
		{
			TlbChkIgnoreContainers.Image	= GetImageWithNoSign(ImlToolbarButtons.Images["Container"], TlbChkIgnoreContainers.Checked);
			TlbChkIgnoreShips.Image			= GetImageWithNoSign(ImlToolbarButtons.Images["Ship"], TlbChkIgnoreShips.Checked);
			TlbChkIgnoreAssembled.Image		= GetImageWithNoSign(ImlToolbarButtons.Images["Box"], TlbChkIgnoreAssembled.Checked);
			TlbChkUseQuantities.Image		= GetImageWithNoSign(ImlToolbarButtons.Images["Number"], !TlbChkUseQuantities.Checked);
		}

		private void TlbChkIgnoreContainers_CheckedChanged(object sender, EventArgs e)
		{
			UpdateToolbarIcons();
			MakeRefineryItemList();
		}

		private void TlbChkIgnoreShips_CheckedChanged(object sender, EventArgs e)
		{
			UpdateToolbarIcons();
			MakeRefineryItemList();
		}

		private void TlbChkIgnoreAssembled_CheckedChanged(object sender, EventArgs e)
		{
			UpdateToolbarIcons();
			MakeRefineryItemList();
		}

		private void TlbBtnWhatsnew_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("whatsnew.txt");
		}

		class CompareColumnIndices : IComparer<int>
		{
			private ListView.ColumnHeaderCollection m_Headers;

			public CompareColumnIndices(ListView.ColumnHeaderCollection a_Headers)
			{
				m_Headers = a_Headers;
			}

			public int Compare(int a_Object1, int a_Object2)
			{
				ColumnHeader header1 = m_Headers[a_Object1];
				ColumnHeader header2 = m_Headers[a_Object2];

				return header1.DisplayIndex - header2.DisplayIndex;
			}
		}

		private List<int> GetVisibleColumnOrder()
		{
			List<int> result = new List<int>();

			for (int i = 0; i < LstRefinery.Columns.Count; i++)
			{
				if (!ListViewEx.IsColumnVisible(LstRefinery.Columns[i]))
					continue;

				result.Add(i);
			}

			CompareColumnIndices comparer = new CompareColumnIndices(LstRefinery.Columns);
			result.Sort(comparer);

			return result;
		}

		private Exporter.ExportedData GetExportedData()
		{
			List<int> columnOrder = GetVisibleColumnOrder();

			int numHeaderItems = 1;

			Exporter.ExportedData result = new Exporter.ExportedData();
			result.Aligns = new HorizontalAlignment[columnOrder.Count];
			result.Rows = new Exporter.ExportedRow[m_ItemList.Length + numHeaderItems];

			// Aligns
			for (int j = 0; j < columnOrder.Count; j++)
			{
				int actualColumn = columnOrder[j];
				result.Aligns[j] = LstRefinery.Columns[actualColumn].TextAlign;
			}

			// Header
			Exporter.ExportedRow currRow = new Exporter.ExportedRow();
			result.Rows[0] = currRow;
			
			currRow.Color = Color.LightGray;
			currRow.Cells = new String[columnOrder.Count];
			for (int j = 0; j < currRow.Cells.Length; j++)
			{
				int actualColumn = columnOrder[j];
				currRow.Cells[j] = LstRefinery.Columns[actualColumn].Text;
			}

			// Visible items
			for (int i = 0; i < m_ItemList.Length; i++)
			{
				RetrieveVirtualItemEventArgs itemData = new RetrieveVirtualItemEventArgs(i);
				Refinery_RetrieveVirtualItem(this, itemData);

				currRow = new Exporter.ExportedRow();
				result.Rows[i + numHeaderItems] = currRow;

				currRow.Color = itemData.Item.BackColor;
				currRow.Cells = new String[columnOrder.Count];

				for (int j = 0; j < currRow.Cells.Length; j++)
				{
					int actualColumn = columnOrder[j];
					currRow.Cells[j] = itemData.Item.SubItems[actualColumn].Text;
				}
			}

			return result;
		}

        private void TlbChkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = !TopMost;
        }

        private void TlbBtnExport_Click(object sender, EventArgs e)
		{
			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.Title = "Select location for exported file";
			fileDialog.Filter = "Comma-separated values (*.csv)|*.csv|Html (*.htm)|*.htm";
			fileDialog.FilterIndex = 2;
			fileDialog.RestoreDirectory = true;
			if (DialogResult.OK != fileDialog.ShowDialog())
				return;

			try
			{
				Exporter.ExportedData data = GetExportedData();

				switch (fileDialog.FilterIndex)
				{
					case 1:
						Exporter.ExportAsCsv(fileDialog.FileName, data);
						break;
					case 2:
						Exporter.ExportAsHtml(fileDialog.FileName, data);
						break;
				}
			}
			catch (System.Exception a_Exception)
			{
				ErrorMessageBox.Show("Failed to export data:\n" + a_Exception.Message);
			}
		}

		private void UpdatePricesSettingsHint()
		{
			IPriceProvider priceProvider = PriceProviderAuto.GetPriceProvider(m_Engine.m_Settings.PriceLoad.Items);
			TlbLblPricesType.Text = priceProvider.GetCurrentFilterHint(m_EveDatabase);
		}

		private void TlbBtnPricesType_Click(object sender, EventArgs e)
		{
			FrmPriceType dialog = new FrmPriceType(m_EveDatabase, m_Engine.m_Settings.PriceLoad.Items);
			if (DialogResult.OK != dialog.ShowDialog(this))
				return;

			UpdatePricesSettingsHint();
			LoadMarketPrices(false, false);
		}
    }
    
    class MainListItem
    {
		public UInt32		TypeID;
		public UInt32		Quantity;
		public ItemRecord	ItemData;
    }
}
