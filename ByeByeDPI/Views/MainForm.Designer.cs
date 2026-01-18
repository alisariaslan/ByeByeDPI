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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            listBox1_results = new System.Windows.Forms.ListBox();
            listContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            toolStripMenuItem30 = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem31 = new System.Windows.Forms.ToolStripMenuItem();
            settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            logsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            paramsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem32 = new System.Windows.Forms.ToolStripMenuItem();
            checkUpdateToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(components);
            poisonStyleExtender1 = new ReaLTaiizor.Controls.PoisonStyleExtender(components);
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            parrotCircleProgressBar1 = new ReaLTaiizor.Controls.ParrotCircleProgressBar();
            royalEllipseButton1_toggle = new ReaLTaiizor.Controls.RoyalEllipseButton();
            royalEllipseButton2_reset = new ReaLTaiizor.Controls.RoyalEllipseButton();
            royalEllipseButton3_apply = new ReaLTaiizor.Controls.RoyalEllipseButton();
            royalEllipseButton4_next = new ReaLTaiizor.Controls.RoyalEllipseButton();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            listContextMenuStrip.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1_results
            // 
            listBox1_results.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listBox1_results.ContextMenuStrip = listContextMenuStrip;
            listBox1_results.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listBox1_results.FormattingEnabled = true;
            listBox1_results.Location = new System.Drawing.Point(20, 100);
            listBox1_results.Margin = new System.Windows.Forms.Padding(2);
            listBox1_results.Name = "listBox1_results";
            listBox1_results.SelectionMode = System.Windows.Forms.SelectionMode.None;
            listBox1_results.Size = new System.Drawing.Size(200, 191);
            listBox1_results.TabIndex = 999;
            listBox1_results.TabStop = false;
            listBox1_results.Visible = false;
            // 
            // listContextMenuStrip
            // 
            listContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyToolStripMenuItem });
            listContextMenuStrip.Name = "listContextMenuStrip";
            listContextMenuStrip.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            copyToolStripMenuItem.Text = "Copy";
            // 
            // menuStrip1
            // 
            poisonStyleExtender1.SetApplyPoisonTheme(menuStrip1, true);
            menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem30, toolStripMenuItem31, toolStripMenuItem32 });
            menuStrip1.Location = new System.Drawing.Point(16, 60);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            menuStrip1.Size = new System.Drawing.Size(208, 25);
            menuStrip1.TabIndex = 11;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem30
            // 
            toolStripMenuItem30.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
            toolStripMenuItem30.Name = "toolStripMenuItem30";
            toolStripMenuItem30.Size = new System.Drawing.Size(42, 21);
            toolStripMenuItem30.Text = "Info";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // toolStripMenuItem31
            // 
            toolStripMenuItem31.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { settingsToolStripMenuItem1, logsToolStripMenuItem1, paramsToolStripMenuItem1 });
            toolStripMenuItem31.Name = "toolStripMenuItem31";
            toolStripMenuItem31.Size = new System.Drawing.Size(66, 21);
            toolStripMenuItem31.Text = "Settings";
            // 
            // settingsToolStripMenuItem1
            // 
            settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            settingsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            settingsToolStripMenuItem1.Text = "Open Settings";
            settingsToolStripMenuItem1.Click += settingsToolStripMenuItem1_Click;
            // 
            // logsToolStripMenuItem1
            // 
            logsToolStripMenuItem1.Name = "logsToolStripMenuItem1";
            logsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            logsToolStripMenuItem1.Text = "Open Logs";
            logsToolStripMenuItem1.Click += logsToolStripMenuItem1_Click;
            // 
            // paramsToolStripMenuItem1
            // 
            paramsToolStripMenuItem1.Name = "paramsToolStripMenuItem1";
            paramsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            paramsToolStripMenuItem1.Text = "Open Parameters";
            paramsToolStripMenuItem1.Click += paramsToolStripMenuItem1_Click;
            // 
            // toolStripMenuItem32
            // 
            toolStripMenuItem32.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { checkUpdateToolStripMenuItem1 });
            toolStripMenuItem32.Name = "toolStripMenuItem32";
            toolStripMenuItem32.Size = new System.Drawing.Size(63, 21);
            toolStripMenuItem32.Text = "Update";
            // 
            // checkUpdateToolStripMenuItem1
            // 
            checkUpdateToolStripMenuItem1.Name = "checkUpdateToolStripMenuItem1";
            checkUpdateToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            checkUpdateToolStripMenuItem1.Text = "Check Update";
            checkUpdateToolStripMenuItem1.Click += checkUpdateToolStripMenuItem1_Click;
            // 
            // poisonStyleManager1
            // 
            poisonStyleManager1.Owner = this;
            poisonStyleManager1.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Orange;
            // 
            // statusStrip1
            // 
            poisonStyleExtender1.SetApplyPoisonTheme(statusStrip1, true);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(16, 382);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            statusStrip1.Size = new System.Drawing.Size(208, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1010;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            toolStripStatusLabel1.ForeColor = System.Drawing.Color.DarkOrange;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(102, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // parrotCircleProgressBar1
            // 
            parrotCircleProgressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            parrotCircleProgressBar1.AnimationSpeed = 5;
            parrotCircleProgressBar1.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            parrotCircleProgressBar1.FilledColor = System.Drawing.Color.DarkOrange;
            parrotCircleProgressBar1.FilledColorAlpha = 130;
            parrotCircleProgressBar1.FilledThickness = 40;
            parrotCircleProgressBar1.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            parrotCircleProgressBar1.IsAnimated = false;
            parrotCircleProgressBar1.Location = new System.Drawing.Point(20, 100);
            parrotCircleProgressBar1.Name = "parrotCircleProgressBar1";
            parrotCircleProgressBar1.Percentage = 50;
            parrotCircleProgressBar1.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            parrotCircleProgressBar1.ShowText = true;
            parrotCircleProgressBar1.Size = new System.Drawing.Size(200, 200);
            parrotCircleProgressBar1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            parrotCircleProgressBar1.TabIndex = 1002;
            parrotCircleProgressBar1.TextColor = System.Drawing.Color.Gray;
            parrotCircleProgressBar1.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            parrotCircleProgressBar1.TextSize = 25;
            parrotCircleProgressBar1.UnFilledColor = System.Drawing.Color.FromArgb(114, 114, 114);
            parrotCircleProgressBar1.UnfilledThickness = 24;
            parrotCircleProgressBar1.Visible = false;
            // 
            // royalEllipseButton1_toggle
            // 
            royalEllipseButton1_toggle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            royalEllipseButton1_toggle.BackColor = System.Drawing.Color.White;
            royalEllipseButton1_toggle.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            royalEllipseButton1_toggle.BorderThickness = 3;
            royalEllipseButton1_toggle.DrawBorder = true;
            royalEllipseButton1_toggle.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            royalEllipseButton1_toggle.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31);
            royalEllipseButton1_toggle.HotTrackColor = System.Drawing.Color.Black;
            royalEllipseButton1_toggle.Image = null;
            royalEllipseButton1_toggle.LayoutFlags = ReaLTaiizor.Util.RoyalLayoutFlags.ImageBeforeText;
            royalEllipseButton1_toggle.Location = new System.Drawing.Point(20, 100);
            royalEllipseButton1_toggle.Name = "royalEllipseButton1_toggle";
            royalEllipseButton1_toggle.PressedColor = System.Drawing.Color.FromArgb(51, 102, 255);
            royalEllipseButton1_toggle.PressedForeColor = System.Drawing.Color.White;
            royalEllipseButton1_toggle.Size = new System.Drawing.Size(200, 200);
            royalEllipseButton1_toggle.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            royalEllipseButton1_toggle.TabIndex = 1006;
            royalEllipseButton1_toggle.Text = "Toggle";
            royalEllipseButton1_toggle.Click += royalEllipseButton1_toggle_Click;
            // 
            // royalEllipseButton2_reset
            // 
            royalEllipseButton2_reset.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            royalEllipseButton2_reset.BackColor = System.Drawing.Color.White;
            royalEllipseButton2_reset.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            royalEllipseButton2_reset.BorderThickness = 3;
            royalEllipseButton2_reset.DrawBorder = true;
            royalEllipseButton2_reset.Enabled = false;
            royalEllipseButton2_reset.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            royalEllipseButton2_reset.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31);
            royalEllipseButton2_reset.HotTrackColor = System.Drawing.Color.Transparent;
            royalEllipseButton2_reset.Image = null;
            royalEllipseButton2_reset.LayoutFlags = ReaLTaiizor.Util.RoyalLayoutFlags.ImageBeforeText;
            royalEllipseButton2_reset.Location = new System.Drawing.Point(20, 310);
            royalEllipseButton2_reset.Name = "royalEllipseButton2_reset";
            royalEllipseButton2_reset.PressedColor = System.Drawing.Color.FromArgb(51, 102, 255);
            royalEllipseButton2_reset.PressedForeColor = System.Drawing.Color.White;
            royalEllipseButton2_reset.Size = new System.Drawing.Size(60, 60);
            royalEllipseButton2_reset.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            royalEllipseButton2_reset.TabIndex = 1007;
            royalEllipseButton2_reset.Text = "Reset";
            royalEllipseButton2_reset.Click += royalEllipseButton2_reset_Click;
            // 
            // royalEllipseButton3_apply
            // 
            royalEllipseButton3_apply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            royalEllipseButton3_apply.BackColor = System.Drawing.Color.White;
            royalEllipseButton3_apply.BorderColor = System.Drawing.Color.DarkOrange;
            royalEllipseButton3_apply.BorderThickness = 3;
            royalEllipseButton3_apply.DrawBorder = true;
            royalEllipseButton3_apply.Enabled = false;
            royalEllipseButton3_apply.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            royalEllipseButton3_apply.ForeColor = System.Drawing.Color.DarkOrange;
            royalEllipseButton3_apply.HotTrackColor = System.Drawing.Color.Transparent;
            royalEllipseButton3_apply.Image = null;
            royalEllipseButton3_apply.LayoutFlags = ReaLTaiizor.Util.RoyalLayoutFlags.ImageBeforeText;
            royalEllipseButton3_apply.Location = new System.Drawing.Point(90, 310);
            royalEllipseButton3_apply.Name = "royalEllipseButton3_apply";
            royalEllipseButton3_apply.PressedColor = System.Drawing.Color.FromArgb(51, 102, 255);
            royalEllipseButton3_apply.PressedForeColor = System.Drawing.Color.White;
            royalEllipseButton3_apply.Size = new System.Drawing.Size(60, 60);
            royalEllipseButton3_apply.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            royalEllipseButton3_apply.TabIndex = 1008;
            royalEllipseButton3_apply.Text = "Apply";
            royalEllipseButton3_apply.Click += royalEllipseButton3_apply_Click;
            // 
            // royalEllipseButton4_next
            // 
            royalEllipseButton4_next.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            royalEllipseButton4_next.BackColor = System.Drawing.Color.White;
            royalEllipseButton4_next.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            royalEllipseButton4_next.BorderThickness = 3;
            royalEllipseButton4_next.DrawBorder = true;
            royalEllipseButton4_next.Enabled = false;
            royalEllipseButton4_next.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            royalEllipseButton4_next.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31);
            royalEllipseButton4_next.HotTrackColor = System.Drawing.Color.Transparent;
            royalEllipseButton4_next.Image = null;
            royalEllipseButton4_next.LayoutFlags = ReaLTaiizor.Util.RoyalLayoutFlags.ImageBeforeText;
            royalEllipseButton4_next.Location = new System.Drawing.Point(160, 310);
            royalEllipseButton4_next.Name = "royalEllipseButton4_next";
            royalEllipseButton4_next.PressedColor = System.Drawing.Color.FromArgb(51, 102, 255);
            royalEllipseButton4_next.PressedForeColor = System.Drawing.Color.White;
            royalEllipseButton4_next.Size = new System.Drawing.Size(60, 60);
            royalEllipseButton4_next.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            royalEllipseButton4_next.TabIndex = 1009;
            royalEllipseButton4_next.Text = "Next";
            royalEllipseButton4_next.Click += royalEllipseButton4_next_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(240, 420);
            Controls.Add(statusStrip1);
            Controls.Add(royalEllipseButton4_next);
            Controls.Add(royalEllipseButton3_apply);
            Controls.Add(royalEllipseButton2_reset);
            Controls.Add(menuStrip1);
            Controls.Add(listBox1_results);
            Controls.Add(parrotCircleProgressBar1);
            Controls.Add(royalEllipseButton1_toggle);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(240, 420);
            MinimumSize = new System.Drawing.Size(240, 420);
            Name = "MainForm";
            Padding = new System.Windows.Forms.Padding(16, 60, 16, 16);
            ShadowType = ReaLTaiizor.Enum.Poison.FormShadowType.AeroShadow;
            Text = "ByeByeDPI";
            listContextMenuStrip.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ToggleDPIBtn;
		private System.Windows.Forms.ListBox listBox1_results;
		private System.Windows.Forms.Button OpenDomainListBtn;
		private System.Windows.Forms.Button OpenParamsBtn;
		private System.Windows.Forms.ContextMenuStrip listContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem30;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Button CheckDomainsBtn;
        private ReaLTaiizor.Controls.PoisonStyleExtender poisonStyleExtender1;
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.ParrotCircleProgressBar parrotCircleProgressBar1;
        private ReaLTaiizor.Controls.RoyalEllipseButton royalEllipseButton1_toggle;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem31;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem32;
        private ReaLTaiizor.Controls.RoyalEllipseButton royalEllipseButton2_reset;
        private ReaLTaiizor.Controls.RoyalEllipseButton royalEllipseButton4_next;
        private ReaLTaiizor.Controls.RoyalEllipseButton royalEllipseButton3_apply;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem logsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem paramsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem checkUpdateToolStripMenuItem1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

