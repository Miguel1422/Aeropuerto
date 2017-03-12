using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aeropuerto
{
    public partial class Form1 : Form
    {
        //ArrayList planes;
        private PrioQueue<Plane> planes;
        private float landX;
        private float landY;
        private int sizePlane;
        public Form1()
        {
            sizePlane = 50;
            InitializeComponent();
            planes = new PrioQueue<Plane>(500);
            landX = landY = 50;
            landX = 100;

            //planes = new ArrayList();
        }

        private void DoubleBufferedPanel1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;

            g.DrawEllipse(Pens.Black, (float)(screen.Width * 0.5), (float)(screen.Height * 0.5) + 100, 10, 10);

            int y = 20;
            foreach (Plane p in planes)
            {
                g.DrawString(p.ToString(), new Font("Arial", 8), Brushes.Black, 5, y);
                y += 10;
                p.Draw(g);

            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            foreach (Plane p in planes)
            {
                p.Update();
                if (p.Fuel <= 10)
                {
                    if (p.Land(landX, landY))
                    {
                        landX += sizePlane;
                        if (landX > Width - sizePlane * 2)
                        {
                            landX = 100;
                            landY += sizePlane + 10;
                        }
                    }
                }
            }

            screen.Invalidate();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            double fuel = double.Parse(textBox2.Text);
            if (planes.Count > 263)
            {
                return;
            }
            planes.Add(new Plane(screen.Width * 0.5, screen.Height * 0.5 + 100, name, fuel));



        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button1_Click(sender, e);
                //textBox2.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }
    }
}

