namespace EveRefinery
{
	partial class FrmAboutBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAboutBox));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.LblNameAndVersion = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.LnkEmail = new System.Windows.Forms.LinkLabel();
            this.LnkWebSite = new System.Windows.Forms.LinkLabel();
            this.LnkForum = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.LblNameAndVersion, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.okButton, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.LnkEmail, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.LnkWebSite, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.LnkForum, 1, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33333F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(446, 265);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 7);
            this.logoPictureBox.Size = new System.Drawing.Size(141, 259);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            // 
            // LblNameAndVersion
            // 
            this.LblNameAndVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblNameAndVersion.Location = new System.Drawing.Point(153, 0);
            this.LblNameAndVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.LblNameAndVersion.MaximumSize = new System.Drawing.Size(0, 17);
            this.LblNameAndVersion.Name = "LblNameAndVersion";
            this.LblNameAndVersion.Size = new System.Drawing.Size(290, 17);
            this.LblNameAndVersion.TabIndex = 19;
            this.LblNameAndVersion.Text = "EVE Refinery";
            this.LblNameAndVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(153, 103);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(290, 131);
            this.textBoxDescription.TabIndex = 23;
            this.textBoxDescription.TabStop = false;
            this.textBoxDescription.Text = "EveRefinery is designed to help you make decisions whether you want to refine or " +
    "sell your loot.\r\n\r\nIn-game donations are welcome! Send them to Codeguard";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(368, 240);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 22);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "&OK";
            // 
            // labelCopyright
            // 
            this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCopyright.Location = new System.Drawing.Point(153, 20);
            this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(290, 17);
            this.labelCopyright.TabIndex = 21;
            this.labelCopyright.Text = "Copyright: Alexander Miloslavskiy 2014";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LnkEmail
            // 
            this.LnkEmail.AutoSize = true;
            this.LnkEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LnkEmail.Location = new System.Drawing.Point(150, 40);
            this.LnkEmail.Name = "LnkEmail";
            this.LnkEmail.Size = new System.Drawing.Size(293, 20);
            this.LnkEmail.TabIndex = 25;
            this.LnkEmail.TabStop = true;
            this.LnkEmail.Text = "alexandr.miloslavskiy@gmail.com";
            this.LnkEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnMailLinkClicked);
            // 
            // LnkWebSite
            // 
            this.LnkWebSite.AutoSize = true;
            this.LnkWebSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LnkWebSite.Location = new System.Drawing.Point(150, 80);
            this.LnkWebSite.Name = "LnkWebSite";
            this.LnkWebSite.Size = new System.Drawing.Size(293, 20);
            this.LnkWebSite.TabIndex = 26;
            this.LnkWebSite.TabStop = true;
            this.LnkWebSite.Text = "http://www.homo-programmatis.com/EveRefinery/";
            this.LnkWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnWebLinkCLicked);
            // 
            // LnkForum
            // 
            this.LnkForum.AutoSize = true;
            this.LnkForum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LnkForum.Location = new System.Drawing.Point(150, 60);
            this.LnkForum.Name = "LnkForum";
            this.LnkForum.Size = new System.Drawing.Size(293, 20);
            this.LnkForum.TabIndex = 27;
            this.LnkForum.TabStop = true;
            this.LnkForum.Text = "http://forums.eveonline.com/default.aspx?g=posts&t=8444";
            this.LnkForum.UseMnemonic = false;
            this.LnkForum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnWebLinkCLicked);
            // 
            // FrmAboutBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 283);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.PictureBox logoPictureBox;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.LinkLabel LnkEmail;
		private System.Windows.Forms.Label LblNameAndVersion;
		private System.Windows.Forms.LinkLabel LnkWebSite;
        private System.Windows.Forms.LinkLabel LnkForum;
	}
}
