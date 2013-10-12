using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private Bitmap _backBuffer;
        int square_w = 50;
        int pion_w = 40;
        int cursor_x, cursor_y;

        List<Pion> liste_pion = new List<Pion>();

        public Form1()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); 
            SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
            cursor_x = -1;
            cursor_y = -1;
            create_pions();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_backBuffer == null) {
                _backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            }
            Graphics g = Graphics.FromImage(_backBuffer);
            // Clear window
            Brush brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, 0, 0, Width, Height);
            // affiche l'ensemble des composants du jeu
            liste_affichage(g);
            g.Dispose();
            //Copy the back buffer to the screen
            e.Graphics.DrawImageUnscaled(_backBuffer, 0, 0);
            //base.OnPaint (e); //optional but not recommended
        }

        protected override void OnClick(EventArgs e)
        {
            //base.OnClick(e);
            MessageBox.Show("abscisse Curseur : " + cursor_x + ", " + "ordonnee Curseur : " + cursor_y);

        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Don't allow the background to paint
        }

        void liste_affichage(Graphics g)
        {
            draw_grid(g);
            draw_cursor();
            draw_pions(g);
        }

        void create_pions()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if ((y % 2) == 0)
                    {
                        liste_pion.Add(new Pion(2 * x + 1, y, pion_w, Pion.pion_gender.Black));
                    }
                    else {
                        liste_pion.Add(new Pion(2 * x, y, pion_w, Pion.pion_gender.Black));
                    }
                }
            }

            for (int y = 6; y < 10; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if ((y % 2) == 0)
                    {
                        liste_pion.Add(new Pion(2 * x + 1, y, pion_w, Pion.pion_gender.White));
                    }
                    else
                    {
                        liste_pion.Add(new Pion(2 * x, y, pion_w, Pion.pion_gender.White));
                    }
                }
            } 
        }

        // quadrillage de la surface de jeu
        void draw_grid(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Black);
            for (int iy = 0; iy < 10; iy++)
            {
                for (int ix = 0; ix < 5; ix++)
                {
                    int x = square_w * 2 * ix + square_w;
                    int y = iy * square_w;
                    if (iy % 2 != 0)
                    {
                        g.FillRectangle(brush, x - square_w, y, square_w, square_w);
                    }
                    else
                    {
                        g.FillRectangle(brush, x, y, square_w, square_w);
                    }
                }
            }
            Pen pen = new Pen(Color.Black);
            g.DrawRectangle(pen, 0, 0, square_w * 10, square_w * 10);
        }

        void draw_pions(Graphics g)
        {  
            foreach (Pion p in liste_pion)
            {
                p.draw(g, square_w);
            }
        }

        void draw_cursor()
        {
            Console.WriteLine("X: " + cursor_x);
            Console.WriteLine("Y: " + cursor_y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Invalidate();
            cursor_x = e.Location.X / 50;
            cursor_y = e.Location.Y / 50;
        }

    }
}
