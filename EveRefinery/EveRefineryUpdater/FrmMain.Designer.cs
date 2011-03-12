namespace EveRefineryUpdater
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
			this.TblMain = new System.Windows.Forms.TableLayoutPanel();
			this.LstFiles = new SpecialFNs.ListViewEx();
			this.ClmFile = new System.Windows.Forms.ColumnHeader();
			this.ClmStatus = new System.Windows.Forms.ColumnHeader();
			this.BtnUpdate = new System.Windows.Forms.Button();
			this.TmrUpdateList = new System.Windows.Forms.Timer(this.components);
			this.TblMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// TblMain
			// 
			this.TblMain.ColumnCount = 1;
			this.TblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TblMain.Controls.Add(this.LstFiles, 0, 0);
			this.TblMain.Controls.Add(this.BtnUpdate, 0, 1);
			this.TblMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TblMain.Location = new System.Drawing.Point(0, 0);
			this.TblMain.Name = "TblMain";
			this.TblMain.RowCount = 2;
			this.TblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.TblMain.Size = new System.Drawing.Size(503, 153);
			this.TblMain.TabIndex = 0;
			// 
			// LstFiles
			// 
			this.LstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ClmFile,
            this.ClmStatus});
			this.LstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LstFiles.FullRowSelect = true;
			this.LstFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.LstFiles.Location = new System.Drawing.Point(3, 3);
			this.LstFiles.MultiSelect = false;
			this.LstFiles.Name = "LstFiles";
			this.LstFiles.ShowItemToolTips = true;
			this.LstFiles.Size = new System.Drawing.Size(497, 117);
			this.LstFiles.TabIndex = 0;
			this.LstFiles.UseCompatibleStateImageBehavior = false;
			this.LstFiles.View = System.Windows.Forms.View.Details;
			// 
			// ClmFile
			// 
			this.ClmFile.Text = "File";
			this.ClmFile.Width = 158;
			// 
			// ClmStatus
			// 
			this.ClmStatus.Text = "Status";
			this.ClmStatus.Width = 311;
			// 
			// BtnUpdate
			// 
			this.BtnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnUpdate.Location = new System.Drawing.Point(3, 126);
			this.BtnUpdate.Name = "BtnUpdate";
			this.BtnUpdate.Size = new System.Drawing.Size(497, 24);
			this.BtnUpdate.TabIndex = 1;
			this.BtnUpdate.Text = "Update!";
			this.BtnUpdate.UseVisualStyleBackColor = true;
			this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
			// 
			// TmrUpdateList
			// 
			this.TmrUpdateList.Interval = 1000;
			this.TmrUpdateList.Tick += new System.EventHandler(this.TmrUpdateList_Tick);
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(503, 153);
			this.Controls.Add(this.TblMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "EveRefinery Update Installer";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.TblMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TblMain;
		private SpecialFNs.ListViewEx LstFiles;
		private System.Windows.Forms.ColumnHeader ClmFile;
		private System.Windows.Forms.ColumnHeader ClmStatus;
		private System.Windows.Forms.Button BtnUpdate;
		private System.Windows.Forms.Timer TmrUpdateList;
	}
}

