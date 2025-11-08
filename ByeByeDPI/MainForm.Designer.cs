namespace ByeByeDPI
{
	partial class MainForm
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
				_viewModel.Dispose();

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.ToggleDPIBtn = new System.Windows.Forms.Button();
			this.ListBoxForMessages = new System.Windows.Forms.ListBox();
			this.listContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HideToTrayChbox = new System.Windows.Forms.CheckBox();
			this.StartWithWindowsChbox = new System.Windows.Forms.CheckBox();
			this.CheckUpdatesChbox = new System.Windows.Forms.CheckBox();
			this.OpenDomainListBtn = new System.Windows.Forms.Button();
			this.OpenParamsBtn = new System.Windows.Forms.Button();
			this.ResetBtn = new System.Windows.Forms.Button();
			this.CheckUpdateNowBtn = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.appToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CheckDomainsBtn = new System.Windows.Forms.Button();
			this.listContextMenuStrip.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ToggleDPIBtn
			// 
			this.ToggleDPIBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ToggleDPIBtn.Location = new System.Drawing.Point(234, 659);
			this.ToggleDPIBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ToggleDPIBtn.Name = "ToggleDPIBtn";
			this.ToggleDPIBtn.Size = new System.Drawing.Size(267, 58);
			this.ToggleDPIBtn.TabIndex = 0;
			this.ToggleDPIBtn.Text = "Start Access";
			this.ToggleDPIBtn.UseVisualStyleBackColor = true;
			this.ToggleDPIBtn.Click += new System.EventHandler(this.ToggleDPIBtn_Click);
			// 
			// ListBoxForMessages
			// 
			this.ListBoxForMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ListBoxForMessages.ContextMenuStrip = this.listContextMenuStrip;
			this.ListBoxForMessages.FormattingEnabled = true;
			this.ListBoxForMessages.ItemHeight = 21;
			this.ListBoxForMessages.Location = new System.Drawing.Point(10, 223);
			this.ListBoxForMessages.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ListBoxForMessages.Name = "ListBoxForMessages";
			this.ListBoxForMessages.Size = new System.Drawing.Size(490, 424);
			this.ListBoxForMessages.TabIndex = 999;
			this.ListBoxForMessages.TabStop = false;
			this.ListBoxForMessages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxForMessages_MouseDown);
			// 
			// listContextMenuStrip
			// 
			this.listContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
			this.listContextMenuStrip.Name = "listContextMenuStrip";
			this.listContextMenuStrip.Size = new System.Drawing.Size(103, 26);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// HideToTrayChbox
			// 
			this.HideToTrayChbox.AutoSize = true;
			this.HideToTrayChbox.Location = new System.Drawing.Point(10, 102);
			this.HideToTrayChbox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.HideToTrayChbox.Name = "HideToTrayChbox";
			this.HideToTrayChbox.Size = new System.Drawing.Size(167, 25);
			this.HideToTrayChbox.TabIndex = 8;
			this.HideToTrayChbox.Text = "Hide to System Tray";
			this.HideToTrayChbox.UseVisualStyleBackColor = true;
			this.HideToTrayChbox.CheckedChanged += new System.EventHandler(this.HideToTrayChbox_CheckedChanged);
			// 
			// StartWithWindowsChbox
			// 
			this.StartWithWindowsChbox.AutoSize = true;
			this.StartWithWindowsChbox.Location = new System.Drawing.Point(10, 71);
			this.StartWithWindowsChbox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.StartWithWindowsChbox.Name = "StartWithWindowsChbox";
			this.StartWithWindowsChbox.Size = new System.Drawing.Size(164, 25);
			this.StartWithWindowsChbox.TabIndex = 7;
			this.StartWithWindowsChbox.Text = "Start with Windows";
			this.StartWithWindowsChbox.UseVisualStyleBackColor = true;
			this.StartWithWindowsChbox.CheckedChanged += new System.EventHandler(this.StartWithWindowsChbox_CheckedChanged);
			// 
			// CheckUpdatesChbox
			// 
			this.CheckUpdatesChbox.AutoSize = true;
			this.CheckUpdatesChbox.Location = new System.Drawing.Point(10, 40);
			this.CheckUpdatesChbox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.CheckUpdatesChbox.Name = "CheckUpdatesChbox";
			this.CheckUpdatesChbox.Size = new System.Drawing.Size(132, 25);
			this.CheckUpdatesChbox.TabIndex = 6;
			this.CheckUpdatesChbox.Text = "Check Updates";
			this.CheckUpdatesChbox.UseVisualStyleBackColor = true;
			this.CheckUpdatesChbox.CheckedChanged += new System.EventHandler(this.CheckUpdatesChbox_CheckedChanged);
			// 
			// OpenDomainListBtn
			// 
			this.OpenDomainListBtn.Location = new System.Drawing.Point(146, 170);
			this.OpenDomainListBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.OpenDomainListBtn.Name = "OpenDomainListBtn";
			this.OpenDomainListBtn.Size = new System.Drawing.Size(146, 47);
			this.OpenDomainListBtn.TabIndex = 3;
			this.OpenDomainListBtn.Text = "Open Domain List";
			this.OpenDomainListBtn.UseVisualStyleBackColor = true;
			this.OpenDomainListBtn.Click += new System.EventHandler(this.OpenDomainListBtn_Click);
			// 
			// OpenParamsBtn
			// 
			this.OpenParamsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OpenParamsBtn.Location = new System.Drawing.Point(384, 170);
			this.OpenParamsBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.OpenParamsBtn.Name = "OpenParamsBtn";
			this.OpenParamsBtn.Size = new System.Drawing.Size(117, 47);
			this.OpenParamsBtn.TabIndex = 4;
			this.OpenParamsBtn.Text = "Open Params";
			this.OpenParamsBtn.UseVisualStyleBackColor = true;
			this.OpenParamsBtn.Click += new System.EventHandler(this.OpenParamsBtn_Click);
			// 
			// ResetBtn
			// 
			this.ResetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ResetBtn.Location = new System.Drawing.Point(10, 659);
			this.ResetBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ResetBtn.Name = "ResetBtn";
			this.ResetBtn.Size = new System.Drawing.Size(82, 58);
			this.ResetBtn.TabIndex = 1;
			this.ResetBtn.Text = "Reset";
			this.ResetBtn.UseVisualStyleBackColor = true;
			this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
			// 
			// CheckUpdateNowBtn
			// 
			this.CheckUpdateNowBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CheckUpdateNowBtn.Location = new System.Drawing.Point(376, 34);
			this.CheckUpdateNowBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.CheckUpdateNowBtn.Name = "CheckUpdateNowBtn";
			this.CheckUpdateNowBtn.Size = new System.Drawing.Size(123, 34);
			this.CheckUpdateNowBtn.TabIndex = 5;
			this.CheckUpdateNowBtn.Text = "Check Update";
			this.CheckUpdateNowBtn.UseVisualStyleBackColor = true;
			this.CheckUpdateNowBtn.Click += new System.EventHandler(this.CheckUpdateNow_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(511, 29);
			this.menuStrip1.TabIndex = 11;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// appToolStripMenuItem
			// 
			this.appToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.appToolStripMenuItem.Name = "appToolStripMenuItem";
			this.appToolStripMenuItem.Size = new System.Drawing.Size(49, 25);
			this.appToolStripMenuItem.Text = "Info";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
			this.helpToolStripMenuItem.Text = "Help";
			this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
			// 
			// CheckDomainsBtn
			// 
			this.CheckDomainsBtn.Location = new System.Drawing.Point(10, 170);
			this.CheckDomainsBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.CheckDomainsBtn.Name = "CheckDomainsBtn";
			this.CheckDomainsBtn.Size = new System.Drawing.Size(131, 47);
			this.CheckDomainsBtn.TabIndex = 2;
			this.CheckDomainsBtn.Text = "Check Domains";
			this.CheckDomainsBtn.UseVisualStyleBackColor = true;
			this.CheckDomainsBtn.Click += new System.EventHandler(this.CheckDomainsBtn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(511, 729);
			this.Controls.Add(this.CheckDomainsBtn);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.CheckUpdateNowBtn);
			this.Controls.Add(this.ResetBtn);
			this.Controls.Add(this.OpenParamsBtn);
			this.Controls.Add(this.OpenDomainListBtn);
			this.Controls.Add(this.CheckUpdatesChbox);
			this.Controls.Add(this.StartWithWindowsChbox);
			this.Controls.Add(this.HideToTrayChbox);
			this.Controls.Add(this.ListBoxForMessages);
			this.Controls.Add(this.ToggleDPIBtn);
			this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(5);
			this.MinimumSize = new System.Drawing.Size(527, 768);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ByeByeDPI";
			this.listContextMenuStrip.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ToggleDPIBtn;
		private System.Windows.Forms.ListBox ListBoxForMessages;
		private System.Windows.Forms.CheckBox HideToTrayChbox;
		private System.Windows.Forms.CheckBox StartWithWindowsChbox;
		private System.Windows.Forms.CheckBox CheckUpdatesChbox;
		private System.Windows.Forms.Button OpenDomainListBtn;
		private System.Windows.Forms.Button OpenParamsBtn;
		private System.Windows.Forms.Button ResetBtn;
		private System.Windows.Forms.ContextMenuStrip listContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.Button CheckUpdateNowBtn;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem appToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.Button CheckDomainsBtn;
	}
}

