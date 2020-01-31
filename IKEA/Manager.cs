/* AUTHOR   : Constantin Herrmann
 * DATE     : 31.01.2020
 * PROJECT  : IKEA
 * CLASS    : Manager
 * DESC.    : Cette classe permet de gérer à la fois les clients et les caisses.
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
    class Manager
    {
        #region Variables

        #region Constantes
        const int CHECKOUTS_AMOUNT = 12;
        int MAX_CLIENT_IN_STORE = 100; // 200 Ce n'est pas un constante car la valeur change au fil des heures
        const int MIN_TIME_IN_STORE = 30; // 30
        const int MAX_TIME_IN_STORE = 300; // 300
        const int CLIENT_SIZE = 50;
        const int TIME_TO_OPEN_CHECKOUT = 30; // 30
        const int SECONDS_IN_DAY = 300; // = 4,8 minutes
        #endregion

        #region Variables privées
        private IKEA _mainframe;
        private bool _needNewCheckout = false;
        private int _timeToNextCheckoutOpening = TIME_TO_OPEN_CHECKOUT;
        private int _waitingForCheckout = 0;
        private List<Checkout> checkouts;
        private List<Client> _clients;
        #endregion

        #endregion

        /// <summary>
        /// Constructeur du MAnager
        /// </summary>
        /// <param name="form">La form qu'il dois gérer</param>
        public Manager(IKEA form)
        {
            this.Mainframe = form;
            this.Checkouts = new List<Checkout>();
            this.Clients = new List<Client>();

            // Création de la première caisse
            this.Create_Checkouts(60, 730);

            // Ouverture par défaut de la première caisse
            this.Checkouts[0].Open();
        }

        /// <summary>
        /// Création de toutes les autres caisses
        /// Les autres caisses seront décallées sur la droite par rapport à la caisse principale
        /// </summary>
        /// <param name="baseX">Position X de la caisse principale</param>
        /// <param name="baseY">Position Y de la caisse principale</param>
        public void Create_Checkouts(int baseX, int baseY)
        {
            int x = baseX;
            int y = baseY;
            int espacement = 20;    // espacement entre les caisses

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
                this.Clients.Add(new Client(IKEA.StaticRandom.Instance.Next(MIN_TIME_IN_STORE, MAX_TIME_IN_STORE + 1), CLIENT_SIZE));
        }

        /// <summary>
        /// Récuérer la quantité maximum de clients qu'il peut y avoir dans le magasin
        /// </summary>
        /// <returns>La quantité de clients maximum</returns>
        public int GetMaxClients()
        {
            return MAX_CLIENT_IN_STORE;
        }

        /// <summary>
        /// Compte le nombre de caisse qui sont ouvertes
        /// </summary>
        /// <returns>Le nombre de caisses ouvertes</returns>
        public int CountOpenCheckout()
        {
            int amount = 0;

            foreach (Checkout checkout in this.Checkouts)
                amount += checkout.IsOpen ? 1 : 0;

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
                if (client.IsAtCheckout())  // On vérifie si il n'est pas à sa caisse
                {
                    client.MyCheckout.NewClient(client);    // On délègue le client à la caisse
                    ToRemove.Add(client);                   // On supprime le client du magasin car celui-ci est gérer par la caisse
                }
                else
                {
                    // Si le client à envie de partir (aller à la caisse) et que celui-ci n'as pas fini d'être dans le magasin (passer en caisse) et qu'il n'a pas de caisse
                    if (client.WantToGo() && !client.Done && !client.HasCheckout())
                        client.MyCheckout = this.GetMyCheckout(client); // On lui attribue la caisse la plus proche avec le mons de monde possible

                    if (client.WantToGo() && client.HasCheckout() && !client.Done)
                        client.OkToGo();

                    if (client.HasCheckout())
                    {
                        // Si la caisse du client est complète ou n'est plus ouverte, il dois en trouver une autre
                        if (client.MyCheckout.IsFull() || !client.MyCheckout.IsOpen)
                            client.NotGoodToGo();
                    }
                }

                // Tant que le client ne souhaites pas partir, on le fait ce déplacer dans le magasin
                if (!client.WantToGo())
                {
                    client.Move(new Point(this.Mainframe.Size.Width, this.Mainframe.Size.Height - this.Mainframe.Size.Height / 5));
                }

                // Si le client souhaites partir et qu'il as une caisse, on le fait y aller
                else if (client.WantToGo() && client.HasCheckout())
                {
                    client.ComeToCheckout();
                }

                // Si le client souhaites partir mais qu'il n'as pas de caisse, on le fait attendre 
                else if (client.WantToGo() && !client.HasCheckout())
                { 
                    client.Move(new Point(this.Mainframe.Size.Width, this.Mainframe.Size.Height - this.Mainframe.Size.Height / 5));
                }
            }

            // On supprime les clients mis dans le cache de suppression
            foreach (Client client1 in ToRemove)
                this.Clients.Remove(client1);

            // On vide la liste cache des clients a supprimer
            ToRemove.Clear();
        }

        /// <summary>
        /// Ce produit à chaque fois qu'une second est passée
        /// </summary>
        public void One_Second_Is_Past()
        {
            // On dis à chaque client qu'une seconde à passé
            foreach (Client client in this.Clients)
                client.OneSecondHasPast();

            // On dis à chaque caisse qu'une seconde à passé
            foreach (Checkout checkout in this.Checkouts)
                checkout.Second_Past();

            // On vérifie si il faut fermer des caisses
            this.CloseCheckout();

            // On vérifie également si il ne faut en ouvrir
            this.OpenningCheckout();

        }

        /// <summary>
        /// Ferme la caisse avec le moins de monde si la situation le lui permet
        /// </summary>
        public void CloseCheckout()
        {
            // On verifie que le nombre de places - 8 (5 pour la caisse que nous allons fermer et 3 par sécurité) 
            // est plus grand que le nombre de client en attente d'une caisse
            if (this.CountClientsWaiting().avaibleSpaces-8 > this.CountClientsWaiting().waiting  && this.CountOpenCheckout() > 1)
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

        /// <summary>
        ///     Met à jour le nombre de clients qui attendent pour une caisse
        /// </summary>
        public void UpdateWainting()
        {
            this.WaitingForCheckout = CountClientsWaiting().waiting;
        }

        /// <summary>
        /// Compte le nombre de clients qui attendent pour une caisse
        /// </summary>
        /// <returns>
        /// waiting : le nombre de personnes qui attentend une caisse
        /// avaibleSpaces : le nombre de places disponibles dans toutes les caisses
        /// amount : le nombre de personnes qui souhaites partir du magasin
        /// </returns>
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

        /// <summary>
        /// Commence le procéssus d'ouverture de caisse
        /// Cette fonction dois être appellée chaque seconde afin d'atteindre le temps d'ouverture d'une caisse pour que la caisse s'ouvre
        /// </summary>
        public void OpenningCheckout()
        {
            if (this.WaitingForCheckout > 0)
            {
                this.TimeToNextCheckoutOpening--;

                if (this.TimeToNextCheckoutOpening <= 0)
                {
                    // On verifie le nombre de caisses qu'il faut ouvrir
                    int amount = this.WaitingForCheckout / 5;

                    if (amount > 1)
                    {
                        for (int i = 0; i < amount; i++)
                            this.OpenNextCheckout();
                    }
                    else
                        this.OpenNextCheckout();

                    this.TimeToNextCheckoutOpening = TIME_TO_OPEN_CHECKOUT;
                }
            }
        }

        /// <summary>
        /// Ouvre une caisse si il en reste de fermer
        /// </summary>
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

        /// <summary>
        /// Change le nombre de personne maximales dans le magasin en fonction de l'heure de la journée (0 à 10) 0 = 9h
        /// </summary>
        /// <param name="x">l'heure actuelle</param>
        public void ChangeAffluence(int x)
        {
            int[] horaires = { 30, 40, 55, 80, 65, 50, 70, 90, 100, 60, 30 };

            if (x >= 0 && x < horaires.Length)
                this.MAX_CLIENT_IN_STORE = horaires[x];
            else
                this.MAX_CLIENT_IN_STORE = 0;
        }

        #region Variables public
        public IKEA Mainframe { get => _mainframe; set => _mainframe = value; }
        internal List<Checkout> Checkouts { get => checkouts; set => checkouts = value; }
        internal List<Client> Clients { get => _clients; set => _clients = value; }
        public bool NeedNewCheckout { get => _needNewCheckout; set => _needNewCheckout = value; }
        public int WaitingForCheckout { get => _waitingForCheckout; set => _waitingForCheckout = value; }
        public int TimeToNextCheckoutOpening { get => _timeToNextCheckoutOpening; set => _timeToNextCheckoutOpening = value; }
        #endregion
    }
}
