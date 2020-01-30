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
            this.lbl_Clients = new System.Windows.Forms.Label();
            this.lbl_checkouts = new System.Windows.Forms.Label();
            this.lbl_Timer = new System.Windows.Forms.Label();
            this.lblClientsToGo = new System.Windows.Forms.Label();
            this.lblAvaibleSpaces = new System.Windows.Forms.Label();
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
            // lbl_Clients
            // 
            this.lbl_Clients.AutoSize = true;
            this.lbl_Clients.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lbl_Clients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Clients.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Clients.Location = new System.Drawing.Point(12, 76);
            this.lbl_Clients.Name = "lbl_Clients";
            this.lbl_Clients.Size = new System.Drawing.Size(193, 29);
            this.lbl_Clients.TabIndex = 0;
            this.lbl_Clients.Text = "Clients : 1 / 200";
            // 
            // lbl_checkouts
            // 
            this.lbl_checkouts.AutoSize = true;
            this.lbl_checkouts.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lbl_checkouts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_checkouts.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_checkouts.Location = new System.Drawing.Point(12, 119);
            this.lbl_checkouts.Name = "lbl_checkouts";
            this.lbl_checkouts.Size = new System.Drawing.Size(305, 29);
            this.lbl_checkouts.TabIndex = 1;
            this.lbl_checkouts.Text = "Caisses ouvertes  : 1 / 12";
            // 
            // lbl_Timer
            // 
            this.lbl_Timer.AutoSize = true;
            this.lbl_Timer.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lbl_Timer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_Timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Timer.Location = new System.Drawing.Point(12, 161);
            this.lbl_Timer.Name = "lbl_Timer";
            this.lbl_Timer.Size = new System.Drawing.Size(334, 29);
            this.lbl_Timer.TabIndex = 2;
            this.lbl_Timer.Text = "Temps avant ouverture : 0 s";
            // 
            // lblClientsToGo
            // 
            this.lblClientsToGo.AutoSize = true;
            this.lblClientsToGo.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lblClientsToGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblClientsToGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientsToGo.Location = new System.Drawing.Point(12, 206);
            this.lblClientsToGo.Name = "lblClientsToGo";
            this.lblClientsToGo.Size = new System.Drawing.Size(277, 29);
            this.lblClientsToGo.TabIndex = 3;
            this.lblClientsToGo.Text = "Clients vers ciasse : xx";
            // 
            // lblAvaibleSpaces
            // 
            this.lblAvaibleSpaces.AutoSize = true;
            this.lblAvaibleSpaces.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lblAvaibleSpaces.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAvaibleSpaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvaibleSpaces.Location = new System.Drawing.Point(12, 252);
            this.lblAvaibleSpaces.Name = "lblAvaibleSpaces";
            this.lblAvaibleSpaces.Size = new System.Drawing.Size(280, 29);
            this.lblAvaibleSpaces.TabIndex = 4;
            this.lblAvaibleSpaces.Text = "Places disponibles : xx";
            // 
            // IKEA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1136, 787);
            this.Controls.Add(this.lblAvaibleSpaces);
            this.Controls.Add(this.lblClientsToGo);
            this.Controls.Add(this.lbl_Timer);
            this.Controls.Add(this.lbl_checkouts);
            this.Controls.Add(this.lbl_Clients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "IKEA";
            this.ShowIcon = false;
            this.Text = "IKEA";
            this.Load += new System.EventHandler(this.IKEA_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.IKEA_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Refresh;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Label lbl_Clients;
        private System.Windows.Forms.Label lbl_checkouts;
        private System.Windows.Forms.Label lbl_Timer;
        private System.Windows.Forms.Label lblClientsToGo;
        private System.Windows.Forms.Label lblAvaibleSpaces;
    }
}

