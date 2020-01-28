using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IKEA
{
    class Manager
    {
        const int CHECKOUTS_AMOUNT = 9;
        const int MAX_CLIENT_IN_STORE = 100; // 200
        const int MIN_TIME_IN_STORE = 30; // 30
        const int MAX_TIME_IN_STORE = 300; // 300
        const int CLIENT_SIZE = 50;
        const int TIME_TO_OPEN_CHECKOUT = 30; // 30

        private IKEA _mainframe;

        private bool _needNewCheckout = false;
        private int TimeToNextCheckoutOpening = TIME_TO_OPEN_CHECKOUT;

        private List<Checkout> checkouts;
        private List<Client> _clients;

        public Manager(IKEA form)
        {
            this.Mainframe = form;
            this.Checkouts = new List<Checkout>();
            this.Clients = new List<Client>();

            this.Create_Checkouts(60, 730);

            this.Checkouts[0].Open();
        }

        public void Create_Checkouts(int baseX, int baseY)
        {
            int x = baseX;
            int y = baseY;
            int espacement = 20;

            for (int i = 0; i < CHECKOUTS_AMOUNT; i++)
            {
                Panel p = this.Mainframe.Create_Checkout("Caisse " + i, x, y);
                this.Checkouts.Add(new Checkout(p, CLIENT_SIZE));
                x += p.Size.Width + espacement;
            }
        }

        /// <summary>
        /// Création d'un nouveau client dans le magasin si le nombre maximum de clients n'est pas dépassé
        /// </summary>
        public void Client_Enter()
        {
            if (this.Clients.Count < MAX_CLIENT_IN_STORE)
            {
                this.Clients.Add(new Client(IKEA.StaticRandom.Instance.Next(MIN_TIME_IN_STORE, MAX_TIME_IN_STORE + 1), CLIENT_SIZE));
            }
        }

        /// <summary>
        /// Déplace tous les clients du magasin
        /// </summary>
        public void Clients_Move()
        {
            List<Client> ToRemove = new List<Client>();

            foreach (Client client in this.Clients)
            {
                if (client.IsAtCheckout())
                {
                    int index = client.MyCheckout.Clients.IndexOf(client);
                    if (index == -1)
                    {
                        client.MyCheckout.NewClient(client);
                        client.GoToPoint(client.MyCheckout.GetLastPositionOfQueue(), 5, 5);
                    }
                    else
                    {
                        client.GoToPoint(client.MyCheckout.GetPointForClient(client), 5, 5);
                    }

                }

                if(client.Exit){
                    ToRemove.Add(client);
                }
                else
                {
                    if (client.WantToGo() && !client.Done && !client.HasCheckout())
                        client.MyCheckout = this.GetMyCheckout();

                    if (client.WantToGo() && client.HasCheckout() && !client.Done)
                        client.OkToGo();

                    if (client.HasCheckout())
                    {
                        if (client.MyCheckout.IsFull() || !client.MyCheckout.IsOpen)
                        {
                            client.NotGoodToGo();
                        }
                    }
                }

                if (!client.WantToGo())
                {
                    client.Move(new Point(this.Mainframe.Size.Width, this.Mainframe.Size.Height - this.Mainframe.Size.Height / 5));
                }
                else if (client.WantToGo() && client.HasCheckout())
                {
                    client.ComeToCheckout();
                }
                else if (client.WantToGo() && !client.HasCheckout())
                { 
                    client.Move(new Point(this.Mainframe.Size.Width, this.Mainframe.Size.Height - this.Mainframe.Size.Height / 5));
                }
            }

            foreach (Client client1 in ToRemove)
            {
                int index = this.Clients.IndexOf(client1);
                if (index != -1)
                {
                    this.Clients.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// Ce produit à chaque fois qu'une second est passée
        /// </summary>
        public void One_Second_Is_Past()
        {
            foreach (Client client in this.Clients)
                client.OneSecondHasPast();

            foreach (Checkout checkout in this.Checkouts)
            {
                checkout.Second_Past();
            }

            this.OpenningCheckout();
        }

        public void OpenningCheckout()
        {
            if (this.NeedNewCheckout)
            {
                Console.WriteLine(this.TimeToNextCheckoutOpening);
                this.TimeToNextCheckoutOpening--;

                if (this.TimeToNextCheckoutOpening <= 0)
                {
                    this.OpenNextCheckout();
                    this.TimeToNextCheckoutOpening = TIME_TO_OPEN_CHECKOUT;
                }
            }
        }

        public void OpenNextCheckout()
        {
            foreach(Checkout checkout in this.Checkouts)
            {
                if (!checkout.IsOpen)
                {
                    checkout.Open();
                    break;
                }
                    
            }
        }

        /// <summary>
        /// Searches the best avaible Checkout
        /// </summary>
        /// <returns>The best checkout</returns>
        public Checkout GetMyCheckout()
        {
            List<Checkout> openOnes = new List<Checkout>();
            Checkout best = null;


            foreach (Checkout checkout in this.Checkouts)
            {
                if (checkout.IsOpen && !checkout.IsFull())
                {
                    openOnes.Add(checkout);
                }
            }


            foreach (Checkout checkoutopen in openOnes)
            {
                if (best == null)
                {
                    best = checkoutopen;
                }
                else if (checkoutopen.ClientsInQueue() < best.ClientsInQueue())
                {
                    best = checkoutopen;
                }
            }

            if (best == null)
            {
                this.NeedNewCheckout = true;
            }
            else
            {
                this.NeedNewCheckout = false;
            }

            return best;
        }
        public IKEA Mainframe { get => _mainframe; set => _mainframe = value; }
        internal List<Checkout> Checkouts { get => checkouts; set => checkouts = value; }
        internal List<Client> Clients { get => _clients; set => _clients = value; }
        public bool NeedNewCheckout { get => _needNewCheckout; set => _needNewCheckout = value; }
    }
}
