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
            ToggleDPIBtn = new System.Windows.Forms.Button();
            MessagesListBox = new System.Windows.Forms.ListBox();
            listContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            OpenDomainListBtn = new System.Windows.Forms.Button();
            OpenParamsBtn = new System.Windows.Forms.Button();
            ResetBtn = new System.Windows.Forms.Button();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            appToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CheckDomainsBtn = new System.Windows.Forms.Button();
            poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(components);
            poisonStyleExtender1 = new ReaLTaiizor.Controls.PoisonStyleExtender(components);
            parrotCircleProgressBar1 = new ReaLTaiizor.Controls.ParrotCircleProgressBar();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            royalEllipseButton1 = new ReaLTaiizor.Controls.RoyalEllipseButton();
            listContextMenuStrip.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // ToggleDPIBtn
            // 
            ToggleDPIBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ToggleDPIBtn.Location = new System.Drawing.Point(365, 929);
            ToggleDPIBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            ToggleDPIBtn.Name = "ToggleDPIBtn";
            ToggleDPIBtn.Size = new System.Drawing.Size(163, 47);
            ToggleDPIBtn.TabIndex = 0;
            ToggleDPIBtn.Text = "Start Access";
            ToggleDPIBtn.UseVisualStyleBackColor = true;
            ToggleDPIBtn.Click += ToggleDPIBtn_Click;
            // 
            // MessagesListBox
            // 
            MessagesListBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            MessagesListBox.ContextMenuStrip = listContextMenuStrip;
            MessagesListBox.FormattingEnabled = true;
            MessagesListBox.Location = new System.Drawing.Point(93, 782);
            MessagesListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            MessagesListBox.Name = "MessagesListBox";
            MessagesListBox.Size = new System.Drawing.Size(246, 157);
            MessagesListBox.TabIndex = 999;
            MessagesListBox.TabStop = false;
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
            // OpenDomainListBtn
            // 
            OpenDomainListBtn.Location = new System.Drawing.Point(228, 726);
            OpenDomainListBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            OpenDomainListBtn.Name = "OpenDomainListBtn";
            OpenDomainListBtn.Size = new System.Drawing.Size(136, 38);
            OpenDomainListBtn.TabIndex = 3;
            OpenDomainListBtn.Text = "Open Domain List";
            OpenDomainListBtn.UseVisualStyleBackColor = true;
            // 
            // OpenParamsBtn
            // 
            OpenParamsBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            OpenParamsBtn.Location = new System.Drawing.Point(394, 831);
            OpenParamsBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            OpenParamsBtn.Name = "OpenParamsBtn";
            OpenParamsBtn.Size = new System.Drawing.Size(143, 38);
            OpenParamsBtn.TabIndex = 4;
            OpenParamsBtn.Text = "Open Params";
            OpenParamsBtn.UseVisualStyleBackColor = true;
            // 
            // ResetBtn
            // 
            ResetBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ResetBtn.Location = new System.Drawing.Point(47, 955);
            ResetBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            ResetBtn.Name = "ResetBtn";
            ResetBtn.Size = new System.Drawing.Size(64, 47);
            ResetBtn.TabIndex = 1;
            ResetBtn.Text = "Reset";
            ResetBtn.UseVisualStyleBackColor = true;
            ResetBtn.Click += ResetBtn_Click;
            // 
            // menuStrip1
            // 
            poisonStyleExtender1.SetApplyPoisonTheme(menuStrip1, true);
            menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { appToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(16, 49);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            menuStrip1.Size = new System.Drawing.Size(1218, 25);
            menuStrip1.TabIndex = 11;
            menuStrip1.Text = "menuStrip1";
            // 
            // appToolStripMenuItem
            // 
            appToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem, helpToolStripMenuItem });
            appToolStripMenuItem.Name = "appToolStripMenuItem";
            appToolStripMenuItem.Size = new System.Drawing.Size(42, 21);
            appToolStripMenuItem.Text = "Info";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            aboutToolStripMenuItem.Text = "About";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            helpToolStripMenuItem.Text = "Help";
            // 
            // CheckDomainsBtn
            // 
            CheckDomainsBtn.Location = new System.Drawing.Point(47, 693);
            CheckDomainsBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            CheckDomainsBtn.Name = "CheckDomainsBtn";
            CheckDomainsBtn.Size = new System.Drawing.Size(126, 38);
            CheckDomainsBtn.TabIndex = 2;
            CheckDomainsBtn.Text = "Check Domains";
            CheckDomainsBtn.UseVisualStyleBackColor = true;
            CheckDomainsBtn.Click += CheckDomainsBtn_Click;
            // 
            // poisonStyleManager1
            // 
            poisonStyleManager1.Owner = this;
            // 
            // parrotCircleProgressBar1
            // 
            parrotCircleProgressBar1.AnimationSpeed = 5;
            parrotCircleProgressBar1.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            parrotCircleProgressBar1.FilledColor = System.Drawing.Color.FromArgb(60, 220, 210);
            parrotCircleProgressBar1.FilledColorAlpha = 130;
            parrotCircleProgressBar1.FilledThickness = 40;
            parrotCircleProgressBar1.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            parrotCircleProgressBar1.IsAnimated = false;
            parrotCircleProgressBar1.Location = new System.Drawing.Point(3, 3);
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
            // 
            // panel1
            // 
            panel1.Controls.Add(royalEllipseButton1);
            panel1.Location = new System.Drawing.Point(19, 77);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(360, 360);
            panel1.TabIndex = 1003;
            // 
            // panel2
            // 
            panel2.Controls.Add(parrotCircleProgressBar1);
            panel2.Location = new System.Drawing.Point(385, 77);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(360, 360);
            panel2.TabIndex = 1004;
            // 
            // panel3
            // 
            panel3.Location = new System.Drawing.Point(752, 555);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(452, 421);
            panel3.TabIndex = 1005;
            // 
            // royalEllipseButton1
            // 
            royalEllipseButton1.BackColor = System.Drawing.Color.White;
            royalEllipseButton1.BorderColor = System.Drawing.Color.FromArgb(180, 180, 180);
            royalEllipseButton1.BorderThickness = 3;
            royalEllipseButton1.DrawBorder = true;
            royalEllipseButton1.ForeColor = System.Drawing.Color.FromArgb(31, 31, 31);
            royalEllipseButton1.HotTrackColor = System.Drawing.Color.FromArgb(221, 221, 221);
            royalEllipseButton1.Image = null;
            royalEllipseButton1.LayoutFlags = ReaLTaiizor.Util.RoyalLayoutFlags.ImageBeforeText;
            royalEllipseButton1.Location = new System.Drawing.Point(3, 3);
            royalEllipseButton1.Name = "royalEllipseButton1";
            royalEllipseButton1.PressedColor = System.Drawing.Color.FromArgb(51, 102, 255);
            royalEllipseButton1.PressedForeColor = System.Drawing.Color.White;
            royalEllipseButton1.Size = new System.Drawing.Size(200, 200);
            royalEllipseButton1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            royalEllipseButton1.TabIndex = 1006;
            royalEllipseButton1.Text = "Toggle";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1250, 1039);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(CheckDomainsBtn);
            Controls.Add(menuStrip1);
            Controls.Add(ResetBtn);
            Controls.Add(OpenParamsBtn);
            Controls.Add(OpenDomainListBtn);
            Controls.Add(MessagesListBox);
            Controls.Add(ToggleDPIBtn);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            MinimumSize = new System.Drawing.Size(410, 622);
            Name = "MainForm";
            Padding = new System.Windows.Forms.Padding(16, 49, 16, 16);
            Text = "ByeByeDPI";
            listContextMenuStrip.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ToggleDPIBtn;
		private System.Windows.Forms.ListBox MessagesListBox;
		private System.Windows.Forms.Button OpenDomainListBtn;
		private System.Windows.Forms.Button OpenParamsBtn;
		private System.Windows.Forms.Button ResetBtn;
		private System.Windows.Forms.ContextMenuStrip listContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem appToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.Button CheckDomainsBtn;
        private ReaLTaiizor.Controls.PoisonStyleExtender poisonStyleExtender1;
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.ParrotCircleProgressBar parrotCircleProgressBar1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private ReaLTaiizor.Controls.RoyalEllipseButton royalEllipseButton1;
    }
}

