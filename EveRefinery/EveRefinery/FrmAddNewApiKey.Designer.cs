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
            this.LnkGetApiKey = new System.Windows.Forms.LinkLabel();
            this.SplTable3 = new System.Windows.Forms.TableLayoutPanel();
            this.LblUserID = new System.Windows.Forms.Label();
            this.LblApiKey = new System.Windows.Forms.Label();
            this.TxtUserID = new System.Windows.Forms.TextBox();
            this.TxtApiKey = new System.Windows.Forms.TextBox();
            this.SplTable1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SplTable3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplTable1
            // 
            this.SplTable1.ColumnCount = 1;
            this.SplTable1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SplTable1.Controls.Add(this.tableLayoutPanel1, 0, 3);
            this.SplTable1.Controls.Add(this.LblContextHelp, 0, 0);
            this.SplTable1.Controls.Add(this.LnkGetApiKey, 0, 1);
            this.SplTable1.Controls.Add(this.SplTable3, 0, 2);
            this.SplTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplTable1.Location = new System.Drawing.Point(0, 0);
            this.SplTable1.Name = "SplTable1";
            this.SplTable1.RowCount = 4;
            this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.SplTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SplTable1.Size = new System.Drawing.Size(293, 175);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 155);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(293, 20);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(133, 0);
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
            this.BtnCancel.Location = new System.Drawing.Point(213, 0);
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
            this.LblContextHelp.Size = new System.Drawing.Size(287, 90);
            this.LblContextHelp.TabIndex = 1;
            this.LblContextHelp.Text = resources.GetString("LblContextHelp.Text");
            // 
            // LnkGetApiKey
            // 
            this.LnkGetApiKey.AutoSize = true;
            this.LnkGetApiKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LnkGetApiKey.Location = new System.Drawing.Point(3, 90);
            this.LnkGetApiKey.Name = "LnkGetApiKey";
            this.LnkGetApiKey.Size = new System.Drawing.Size(287, 20);
            this.LnkGetApiKey.TabIndex = 2;
            this.LnkGetApiKey.TabStop = true;
            this.LnkGetApiKey.Text = "http://www.eveonline.com/api/default.asp";
            this.LnkGetApiKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkGetApiKey_LinkClicked);
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
            this.SplTable3.Location = new System.Drawing.Point(0, 110);
            this.SplTable3.Margin = new System.Windows.Forms.Padding(0);
            this.SplTable3.Name = "SplTable3";
            this.SplTable3.RowCount = 2;
            this.SplTable3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SplTable3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SplTable3.Size = new System.Drawing.Size(293, 45);
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
            this.LblUserID.Text = "UserID :";
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
            this.LblApiKey.Text = "Full API key :";
            // 
            // TxtUserID
            // 
            this.TxtUserID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtUserID.Location = new System.Drawing.Point(82, 2);
            this.TxtUserID.Margin = new System.Windows.Forms.Padding(2);
            this.TxtUserID.Name = "TxtUserID";
            this.TxtUserID.Size = new System.Drawing.Size(209, 20);
            this.TxtUserID.TabIndex = 2;
            // 
            // TxtApiKey
            // 
            this.TxtApiKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtApiKey.Location = new System.Drawing.Point(82, 24);
            this.TxtApiKey.Margin = new System.Windows.Forms.Padding(2);
            this.TxtApiKey.Name = "TxtApiKey";
            this.TxtApiKey.Size = new System.Drawing.Size(209, 20);
            this.TxtApiKey.TabIndex = 3;
            // 
            // FrmAddNewApiKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 175);
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
		private System.Windows.Forms.LinkLabel LnkGetApiKey;
		private System.Windows.Forms.Button BtnOk;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.TableLayoutPanel SplTable3;
		private System.Windows.Forms.Label LblUserID;
		private System.Windows.Forms.Label LblApiKey;
		private System.Windows.Forms.TextBox TxtUserID;
		private System.Windows.Forms.TextBox TxtApiKey;
	}
}