using System;
using System.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Aeropuerto
{
    class Plane : IComparable<Plane>
    {
        private static Image IMG = Image.FromFile(@"./img/plane.png");
        private static Dictionary<int, Bitmap> imageCahe = new Dictionary<int, Bitmap>();
        private Image imgLanded;
        private Image imgLanding;

        private static Random r = new Random();
        private static Font font = new Font("Arial", 8);
        private double x;
        private double y;
        private double xC;
        private double yC;
        private double xF;
        private double yF;
        //private double r;
        private double a;
        private double b;
        private double theta;
        private double ang;
        private string name;
        private double fuel;
        private Vector dir;
        private Vector landSite;
        private bool landing;
        private bool landed;

        private DateTime prev;

        public Plane(double xC, double yC, string name, double fuel)
        {
            this.fuel = fuel;
            this.name = name;
            this.xC = xC;
            this.yC = yC;
            this.landing = false;
            prev = DateTime.Now;
            this.a = 60 * 4;
            this.b = 35 * 4;
            this.ang = r.NextDouble() * 2 * Math.PI;
            this.theta = r.NextDouble() * 2 * Math.PI;
            this.x = 0;
            this.y = 0;
        }

        public void Update()
        {

            if (landing)
            {
                if (Math.Abs(landSite.X - x) < 3 && Math.Abs(landSite.Y - y) < 3)
                {
                    landed = true;
                    return;
                }
                x = x + (dir.X) * 5;
                y = y + (dir.Y) * 5;
                return;
            }
            DateTime now = DateTime.Now;
            TimeSpan elap = now - prev;


            if (elap.TotalSeconds >= 2)
            {
                fuel -= 10;
                prev = now;
            }
            x = xC + a * Math.Cos(theta) * Math.Cos(ang) - b * Math.Sin(theta) * Math.Sin(ang);
            y = yC + a * Math.Cos(theta) * Math.Sin(ang) + b * Math.Sin(theta) * Math.Cos(ang);

            ang += 0.005;
            theta += 0.01;

            if (ang > 2 * Math.PI)
            {
                ang = 0;
            }

            if (theta > 2 * Math.PI)
            {
                theta = 0;
            }

        }

        public bool Land(float x, float y)
        {
            if (landing)
            {
                return false;
            }
            landing = true;
            xF = x;
            yF = y;

            landSite = new Vector(x, y);
            dir = new Vector(x - this.x, y - this.y);
            dir.Normalize();
            return true;
        }

        public double Fuel
        {
            get { return fuel; }
        }

        public string Name
        {
            get { return name; }
        }


        public static Bitmap RotateImage(Image image, float angle)
        {

            if (imageCahe.ContainsKey((int)angle))
            {
                return imageCahe[(int)angle];
            }

            System.Drawing.Point offset = new System.Drawing.Point(image.Width / 2, image.Height / 2);
            if (image == null)
                throw new ArgumentNullException("image");
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics g = Graphics.FromImage(rotatedBmp);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(image, new PointF(0, 0));




            return imageCahe[(int)angle] = rotatedBmp;
        }

        public void Draw(Graphics g)
        {
            float angle;
            if (landed)
            {
                angle = -90;
            }
            else if (landing)
            {
                angle = (float)(Math.Atan2(y - landSite.Y, x - landSite.X) * (180 / Math.PI) + 180);
            }
            else
            {
                angle = (float)(Math.Atan2(y - yC, x - xC) * (180 / Math.PI) + 90);

            }
            Image tmp = RotateImage(IMG, angle);
            g.DrawImage(tmp, (float)x - 10, (float)y - 50, 50, 50);
            g.DrawString(name + " " + fuel, font, Brushes.Black, (float)x, (float)y);
        }

        /*public void Draw(Graphics g)
        {
            float angle;
            if (landed)
            {
                angle = -90;
                if (imgLanded != null)
                {
                    g.DrawImage(imgLanded, (float)x - 10, (float)y - 50, 50, 50);
                    g.DrawString(name + " " + fuel, font, Brushes.Black, (float)x, (float)y);
                    return;
                }
                imgLanded = RotateImage(IMG, angle);
                g.DrawImage(imgLanded, (float)x - 10, (float)y - 50, 50, 50);
                g.DrawString(name + " " + fuel, font, Brushes.Black, (float)x, (float)y);
            }
            else if (landing)
            {
                angle = (float)(Math.Atan2(y - landSite.Y, x - landSite.X) * (180 / Math.PI) + 180);

                if (imgLanding != null)
                {
                    g.DrawImage(imgLanding, (float)x - 10, (float)y - 50, 50, 50);
                    g.DrawString(name + " " + fuel, font, Brushes.Black, (float)x, (float)y);
                    return;
                }
                imgLanding = RotateImage(IMG, angle);
                g.DrawImage(imgLanding, (float)x - 10, (float)y - 50, 50, 50);
                g.DrawString(name + " " + fuel, font, Brushes.Black, (float)x, (float)y);
            }
            else
            {
                angle = (float)(Math.Atan2(y - yC, x - xC) * (180 / Math.PI) + 90);
                Image tmp = RotateImage(IMG, angle);
                g.DrawImage(tmp, (float)x - 10, (float)y - 50, 50, 50);
                g.DrawString(name + " " + fuel, font, Brushes.Black, (float)x, (float)y);
            }
            //g.DrawEllipse(Pens.White, (float)x, (float)y, 20, 20);
        }*/



        override public string ToString()
        {
            if (landed)
            {
                return name + " aterrizó";
            }
            if (landing)
            {
                return name + " aterrizando";
            }

            return name + " combustible: " + fuel;
        }

        public int CompareTo(Plane other)
        {

            if (fuel > other.fuel)
            {
                return 1;
            }
            else if (fuel < other.fuel)
            {
                return -1;
            }
            return 0;
        }
    }
}
