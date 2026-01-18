namespace ByeByeDPI.Views
{
    partial class WebDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebDialog));
            poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(components);
            poisonStyleExtender1 = new ReaLTaiizor.Controls.PoisonStyleExtender(components);
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).BeginInit();
            SuspendLayout();
            // 
            // poisonStyleManager1
            // 
            poisonStyleManager1.Owner = this;
            // 
            // WebDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(540, 420);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(540, 420);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(540, 420);
            Name = "WebDialog";
            ShadowType = ReaLTaiizor.Enum.Poison.FormShadowType.AeroShadow;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Title";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)poisonStyleManager1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.PoisonStyleExtender poisonStyleExtender1;
    }
}