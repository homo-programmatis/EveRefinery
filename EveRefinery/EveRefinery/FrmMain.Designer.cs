namespace EveRefinery
{
	partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.ImlSortIcons = new System.Windows.Forms.ImageList(this.components);
			this.TmrUpdate = new System.Windows.Forms.Timer(this.components);
			this.TlcToolContainer = new System.Windows.Forms.ToolStripContainer();
			this.SplTable1 = new System.Windows.Forms.TableLayoutPanel();
			this.LstRefinery = new SpecialFNs.ListViewEx();
			this.ClmItemName = new System.Windows.Forms.ColumnHeader();
			this.ClmMetaLevel = new System.Windows.Forms.ColumnHeader();
			this.ClmRefinedCost = new System.Windows.Forms.ColumnHeader();
			this.ClmSellPrice = new System.Windows.Forms.ColumnHeader();
			this.ClmDeltaPrice = new System.Windows.Forms.ColumnHeader();
			this.ClmTritanium = new System.Windows.Forms.ColumnHeader();
			this.ClmPyerite = new System.Windows.Forms.ColumnHeader();
			this.ClmMexallon = new System.Windows.Forms.ColumnHeader();
			this.ClmIsogen = new System.Windows.Forms.ColumnHeader();
			this.ClmNocxium = new System.Windows.Forms.ColumnHeader();
			this.ClmZydrine = new System.Windows.Forms.ColumnHeader();
			this.ClmMegacyte = new System.Windows.Forms.ColumnHeader();
			this.ClmMorphite = new System.Windows.Forms.ColumnHeader();
			this.ClmQuantity = new System.Windows.Forms.ColumnHeader();
			this.ClmItemType = new System.Windows.Forms.ColumnHeader();
			this.ClmLossPercent = new System.Windows.Forms.ColumnHeader();
			this.ClmVolume = new System.Windows.Forms.ColumnHeader();
			this.ClmRefinedVolume = new System.Windows.Forms.ColumnHeader();
			this.TblStatusBar = new System.Windows.Forms.TableLayoutPanel();
			this.LblPriceQueueHint = new System.Windows.Forms.Label();
			this.LblPriceQueue = new System.Windows.Forms.Label();
			this.LblAssetsCacheHint = new System.Windows.Forms.Label();
			this.LblAssetsCache = new System.Windows.Forms.Label();
			this.TlbMainToolbar = new System.Windows.Forms.ToolStrip();
			this.TlbBtnExport = new System.Windows.Forms.ToolStripButton();
			this.TlbBtnSettings = new System.Windows.Forms.ToolStripButton();
			this.TlbBtnAbout = new System.Windows.Forms.ToolStripButton();
			this.TlbBtnWhatsnew = new System.Windows.Forms.ToolStripButton();
			this.TlbPrices = new System.Windows.Forms.ToolStrip();
			this.TlbLblPrices = new System.Windows.Forms.ToolStripLabel();
			this.TlbLblPricesType = new System.Windows.Forms.ToolStripLabel();
			this.TlbBtnPricesType = new System.Windows.Forms.ToolStripButton();
			this.TlbBtnUpdatePrices = new System.Windows.Forms.ToolStripButton();
			this.TlbAssets = new System.Windows.Forms.ToolStrip();
			this.TlbLblAssets = new System.Windows.Forms.ToolStripLabel();
			this.TlbCmbCharacter = new SpecialFNs.ToolStripComboBoxEx();
			this.TlbCmbLocation = new System.Windows.Forms.ToolStripComboBox();
			this.TlbCmbContainer = new System.Windows.Forms.ToolStripComboBox();
			this.TlbChkIgnoreContainers = new System.Windows.Forms.ToolStripButton();
			this.TlbChkIgnoreShips = new System.Windows.Forms.ToolStripButton();
			this.TlbChkIgnoreAssembled = new System.Windows.Forms.ToolStripButton();
			this.TlbChkUseQuantities = new System.Windows.Forms.ToolStripButton();
			this.ImlToolbarButtons = new System.Windows.Forms.ImageList(this.components);
			this.TlcToolContainer.ContentPanel.SuspendLayout();
			this.TlcToolContainer.TopToolStripPanel.SuspendLayout();
			this.TlcToolContainer.SuspendLayout();
			this.SplTable1.SuspendLayout();
			this.TblStatusBar.SuspendLayout();
			this.TlbMainToolbar.SuspendLayout();
			this.TlbPrices.SuspendLayout();
			this.TlbAssets.SuspendLayout();
			this.SuspendLayout();
			// 
			// ImlSortIcons
			// 
			this.ImlSortIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImlSortIcons.ImageStream")));
			this.ImlSortIcons.TransparentColor = System.Drawing.Color.White;
			this.ImlSortIcons.Images.SetKeyName(0, "ArrowDown.png");
			this.ImlSortIcons.Images.SetKeyName(1, "ArrowUp.png");
			// 
			// TmrUpdate
			// 
			this.TmrUpdate.Interval = 5000;
			this.TmrUpdate.Tick += new System.EventHandler(this.TmrUpdate_Tick);
			// 
			// TlcToolContainer
			// 
			// 
			// TlcToolContainer.ContentPanel
			// 
			this.TlcToolContainer.ContentPanel.Controls.Add(this.SplTable1);
			this.TlcToolContainer.ContentPanel.Size = new System.Drawing.Size(1075, 264);
			this.TlcToolContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TlcToolContainer.Location = new System.Drawing.Point(0, 0);
			this.TlcToolContainer.Name = "TlcToolContainer";
			this.TlcToolContainer.Size = new System.Drawing.Size(1075, 314);
			this.TlcToolContainer.TabIndex = 0;
			this.TlcToolContainer.Text = "toolStripContainer1";
			// 
			// TlcToolContainer.TopToolStripPanel
			// 
			this.TlcToolContainer.TopToolStripPanel.Controls.Add(this.TlbMainToolbar);
			this.TlcToolContainer.TopToolStripPanel.Controls.Add(this.TlbPrices);
			this.TlcToolContainer.TopToolStripPanel.Controls.Add(this.TlbAssets);
			// 
			// SplTable1
			// 
			this.SplTable1.ColumnCount = 1;
			this.SplTable1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable1.Controls.Add(this.LstRefinery, 0, 0);
			this.SplTable1.Controls.Add(this.TblStatusBar, 0, 1);
			this.SplTable1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplTable1.Location = new System.Drawing.Point(0, 0);
			this.SplTable1.Name = "SplTable1";
			this.SplTable1.RowCount = 2;
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.Size = new System.Drawing.Size(1075, 264);
			this.SplTable1.TabIndex = 5;
			// 
			// LstRefinery
			// 
			this.LstRefinery.AllowColumnReorder = true;
			this.LstRefinery.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ClmItemName,
            this.ClmMetaLevel,
            this.ClmRefinedCost,
            this.ClmSellPrice,
            this.ClmDeltaPrice,
            this.ClmTritanium,
            this.ClmPyerite,
            this.ClmMexallon,
            this.ClmIsogen,
            this.ClmNocxium,
            this.ClmZydrine,
            this.ClmMegacyte,
            this.ClmMorphite,
            this.ClmQuantity,
            this.ClmItemType,
            this.ClmLossPercent,
            this.ClmVolume,
            this.ClmRefinedVolume});
			this.LstRefinery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LstRefinery.FullRowSelect = true;
			this.LstRefinery.HideSelection = false;
			this.LstRefinery.Location = new System.Drawing.Point(3, 3);
			this.LstRefinery.Name = "LstRefinery";
			this.LstRefinery.ShowItemToolTips = true;
			this.LstRefinery.Size = new System.Drawing.Size(1069, 238);
			this.LstRefinery.TabIndex = 1;
			this.LstRefinery.UseCompatibleStateImageBehavior = false;
			this.LstRefinery.View = System.Windows.Forms.View.Details;
			this.LstRefinery.VirtualMode = true;
			this.LstRefinery.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.LstRefinery_ColumnClick);
			this.LstRefinery.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.Refinery_RetrieveVirtualItem);
			// 
			// ClmItemName
			// 
			this.ClmItemName.Text = "Item Name";
			this.ClmItemName.Width = 200;
			// 
			// ClmMetaLevel
			// 
			this.ClmMetaLevel.Text = "Meta";
			this.ClmMetaLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ClmMetaLevel.Width = 40;
			// 
			// ClmRefinedCost
			// 
			this.ClmRefinedCost.DisplayIndex = 3;
			this.ClmRefinedCost.Text = "Refined Cost";
			this.ClmRefinedCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ClmRefinedCost.Width = 80;
			// 
			// ClmSellPrice
			// 
			this.ClmSellPrice.DisplayIndex = 4;
			this.ClmSellPrice.Text = "Sell Price";
			this.ClmSellPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ClmSellPrice.Width = 80;
			// 
			// ClmDeltaPrice
			// 
			this.ClmDeltaPrice.DisplayIndex = 5;
			this.ClmDeltaPrice.Text = "Delta";
			this.ClmDeltaPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ClmDeltaPrice.Width = 80;
			// 
			// ClmTritanium
			// 
			this.ClmTritanium.DisplayIndex = 6;
			this.ClmTritanium.Text = "Tritanium";
			this.ClmTritanium.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmPyerite
			// 
			this.ClmPyerite.DisplayIndex = 7;
			this.ClmPyerite.Text = "Pyerite";
			this.ClmPyerite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmMexallon
			// 
			this.ClmMexallon.DisplayIndex = 8;
			this.ClmMexallon.Text = "Mexallon";
			this.ClmMexallon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmIsogen
			// 
			this.ClmIsogen.DisplayIndex = 9;
			this.ClmIsogen.Text = "Isogen";
			this.ClmIsogen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmNocxium
			// 
			this.ClmNocxium.DisplayIndex = 10;
			this.ClmNocxium.Text = "Nocxium";
			this.ClmNocxium.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmZydrine
			// 
			this.ClmZydrine.DisplayIndex = 11;
			this.ClmZydrine.Text = "Zydrine";
			this.ClmZydrine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmMegacyte
			// 
			this.ClmMegacyte.DisplayIndex = 12;
			this.ClmMegacyte.Text = "Megacyte";
			this.ClmMegacyte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmMorphite
			// 
			this.ClmMorphite.DisplayIndex = 13;
			this.ClmMorphite.Text = "Morphite";
			this.ClmMorphite.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmQuantity
			// 
			this.ClmQuantity.DisplayIndex = 2;
			this.ClmQuantity.Text = "Qty";
			this.ClmQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ClmQuantity.Width = 50;
			// 
			// ClmItemType
			// 
			this.ClmItemType.Text = "Type";
			this.ClmItemType.Width = 200;
			// 
			// ClmLossPercent
			// 
			this.ClmLossPercent.Text = "Loss %";
			this.ClmLossPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmVolume
			// 
			this.ClmVolume.Text = "Volume";
			this.ClmVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// ClmRefinedVolume
			// 
			this.ClmRefinedVolume.Text = "Refined Volume";
			this.ClmRefinedVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// TblStatusBar
			// 
			this.TblStatusBar.ColumnCount = 5;
			this.TblStatusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.TblStatusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.TblStatusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
			this.TblStatusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.TblStatusBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TblStatusBar.Controls.Add(this.LblPriceQueueHint, 0, 0);
			this.TblStatusBar.Controls.Add(this.LblPriceQueue, 1, 0);
			this.TblStatusBar.Controls.Add(this.LblAssetsCacheHint, 2, 0);
			this.TblStatusBar.Controls.Add(this.LblAssetsCache, 3, 0);
			this.TblStatusBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TblStatusBar.Location = new System.Drawing.Point(0, 244);
			this.TblStatusBar.Margin = new System.Windows.Forms.Padding(0);
			this.TblStatusBar.Name = "TblStatusBar";
			this.TblStatusBar.RowCount = 1;
			this.TblStatusBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TblStatusBar.Size = new System.Drawing.Size(1075, 20);
			this.TblStatusBar.TabIndex = 2;
			// 
			// LblPriceQueueHint
			// 
			this.LblPriceQueueHint.AutoSize = true;
			this.LblPriceQueueHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblPriceQueueHint.Location = new System.Drawing.Point(3, 0);
			this.LblPriceQueueHint.Name = "LblPriceQueueHint";
			this.LblPriceQueueHint.Size = new System.Drawing.Size(84, 20);
			this.LblPriceQueueHint.TabIndex = 5;
			this.LblPriceQueueHint.Text = "Prices queue :";
			this.LblPriceQueueHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblPriceQueue
			// 
			this.LblPriceQueue.AutoSize = true;
			this.LblPriceQueue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblPriceQueue.Location = new System.Drawing.Point(93, 0);
			this.LblPriceQueue.Name = "LblPriceQueue";
			this.LblPriceQueue.Size = new System.Drawing.Size(44, 20);
			this.LblPriceQueue.TabIndex = 6;
			this.LblPriceQueue.Text = "0";
			this.LblPriceQueue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblAssetsCacheHint
			// 
			this.LblAssetsCacheHint.AutoSize = true;
			this.LblAssetsCacheHint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblAssetsCacheHint.Location = new System.Drawing.Point(143, 0);
			this.LblAssetsCacheHint.Name = "LblAssetsCacheHint";
			this.LblAssetsCacheHint.Size = new System.Drawing.Size(124, 20);
			this.LblAssetsCacheHint.TabIndex = 7;
			this.LblAssetsCacheHint.Text = "Can\'t update assets for :";
			this.LblAssetsCacheHint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblAssetsCache
			// 
			this.LblAssetsCache.AutoSize = true;
			this.LblAssetsCache.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblAssetsCache.Location = new System.Drawing.Point(273, 0);
			this.LblAssetsCache.Name = "LblAssetsCache";
			this.LblAssetsCache.Size = new System.Drawing.Size(104, 20);
			this.LblAssetsCache.TabIndex = 8;
			this.LblAssetsCache.Text = "Now";
			this.LblAssetsCache.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TlbMainToolbar
			// 
			this.TlbMainToolbar.Dock = System.Windows.Forms.DockStyle.None;
			this.TlbMainToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TlbBtnExport,
            this.TlbBtnSettings,
            this.TlbBtnAbout,
            this.TlbBtnWhatsnew});
			this.TlbMainToolbar.Location = new System.Drawing.Point(3, 0);
			this.TlbMainToolbar.Name = "TlbMainToolbar";
			this.TlbMainToolbar.Size = new System.Drawing.Size(104, 25);
			this.TlbMainToolbar.TabIndex = 5;
			this.TlbMainToolbar.Text = "Main Toolbar";
			// 
			// TlbBtnExport
			// 
			this.TlbBtnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbBtnExport.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnExport.Image")));
			this.TlbBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TlbBtnExport.Name = "TlbBtnExport";
			this.TlbBtnExport.Size = new System.Drawing.Size(23, 22);
			this.TlbBtnExport.Text = "Export visible data";
			this.TlbBtnExport.Click += new System.EventHandler(this.TlbBtnExport_Click);
			// 
			// TlbBtnSettings
			// 
			this.TlbBtnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbBtnSettings.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnSettings.Image")));
			this.TlbBtnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TlbBtnSettings.Name = "TlbBtnSettings";
			this.TlbBtnSettings.Size = new System.Drawing.Size(23, 22);
			this.TlbBtnSettings.Text = "Configure settings...";
			this.TlbBtnSettings.Click += new System.EventHandler(this.TlbBtnSettings_Click);
			// 
			// TlbBtnAbout
			// 
			this.TlbBtnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbBtnAbout.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnAbout.Image")));
			this.TlbBtnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TlbBtnAbout.Name = "TlbBtnAbout";
			this.TlbBtnAbout.Size = new System.Drawing.Size(23, 22);
			this.TlbBtnAbout.Text = "Show About box...";
			this.TlbBtnAbout.Click += new System.EventHandler(this.TlbBtnAbout_Click);
			// 
			// TlbBtnWhatsnew
			// 
			this.TlbBtnWhatsnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbBtnWhatsnew.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnWhatsnew.Image")));
			this.TlbBtnWhatsnew.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.TlbBtnWhatsnew.Name = "TlbBtnWhatsnew";
			this.TlbBtnWhatsnew.Size = new System.Drawing.Size(23, 22);
			this.TlbBtnWhatsnew.Text = "Whats new?";
			this.TlbBtnWhatsnew.Click += new System.EventHandler(this.TlbBtnWhatsnew_Click);
			// 
			// TlbPrices
			// 
			this.TlbPrices.Dock = System.Windows.Forms.DockStyle.None;
			this.TlbPrices.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TlbLblPrices,
            this.TlbLblPricesType,
            this.TlbBtnPricesType,
            this.TlbBtnUpdatePrices});
			this.TlbPrices.Location = new System.Drawing.Point(107, 0);
			this.TlbPrices.Name = "TlbPrices";
			this.TlbPrices.Size = new System.Drawing.Size(252, 25);
			this.TlbPrices.TabIndex = 8;
			this.TlbPrices.Text = "Prices";
			// 
			// TlbLblPrices
			// 
			this.TlbLblPrices.Name = "TlbLblPrices";
			this.TlbLblPrices.Size = new System.Drawing.Size(44, 22);
			this.TlbLblPrices.Text = "Prices :";
			// 
			// TlbLblPricesType
			// 
			this.TlbLblPricesType.Name = "TlbLblPricesType";
			this.TlbLblPricesType.Size = new System.Drawing.Size(114, 22);
			this.TlbLblPricesType.Text = "[Prices settings hint]";
			// 
			// TlbBtnPricesType
			// 
			this.TlbBtnPricesType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.TlbBtnPricesType.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnPricesType.Image")));
			this.TlbBtnPricesType.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TlbBtnPricesType.Name = "TlbBtnPricesType";
			this.TlbBtnPricesType.Size = new System.Drawing.Size(28, 22);
			this.TlbBtnPricesType.Text = "[...]";
			this.TlbBtnPricesType.ToolTipText = "Click to change price settings";
			this.TlbBtnPricesType.Click += new System.EventHandler(this.TlbBtnPricesType_Click);
			// 
			// TlbBtnUpdatePrices
			// 
			this.TlbBtnUpdatePrices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbBtnUpdatePrices.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnUpdatePrices.Image")));
			this.TlbBtnUpdatePrices.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TlbBtnUpdatePrices.Name = "TlbBtnUpdatePrices";
			this.TlbBtnUpdatePrices.Size = new System.Drawing.Size(23, 22);
			this.TlbBtnUpdatePrices.Text = "Update prices";
			this.TlbBtnUpdatePrices.Click += new System.EventHandler(this.TlbBtnUpdatePrices_Click);
			// 
			// TlbAssets
			// 
			this.TlbAssets.Dock = System.Windows.Forms.DockStyle.None;
			this.TlbAssets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TlbLblAssets,
            this.TlbCmbCharacter,
            this.TlbCmbLocation,
            this.TlbCmbContainer,
            this.TlbChkIgnoreContainers,
            this.TlbChkIgnoreShips,
            this.TlbChkIgnoreAssembled,
            this.TlbChkUseQuantities});
			this.TlbAssets.Location = new System.Drawing.Point(3, 25);
			this.TlbAssets.Name = "TlbAssets";
			this.TlbAssets.Size = new System.Drawing.Size(648, 25);
			this.TlbAssets.TabIndex = 9;
			this.TlbAssets.Text = "Assets";
			// 
			// TlbLblAssets
			// 
			this.TlbLblAssets.Name = "TlbLblAssets";
			this.TlbLblAssets.Size = new System.Drawing.Size(46, 22);
			this.TlbLblAssets.Text = "Assets :";
			// 
			// TlbCmbCharacter
			// 
			this.TlbCmbCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TlbCmbCharacter.Name = "TlbCmbCharacter";
			this.TlbCmbCharacter.Size = new System.Drawing.Size(121, 25);
			this.TlbCmbCharacter.Sorted = true;
			this.TlbCmbCharacter.SelectedIndexChanging += new System.ComponentModel.CancelEventHandler(this.TlbCmbCharacter_SelectedIndexChanging);
			this.TlbCmbCharacter.SelectedIndexChanged += new System.EventHandler(this.TlbCmbCharacter_SelectedIndexChanged);
			// 
			// TlbCmbLocation
			// 
			this.TlbCmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TlbCmbLocation.DropDownWidth = 350;
			this.TlbCmbLocation.Name = "TlbCmbLocation";
			this.TlbCmbLocation.Size = new System.Drawing.Size(250, 25);
			this.TlbCmbLocation.Sorted = true;
			this.TlbCmbLocation.SelectedIndexChanged += new System.EventHandler(this.TlbCmbLocation_SelectedIndexChanged);
			// 
			// TlbCmbContainer
			// 
			this.TlbCmbContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TlbCmbContainer.DropDownWidth = 350;
			this.TlbCmbContainer.Name = "TlbCmbContainer";
			this.TlbCmbContainer.Size = new System.Drawing.Size(121, 25);
			this.TlbCmbContainer.Sorted = true;
			this.TlbCmbContainer.SelectedIndexChanged += new System.EventHandler(this.TlbCmbContainer_SelectedIndexChanged);
			// 
			// TlbChkIgnoreContainers
			// 
			this.TlbChkIgnoreContainers.CheckOnClick = true;
			this.TlbChkIgnoreContainers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbChkIgnoreContainers.Image = ((System.Drawing.Image)(resources.GetObject("TlbChkIgnoreContainers.Image")));
			this.TlbChkIgnoreContainers.ImageTransparentColor = System.Drawing.Color.White;
			this.TlbChkIgnoreContainers.Name = "TlbChkIgnoreContainers";
			this.TlbChkIgnoreContainers.Size = new System.Drawing.Size(23, 22);
			this.TlbChkIgnoreContainers.Text = "Ignore assembled containers and their contents";
			this.TlbChkIgnoreContainers.CheckedChanged += new System.EventHandler(this.TlbChkIgnoreContainers_CheckedChanged);
			// 
			// TlbChkIgnoreShips
			// 
			this.TlbChkIgnoreShips.CheckOnClick = true;
			this.TlbChkIgnoreShips.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbChkIgnoreShips.Image = ((System.Drawing.Image)(resources.GetObject("TlbChkIgnoreShips.Image")));
			this.TlbChkIgnoreShips.ImageTransparentColor = System.Drawing.Color.White;
			this.TlbChkIgnoreShips.Name = "TlbChkIgnoreShips";
			this.TlbChkIgnoreShips.Size = new System.Drawing.Size(23, 22);
			this.TlbChkIgnoreShips.Text = "Ignore assembled ships and their contents";
			this.TlbChkIgnoreShips.CheckedChanged += new System.EventHandler(this.TlbChkIgnoreShips_CheckedChanged);
			// 
			// TlbChkIgnoreAssembled
			// 
			this.TlbChkIgnoreAssembled.CheckOnClick = true;
			this.TlbChkIgnoreAssembled.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbChkIgnoreAssembled.Image = ((System.Drawing.Image)(resources.GetObject("TlbChkIgnoreAssembled.Image")));
			this.TlbChkIgnoreAssembled.ImageTransparentColor = System.Drawing.Color.White;
			this.TlbChkIgnoreAssembled.Name = "TlbChkIgnoreAssembled";
			this.TlbChkIgnoreAssembled.Size = new System.Drawing.Size(23, 22);
			this.TlbChkIgnoreAssembled.Text = "Ignore assembled items and their contents";
			this.TlbChkIgnoreAssembled.CheckedChanged += new System.EventHandler(this.TlbChkIgnoreAssembled_CheckedChanged);
			// 
			// TlbChkUseQuantities
			// 
			this.TlbChkUseQuantities.CheckOnClick = true;
			this.TlbChkUseQuantities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TlbChkUseQuantities.Image = ((System.Drawing.Image)(resources.GetObject("TlbChkUseQuantities.Image")));
			this.TlbChkUseQuantities.ImageTransparentColor = System.Drawing.Color.White;
			this.TlbChkUseQuantities.Name = "TlbChkUseQuantities";
			this.TlbChkUseQuantities.Size = new System.Drawing.Size(23, 22);
			this.TlbChkUseQuantities.Text = "Use quantities";
			this.TlbChkUseQuantities.CheckedChanged += new System.EventHandler(this.TlbChkUseQuantities_CheckedChanged);
			// 
			// ImlToolbarButtons
			// 
			this.ImlToolbarButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImlToolbarButtons.ImageStream")));
			this.ImlToolbarButtons.TransparentColor = System.Drawing.Color.White;
			this.ImlToolbarButtons.Images.SetKeyName(0, "Container");
			this.ImlToolbarButtons.Images.SetKeyName(1, "Ship");
			this.ImlToolbarButtons.Images.SetKeyName(2, "NoSign");
			this.ImlToolbarButtons.Images.SetKeyName(3, "Box");
			this.ImlToolbarButtons.Images.SetKeyName(4, "Number");
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1075, 314);
			this.Controls.Add(this.TlcToolContainer);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmMain";
			this.Text = "Eve Refinery";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.TlcToolContainer.ContentPanel.ResumeLayout(false);
			this.TlcToolContainer.TopToolStripPanel.ResumeLayout(false);
			this.TlcToolContainer.TopToolStripPanel.PerformLayout();
			this.TlcToolContainer.ResumeLayout(false);
			this.TlcToolContainer.PerformLayout();
			this.SplTable1.ResumeLayout(false);
			this.TblStatusBar.ResumeLayout(false);
			this.TblStatusBar.PerformLayout();
			this.TlbMainToolbar.ResumeLayout(false);
			this.TlbMainToolbar.PerformLayout();
			this.TlbPrices.ResumeLayout(false);
			this.TlbPrices.PerformLayout();
			this.TlbAssets.ResumeLayout(false);
			this.TlbAssets.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Timer TmrUpdate;
		private System.Windows.Forms.ImageList ImlSortIcons;
		private System.Windows.Forms.TableLayoutPanel SplTable1;
		private System.Windows.Forms.ToolStrip TlbPrices;
		private System.Windows.Forms.ToolStripLabel TlbLblPrices;
		private SpecialFNs.ListViewEx LstRefinery;
		private System.Windows.Forms.ColumnHeader ClmItemName;
		private System.Windows.Forms.ColumnHeader ClmRefinedCost;
		private System.Windows.Forms.ColumnHeader ClmSellPrice;
		private System.Windows.Forms.ColumnHeader ClmDeltaPrice;
		private System.Windows.Forms.ColumnHeader ClmTritanium;
		private System.Windows.Forms.ColumnHeader ClmPyerite;
		private System.Windows.Forms.ColumnHeader ClmMexallon;
		private System.Windows.Forms.ColumnHeader ClmIsogen;
		private System.Windows.Forms.ColumnHeader ClmNocxium;
		private System.Windows.Forms.ColumnHeader ClmZydrine;
		private System.Windows.Forms.ColumnHeader ClmMegacyte;
		private System.Windows.Forms.ColumnHeader ClmMorphite;
		private System.Windows.Forms.ToolStrip TlbMainToolbar;
		private System.Windows.Forms.ToolStripButton TlbBtnExport;
		private System.Windows.Forms.ToolStripButton TlbBtnSettings;
		private System.Windows.Forms.ToolStripButton TlbBtnAbout;
		private System.Windows.Forms.ToolStripContainer TlcToolContainer;
		private System.Windows.Forms.ToolStrip TlbAssets;
		private System.Windows.Forms.ToolStripLabel TlbLblAssets;
		private SpecialFNs.ToolStripComboBoxEx TlbCmbCharacter;
		private System.Windows.Forms.ToolStripComboBox TlbCmbLocation;
		private System.Windows.Forms.ColumnHeader ClmQuantity;
		private System.Windows.Forms.ColumnHeader ClmItemType;
		private System.Windows.Forms.ToolStripButton TlbBtnUpdatePrices;
		private System.Windows.Forms.ColumnHeader ClmLossPercent;
		private System.Windows.Forms.ToolStripComboBox TlbCmbContainer;
		private System.Windows.Forms.ToolStripButton TlbChkIgnoreContainers;
		private System.Windows.Forms.ToolStripButton TlbChkIgnoreShips;
		private System.Windows.Forms.ImageList ImlToolbarButtons;
		private System.Windows.Forms.ToolStripButton TlbChkIgnoreAssembled;
		private System.Windows.Forms.ToolStripButton TlbChkUseQuantities;
		private System.Windows.Forms.ToolStripButton TlbBtnWhatsnew;
		private System.Windows.Forms.TableLayoutPanel TblStatusBar;
		private System.Windows.Forms.Label LblPriceQueueHint;
		private System.Windows.Forms.Label LblPriceQueue;
		private System.Windows.Forms.Label LblAssetsCacheHint;
		private System.Windows.Forms.Label LblAssetsCache;
		private System.Windows.Forms.ColumnHeader ClmVolume;
        private System.Windows.Forms.ColumnHeader ClmMetaLevel;
		private System.Windows.Forms.ColumnHeader ClmRefinedVolume;
		private System.Windows.Forms.ToolStripButton TlbBtnPricesType;
		private System.Windows.Forms.ToolStripLabel TlbLblPricesType;

    }
}

