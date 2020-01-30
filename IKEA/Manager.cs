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
        const int CHECKOUTS_AMOUNT = 12;
        const int MAX_CLIENT_IN_STORE = 100; // 200
        const int MIN_TIME_IN_STORE = 30; // 30
        const int MAX_TIME_IN_STORE = 300; // 300
        const int CLIENT_SIZE = 50;
        const int TIME_TO_OPEN_CHECKOUT = 30; // 30
        const int SECONDS_IN_DAY = 300; // = 4,8 minutes


        private IKEA _mainframe;

        private bool _needNewCheckout = false;
        public int TimeToNextCheckoutOpening = TIME_TO_OPEN_CHECKOUT;

        private int _waitingForCheckout = 0;

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

        public int GetMaxClients()
        {
            return MAX_CLIENT_IN_STORE;
        }

        public int CountOpenCheckout()
        {
            int amount = 0;

            foreach (Checkout checkout in this.Checkouts)
            {
                amount += checkout.IsOpen ? 1 : 0;
            }

            return amount;
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
                    client.MyCheckout.NewClient(client);
                    ToRemove.Add(client);
                    /*int index = client.MyCheckout.Clients.IndexOf(client);
                    if (index == -1)
                    {
                        client.MyCheckout.NewClient(client);
                        client.GoToPoint(client.MyCheckout.GetLastPositionOfQueue(), 5, 5);
                    }
                    else
                    {
                        client.GoToPoint(client.MyCheckout.GetPointForClient(client), 5, 5);
                    }*/

                }
                else
                {
                    if (client.WantToGo() && !client.Done && !client.HasCheckout())
                        client.MyCheckout = this.GetMyCheckout(client);

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
                this.Clients.Remove(client1);
            }

            ToRemove.Clear();
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

            this.CloseCheckout();
            this.OpenningCheckout();

        }

        public void CloseCheckout()
        {
            if (this.CountClientsWaiting().avaibleSpaces > this.CountClientsWaiting().waiting  && this.CountOpenCheckout() > 1)
            {
                Checkout bestCheckout = null;
                foreach (Checkout check in this.Checkouts)
                {
                    if (check.IsOpen)
                    {
                        if (bestCheckout == null)
                            bestCheckout = check;

                        if (check.ClientsInQueue() < bestCheckout.ClientsInQueue())
                            bestCheckout = check;
                    }

                }

                int index = this.Checkouts.IndexOf(bestCheckout);
                this.Checkouts[index].Close();
            }
        }

        public void UpdateWainting()
        {
            this.WaitingForCheckout = CountClientsWaiting().waiting;
        }

        public (int waiting, int avaibleSpaces, int amount) CountClientsWaiting()
        {
            int amount = 0;
            int avaibleSpaces = 0;
            foreach (Checkout checkout in this.Checkouts)
            {
                if (checkout.Clients.Count < checkout.MaxClientAtCheckout() && checkout.IsOpen)
                    avaibleSpaces += (checkout.MaxClientAtCheckout() - checkout.Clients.Count);
            }

            foreach (Client client in this.Clients)
            {
                if (client.WantToGo())
                    amount++;
            }

            //Console.WriteLine("Clients to go : {0} || Avaible spaces : {1}", amount, avaibleSpaces);

            return (amount - avaibleSpaces, avaibleSpaces, amount);
        }

        public void OpenningCheckout()
        {
            if (this.WaitingForCheckout > 0)
            {
                //Console.WriteLine(this.TimeToNextCheckoutOpening);
                this.TimeToNextCheckoutOpening--;

                if (this.TimeToNextCheckoutOpening <= 0)
                {
                    int amount = this.WaitingForCheckout / 5;

                    if (amount > 1)
                    {
                        for (int i = 0; i < amount; i++)
                            this.OpenNextCheckout();
                    }
                    else
                    {
                        this.OpenNextCheckout();
                    }
                    

                    this.TimeToNextCheckoutOpening = TIME_TO_OPEN_CHECKOUT;
                }
            }
        }

        public void OpenNextCheckout()
        {
            List<Checkout> closed = new List<Checkout>();
            foreach(Checkout checkout in this.Checkouts)
            {
                if (!checkout.IsOpen)
                    closed.Add(checkout); 
            }

            if (closed.Count > 0)
                closed[0].Open();
        }

        /// <summary>
        /// Searches the best avaible Checkout
        /// </summary>
        /// <returns>The best checkout</returns>
        public Checkout GetMyCheckout(Client client)
        {
            List<Checkout> openOnes = new List<Checkout>();
            Checkout best = null;
            int bestdistance = -1;

            foreach (Checkout checkout in this.Checkouts)
            {
                if (checkout.IsOpen && !checkout.IsFull())
                {
                    openOnes.Add(checkout);
                }
            }


            foreach (Checkout checkoutopen in openOnes)
            {
                int distance = Convert.ToInt32(Math.Sqrt(Math.Pow(checkoutopen.GetLastPositionOfQueue().X - client.Position.X, 2) + Math.Pow(checkoutopen.GetLastPositionOfQueue().Y - client.Position.Y, 2)));
                if (best == null && bestdistance == -1)
                {
                    best = checkoutopen;
                    bestdistance = distance;
                }
                else if (checkoutopen.ClientsInQueue() <= best.ClientsInQueue() && distance < bestdistance)
                {
                    best = checkoutopen;
                    bestdistance = distance;
                }
            }
            return best;
        }

        public int affluence(int x)
        {
            return (x - 2) * (x - 1) * (x + 1) * (x + 2);
        }
        public IKEA Mainframe { get => _mainframe; set => _mainframe = value; }
        internal List<Checkout> Checkouts { get => checkouts; set => checkouts = value; }
        internal List<Client> Clients { get => _clients; set => _clients = value; }
        public bool NeedNewCheckout { get => _needNewCheckout; set => _needNewCheckout = value; }
        public int WaitingForCheckout { get => _waitingForCheckout; set => _waitingForCheckout = value; }
    }
}
