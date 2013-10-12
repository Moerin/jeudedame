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
        // Buffer pour le double buffering et eviter le flickering a chaque evenement
        private Bitmap _backBuffer;

        // Taille d'une case
        int square_w = 50;
        // Taille d'un pion
        int pion_w = 40;
        // Coordonnees
        int cursor_x, cursor_y;

        // Listes de pions pour les garder
        List<Pion> liste_pion = new List<Pion>();

        // La fenetre principale
        public Form1()
        {
            // Double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); 
            SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();

            // ???
            cursor_x = -1;
            cursor_y = -1;

            // Appel de la focntion creations de pions
            create_pions();
        }

        // Dessine les elements sur le plateau de jeu
        protected override void OnPaint(PaintEventArgs e)
        {
            // Singleton du buffer pour le double buffering
            if (_backBuffer == null) {
                _backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            }

            // Objet graphique pour dessiner l'image
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

        // Gere les evenement en lien avec les cliques de la souris
        protected override void OnClick(EventArgs e)
        {
            //base.OnClick(e);
            
            // TODO: Test position pion et position curseur
            foreach (Pion p in liste_pion)
            {
                Pion newPion = new Pion(cursor_x, cursor_y, square_w, Pion.pion_gender.Black);
                
                if (cursor_x == p.getXPosition() && cursor_y == p.getYPosition())
                {
                    liste_pion.Remove(p);
                }
                else { 
                    
                }
            }

            // Boite de message qui affiche les coordonnes du curseur
            //MessageBox.Show("abscisse Curseur : " + cursor_x + ", " + "ordonnee Curseur : " + cursor_y);

        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Don't allow the background to paint
        }

        // Affiche tous les elements qui doivent etre present sur la fenetre
        void liste_affichage(Graphics g)
        {
            // Dessine les cases
            draw_grid(g);

            // Affiche les coordonnees du curseur dans la boite de dialogue
            draw_cursor();

            // Dessine les pions sur le damier
            draw_pions(g);
        }

        // Creation des pions
        void create_pions()
        {
            // Boucle représentant le parcours des cases en colonne
            for (int y = 0; y < 4; y++)
            {
                // Boucle représentant le parcours des cases en ligne
                for (int x = 0; x < 5; x++)
                {
                    // Test si les lignes du damier sont paires et si c'est le cas les pions seront décalés d'une case
                    if ((y % 2) == 0)
                    {
                        // Ajout des pions noirs
                        liste_pion.Add(new Pion(2 * x + 1, y, pion_w, Pion.pion_gender.Black));
                    }
                    // Sinon ils ne seront pas décalés
                    else {
                        // Ajout des pions noirs
                        liste_pion.Add(new Pion(2 * x, y, pion_w, Pion.pion_gender.Black));
                    }
                }
            }

            // Boucle représentant le parcours des cases en colonne
            for (int y = 6; y < 10; y++)
            {

                // Boucle représentant le parcours des cases en ligne
                for (int x = 0; x < 5; x++)
                {
                    // Test si les lignes du damier sont paires et si c'est le cas les pions seront décalés d'une case
                    if ((y % 2) == 0)
                    {
                        // Ajout des pions blancs
                        liste_pion.Add(new Pion(2 * x + 1, y, pion_w, Pion.pion_gender.White));
                    }
                    // Sinon ils ne seront pas décalés
                    else
                    {
                        // Ajout des pions blancs
                        liste_pion.Add(new Pion(2 * x, y, pion_w, Pion.pion_gender.White));
                    }
                }
            } 
        }

        // Quadrillage de la surface de jeu
        void draw_grid(Graphics g)
        {
            // ???
            Brush brush = new SolidBrush(Color.Black);

            // Boucle qui parcours le damier en colonne
            for (int iy = 0; iy < 10; iy++)
            {
                // Boucle qui parcours le damier en ligne
                for (int ix = 0; ix < 5; ix++)
                {
                    // Ajout des case noires
                    int x = square_w * 2 * ix + square_w;
                    int y = iy * square_w;
                    
                    // si le chiffre de la colonne est pair il y a decalage
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

            // ???
            Pen pen = new Pen(Color.Black);
            g.DrawRectangle(pen, 0, 0, square_w * 10, square_w * 10);
        }

        // Dessin des pions
        void draw_pions(Graphics g)
        {  
            foreach (Pion p in liste_pion)
            {
                p.draw(g, square_w);
            }
        }

        // Affichage les coordonnées du curseur dans la boite de dialogue
        void draw_cursor()
        {
            Console.WriteLine("X: " + cursor_x);
            Console.WriteLine("Y: " + cursor_y);
        }

        // Methode qui recupere les coordonnée du curseur
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Invalidate();
            cursor_x = e.Location.X / 50;
            cursor_y = e.Location.Y / 50;
        }

    }
}
