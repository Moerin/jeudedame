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
        int square_w = 50;
        int pion_w = 40;

        List<Pion> liste_pion = new List<Pion>();

        public Form1()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); 
            SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
            panel1.Paint += new PaintEventHandler(panel_Paint);
            create_pions();
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

        void panel_Paint(object sender, PaintEventArgs e) {
            using (Graphics g = this.panel1.CreateGraphics())
            {
                draw_grid(g);
                draw_cursor();
                draw_pions(g);
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
                    int y = iy * 50;
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
            Console.WriteLine("***");
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            panel1.Invalidate();
            Console.WriteLine("x. " + Location.X.ToString());
            Console.WriteLine("y. " + Location.Y.ToString());
        }

    }
}
