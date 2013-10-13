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

        // Objet pion
        Pion pionSelectionne = null;
        Pion pionAmanger = null;

        // Listes de pions pour les garder
        List<Pion> liste_pion = new List<Pion>();

        // Variable Score
        uint scoreBlack = 0;
        uint scoreWhite = 0;

        Pion.pion_gender player = Pion.pion_gender.White;

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
            draw_cursor(g);

            // Dessine les pions sur le damier
            draw_pions(g);

            draw_informations();
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

        // Dessine un contour de selection
        void draw_cursor(Graphics g)
        {
            if (cursor_x != -1 || cursor_y != -1)
            {
                // Operation ternaire sur le choix de la couleur du curseur
                Color c = (pionSelectionne != null) ? Color.Orange : Color.Yellow ;
                Pen pen = new Pen(c, 3);
                g.DrawRectangle(pen, cursor_x * square_w, cursor_y * square_w, square_w, square_w);
            }
        }

        // Methode qui recupere les coordonnée du curseur
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {   
            // conversion de la position de la souris en position sur le damier
            int x = e.Location.X / 50; 
            int y = e.Location.Y / 50;
            
            if (IsCaseAccessible(x, y))
            {
                // Si aucun pion n'est pas selectionne on copie simplement la position de la souris dans le curseur
                if (pionSelectionne == null)
                {
                    cursor_x = x;
                    cursor_y = y;
                }
                // Si un pion est selectionne on cherche les positions en diagonale de ce pion
                else
                {
                    Pion pion_a_manger = null;
                    bool movable = isMovablePion(pionSelectionne, x, y, out pion_a_manger);
                    if (movable)
                    {
                        cursor_x = x;
                        cursor_y = y;
                        pionAmanger = pion_a_manger;
                    }
                }
            }

            Invalidate();
        }

        // Gere les evenement en lien avec les cliques de la souris
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Si le pion selectionne n'existe pas
            if (pionSelectionne == null) {
                Pion pion = getPion(cursor_x, cursor_y);
                if (pion != null)
                {
                    if (pion.getGender() == player)
                    {
                        pionSelectionne = pion;
                    }
                }
            }
            // Si le pion existe
            else 
            {
                pionSelectionne.setPosition(cursor_x, cursor_y);
                if (pionAmanger != null) {
                    liste_pion.Remove(pionAmanger);
                    Pion pion_a_manger = null;
                    
                    isMovablePion(pionSelectionne, pionSelectionne.getXPosition(), pionSelectionne.getYPosition(), out pion_a_manger);

                    if (pionAmanger.getGender() == Pion.pion_gender.White)
                    {
                        scoreBlack += 1;
                    }
                    else {
                        scoreWhite += 1;
                    }
                }
                if (pionAmanger == null || !EstCeQueLePionPeutManger(pionSelectionne))
                {
                    // On remet le pion a zero
                    pionSelectionne = null;
                    // Operation ternaire sur le changement de couleur de joueur
                    player = (player == Pion.pion_gender.White) ? Pion.pion_gender.Black : Pion.pion_gender.White;
                }
                pionAmanger = null;
            }
            
        }

        // Verifie si la case est noire
        private bool IsCaseAccessible(int x, int y) {
            if (x > 9 || y > 9 || x < 0 || y < 0)
            {
                return false;
            }
            if (y % 2 == 0)
            {
                if (x % 2 != 0)
                {
                    return true;
                }
            }
            else
            {
                if (x % 2 == 0)
                {
                    return true;
                }
            }    
            return false;
        }

        // Fonction récuperation du Pion aux coordonnées demandées
        private Pion getPion(int x, int y){
            foreach (Pion p in liste_pion){
                if(p.getXPosition() == x && p.getYPosition() == y) {
                    return p;
                }
            }
	        return null;
        }

        private void draw_informations() {
            label1.Text = "Score : \n\n   Black : " + scoreBlack + "\n   White : " + scoreWhite + "\n\n\n\n\n" + player + " play\n";
        }

        private bool EstCeQueLePionPeutManger(Pion pion)
        {
            int px = pion.getXPosition();
            int py = pion.getYPosition();
            Pion pion_a_manger = null;

            // Si le pion selectionne peut encore manger
            if (isMovablePion(pion, px - 2, py + 2, out pion_a_manger) && pion_a_manger != null) {
                return true;  
            }
            if (isMovablePion(pion, px + 2, py + 2, out pion_a_manger) && pion_a_manger != null) {
                return true;  
            }
            if (isMovablePion(pion, px + 2, py - 2, out pion_a_manger) && pion_a_manger != null) {
                return true;  
            }
            if (isMovablePion(pion, px - 2, py - 2, out pion_a_manger) && pion_a_manger != null)
            {
                return true;  
            }
            return false;
        }

        private bool isMovablePion(Pion pion, int move_to_x, int move_to_y, out Pion pion_a_manger)
        {
            if (!IsCaseAccessible(move_to_x, move_to_y))
            {
                pion_a_manger = null;
                return false; 
            }
            int px = pion.getXPosition();
            int py = pion.getYPosition();
            // Si le pion selectionne est noir
            if (pion.getGender() == Pion.pion_gender.Black)
            {
                // Pion a gauche
                if ((px - 1) == move_to_x && (py + 1) == move_to_y && getPion(px - 1, py + 1) == null)
                {
                    pion_a_manger = null;
                    return true;
                }
                else if ((px - 2) == move_to_x && (py + 2) == move_to_y &&
                            getPion(px - 1, py + 1) != null &&
                            getPion(px - 1, py + 1).getGender() != pion.getGender() &&
                            getPion(px - 2, py + 2) == null)
                {
                    
                    pion_a_manger = getPion(px - 1, py + 1);
                    return true;
                }

                // Pion a droite
                if ((px + 1) == move_to_x && (py + 1) == move_to_y && getPion(px + 1, py + 1) == null)
                {
                    pion_a_manger = null;
                    return true;
                }
                else if ((px + 2) == move_to_x && (py + 2) == move_to_y &&
                            getPion(px + 1, py + 1) != null &&
                            getPion(px + 1, py + 1).getGender() != pion.getGender() &&
                            getPion(px + 2, py + 2) == null)
                {
                    pion_a_manger = getPion(px + 1, py + 1);
                    return true;
                }
            }
            // Si le pion selectionne est blanc
            if (pion.getGender() == Pion.pion_gender.White)
            {
                // Pion a gauche
                if ((px - 1) == move_to_x && (py - 1) == move_to_y && getPion(px - 1, py - 1) == null)
                {
                    pion_a_manger = null;
                    return true;
                }
                else if ((px - 2) == move_to_x && (py - 2) == move_to_y &&
                            getPion(px - 1, py - 1) != null &&
                            getPion(px - 1, py - 1).getGender() != pion.getGender() &&
                            getPion(px - 2, py - 2) == null)
                {
                    pion_a_manger = getPion(px - 1, py - 1);
                    return true;
                }

                // Pion a droite
                if ((px + 1) == move_to_x && (py - 1) == move_to_y && getPion(px + 1, py - 1) == null)
                {
                    pion_a_manger = null;
                    return true;
                }
                else if ((px + 2) == move_to_x && (py - 2) == move_to_y &&
                            getPion(px + 1, py - 1) != null &&
                            getPion(px + 1, py - 1).getGender() != pion.getGender() &&
                            getPion(px + 2, py - 2) == null)
                {
                    pion_a_manger = getPion(px + 1, py - 1);
                    return true; 
                }
            }
            pion_a_manger = null;
            return false;
        }
    }
}
