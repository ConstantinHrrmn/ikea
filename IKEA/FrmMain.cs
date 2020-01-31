/* AUTHOR   : Constantin Herrmann
 * DATE     : 31.01.2020
 * PROJECT  : IKEA
 * CLASS    : FrmMain
 * DESC.    : La form de l'application
 */

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
        #region Variables privées
        private Manager _shopManager;
        int hours = 0;
        int minutes = 0;
        #endregion

        /// <summary>
        /// Constructeur par défaut de la form
        /// </summary>
        public IKEA()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        internal Manager ShopManager { get => _shopManager; set => _shopManager = value; }

        private void IKEA_Load(object sender, EventArgs e)
        {
            this.ShopManager = new Manager(this);
            this.ShopManager.ChangeAffluence(this.hours);
        }

        /// <summary>
        /// Creation du panel qui sera utiliser pour le checkout
        /// </summary>
        /// <param name="name">Le nom du panel</param>
        /// <param name="x">La position x</param>
        /// <param name="y">La position y</param>
        /// <returns></returns>
        public Panel Create_Checkout(string name, int x, int y)
        {
            Panel p = new Panel();

            p.BackColor = System.Drawing.Color.DarkRed;
            p.Location = new System.Drawing.Point(x, y);
            p.Name = name;
            p.Size = new System.Drawing.Size(50, 50);
            p.TabIndex = 0;

            Label lbl = new Label();
            lbl.Text = "-";
            lbl.Font = new Font(FontFamily.GenericSansSerif, 18);
            lbl.ForeColor = Color.White;

            p.Controls.Add(lbl);

            this.Controls.Add(p);

            return p;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            // Chaque seconde 
            this.ShopManager.One_Second_Is_Past(); // On averti le manager qu'une seconde à passer

            this.minutes++;
            if (this.minutes > 59)
            {
                this.minutes = 0;
                this.hours++;
            }

            if (this.hours > 10)
                this.hours = 0;

            this.ShopManager.ChangeAffluence(this.hours);   // Recherche l'affluence dans le magasin d'après l'heure
            this.lblClock.Text = string.Format("{0}:{1}", this.hours + 9, this.minutes);
        }

        private void Refresh_Tick(object sender, EventArgs e)
        {
            // Chaque 0.24 Secondes

            this.ShopManager.UpdateWainting();  // Met à jour la liste d'attente des clients
            this.ShopManager.Clients_Move(); // Fait déplacer tous les clients

            foreach (Checkout checkout in this.ShopManager.Checkouts)
                checkout.MoveClients();

            this.lbl_Clients.Text = string.Format("Clients: {0} / {1}", this.ShopManager.Clients.Count, this.ShopManager.GetMaxClients());
            this.lbl_checkouts.Text = string.Format("Caisse: {0} / {1}",this.ShopManager.CountOpenCheckout(), this.ShopManager.Checkouts.Count);
            this.lbl_Timer.Text = string.Format("Temps avant ouverture : {0} s", this.ShopManager.TimeToNextCheckoutOpening);
            this.lblClientsToGo.Text = string.Format("Clients sans caisse : {0}", this.ShopManager.CountClientsWaiting().amount);
            this.lblAvaibleSpaces.Text = string.Format("Places disponibles : {0}", this.ShopManager.CountClientsWaiting().avaibleSpaces);
            
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
            // Dessiner tous les clients sur la form
            foreach (Client client in this.ShopManager.Clients)
            {
                SolidBrush br = new SolidBrush(client.MyColor);

                e.Graphics.FillEllipse(br, client.Position.X, client.Position.Y, client.Size, client.Size);
            }

            // Dessiner tous les clients dans la file d'attente des caisses
            foreach (Checkout checkout in this.ShopManager.Checkouts)
            {
                foreach (Client client in checkout.Clients)
                {
                    SolidBrush br = new SolidBrush(client.MyColor);

                    e.Graphics.FillEllipse(br, client.Position.X, client.Position.Y, client.Size, client.Size);
                }
            }
        }

        private void TClientSpawner_Tick(object sender, EventArgs e)
        {
            this.ShopManager.Client_Enter();
        }
    }
}
