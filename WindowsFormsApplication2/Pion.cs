using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{
    class Pion
    {
        public enum pion_gender { White, Black };

        private int x;
        private int y;
        private int size;
        private pion_gender gender;


        public Pion(int x, int y, int size, pion_gender gender) 
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.gender = gender;
        }

        public void draw(Graphics g, int square_w)
        {
            Brush brush;
            if (gender == pion_gender.White)
            {
                brush = new SolidBrush(Color.Beige);
            }
            else {
                brush = new SolidBrush(Color.Maroon);
            }
            Pen pen = new Pen(Color.Snow);

            int x_px = x * square_w + (square_w - size) / 2;
            int y_px = y * square_w + (square_w - size) / 2;
            int w_px = size;
            int h_px = size;
            g.FillEllipse(brush, x_px, y_px, w_px, h_px);
            g.DrawEllipse(pen, x_px, y_px, w_px, h_px);
        }
    }
}
