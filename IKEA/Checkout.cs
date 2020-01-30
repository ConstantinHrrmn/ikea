using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IKEA
{
    class Checkout
    {
        private bool _open;
        private int clientSize;
        private Panel _myPanel;

        private const int MAX_CLIENT_IN_QUEUE = 5;
        private int TimeWitchClient = 0;

        private List<Client> _clients = new List<Client>();
        private List<Point> _waitingPoints = new List<Point>();

        public Checkout(Panel a_panel, int a_clientSize)
        {
            this.MyPanel = a_panel;
            this.IsOpen = false;
            this.clientSize = a_clientSize;
            this.MakeWaitingPoints();
        }

        private void MakeWaitingPoints()
        {
            for (int i = 0; i < MAX_CLIENT_IN_QUEUE + 3; i++)
            {
                this.WaitingPoints.Add(new Point(this.MyPanel.Location.X, this.MyPanel.Location.Y - (this.clientSize * i) - 5));
            }    
        }

        public int MaxClientAtCheckout()
        {
            return MAX_CLIENT_IN_QUEUE;
        }

        public bool IsOpen { get => _open; set => _open = value; }
        public Panel MyPanel { get => _myPanel; set => _myPanel = value; }
        public List<Client> Clients { get => _clients; set => _clients = value; }
        public List<Point> WaitingPoints { get => _waitingPoints; set => _waitingPoints = value; }

        public void Close()
        {
            this.MyPanel.BackColor = Color.DarkRed;
            this.IsOpen = false;
        }

        public void Open()
        {
            this.MyPanel.BackColor = Color.Green;
            this.IsOpen = true;
        }

        public void NewClient(Client cl)
        {
            this.Clients.Add(cl);
            //Console.WriteLine(this.ClientsInQueue());
        }

        public void NextClient()
        {
            if (this.Clients.Count >= 1)
                this.Clients.RemoveAt(0);
        }

        public int ClientsInQueue()
        {
            return this.Clients.Count;
        }

        public bool IsFull()
        {
            return this.Clients.Count >= MAX_CLIENT_IN_QUEUE;
        }

        public bool State()
        {
            // Retourne TRUE si la caisse est ouverte
            return this.IsOpen;
        }

        public bool StillHasClients()
        {
            return this.Clients.Count > 0;
        }

        public void Second_Past()
        {
            //this.MoveClients();



            if (this.StillHasClients())
            {
                this.TimeWitchClient++;

                int time = (this.Clients[0].GetTimeAtCheckout() - this.TimeWitchClient);
                //Console.WriteLine(time);

                this.MyPanel.Controls[0].Text = time.ToString();

                if (this.TimeWitchClient >= this.Clients[0].GetTimeAtCheckout())
                {        
                    this.TimeWitchClient = 0;
                    this.Clients[0].Exit = true;
                    this.Clients.Remove(this.Clients[0]);
                    //Console.WriteLine("CLIENT EXIT");
                }
            }
            else
                this.MyPanel.Controls[0].Text = "-";

        }

        public Point GetLastPositionOfQueue()
        {
            int len = this.Clients.Count;
            if (len == 0 )
            {
                return this.WaitingPoints[0];
            }
            else
            {
                return this.WaitingPoints[len];
            }
            
        }

        public void MoveClients()
        {
            foreach (Client client in this.Clients)
            {
                client.ComeToCheckout();
            }
        }

        public Point GetPointForClient(Client client)
        {
            int index = this.Clients.IndexOf(client);
            if (index == -1)
            {
                return this.GetLastPositionOfQueue();
            }
            else
            {
                return this.WaitingPoints[index];
            }
        }
    }
}
