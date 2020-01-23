namespace IKEA
{
    partial class IKEA
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
            this.Refresh = new System.Windows.Forms.Timer(this.components);
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Refresh
            // 
            this.Refresh.Enabled = true;
            this.Refresh.Interval = 24;
            this.Refresh.Tick += new System.EventHandler(this.Refresh_Tick);
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Interval = 1000;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // IKEA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1136, 787);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "IKEA";
            this.ShowIcon = false;
            this.Text = "IKEA";
            this.Load += new System.EventHandler(this.IKEA_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.IKEA_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Refresh;
        private System.Windows.Forms.Timer MainTimer;
    }
}

