namespace ByeByeDPI.Views
{
	partial class SettingsForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            poisonButton1_resetDefaults = new ReaLTaiizor.Controls.PoisonButton();
            poisonTabControl1 = new ReaLTaiizor.Controls.PoisonTabControl();
            tabPage1_general = new System.Windows.Forms.TabPage();
            poisonTile1_startWithWindows = new ReaLTaiizor.Controls.PoisonTile();
            poisonToggle1_startWithWindows = new ReaLTaiizor.Controls.PoisonToggle();
            poisonLabel1_startWithWindows = new ReaLTaiizor.Controls.PoisonLabel();
            poisonTile1_checkUpdates = new ReaLTaiizor.Controls.PoisonTile();
            poisonLabel1_checkUpdates = new ReaLTaiizor.Controls.PoisonLabel();
            poisonToggle1_checkUpdates = new ReaLTaiizor.Controls.PoisonToggle();
            poisonTile1_autoHide = new ReaLTaiizor.Controls.PoisonTile();
            poisonToggle1_autoHide = new ReaLTaiizor.Controls.PoisonToggle();
            poisonLabel1_autoHide = new ReaLTaiizor.Controls.PoisonLabel();
            poisonTile1_hideToSystemTray = new ReaLTaiizor.Controls.PoisonTile();
            poisonToggle1_hideToSystemTray = new ReaLTaiizor.Controls.PoisonToggle();
            poisonLabel1_hideToSystemTray = new ReaLTaiizor.Controls.PoisonLabel();
            tabPage2_general = new System.Windows.Forms.TabPage();
            poisonTile1_theme = new ReaLTaiizor.Controls.PoisonTile();
            poisonDropDownButton1_selectTheme = new ReaLTaiizor.Controls.PoisonDropDownButton();
            contextMenuStrip1_themes = new System.Windows.Forms.ContextMenuStrip(components);
            defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            darkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            poisonLabel1_theme = new ReaLTaiizor.Controls.PoisonLabel();
            poisonTile1_showInTaskbar = new ReaLTaiizor.Controls.PoisonTile();
            poisonToggle1_showInTaskbar = new ReaLTaiizor.Controls.PoisonToggle();
            poisonLabel1_showInTaskbar = new ReaLTaiizor.Controls.PoisonLabel();
            poisonTile1_alwaysTopMost = new ReaLTaiizor.Controls.PoisonTile();
            poisonToggle1_alwaysTopMost = new ReaLTaiizor.Controls.PoisonToggle();
            poisonLabel1_alwaysTopMost = new ReaLTaiizor.Controls.PoisonLabel();
            tabPage5_hotkey = new System.Windows.Forms.TabPage();
            poisonTile1_showWindowHotkey = new ReaLTaiizor.Controls.PoisonTile();
            poisonTextBox1_showWindowHotkey = new ReaLTaiizor.Controls.PoisonTextBox();
            poisonLabel1_showWindowHotkey = new ReaLTaiizor.Controls.PoisonLabel();
            poisonTile1_globalHotkey = new ReaLTaiizor.Controls.PoisonTile();
            poisonToggle1_globalHotkeys = new ReaLTaiizor.Controls.PoisonToggle();
            poisonLabel1_globalHotkey = new ReaLTaiizor.Controls.PoisonLabel();
            poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(components);
            poisonStyleExtender1 = new ReaLTaiizor.Controls.PoisonStyleExtender(components);
            poisonTabControl1.SuspendLayout();
            tabPage1_general.SuspendLayout();
            poisonTile1_startWithWindows.SuspendLayout();
            poisonTile1_checkUpdates.SuspendLayout();
            poisonTile1_autoHide.SuspendLayout();
            poisonTile1_hideToSystemTray.SuspendLayout();
            tabPage2_general.SuspendLayout();
            poisonTile1_theme.SuspendLayout();
            contextMenuStrip1_themes.SuspendLayout();
            poisonTile1_showInTaskbar.SuspendLayout();
            poisonTile1_alwaysTopMost.SuspendLayout();
            tabPage5_hotkey.SuspendLayout();
            poisonTile1_showWindowHotkey.SuspendLayout();
            poisonTile1_globalHotkey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).BeginInit();
            SuspendLayout();
            // 
            // poisonButton1_resetDefaults
            // 
            poisonButton1_resetDefaults.Dock = System.Windows.Forms.DockStyle.Bottom;
            poisonButton1_resetDefaults.Location = new System.Drawing.Point(20, 470);
            poisonButton1_resetDefaults.Name = "poisonButton1_resetDefaults";
            poisonButton1_resetDefaults.Size = new System.Drawing.Size(440, 30);
            poisonButton1_resetDefaults.TabIndex = 24;
            poisonButton1_resetDefaults.Text = "Reset to Defaults";
            poisonButton1_resetDefaults.UseSelectable = true;
            poisonButton1_resetDefaults.Click += poisonButton1_resetDefaults_Click;
            // 
            // poisonTabControl1
            // 
            poisonTabControl1.Controls.Add(tabPage1_general);
            poisonTabControl1.Controls.Add(tabPage2_general);
            poisonTabControl1.Controls.Add(tabPage5_hotkey);
            poisonTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            poisonTabControl1.Location = new System.Drawing.Point(20, 60);
            poisonTabControl1.Name = "poisonTabControl1";
            poisonTabControl1.Padding = new System.Drawing.Point(6, 8);
            poisonTabControl1.SelectedIndex = 0;
            poisonTabControl1.Size = new System.Drawing.Size(440, 410);
            poisonTabControl1.TabIndex = 25;
            poisonTabControl1.UseSelectable = true;
            // 
            // tabPage1_general
            // 
            poisonStyleExtender1.SetApplyPoisonTheme(tabPage1_general, true);
            tabPage1_general.BackColor = System.Drawing.Color.Transparent;
            tabPage1_general.Controls.Add(poisonTile1_startWithWindows);
            tabPage1_general.Controls.Add(poisonTile1_checkUpdates);
            tabPage1_general.Controls.Add(poisonTile1_autoHide);
            tabPage1_general.Controls.Add(poisonTile1_hideToSystemTray);
            tabPage1_general.Location = new System.Drawing.Point(4, 38);
            tabPage1_general.Name = "tabPage1_general";
            tabPage1_general.Padding = new System.Windows.Forms.Padding(3);
            tabPage1_general.Size = new System.Drawing.Size(432, 368);
            tabPage1_general.TabIndex = 0;
            tabPage1_general.Text = "General";
            // 
            // poisonTile1_startWithWindows
            // 
            poisonTile1_startWithWindows.ActiveControl = null;
            poisonTile1_startWithWindows.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_startWithWindows.Controls.Add(poisonToggle1_startWithWindows);
            poisonTile1_startWithWindows.Controls.Add(poisonLabel1_startWithWindows);
            poisonTile1_startWithWindows.Location = new System.Drawing.Point(6, 72);
            poisonTile1_startWithWindows.Name = "poisonTile1_startWithWindows";
            poisonTile1_startWithWindows.Size = new System.Drawing.Size(420, 60);
            poisonTile1_startWithWindows.TabIndex = 4;
            poisonTile1_startWithWindows.Text = "Launch the application automatically when Windows starts.";
            poisonTile1_startWithWindows.UseSelectable = true;
            // 
            // poisonToggle1_startWithWindows
            // 
            poisonToggle1_startWithWindows.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_startWithWindows.AutoSize = true;
            poisonToggle1_startWithWindows.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_startWithWindows.Name = "poisonToggle1_startWithWindows";
            poisonToggle1_startWithWindows.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_startWithWindows.TabIndex = 7;
            poisonToggle1_startWithWindows.Text = "Off";
            poisonToggle1_startWithWindows.UseSelectable = true;
            // 
            // poisonLabel1_startWithWindows
            // 
            poisonLabel1_startWithWindows.AutoSize = true;
            poisonLabel1_startWithWindows.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_startWithWindows.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_startWithWindows.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_startWithWindows.Name = "poisonLabel1_startWithWindows";
            poisonLabel1_startWithWindows.Size = new System.Drawing.Size(138, 19);
            poisonLabel1_startWithWindows.TabIndex = 6;
            poisonLabel1_startWithWindows.Text = "Start with Windows";
            // 
            // poisonTile1_checkUpdates
            // 
            poisonTile1_checkUpdates.ActiveControl = null;
            poisonTile1_checkUpdates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_checkUpdates.Controls.Add(poisonLabel1_checkUpdates);
            poisonTile1_checkUpdates.Controls.Add(poisonToggle1_checkUpdates);
            poisonTile1_checkUpdates.Location = new System.Drawing.Point(6, 6);
            poisonTile1_checkUpdates.Name = "poisonTile1_checkUpdates";
            poisonTile1_checkUpdates.Size = new System.Drawing.Size(420, 60);
            poisonTile1_checkUpdates.TabIndex = 3;
            poisonTile1_checkUpdates.Text = "Automatically check for the latest software updates.";
            poisonTile1_checkUpdates.UseSelectable = true;
            // 
            // poisonLabel1_checkUpdates
            // 
            poisonLabel1_checkUpdates.AutoSize = true;
            poisonLabel1_checkUpdates.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_checkUpdates.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_checkUpdates.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_checkUpdates.Name = "poisonLabel1_checkUpdates";
            poisonLabel1_checkUpdates.Size = new System.Drawing.Size(108, 19);
            poisonLabel1_checkUpdates.TabIndex = 5;
            poisonLabel1_checkUpdates.Text = "Check Updates";
            // 
            // poisonToggle1_checkUpdates
            // 
            poisonToggle1_checkUpdates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_checkUpdates.AutoSize = true;
            poisonToggle1_checkUpdates.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_checkUpdates.Name = "poisonToggle1_checkUpdates";
            poisonToggle1_checkUpdates.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_checkUpdates.TabIndex = 4;
            poisonToggle1_checkUpdates.Text = "Off";
            poisonToggle1_checkUpdates.UseSelectable = true;
            // 
            // poisonTile1_autoHide
            // 
            poisonTile1_autoHide.ActiveControl = null;
            poisonTile1_autoHide.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_autoHide.Controls.Add(poisonToggle1_autoHide);
            poisonTile1_autoHide.Controls.Add(poisonLabel1_autoHide);
            poisonTile1_autoHide.Location = new System.Drawing.Point(6, 204);
            poisonTile1_autoHide.Name = "poisonTile1_autoHide";
            poisonTile1_autoHide.Size = new System.Drawing.Size(420, 60);
            poisonTile1_autoHide.TabIndex = 26;
            poisonTile1_autoHide.Text = "Automatically hide the app when it loses focus.";
            poisonTile1_autoHide.UseSelectable = true;
            // 
            // poisonToggle1_autoHide
            // 
            poisonToggle1_autoHide.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_autoHide.AutoSize = true;
            poisonToggle1_autoHide.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_autoHide.Name = "poisonToggle1_autoHide";
            poisonToggle1_autoHide.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_autoHide.TabIndex = 9;
            poisonToggle1_autoHide.Text = "Off";
            poisonToggle1_autoHide.UseSelectable = true;
            // 
            // poisonLabel1_autoHide
            // 
            poisonLabel1_autoHide.AutoSize = true;
            poisonLabel1_autoHide.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_autoHide.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_autoHide.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_autoHide.Name = "poisonLabel1_autoHide";
            poisonLabel1_autoHide.Size = new System.Drawing.Size(77, 19);
            poisonLabel1_autoHide.TabIndex = 5;
            poisonLabel1_autoHide.Text = "Auto Hide";
            // 
            // poisonTile1_hideToSystemTray
            // 
            poisonTile1_hideToSystemTray.ActiveControl = null;
            poisonTile1_hideToSystemTray.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_hideToSystemTray.Controls.Add(poisonToggle1_hideToSystemTray);
            poisonTile1_hideToSystemTray.Controls.Add(poisonLabel1_hideToSystemTray);
            poisonTile1_hideToSystemTray.Location = new System.Drawing.Point(6, 138);
            poisonTile1_hideToSystemTray.Name = "poisonTile1_hideToSystemTray";
            poisonTile1_hideToSystemTray.Size = new System.Drawing.Size(420, 60);
            poisonTile1_hideToSystemTray.TabIndex = 24;
            poisonTile1_hideToSystemTray.Text = "Minimize the app to the tray instead of closing it.";
            poisonTile1_hideToSystemTray.UseSelectable = true;
            // 
            // poisonToggle1_hideToSystemTray
            // 
            poisonToggle1_hideToSystemTray.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_hideToSystemTray.AutoSize = true;
            poisonToggle1_hideToSystemTray.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_hideToSystemTray.Name = "poisonToggle1_hideToSystemTray";
            poisonToggle1_hideToSystemTray.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_hideToSystemTray.TabIndex = 8;
            poisonToggle1_hideToSystemTray.Text = "Off";
            poisonToggle1_hideToSystemTray.UseSelectable = true;
            // 
            // poisonLabel1_hideToSystemTray
            // 
            poisonLabel1_hideToSystemTray.AutoSize = true;
            poisonLabel1_hideToSystemTray.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_hideToSystemTray.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_hideToSystemTray.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_hideToSystemTray.Name = "poisonLabel1_hideToSystemTray";
            poisonLabel1_hideToSystemTray.Size = new System.Drawing.Size(144, 19);
            poisonLabel1_hideToSystemTray.TabIndex = 5;
            poisonLabel1_hideToSystemTray.Text = "Hide to System Tray";
            // 
            // tabPage2_general
            // 
            tabPage2_general.BackColor = System.Drawing.Color.Transparent;
            tabPage2_general.Controls.Add(poisonTile1_theme);
            tabPage2_general.Controls.Add(poisonTile1_showInTaskbar);
            tabPage2_general.Controls.Add(poisonTile1_alwaysTopMost);
            tabPage2_general.Location = new System.Drawing.Point(4, 38);
            tabPage2_general.Name = "tabPage2_general";
            tabPage2_general.Padding = new System.Windows.Forms.Padding(3);
            tabPage2_general.Size = new System.Drawing.Size(432, 368);
            tabPage2_general.TabIndex = 1;
            tabPage2_general.Text = "Appearance";
            // 
            // poisonTile1_theme
            // 
            poisonTile1_theme.ActiveControl = null;
            poisonTile1_theme.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_theme.Controls.Add(poisonDropDownButton1_selectTheme);
            poisonTile1_theme.Controls.Add(poisonLabel1_theme);
            poisonTile1_theme.Location = new System.Drawing.Point(6, 138);
            poisonTile1_theme.Name = "poisonTile1_theme";
            poisonTile1_theme.Size = new System.Drawing.Size(420, 60);
            poisonTile1_theme.TabIndex = 20;
            poisonTile1_theme.Text = "Shows the theme currently applied.";
            poisonTile1_theme.UseSelectable = true;
            // 
            // poisonDropDownButton1_selectTheme
            // 
            poisonDropDownButton1_selectTheme.AutoSize = true;
            poisonDropDownButton1_selectTheme.ContextMenuStrip = contextMenuStrip1_themes;
            poisonDropDownButton1_selectTheme.Location = new System.Drawing.Point(258, 8);
            poisonDropDownButton1_selectTheme.Name = "poisonDropDownButton1_selectTheme";
            poisonDropDownButton1_selectTheme.Size = new System.Drawing.Size(152, 30);
            poisonDropDownButton1_selectTheme.SplitMenuStrip = contextMenuStrip1_themes;
            poisonDropDownButton1_selectTheme.TabIndex = 6;
            poisonDropDownButton1_selectTheme.Text = "Select Theme";
            poisonDropDownButton1_selectTheme.UseSelectable = true;
            poisonDropDownButton1_selectTheme.Click += poisonDropDownButton1_selectTheme_Click;
            // 
            // contextMenuStrip1_themes
            // 
            contextMenuStrip1_themes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { defaultToolStripMenuItem, lightToolStripMenuItem, darkToolStripMenuItem });
            contextMenuStrip1_themes.Name = "contextMenuStrip1_themes";
            contextMenuStrip1_themes.Size = new System.Drawing.Size(162, 70);
            // 
            // defaultToolStripMenuItem
            // 
            defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            defaultToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            defaultToolStripMenuItem.Text = "Default (System)";
            defaultToolStripMenuItem.Click += defaultToolStripMenuItem_Click;
            // 
            // lightToolStripMenuItem
            // 
            lightToolStripMenuItem.Name = "lightToolStripMenuItem";
            lightToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            lightToolStripMenuItem.Text = "Light";
            lightToolStripMenuItem.Click += lightToolStripMenuItem_Click;
            // 
            // darkToolStripMenuItem
            // 
            darkToolStripMenuItem.Name = "darkToolStripMenuItem";
            darkToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            darkToolStripMenuItem.Text = "Dark";
            darkToolStripMenuItem.Click += darkToolStripMenuItem_Click;
            // 
            // poisonLabel1_theme
            // 
            poisonLabel1_theme.AutoSize = true;
            poisonLabel1_theme.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_theme.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_theme.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_theme.Name = "poisonLabel1_theme";
            poisonLabel1_theme.Size = new System.Drawing.Size(108, 19);
            poisonLabel1_theme.TabIndex = 5;
            poisonLabel1_theme.Text = "Current Theme";
            // 
            // poisonTile1_showInTaskbar
            // 
            poisonTile1_showInTaskbar.ActiveControl = null;
            poisonTile1_showInTaskbar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_showInTaskbar.Controls.Add(poisonToggle1_showInTaskbar);
            poisonTile1_showInTaskbar.Controls.Add(poisonLabel1_showInTaskbar);
            poisonTile1_showInTaskbar.Location = new System.Drawing.Point(6, 72);
            poisonTile1_showInTaskbar.Name = "poisonTile1_showInTaskbar";
            poisonTile1_showInTaskbar.Size = new System.Drawing.Size(420, 60);
            poisonTile1_showInTaskbar.TabIndex = 18;
            poisonTile1_showInTaskbar.Text = "Display the app in the windows taskbar.";
            poisonTile1_showInTaskbar.UseSelectable = true;
            // 
            // poisonToggle1_showInTaskbar
            // 
            poisonToggle1_showInTaskbar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_showInTaskbar.AutoSize = true;
            poisonToggle1_showInTaskbar.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_showInTaskbar.Name = "poisonToggle1_showInTaskbar";
            poisonToggle1_showInTaskbar.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_showInTaskbar.TabIndex = 7;
            poisonToggle1_showInTaskbar.Text = "Off";
            poisonToggle1_showInTaskbar.UseSelectable = true;
            // 
            // poisonLabel1_showInTaskbar
            // 
            poisonLabel1_showInTaskbar.AutoSize = true;
            poisonLabel1_showInTaskbar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_showInTaskbar.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_showInTaskbar.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_showInTaskbar.Name = "poisonLabel1_showInTaskbar";
            poisonLabel1_showInTaskbar.Size = new System.Drawing.Size(117, 19);
            poisonLabel1_showInTaskbar.TabIndex = 5;
            poisonLabel1_showInTaskbar.Text = "Show In Taskbar";
            // 
            // poisonTile1_alwaysTopMost
            // 
            poisonTile1_alwaysTopMost.ActiveControl = null;
            poisonTile1_alwaysTopMost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_alwaysTopMost.Controls.Add(poisonToggle1_alwaysTopMost);
            poisonTile1_alwaysTopMost.Controls.Add(poisonLabel1_alwaysTopMost);
            poisonTile1_alwaysTopMost.Location = new System.Drawing.Point(6, 6);
            poisonTile1_alwaysTopMost.Name = "poisonTile1_alwaysTopMost";
            poisonTile1_alwaysTopMost.Size = new System.Drawing.Size(420, 60);
            poisonTile1_alwaysTopMost.TabIndex = 17;
            poisonTile1_alwaysTopMost.Text = "Keep the app above all other apps.";
            poisonTile1_alwaysTopMost.UseSelectable = true;
            // 
            // poisonToggle1_alwaysTopMost
            // 
            poisonToggle1_alwaysTopMost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_alwaysTopMost.AutoSize = true;
            poisonToggle1_alwaysTopMost.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_alwaysTopMost.Name = "poisonToggle1_alwaysTopMost";
            poisonToggle1_alwaysTopMost.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_alwaysTopMost.TabIndex = 7;
            poisonToggle1_alwaysTopMost.Text = "Off";
            poisonToggle1_alwaysTopMost.UseSelectable = true;
            // 
            // poisonLabel1_alwaysTopMost
            // 
            poisonLabel1_alwaysTopMost.AutoSize = true;
            poisonLabel1_alwaysTopMost.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_alwaysTopMost.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_alwaysTopMost.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_alwaysTopMost.Name = "poisonLabel1_alwaysTopMost";
            poisonLabel1_alwaysTopMost.Size = new System.Drawing.Size(122, 19);
            poisonLabel1_alwaysTopMost.TabIndex = 5;
            poisonLabel1_alwaysTopMost.Text = "Always Top Most";
            // 
            // tabPage5_hotkey
            // 
            tabPage5_hotkey.BackColor = System.Drawing.Color.Transparent;
            tabPage5_hotkey.Controls.Add(poisonTile1_showWindowHotkey);
            tabPage5_hotkey.Controls.Add(poisonTile1_globalHotkey);
            tabPage5_hotkey.Location = new System.Drawing.Point(4, 38);
            tabPage5_hotkey.Name = "tabPage5_hotkey";
            tabPage5_hotkey.Size = new System.Drawing.Size(432, 368);
            tabPage5_hotkey.TabIndex = 4;
            tabPage5_hotkey.Text = "Hotkeys";
            // 
            // poisonTile1_showWindowHotkey
            // 
            poisonTile1_showWindowHotkey.ActiveControl = null;
            poisonTile1_showWindowHotkey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_showWindowHotkey.Controls.Add(poisonTextBox1_showWindowHotkey);
            poisonTile1_showWindowHotkey.Controls.Add(poisonLabel1_showWindowHotkey);
            poisonTile1_showWindowHotkey.Location = new System.Drawing.Point(6, 72);
            poisonTile1_showWindowHotkey.Name = "poisonTile1_showWindowHotkey";
            poisonTile1_showWindowHotkey.Size = new System.Drawing.Size(420, 60);
            poisonTile1_showWindowHotkey.TabIndex = 26;
            poisonTile1_showWindowHotkey.Text = "Key combination to open window.";
            poisonTile1_showWindowHotkey.UseSelectable = true;
            // 
            // poisonTextBox1_showWindowHotkey
            // 
            poisonTextBox1_showWindowHotkey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            // 
            // 
            // 
            poisonTextBox1_showWindowHotkey.CustomButton.Image = null;
            poisonTextBox1_showWindowHotkey.CustomButton.Location = new System.Drawing.Point(58, 1);
            poisonTextBox1_showWindowHotkey.CustomButton.Name = "";
            poisonTextBox1_showWindowHotkey.CustomButton.Size = new System.Drawing.Size(21, 21);
            poisonTextBox1_showWindowHotkey.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            poisonTextBox1_showWindowHotkey.CustomButton.TabIndex = 1;
            poisonTextBox1_showWindowHotkey.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            poisonTextBox1_showWindowHotkey.CustomButton.UseSelectable = true;
            poisonTextBox1_showWindowHotkey.CustomButton.Visible = false;
            poisonTextBox1_showWindowHotkey.Enabled = false;
            poisonTextBox1_showWindowHotkey.Location = new System.Drawing.Point(330, 8);
            poisonTextBox1_showWindowHotkey.MaxLength = 32767;
            poisonTextBox1_showWindowHotkey.Name = "poisonTextBox1_showWindowHotkey";
            poisonTextBox1_showWindowHotkey.PasswordChar = '\0';
            poisonTextBox1_showWindowHotkey.PromptText = "Undefined";
            poisonTextBox1_showWindowHotkey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            poisonTextBox1_showWindowHotkey.SelectedText = "";
            poisonTextBox1_showWindowHotkey.SelectionLength = 0;
            poisonTextBox1_showWindowHotkey.SelectionStart = 0;
            poisonTextBox1_showWindowHotkey.ShortcutsEnabled = true;
            poisonTextBox1_showWindowHotkey.Size = new System.Drawing.Size(80, 23);
            poisonTextBox1_showWindowHotkey.TabIndex = 8;
            poisonTextBox1_showWindowHotkey.UseSelectable = true;
            poisonTextBox1_showWindowHotkey.WaterMark = "Undefined";
            poisonTextBox1_showWindowHotkey.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
            poisonTextBox1_showWindowHotkey.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // poisonLabel1_showWindowHotkey
            // 
            poisonLabel1_showWindowHotkey.AutoSize = true;
            poisonLabel1_showWindowHotkey.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_showWindowHotkey.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_showWindowHotkey.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_showWindowHotkey.Name = "poisonLabel1_showWindowHotkey";
            poisonLabel1_showWindowHotkey.Size = new System.Drawing.Size(157, 19);
            poisonLabel1_showWindowHotkey.TabIndex = 5;
            poisonLabel1_showWindowHotkey.Text = "Show Window Hotkey";
            // 
            // poisonTile1_globalHotkey
            // 
            poisonTile1_globalHotkey.ActiveControl = null;
            poisonTile1_globalHotkey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            poisonTile1_globalHotkey.Controls.Add(poisonToggle1_globalHotkeys);
            poisonTile1_globalHotkey.Controls.Add(poisonLabel1_globalHotkey);
            poisonTile1_globalHotkey.Location = new System.Drawing.Point(6, 6);
            poisonTile1_globalHotkey.Name = "poisonTile1_globalHotkey";
            poisonTile1_globalHotkey.Size = new System.Drawing.Size(420, 60);
            poisonTile1_globalHotkey.TabIndex = 25;
            poisonTile1_globalHotkey.Text = "Capture a key combination system-wide in Windows.";
            poisonTile1_globalHotkey.UseSelectable = true;
            // 
            // poisonToggle1_globalHotkeys
            // 
            poisonToggle1_globalHotkeys.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            poisonToggle1_globalHotkeys.AutoSize = true;
            poisonToggle1_globalHotkeys.Location = new System.Drawing.Point(330, 8);
            poisonToggle1_globalHotkeys.Name = "poisonToggle1_globalHotkeys";
            poisonToggle1_globalHotkeys.Size = new System.Drawing.Size(80, 21);
            poisonToggle1_globalHotkeys.TabIndex = 9;
            poisonToggle1_globalHotkeys.Text = "Off";
            poisonToggle1_globalHotkeys.UseSelectable = true;
            // 
            // poisonLabel1_globalHotkey
            // 
            poisonLabel1_globalHotkey.AutoSize = true;
            poisonLabel1_globalHotkey.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            poisonLabel1_globalHotkey.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            poisonLabel1_globalHotkey.Location = new System.Drawing.Point(4, 8);
            poisonLabel1_globalHotkey.Name = "poisonLabel1_globalHotkey";
            poisonLabel1_globalHotkey.Size = new System.Drawing.Size(106, 19);
            poisonLabel1_globalHotkey.TabIndex = 5;
            poisonLabel1_globalHotkey.Text = "Global Hotkey";
            // 
            // poisonStyleManager1
            // 
            poisonStyleManager1.Owner = this;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(480, 520);
            Controls.Add(poisonTabControl1);
            Controls.Add(poisonButton1_resetDefaults);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(480, 520);
            Name = "SettingsForm";
            ShadowType = ReaLTaiizor.Enum.Poison.FormShadowType.AeroShadow;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Default;
            TopMost = true;
            poisonTabControl1.ResumeLayout(false);
            tabPage1_general.ResumeLayout(false);
            poisonTile1_startWithWindows.ResumeLayout(false);
            poisonTile1_startWithWindows.PerformLayout();
            poisonTile1_checkUpdates.ResumeLayout(false);
            poisonTile1_checkUpdates.PerformLayout();
            poisonTile1_autoHide.ResumeLayout(false);
            poisonTile1_autoHide.PerformLayout();
            poisonTile1_hideToSystemTray.ResumeLayout(false);
            poisonTile1_hideToSystemTray.PerformLayout();
            tabPage2_general.ResumeLayout(false);
            poisonTile1_theme.ResumeLayout(false);
            poisonTile1_theme.PerformLayout();
            contextMenuStrip1_themes.ResumeLayout(false);
            poisonTile1_showInTaskbar.ResumeLayout(false);
            poisonTile1_showInTaskbar.PerformLayout();
            poisonTile1_alwaysTopMost.ResumeLayout(false);
            poisonTile1_alwaysTopMost.PerformLayout();
            tabPage5_hotkey.ResumeLayout(false);
            poisonTile1_showWindowHotkey.ResumeLayout(false);
            poisonTile1_showWindowHotkey.PerformLayout();
            poisonTile1_globalHotkey.ResumeLayout(false);
            poisonTile1_globalHotkey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private ReaLTaiizor.Controls.PoisonButton poisonButton1_resetDefaults;
        private ReaLTaiizor.Controls.PoisonTabControl poisonTabControl1;
        private System.Windows.Forms.TabPage tabPage1_general;
        private System.Windows.Forms.TabPage tabPage2_general;
        private System.Windows.Forms.TabPage tabPage5_hotkey;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_checkUpdates;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_checkUpdates;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_checkUpdates;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_startWithWindows;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_startWithWindows;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_startWithWindows;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_showInTaskbar;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_showInTaskbar;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_alwaysTopMost;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_alwaysTopMost;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_showInTaskbar;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_alwaysTopMost;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_hideToSystemTray;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_hideToSystemTray;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_autoHide;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_autoHide;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_globalHotkey;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_globalHotkey;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_showWindowHotkey;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_showWindowHotkey;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_hideToSystemTray;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_autoHide;
        private ReaLTaiizor.Controls.PoisonToggle poisonToggle1_globalHotkeys;
        private ReaLTaiizor.Controls.PoisonTextBox poisonTextBox1_showWindowHotkey;
        private ReaLTaiizor.Controls.PoisonTile poisonTile1_theme;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1_theme;
        private ReaLTaiizor.Controls.PoisonDropDownButton poisonDropDownButton1_selectTheme;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1_themes;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkToolStripMenuItem;
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.PoisonStyleExtender poisonStyleExtender1;
    }
}
