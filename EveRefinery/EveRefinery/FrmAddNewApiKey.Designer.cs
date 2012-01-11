namespace EveRefinery
{
	partial class FrmAddNewApiKey
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddNewApiKey));
			this.SplTable1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnOk = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.LblContextHelp = new System.Windows.Forms.Label();
			this.LnkCreateKey = new System.Windows.Forms.LinkLabel();
			this.SplTable3 = new System.Windows.Forms.TableLayoutPanel();
			this.LblUserID = new System.Windows.Forms.Label();
			this.LblApiKey = new System.Windows.Forms.Label();
			this.TxtUserID = new System.Windows.Forms.TextBox();
			this.TxtApiKey = new System.Windows.Forms.TextBox();
			this.LblCreateKey = new System.Windows.Forms.Label();
			this.LnkKeysManagement = new System.Windows.Forms.LinkLabel();
			this.SplTable1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SplTable3.SuspendLayout();
			this.SuspendLayout();
			// 
			// SplTable1
			// 
			this.SplTable1.ColumnCount = 1;
			this.SplTable1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable1.Controls.Add(this.tableLayoutPanel1, 0, 5);
			this.SplTable1.Controls.Add(this.LblContextHelp, 0, 0);
			this.SplTable1.Controls.Add(this.LnkCreateKey, 0, 3);
			this.SplTable1.Controls.Add(this.SplTable3, 0, 4);
			this.SplTable1.Controls.Add(this.LblCreateKey, 0, 2);
			this.SplTable1.Controls.Add(this.LnkKeysManagement, 0, 1);
			this.SplTable1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplTable1.Location = new System.Drawing.Point(0, 0);
			this.SplTable1.Name = "SplTable1";
			this.SplTable1.RowCount = 6;
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.SplTable1.Size = new System.Drawing.Size(327, 194);
			this.SplTable1.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.Controls.Add(this.BtnOk, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 174);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 20);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// BtnOk
			// 
			this.BtnOk.Location = new System.Drawing.Point(167, 0);
			this.BtnOk.Margin = new System.Windows.Forms.Padding(0);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.Size = new System.Drawing.Size(75, 20);
			this.BtnOk.TabIndex = 0;
			this.BtnOk.Text = "OK";
			this.BtnOk.UseVisualStyleBackColor = true;
			this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.Location = new System.Drawing.Point(247, 0);
			this.BtnCancel.Margin = new System.Windows.Forms.Padding(0);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(75, 20);
			this.BtnCancel.TabIndex = 1;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// LblContextHelp
			// 
			this.LblContextHelp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblContextHelp.Location = new System.Drawing.Point(3, 0);
			this.LblContextHelp.Name = "LblContextHelp";
			this.LblContextHelp.Size = new System.Drawing.Size(321, 69);
			this.LblContextHelp.TabIndex = 1;
			this.LblContextHelp.Text = resources.GetString("LblContextHelp.Text");
			// 
			// LnkCreateKey
			// 
			this.LnkCreateKey.AutoSize = true;
			this.LnkCreateKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LnkCreateKey.Location = new System.Drawing.Point(3, 109);
			this.LnkCreateKey.Name = "LnkCreateKey";
			this.LnkCreateKey.Size = new System.Drawing.Size(321, 20);
			this.LnkCreateKey.TabIndex = 2;
			this.LnkCreateKey.TabStop = true;
			this.LnkCreateKey.Text = "https://support.eveonline.com/api/key/createpredefined/2";
			this.LnkCreateKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// SplTable3
			// 
			this.SplTable3.ColumnCount = 2;
			this.SplTable3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.SplTable3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable3.Controls.Add(this.LblUserID, 0, 0);
			this.SplTable3.Controls.Add(this.LblApiKey, 0, 1);
			this.SplTable3.Controls.Add(this.TxtUserID, 1, 0);
			this.SplTable3.Controls.Add(this.TxtApiKey, 1, 1);
			this.SplTable3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplTable3.Location = new System.Drawing.Point(0, 129);
			this.SplTable3.Margin = new System.Windows.Forms.Padding(0);
			this.SplTable3.Name = "SplTable3";
			this.SplTable3.RowCount = 2;
			this.SplTable3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.SplTable3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.SplTable3.Size = new System.Drawing.Size(327, 45);
			this.SplTable3.TabIndex = 3;
			// 
			// LblUserID
			// 
			this.LblUserID.AutoSize = true;
			this.LblUserID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblUserID.Location = new System.Drawing.Point(2, 2);
			this.LblUserID.Margin = new System.Windows.Forms.Padding(2);
			this.LblUserID.Name = "LblUserID";
			this.LblUserID.Size = new System.Drawing.Size(76, 18);
			this.LblUserID.TabIndex = 0;
			this.LblUserID.Text = "ID:";
			// 
			// LblApiKey
			// 
			this.LblApiKey.AutoSize = true;
			this.LblApiKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblApiKey.Location = new System.Drawing.Point(2, 24);
			this.LblApiKey.Margin = new System.Windows.Forms.Padding(2);
			this.LblApiKey.Name = "LblApiKey";
			this.LblApiKey.Size = new System.Drawing.Size(76, 19);
			this.LblApiKey.TabIndex = 1;
			this.LblApiKey.Text = "Verif. code :";
			// 
			// TxtUserID
			// 
			this.TxtUserID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtUserID.Location = new System.Drawing.Point(82, 2);
			this.TxtUserID.Margin = new System.Windows.Forms.Padding(2);
			this.TxtUserID.Name = "TxtUserID";
			this.TxtUserID.Size = new System.Drawing.Size(243, 20);
			this.TxtUserID.TabIndex = 2;
			// 
			// TxtApiKey
			// 
			this.TxtApiKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtApiKey.Location = new System.Drawing.Point(82, 24);
			this.TxtApiKey.Margin = new System.Windows.Forms.Padding(2);
			this.TxtApiKey.Name = "TxtApiKey";
			this.TxtApiKey.Size = new System.Drawing.Size(243, 20);
			this.TxtApiKey.TabIndex = 3;
			// 
			// LblCreateKey
			// 
			this.LblCreateKey.AutoSize = true;
			this.LblCreateKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblCreateKey.Location = new System.Drawing.Point(3, 89);
			this.LblCreateKey.Name = "LblCreateKey";
			this.LblCreateKey.Size = new System.Drawing.Size(321, 20);
			this.LblCreateKey.TabIndex = 4;
			this.LblCreateKey.Text = "In order to create a new key, visit :";
			// 
			// LnkKeysManagement
			// 
			this.LnkKeysManagement.AutoSize = true;
			this.LnkKeysManagement.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LnkKeysManagement.Location = new System.Drawing.Point(3, 69);
			this.LnkKeysManagement.Name = "LnkKeysManagement";
			this.LnkKeysManagement.Size = new System.Drawing.Size(321, 20);
			this.LnkKeysManagement.TabIndex = 5;
			this.LnkKeysManagement.TabStop = true;
			this.LnkKeysManagement.Text = "https://support.eveonline.com/api/Key/Index";
			this.LnkKeysManagement.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
			// 
			// FrmAddNewApiKey
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(327, 194);
			this.Controls.Add(this.SplTable1);
			this.Name = "FrmAddNewApiKey";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Add new API key";
			this.SplTable1.ResumeLayout(false);
			this.SplTable1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.SplTable3.ResumeLayout(false);
			this.SplTable3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel SplTable1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label LblContextHelp;
		private System.Windows.Forms.Button BtnOk;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.TableLayoutPanel SplTable3;
		private System.Windows.Forms.Label LblUserID;
		private System.Windows.Forms.Label LblApiKey;
		private System.Windows.Forms.TextBox TxtUserID;
		private System.Windows.Forms.TextBox TxtApiKey;
		private System.Windows.Forms.LinkLabel LnkCreateKey;
		private System.Windows.Forms.Label LblCreateKey;
		private System.Windows.Forms.LinkLabel LnkKeysManagement;
	}
}