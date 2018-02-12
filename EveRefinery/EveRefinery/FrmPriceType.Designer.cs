namespace EveRefinery
{
	partial class FrmPriceType
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.SplTable2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnOk = new System.Windows.Forms.Button();
		    this.label0 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
		    this.CmbProvider = new System.Windows.Forms.ComboBox();
			this.CmbRegion = new System.Windows.Forms.ComboBox();
			this.CmbSolar = new System.Windows.Forms.ComboBox();
			this.CmbPriceType = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SplTable2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.SplTable2, 0, 9);
		    this.tableLayoutPanel1.Controls.Add(this.label0, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 6);
		    this.tableLayoutPanel1.Controls.Add(this.CmbProvider, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.CmbRegion, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.CmbSolar, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.CmbPriceType, 0, 7);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel1.RowCount = 11;
		    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
		    this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 170);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// SplTable2
			// 
			this.SplTable2.ColumnCount = 4;
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.SplTable2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.SplTable2.Controls.Add(this.BtnCancel, 3, 0);
			this.SplTable2.Controls.Add(this.BtnOk, 1, 0);
			this.SplTable2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplTable2.Location = new System.Drawing.Point(7, 142);
			this.SplTable2.Margin = new System.Windows.Forms.Padding(2);
			this.SplTable2.Name = "SplTable2";
			this.SplTable2.RowCount = 1;
			this.SplTable2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SplTable2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.SplTable2.Size = new System.Drawing.Size(270, 21);
			this.SplTable2.TabIndex = 7;
			// 
			// BtnCancel
			// 
			this.BtnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnCancel.Location = new System.Drawing.Point(190, 0);
			this.BtnCancel.Margin = new System.Windows.Forms.Padding(0);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(80, 21);
			this.BtnCancel.TabIndex = 0;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// BtnOk
			// 
			this.BtnOk.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnOk.Location = new System.Drawing.Point(105, 0);
			this.BtnOk.Margin = new System.Windows.Forms.Padding(0);
			this.BtnOk.Name = "BtnOk";
			this.BtnOk.Size = new System.Drawing.Size(80, 21);
			this.BtnOk.TabIndex = 1;
			this.BtnOk.Text = "OK";
			this.BtnOk.UseVisualStyleBackColor = true;
			this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
		    // 
		    // label0
		    // 
		    this.label0.AutoSize = true;
		    this.label0.Dock = System.Windows.Forms.DockStyle.Fill;
		    this.label0.Location = new System.Drawing.Point(8, 5);
		    this.label0.Name = "label0";
		    this.label0.Size = new System.Drawing.Size(268, 15);
		    this.label0.TabIndex = 0;
		    this.label0.Text = "Provider :";
		    this.label0.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(8, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(268, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Region :";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(8, 85);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(268, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Solar system :";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(8, 125);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(268, 15);
			this.label3.TabIndex = 3;
			this.label3.Text = "Price type :";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		    // 
		    // CmbProvider
		    // 
		    this.CmbProvider.Dock = System.Windows.Forms.DockStyle.Fill;
		    this.CmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		    this.CmbProvider.FormattingEnabled = true;
		    this.CmbProvider.Location = new System.Drawing.Point(8, 23);
		    this.CmbProvider.Name = "CmbProvider";
		    this.CmbProvider.Size = new System.Drawing.Size(268, 21);
		    this.CmbProvider.Sorted = true;
		    this.CmbProvider.TabIndex = 4;
		    this.CmbProvider.SelectedIndexChanged += new System.EventHandler(this.CmbProvider_SelectedIndexChanged);
			// 
			// CmbRegion
			// 
			this.CmbRegion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbRegion.FormattingEnabled = true;
			this.CmbRegion.Location = new System.Drawing.Point(8, 63);
			this.CmbRegion.Name = "CmbRegion";
			this.CmbRegion.Size = new System.Drawing.Size(268, 21);
			this.CmbRegion.Sorted = true;
			this.CmbRegion.TabIndex = 5;
			this.CmbRegion.SelectedIndexChanged += new System.EventHandler(this.CmbRegion_SelectedIndexChanged);
			// 
			// CmbSolar
			// 
			this.CmbSolar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CmbSolar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbSolar.FormattingEnabled = true;
			this.CmbSolar.Location = new System.Drawing.Point(8, 103);
			this.CmbSolar.Name = "CmbSolar";
			this.CmbSolar.Size = new System.Drawing.Size(268, 21);
			this.CmbSolar.Sorted = true;
			this.CmbSolar.TabIndex = 6;
			// 
			// CmbPriceType
			// 
			this.CmbPriceType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CmbPriceType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.CmbPriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbPriceType.DropDownWidth = 300;
			this.CmbPriceType.FormattingEnabled = true;
			this.CmbPriceType.Location = new System.Drawing.Point(8, 123);
			this.CmbPriceType.Name = "CmbPriceType";
			this.CmbPriceType.Size = new System.Drawing.Size(268, 21);
			this.CmbPriceType.TabIndex = 5;
			// 
			// FrmPriceType
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 210);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FrmPriceType";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Price Settings";
			this.Load += new System.EventHandler(this.FrmPriceType_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.SplTable2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	    private System.Windows.Forms.Label label0;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
	    private System.Windows.Forms.ComboBox CmbProvider;
		private System.Windows.Forms.ComboBox CmbRegion;
		private System.Windows.Forms.ComboBox CmbSolar;
		private System.Windows.Forms.ComboBox CmbPriceType;
		private System.Windows.Forms.TableLayoutPanel SplTable2;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnOk;
	}
}