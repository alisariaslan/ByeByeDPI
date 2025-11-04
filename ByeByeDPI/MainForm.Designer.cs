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
			this.OpenCheckListBtn = new System.Windows.Forms.Button();
			this.ParamsBtn = new System.Windows.Forms.Button();
			this.ResetBtn = new System.Windows.Forms.Button();
			this.CheckUpdateNow = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.appToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RunBtn = new System.Windows.Forms.Button();
			this.listContextMenuStrip.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ToggleDPIBtn
			// 
			this.ToggleDPIBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ToggleDPIBtn.Location = new System.Drawing.Point(286, 611);
			this.ToggleDPIBtn.Name = "ToggleDPIBtn";
			this.ToggleDPIBtn.Size = new System.Drawing.Size(326, 58);
			this.ToggleDPIBtn.TabIndex = 1;
			this.ToggleDPIBtn.Text = "Start Access";
			this.ToggleDPIBtn.UseVisualStyleBackColor = true;
			this.ToggleDPIBtn.Click += new System.EventHandler(this.ToggleDPIBtn_Click);
			// 
			// ListBoxForMessages
			// 
			this.ListBoxForMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ListBoxForMessages.ContextMenuStrip = this.listContextMenuStrip;
			this.ListBoxForMessages.FormattingEnabled = true;
			this.ListBoxForMessages.ItemHeight = 21;
			this.ListBoxForMessages.Location = new System.Drawing.Point(12, 181);
			this.ListBoxForMessages.Name = "ListBoxForMessages";
			this.ListBoxForMessages.Size = new System.Drawing.Size(598, 424);
			this.ListBoxForMessages.TabIndex = 0;
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
			this.HideToTrayChbox.Checked = true;
			this.HideToTrayChbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.HideToTrayChbox.Location = new System.Drawing.Point(12, 71);
			this.HideToTrayChbox.Name = "HideToTrayChbox";
			this.HideToTrayChbox.Size = new System.Drawing.Size(209, 25);
			this.HideToTrayChbox.TabIndex = 4;
			this.HideToTrayChbox.Text = "Hide to System Tray";
			this.HideToTrayChbox.UseVisualStyleBackColor = true;
			this.HideToTrayChbox.CheckedChanged += new System.EventHandler(this.HideToTrayChbox_CheckedChanged);
			// 
			// StartWithWindowsChbox
			// 
			this.StartWithWindowsChbox.AutoSize = true;
			this.StartWithWindowsChbox.Checked = true;
			this.StartWithWindowsChbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.StartWithWindowsChbox.Location = new System.Drawing.Point(12, 40);
			this.StartWithWindowsChbox.Name = "StartWithWindowsChbox";
			this.StartWithWindowsChbox.Size = new System.Drawing.Size(206, 25);
			this.StartWithWindowsChbox.TabIndex = 3;
			this.StartWithWindowsChbox.Text = "Start With Windows";
			this.StartWithWindowsChbox.UseVisualStyleBackColor = true;
			this.StartWithWindowsChbox.CheckedChanged += new System.EventHandler(this.StartWithWindowsChbox_CheckedChanged);
			// 
			// CheckUpdatesChbox
			// 
			this.CheckUpdatesChbox.AutoSize = true;
			this.CheckUpdatesChbox.Checked = true;
			this.CheckUpdatesChbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CheckUpdatesChbox.Location = new System.Drawing.Point(290, 40);
			this.CheckUpdatesChbox.Name = "CheckUpdatesChbox";
			this.CheckUpdatesChbox.Size = new System.Drawing.Size(162, 25);
			this.CheckUpdatesChbox.TabIndex = 2;
			this.CheckUpdatesChbox.Text = "Check Updates";
			this.CheckUpdatesChbox.UseVisualStyleBackColor = true;
			this.CheckUpdatesChbox.CheckedChanged += new System.EventHandler(this.CheckUpdatesChbox_CheckedChanged);
			// 
			// OpenCheckListBtn
			// 
			this.OpenCheckListBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.OpenCheckListBtn.Location = new System.Drawing.Point(83, 128);
			this.OpenCheckListBtn.Name = "OpenCheckListBtn";
			this.OpenCheckListBtn.Size = new System.Drawing.Size(135, 47);
			this.OpenCheckListBtn.TabIndex = 6;
			this.OpenCheckListBtn.Text = "Check List";
			this.OpenCheckListBtn.UseVisualStyleBackColor = true;
			this.OpenCheckListBtn.Click += new System.EventHandler(this.OpenCheckListBtn_Click);
			// 
			// ParamsBtn
			// 
			this.ParamsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ParamsBtn.Location = new System.Drawing.Point(501, 128);
			this.ParamsBtn.Name = "ParamsBtn";
			this.ParamsBtn.Size = new System.Drawing.Size(109, 47);
			this.ParamsBtn.TabIndex = 7;
			this.ParamsBtn.Text = "Params";
			this.ParamsBtn.UseVisualStyleBackColor = true;
			this.ParamsBtn.Click += new System.EventHandler(this.ParamsBtn_Click);
			// 
			// ResetBtn
			// 
			this.ResetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ResetBtn.Location = new System.Drawing.Point(12, 611);
			this.ResetBtn.Name = "ResetBtn";
			this.ResetBtn.Size = new System.Drawing.Size(108, 58);
			this.ResetBtn.TabIndex = 8;
			this.ResetBtn.Text = "Reset";
			this.ResetBtn.UseVisualStyleBackColor = true;
			this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
			// 
			// CheckUpdateNow
			// 
			this.CheckUpdateNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CheckUpdateNow.Location = new System.Drawing.Point(481, 34);
			this.CheckUpdateNow.Name = "CheckUpdateNow";
			this.CheckUpdateNow.Size = new System.Drawing.Size(129, 34);
			this.CheckUpdateNow.TabIndex = 9;
			this.CheckUpdateNow.Text = "Check Now";
			this.CheckUpdateNow.UseVisualStyleBackColor = true;
			this.CheckUpdateNow.Click += new System.EventHandler(this.CheckUpdateNow_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Font = new System.Drawing.Font("Audiowide", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(624, 25);
			this.menuStrip1.TabIndex = 11;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// appToolStripMenuItem
			// 
			this.appToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.appToolStripMenuItem.Name = "appToolStripMenuItem";
			this.appToolStripMenuItem.Size = new System.Drawing.Size(49, 21);
			this.appToolStripMenuItem.Text = "Info";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.helpToolStripMenuItem.Text = "Help";
			this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
			// 
			// RunBtn
			// 
			this.RunBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RunBtn.Location = new System.Drawing.Point(12, 128);
			this.RunBtn.Name = "RunBtn";
			this.RunBtn.Size = new System.Drawing.Size(65, 47);
			this.RunBtn.TabIndex = 12;
			this.RunBtn.Text = "Run";
			this.RunBtn.UseVisualStyleBackColor = true;
			this.RunBtn.Click += new System.EventHandler(this.RunBtn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 681);
			this.Controls.Add(this.RunBtn);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.CheckUpdateNow);
			this.Controls.Add(this.ResetBtn);
			this.Controls.Add(this.ParamsBtn);
			this.Controls.Add(this.OpenCheckListBtn);
			this.Controls.Add(this.CheckUpdatesChbox);
			this.Controls.Add(this.StartWithWindowsChbox);
			this.Controls.Add(this.HideToTrayChbox);
			this.Controls.Add(this.ListBoxForMessages);
			this.Controls.Add(this.ToggleDPIBtn);
			this.Font = new System.Drawing.Font("Audiowide", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.MinimumSize = new System.Drawing.Size(640, 720);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ByeByeDPI";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
		private System.Windows.Forms.Button OpenCheckListBtn;
		private System.Windows.Forms.Button ParamsBtn;
		private System.Windows.Forms.Button ResetBtn;
		private System.Windows.Forms.ContextMenuStrip listContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.Button CheckUpdateNow;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem appToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.Button RunBtn;
	}
}

