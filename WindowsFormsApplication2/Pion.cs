﻿using System;
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

        // Getter sur la position du pion
        public int getXPosition(){
            return this.x;
        }

        public int getYPosition()
        {
            return this.y;
        }

        // Setter sur la postion du pion
        public void setXPosition(int x) {
            this.x = x;
        }

        public void setYPosition(int y) {
            this.y = y;
        }

        public void setPosition(int x, int y)
        {
            this.y = y;
            this.x = x;
        }

        public pion_gender getGender() {
            return this.gender;
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

            // Centrage du pion dans la case
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
