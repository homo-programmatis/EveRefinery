﻿namespace EveRefinery
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
            this.TblStatusBar = new System.Windows.Forms.TableLayoutPanel();
            this.LblPriceQueueHint = new System.Windows.Forms.Label();
            this.LblPriceQueue = new System.Windows.Forms.Label();
            this.LblAssetsCacheHint = new System.Windows.Forms.Label();
            this.LblAssetsCache = new System.Windows.Forms.Label();
            this.TlbMainToolbar = new System.Windows.Forms.ToolStrip();
            this.TlbLblSortBy = new System.Windows.Forms.ToolStripLabel();
            this.TlbBtnSortByName = new System.Windows.Forms.ToolStripButton();
            this.TlbBtnSortByType = new System.Windows.Forms.ToolStripButton();
            this.TlbLblSettings = new System.Windows.Forms.ToolStripLabel();
            this.TlbBtnSettings = new System.Windows.Forms.ToolStripButton();
            this.TlbLblAbout = new System.Windows.Forms.ToolStripLabel();
            this.TlbBtnAbout = new System.Windows.Forms.ToolStripButton();
            this.TlbBtnWhatsnew = new System.Windows.Forms.ToolStripButton();
            this.TlbPrices = new System.Windows.Forms.ToolStrip();
            this.TlbLblPrices = new System.Windows.Forms.ToolStripLabel();
            this.TlbCmbPriceRegion = new System.Windows.Forms.ToolStripComboBox();
            this.TlbCmbPriceType = new System.Windows.Forms.ToolStripComboBox();
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
            this.ClmMetaLevel = new System.Windows.Forms.ColumnHeader();
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
            this.TlcToolContainer.TopToolStripPanel.Controls.Add(this.TlbAssets);
            this.TlcToolContainer.TopToolStripPanel.Controls.Add(this.TlbPrices);
            this.TlcToolContainer.TopToolStripPanel.Controls.Add(this.TlbMainToolbar);
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
            this.ClmVolume});
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
            // ClmRefinedCost
            // 
            this.ClmRefinedCost.Text = "Refined Cost";
            this.ClmRefinedCost.Width = 80;
            // 
            // ClmSellPrice
            // 
            this.ClmSellPrice.Text = "Sell Price";
            this.ClmSellPrice.Width = 80;
            // 
            // ClmDeltaPrice
            // 
            this.ClmDeltaPrice.Text = "Delta";
            this.ClmDeltaPrice.Width = 80;
            // 
            // ClmTritanium
            // 
            this.ClmTritanium.Text = "Tritanium";
            // 
            // ClmPyerite
            // 
            this.ClmPyerite.Text = "Pyerite";
            // 
            // ClmMexallon
            // 
            this.ClmMexallon.Text = "Mexallon";
            // 
            // ClmIsogen
            // 
            this.ClmIsogen.Text = "Isogen";
            // 
            // ClmNocxium
            // 
            this.ClmNocxium.Text = "Nocxium";
            // 
            // ClmZydrine
            // 
            this.ClmZydrine.Text = "Zydrine";
            // 
            // ClmMegacyte
            // 
            this.ClmMegacyte.Text = "Megacyte";
            // 
            // ClmMorphite
            // 
            this.ClmMorphite.Text = "Morphite";
            // 
            // ClmQuantity
            // 
            this.ClmQuantity.Text = "Qty";
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
            // 
            // ClmVolume
            // 
            this.ClmVolume.Text = "Volume";
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
            this.TlbLblSortBy,
            this.TlbBtnSortByName,
            this.TlbBtnSortByType,
            this.TlbLblSettings,
            this.TlbBtnSettings,
            this.TlbLblAbout,
            this.TlbBtnAbout,
            this.TlbBtnWhatsnew});
            this.TlbMainToolbar.Location = new System.Drawing.Point(3, 0);
            this.TlbMainToolbar.Name = "TlbMainToolbar";
            this.TlbMainToolbar.Size = new System.Drawing.Size(266, 25);
            this.TlbMainToolbar.TabIndex = 5;
            this.TlbMainToolbar.Text = "Main Toolbar";
            // 
            // TlbLblSortBy
            // 
            this.TlbLblSortBy.Name = "TlbLblSortBy";
            this.TlbLblSortBy.Size = new System.Drawing.Size(50, 22);
            this.TlbLblSortBy.Text = "Sort By :";
            // 
            // TlbBtnSortByName
            // 
            this.TlbBtnSortByName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TlbBtnSortByName.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnSortByName.Image")));
            this.TlbBtnSortByName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TlbBtnSortByName.Name = "TlbBtnSortByName";
            this.TlbBtnSortByName.Size = new System.Drawing.Size(23, 22);
            this.TlbBtnSortByName.Text = "By Name";
            this.TlbBtnSortByName.Click += new System.EventHandler(this.TlbBtnSortByName_Click);
            // 
            // TlbBtnSortByType
            // 
            this.TlbBtnSortByType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TlbBtnSortByType.Image = ((System.Drawing.Image)(resources.GetObject("TlbBtnSortByType.Image")));
            this.TlbBtnSortByType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TlbBtnSortByType.Name = "TlbBtnSortByType";
            this.TlbBtnSortByType.Size = new System.Drawing.Size(23, 22);
            this.TlbBtnSortByType.Text = "By Type";
            this.TlbBtnSortByType.Click += new System.EventHandler(this.TlbBtnSortByType_Click);
            // 
            // TlbLblSettings
            // 
            this.TlbLblSettings.Name = "TlbLblSettings";
            this.TlbLblSettings.Size = new System.Drawing.Size(49, 22);
            this.TlbLblSettings.Text = "Settings";
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
            // TlbLblAbout
            // 
            this.TlbLblAbout.Name = "TlbLblAbout";
            this.TlbLblAbout.Size = new System.Drawing.Size(40, 22);
            this.TlbLblAbout.Text = "About";
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
            this.TlbCmbPriceRegion,
            this.TlbCmbPriceType,
            this.TlbBtnUpdatePrices});
            this.TlbPrices.Location = new System.Drawing.Point(269, 0);
            this.TlbPrices.Name = "TlbPrices";
            this.TlbPrices.Size = new System.Drawing.Size(294, 25);
            this.TlbPrices.TabIndex = 8;
            this.TlbPrices.Text = "Prices";
            // 
            // TlbLblPrices
            // 
            this.TlbLblPrices.Name = "TlbLblPrices";
            this.TlbLblPrices.Size = new System.Drawing.Size(44, 22);
            this.TlbLblPrices.Text = "Prices :";
            // 
            // TlbCmbPriceRegion
            // 
            this.TlbCmbPriceRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TlbCmbPriceRegion.Name = "TlbCmbPriceRegion";
            this.TlbCmbPriceRegion.Size = new System.Drawing.Size(121, 25);
            this.TlbCmbPriceRegion.Sorted = true;
            this.TlbCmbPriceRegion.SelectedIndexChanged += new System.EventHandler(this.TlbCmbPriceRegion_SelectedIndexChanged);
            // 
            // TlbCmbPriceType
            // 
            this.TlbCmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TlbCmbPriceType.DropDownWidth = 350;
            this.TlbCmbPriceType.Name = "TlbCmbPriceType";
            this.TlbCmbPriceType.Size = new System.Drawing.Size(90, 25);
            this.TlbCmbPriceType.Sorted = true;
            this.TlbCmbPriceType.SelectedIndexChanged += new System.EventHandler(this.TlbCmbPriceType_SelectedIndexChanged);
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
            // ClmMetaLevel
            // 
            this.ClmMetaLevel.Text = "Meta";
            this.ClmMetaLevel.Width = 40;
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
		private System.Windows.Forms.ToolStripComboBox TlbCmbPriceRegion;
		private System.Windows.Forms.ToolStripComboBox TlbCmbPriceType;
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
		private System.Windows.Forms.ToolStripLabel TlbLblSortBy;
		private System.Windows.Forms.ToolStripButton TlbBtnSortByName;
		private System.Windows.Forms.ToolStripButton TlbBtnSortByType;
		private System.Windows.Forms.ToolStripLabel TlbLblSettings;
		private System.Windows.Forms.ToolStripButton TlbBtnSettings;
		private System.Windows.Forms.ToolStripLabel TlbLblAbout;
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

    }
}
