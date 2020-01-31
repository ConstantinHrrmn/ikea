/* AUTHOR   : Constantin Herrmann
 * DATE     : 31.01.2020
 * PROJECT  : IKEA
 * CLASS    : Client
 * DESC.    : Représente un client
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
    class Client
    {
        #region Variables privées
        private int _timeInStore;
        private int _timeAtCheckout;
        private Color _myColor;

        private Checkout _myCheckout;

        private Point _position = new Point(0, 0);

        private int _size;
        private int[] _speed = new int[2];

        private bool done;
        private bool _exit;
        #endregion

        /// <summary>
        /// Constructeur par défaut d'un client
        /// </summary>
        /// <param name="tis">Le temps que va passer le client dans le magasin</param>
        /// <param name="a_size">sa taille</param>
        public Client(int tis, int a_size)
        {
            this.TimeInStore = tis;
            this.TimeAtCheckout = tis / 10;
            this.Size = a_size;
            this.Speed[0] = IKEA.StaticRandom.Instance.Next(1, 11);
            this.Speed[1] = IKEA.StaticRandom.Instance.Next(1, 11);
            this.Exit = false;
        }

        /// <summary>
        /// Retourne la couleur en fonction du temps que passe le client dans le magasin
        /// </summary>
        /// <returns>La couleur du client</returns>
        public Color TimeColor()
        {
            if (this.TimeInStore > 200)
                return Color.Black;
            else if(this.TimeInStore > 100)
                return Color.Gray;
            else if(this.TimeInStore > 0)
                return Color.LightGray;
            else if(this.TimeInStore == 0 && !this.HasCheckout())
                return Color.Red;
            else
                return Color.Blue;
        }

        /// <summary>
        /// Déplace le client 
        /// </summary>
        /// <param name="maxBorder">les bordures maximales pour le déplacement</param>
        public void Move(Point maxBorder)
        {
            // Si la position X est plus grande que la bordure ou plus petite que 0 alors on inverse la vitesse
            if (this.Position.X + this.Size > maxBorder.X || this.Position.X < 0)
                this.Speed[0] *= -1;

            // Si la position Y est plus grande que la bordure ou plus petite que 0 alors on inverse la vitesse
            if (this.Position.Y + this.Size > maxBorder.Y || this.Position.Y < 0)
                this.Speed[1] *= -1;

            // On met à jour la position
            this.Position = new Point(this.Position.X + this.Speed[0], this.Position.Y + this.Speed[1]);
        }

        /// <summary>
        /// Déplace le client vers sa caisse
        /// </summary>
        public void ComeToCheckout()
        {
            int distanceToSlowDown = 12; // Distance par rapport à la caisse ou le client ralenti
            Point destination = this.MyCheckout.GetPointForClient(this);
            Point vector = new Point(destination.X - this.Position.X, destination.Y - this.Position.Y); // Compare la position du client et celle de sa caisse
            var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);

            if (length < distanceToSlowDown && length > 0)
            {
                Point unitVector = new Point(Convert.ToInt32(vector.X / length), Convert.ToInt32(vector.Y / length));
                this.Position = new Point(this.Position.X + unitVector.X, this.Position.Y + unitVector.Y);
            }
            else if (length >= distanceToSlowDown)
            {
                if (this.Speed[0] < 0)
                    this.Speed[0] *= -1;
                if (this.Speed[1] < 0)
                    this.Speed[1] *= -1;

                Point unitVector = new Point(Convert.ToInt32(vector.X / length), Convert.ToInt32(vector.Y / length));
                this.Position = new Point(this.Position.X + unitVector.X * this.Speed[0], this.Position.Y + unitVector.Y * this.Speed[1]);
            }
        }

        /// <summary>
        /// Fait passer une seconde
        /// </summary>
        public void OneSecondHasPast()
        {
            if (this.TimeInStore > 0)
                this.TimeInStore--;
        }

        /// <summary>
        /// Vérifie si le client souhaites partir du magasin et donc trouver une caisse
        /// </summary>
        /// <returns>True si le client souhaites partir, False si non</returns>
        public bool WantToGo()
        {
            return (this.TimeInStore == 0);

        }

        /// <summary>
        /// Donne l'accord au client qu'il peut y aller
        /// </summary>
        public void OkToGo()
        {
            this.Done = true;
        }

        /// <summary>
        /// Retire l'accord au client d'y aller et supprime sa caisse
        /// </summary>
        public void NotGoodToGo()
        {
            this.Done = false;
            this.MyCheckout = null;
        }

        /// <summary>
        /// Vérifie si le client possède une caisse assignée ou non
        /// </summary>
        /// <returns>True si il possède une caisse, False si non</returns>
        public bool HasCheckout()
        {
            return this.MyCheckout != null;
        }

        /// <summary>
        /// Vérifie si le client ce trouve à sa caisse
        /// </summary>
        /// <returns>True si il est à sa caisse, False si non</returns>
        public bool IsAtCheckout()
        {
            if (this.MyCheckout != null)
                return this.Position == this.MyCheckout.GetPointForClient(this);
            return false;

        }

        /// <summary>
        /// Retourne le temps que va passer le client à la caisse
        /// </summary>
        /// <returns>Le temps que passe le client à la caisse</returns>
        public int GetTimeAtCheckout()
        {
            return this.TimeAtCheckout;
        }

        #region Variables publiques
        public int TimeInStore { get => _timeInStore; set => _timeInStore = value; }
        public int TimeAtCheckout { get => _timeAtCheckout; set => _timeAtCheckout = value; }
        public Point Position { get => _position; set => _position = value; }
        public Color MyColor { get => this.TimeColor(); set => _myColor = value; }
        public int Size { get => _size; set => _size = value; }
        public int[] Speed { get => _speed; set => _speed = value; }
        internal Checkout MyCheckout { get => _myCheckout; set => _myCheckout = value; }
        public bool Done { get => done; set => done = value; }
        public bool Exit { get => _exit; set => _exit = value; }
        #endregion
    }
}
