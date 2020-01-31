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
        private int _timeInStore;
        private int _timeAtCheckout;
        private Color _myColor;

        private Checkout _myCheckout;

        Point _position = new Point(0, 0);

        private int _size;
        private int[] _speed = new int[2];

        private bool done;
        private bool _exit;

        public Client(int tis, int a_size)
        {
            this.TimeInStore = tis;
            this.TimeAtCheckout = tis / 10;
            this.Size = a_size;
            this.Speed[0] = IKEA.StaticRandom.Instance.Next(1, 11);
            this.Speed[1] = IKEA.StaticRandom.Instance.Next(1, 11);
            this.Exit = false;
        }

        public Color TimeColor()
        {
            if (this.TimeInStore > 200)
            {
                return Color.Black;
            }
            else if(this.TimeInStore > 100)
            {
                return Color.Gray;
            }
            else if(this.TimeInStore > 0)
            {
                return Color.LightGray;
            }else if(this.TimeInStore == 0 && !this.HasCheckout())
            {
                return Color.Red;
            }
            else
            {
                return Color.Blue;
            }
        }

        public void Move(Point maxBorder)
        {
            if (this.Position.X + this.Size > maxBorder.X || this.Position.X < 0)
            {
                this.Speed[0] *= -1;
            }

            if (this.Position.Y + this.Size > maxBorder.Y || this.Position.Y < 0)
            {
                this.Speed[1] *= -1;
            }

            this.Position = new Point(this.Position.X + this.Speed[0], this.Position.Y + this.Speed[1]);
        }

        public void ComeToCheckout()
        {
            int distanceToSlowDown = 12;
            Point destination = this.MyCheckout.GetPointForClient(this);
            Point vector = new Point(destination.X - this.Position.X, destination.Y - this.Position.Y);
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

        public void GoToPoint(Point destination, int speedX, int speedY)
        {
            if (this.Position != destination)
            {
                if (speedX < 0)
                    speedX *= -1;

                if (speedY < 0)
                    speedY *= -1;

                if (this.Position.X > destination.X)
                    speedX *= -1;

                if (this.Position.Y > destination.Y)
                    speedY *= -1;

                this.Position = new Point(this.Position.X + speedX, this.Position.Y + speedY);
            }
        }

        public void OneSecondHasPast()
        {
            if (this.TimeInStore > 0)
            {
                this.TimeInStore--;
            }
        }

        public bool WantToGo()
        {
            return (this.TimeInStore == 0);

        }

        public void OkToGo()
        {
            this.Done = true;
        }

        public void NotGoodToGo()
        {
            this.Done = false;
            this.MyCheckout = null;
        }

        public bool HasCheckout()
        {
            return this.MyCheckout != null;
        }

        public bool IsAtCheckout()
        {
            if (this.MyCheckout != null)
                return this.Position == this.MyCheckout.GetPointForClient(this);
            return false;

        }

        public int GetTimeAtCheckout()
        {
            return this.TimeAtCheckout;
        }

        public bool IsWaitingForCheckout()
        {
            return (WantToGo() && !HasCheckout());
        }

        public int TimeInStore { get => _timeInStore; set => _timeInStore = value; }
        public int TimeAtCheckout { get => _timeAtCheckout; set => _timeAtCheckout = value; }
        public Point Position { get => _position; set => _position = value; }
        public Color MyColor { get => this.TimeColor(); set => _myColor = value; }
        public int Size { get => _size; set => _size = value; }
        public int[] Speed { get => _speed; set => _speed = value; }
        internal Checkout MyCheckout { get => _myCheckout; set => _myCheckout = value; }
        public bool Done { get => done; set => done = value; }
        public bool Exit { get => _exit; set => _exit = value; }
    }
}
