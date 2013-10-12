using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    /// <summary>
    /// Classe representant les pions du jeu
    /// </summary>
    class Pion
    {
        // Enumeration representant la couleur noire et blanche
        public enum pion_gender { White, Black };

        // Coordonnées pion
        private int x;
        private int y;

        // Taille pion
        private int size;

        // Couleur du pion
        private pion_gender gender;

        // Constructeur
        public Pion(int x, int y, int size, pion_gender gender) 
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.gender = gender;
        }

        // Dessin du pion
        public void draw(Graphics g, int square_w)
        {
            Brush brush;

            // Test couleur du pion
            if (gender == pion_gender.White)
            {
                brush = new SolidBrush(Color.Beige);
            }
            else {
                brush = new SolidBrush(Color.Maroon);
            }
            Pen pen = new Pen(Color.Snow);

            // Conversion coordonnées en entier [0-9] --- Centrage du pion dans la case
            int x_px = x * square_w + (square_w - size) / 2;
            int y_px = y * square_w + (square_w - size) / 2;
            // Taille du pion
            int w_px = size;
            int h_px = size;
            // Remplissage couleur
            g.FillEllipse(brush, x_px, y_px, w_px, h_px);
            g.DrawEllipse(pen, x_px, y_px, w_px, h_px);
        }
    }
}
