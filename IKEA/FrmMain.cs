using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IKEA
{
    public partial class IKEA : Form
    {
        private Manager _shopManager;

        public IKEA()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        internal Manager ShopManager { get => _shopManager; set => _shopManager = value; }

        private void IKEA_Load(object sender, EventArgs e)
        {
            this.ShopManager = new Manager(this);
            
        }

        public Panel Create_Checkout(string name, int x, int y)
        {
            Panel p = new Panel();

            p.BackColor = System.Drawing.Color.DarkRed;
            p.Location = new System.Drawing.Point(x, y);
            p.Name = name;
            p.Size = new System.Drawing.Size(50, 50);
            p.TabIndex = 0;

            this.Controls.Add(p);

            return p;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            // EVERY SECOND
            this.ShopManager.Client_Enter();
            this.ShopManager.One_Second_Is_Past();
        }

        private void Refresh_Tick(object sender, EventArgs e)
        {
            // EVERY 0.24 SECOND
            this.ShopManager.Clients_Move();
            Invalidate();
        }

        public static class StaticRandom
        {
            private static int seed;

            private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
                (() => new Random(Interlocked.Increment(ref seed)));

            static StaticRandom()
            {
                seed = Environment.TickCount;
            }

            public static Random Instance { get { return threadLocal.Value; } }
        }

        private void IKEA_Paint(object sender, PaintEventArgs e)
        {
            foreach (Client client in this.ShopManager.Clients)
            {
                SolidBrush br = new SolidBrush(client.MyColor);

                e.Graphics.FillEllipse(br, client.Position.X, client.Position.Y, client.Size, client.Size);
            }

            foreach (Checkout checkout in this.ShopManager.Checkouts)
            {
                foreach (Client client in checkout.Clients)
                {
                    SolidBrush br = new SolidBrush(client.MyColor);

                    e.Graphics.FillEllipse(br, client.Position.X, client.Position.Y, client.Size, client.Size);
                }
            }
        }
    }
}
