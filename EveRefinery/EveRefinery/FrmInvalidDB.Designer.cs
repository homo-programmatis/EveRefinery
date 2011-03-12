namespace EveRefinery
{
	partial class FrmInvalidDB
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInvalidDB));
			this.label2 = new System.Windows.Forms.Label();
			this.LblBadDatabase = new System.Windows.Forms.Label();
			this.BtnBrowseForDB = new System.Windows.Forms.Button();
			this.BtnExit = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.SplTable2 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1.SuspendLayout();
			this.SplTable2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(348, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Failed to read the database:";
			// 
			// LblBadDatabase
			// 
			this.LblBadDatabase.AutoEllipsis = true;
			this.LblBadDatabase.AutoSize = true;
			this.LblBadDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblBadDatabase.Location = new System.Drawing.Point(3, 20);
			this.LblBadDatabase.Name = "LblBadDatabase";
			this.LblBadDatabase.Size = new System.Drawing.Size(348, 20);
			this.LblBadDatabase.TabIndex = 2;
			this.LblBadDatabase.Text = "[ Bad Database Path ]";
			// 
			// BtnBrowseForDB
			// 
			this.BtnBrowseForDB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnBrowseForDB.Location = new System.Drawing.Point(157, 3);
			this.BtnBrowseForDB.Name = "BtnBrowseForDB";
			this.BtnBrowseForDB.Size = new System.Drawing.Size(94, 19);
			this.BtnBrowseForDB.TabIndex = 4;
			this.BtnBrowseForDB.Text = "Browse for DB...";
			this.BtnBrowseForDB.UseVisualStyleBackColor = true;
			this.BtnBrowseForDB.Click += new System.EventHandler(this.BtnBrowseForDB_Click);
			// 
			// BtnExit
			// 
			this.BtnExit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnExit.Location = new System.Drawing.Point(257, 3);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.Size = new System.Drawing.Size(94, 19);
			this.BtnExit.TabIndex = 5;
			this.BtnExit.Text = "Exit";
			this.BtnExit.UseVisualStyleBackColor = true;
			this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblBadDatabase, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.SplTable2, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(354, 91);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// SplTable2
			// 
			this.SplTable2.ColumnCount = 3;
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.SplTable2.Controls.Add(this.BtnBrowseForDB, 1, 0);
			this.SplTable2.Controls.Add(this.BtnExit, 2, 0);
			this.SplTable2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplTable2.Location = new System.Drawing.Point(0, 66);
			this.SplTable2.Margin = new System.Windows.Forms.Padding(0);
			this.SplTable2.Name = "SplTable2";
			this.SplTable2.RowCount = 1;
			this.SplTable2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable2.Size = new System.Drawing.Size(354, 25);
			this.SplTable2.TabIndex = 3;
			// 
			// FrmInvalidDB
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(354, 91);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmInvalidDB";
			this.Text = "Eve Refinery";
			this.Load += new System.EventHandler(this.FrmInvalidDB_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.SplTable2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label LblBadDatabase;
		private System.Windows.Forms.Button BtnBrowseForDB;
		private System.Windows.Forms.Button BtnExit;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel SplTable2;
	}
}