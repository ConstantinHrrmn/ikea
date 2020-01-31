/* AUTHOR   : Constantin Herrmann
 * DATE     : 31.01.2020
 * PROJECT  : IKEA
 * CLASS    : Checkout
 * DESC.    : Représente une caisse
 */

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
        #region Variables privées
        private bool _open;
        private int clientSize;
        private Panel _myPanel;

        private const int MAX_CLIENT_IN_QUEUE = 5;
        private int TimeWitchClient = 0;

        private List<Client> _clients = new List<Client>();
        private List<Point> _waitingPoints = new List<Point>();
        #endregion

        #region Variables publiques
        public bool IsOpen { get => _open; set => _open = value; }
        public Panel MyPanel { get => _myPanel; set => _myPanel = value; }
        public List<Client> Clients { get => _clients; set => _clients = value; }
        public List<Point> WaitingPoints { get => _waitingPoints; set => _waitingPoints = value; }
        #endregion

        /// <summary>
        /// Constructeur par défaut d'un caisse
        /// </summary>
        /// <param name="a_panel">Son panel (qui doit être créer en amont</param>
        /// <param name="a_clientSize">la taille des clients qui viennent</param>
        public Checkout(Panel a_panel, int a_clientSize)
        {
            this.MyPanel = a_panel;
            this.IsOpen = false;
            this.clientSize = a_clientSize;
            this.MakeWaitingPoints();
        }

        /// <summary>
        /// Création des coordonnées pour la file d'attente devant la caisse
        /// </summary>
        private void MakeWaitingPoints()
        {
            for (int i = 0; i < MAX_CLIENT_IN_QUEUE + 3; i++)
                this.WaitingPoints.Add(new Point(this.MyPanel.Location.X, this.MyPanel.Location.Y - (this.clientSize * i) - 5));
        }

        /// <summary>
        /// Retourne le nombre de clients maximum qu'il peut y avoir à la caisse
        /// </summary>
        /// <returns>le nombre maximum de clients à la caisse</returns>
        public int MaxClientAtCheckout()
        {
            return MAX_CLIENT_IN_QUEUE;
        }

        /// <summary>
        /// Ferme la caisse
        /// </summary>
        public void Close()
        {
            this.MyPanel.BackColor = Color.DarkRed;
            this.IsOpen = false;
        }

        /// <summary>
        /// Ouvre la caisse
        /// </summary>
        public void Open()
        {
            this.MyPanel.BackColor = Color.Green;
            this.IsOpen = true;
        }

        /// <summary>
        /// Ajout un nouveau client à la file d'attente de la caisse
        /// </summary>
        /// <param name="cl">Le client à ajouter</param>
        public void NewClient(Client cl)
        {
            this.Clients.Add(cl);
        }

        /// <summary>
        /// Le nombre de clients dans la file d'attente
        /// </summary>
        /// <returns>le nombre de clients dans la file d'attente</returns>
        public int ClientsInQueue()
        {
            return this.Clients.Count;
        }

        /// <summary>
        /// Vérifie si la caisse est pleine ou pas
        /// </summary>
        /// <returns>True si la caisse est pleine, False si non</returns>
        public bool IsFull()
        {
            return this.Clients.Count >= MAX_CLIENT_IN_QUEUE;
        }

        /// <summary>
        /// Ce produit chaque seconde
        /// </summary>
        public void Second_Past()
        {
            // Vérifie si il y a encore des clients dans la queue
            if (this.ClientsInQueue() > 0)
            {
                this.TimeWitchClient++;

                int time = (this.Clients[0].GetTimeAtCheckout() - this.TimeWitchClient);
                this.MyPanel.Controls[0].Text = time.ToString();

                if (this.TimeWitchClient >= this.Clients[0].GetTimeAtCheckout())
                {        
                    this.TimeWitchClient = 0;
                    this.Clients[0].Exit = true;
                    this.Clients.Remove(this.Clients[0]);
                }
            }
            else
                this.MyPanel.Controls[0].Text = "-";

        }

        /// <summary>
        /// Recupère la dernière position de la file d'attente disponbile
        /// </summary>
        /// <returns>La dernière position</returns>
        public Point GetLastPositionOfQueue()
        {
            int len = this.Clients.Count;
            if (len == 0 )
                return this.WaitingPoints[0];
            else
                return this.WaitingPoints[len];
        }

        /// <summary>
        /// Fait venir tous les clietns en direction de la caisse
        /// </summary>
        public void MoveClients()
        {
            foreach (Client client in this.Clients)
                client.ComeToCheckout();
        }

        /// <summary>
        /// La position dans la queue destinée au client
        /// </summary>
        /// <param name="client">le client</param>
        /// <returns>la position sous forme de Point</returns>
        public Point GetPointForClient(Client client)
        {
            int index = this.Clients.IndexOf(client);
            if (index == -1)
                return this.GetLastPositionOfQueue();
            else
                return this.WaitingPoints[index];
        }
    }
}
