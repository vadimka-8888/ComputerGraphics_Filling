using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics_3
{

    public partial class RecurA : Form
    {
        private List<List<Point>> border;
        private List<Point> col_pix;
        private SolidBrush solid;
        private Pen pen;
        private bool cont;
        private bool draw_border;
        private bool fill;
        private bool fill_with_photo;

        public RecurA()
        {
            InitializeComponent();
            this.solid = new SolidBrush(Color.Red);
            this.pen = new Pen(Color.Black);
            this.border = new List<List<Point>>();
            this.col_pix = new List<Point>();
            this.cont = false;
            this.draw_border = true;
            this.fill = false;
            this.fill_with_photo = false;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            pictureBox1.Paint += new PaintEventHandler(DrawPoint);
            this.border.Add(new List<Point>());
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.draw_border) return;
            if (!cont)
            {
                List<Point> current_line = new List<Point>();
                this.border.Add(current_line);
            }
            if (Control.MouseButtons == MouseButtons.Left)
            {
                Point pixel = new Point();
                pixel.x = e.X;
                pixel.y = e.Y;
                this.border.Last().Add(pixel);
                pictureBox1.Refresh();
                this.cont = true;
            }
            //if (pictureBox1.Image == null) label1.Text = "777";
        }

        private void DrawImagePointF(object sender, PaintEventArgs e)
        {

            // Create image.
            Image image = Image.FromFile("SampImag.jpg");

            // Create point for upper-left corner of image.
            PointF ulCorner = new PointF(100.0F, 100.0F);

            // Draw image to screen.
            e.Graphics.DrawImage(image, ulCorner);
        }

        private void DrawPoint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(((PictureBox)sender).Image);
            List<Point> line = this.border.Last();
            for (int i = 1; i < line.Count; i++)
            {
                g.DrawLine(this.pen, line[i - 1].x, line[i - 1].y, line[i].x, line[i].y);
            }
            //if (pictureBox1.Image == null) label1.Text = "777"; else label1.Text = "666";

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.cont = false;
        }

        private void PictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (this.fill)
            {
                Bitmap bmp = new Bitmap(((PictureBox)sender).Image);
                FillRec(e.X, e.Y, ref bmp);
                pictureBox1.Image = bmp;
                pictureBox1.Invalidate();
            }
        }

        private void FillRec(int x, int y, ref Bitmap bmp)
        {
            int x_forward = x;
            int x_back = x;
            Color start_col = bmp.GetPixel(x, y);
            //bmp.SetPixel(x_forward, y, Color.Red);
            FillLine(x, y, ref bmp);
            Color fill_col = bmp.GetPixel(x, y);
            x_forward++;
            x_back--;
            Color col_f = bmp.GetPixel(x_forward, y);
            Color col_b = bmp.GetPixel(x_back, y);
            while (col_f == fill_col)
            { 
                Color col_h_1 = bmp.GetPixel(x_forward, y + 1);
                Color col_l_1 = bmp.GetPixel(x_forward, y - 1);
                if (col_h_1 == start_col)
                {
                    FillRec(x_forward, y + 1, ref bmp);
                }
                if (col_l_1 == start_col)
                {
                    FillRec(x_forward, y - 1, ref bmp);
                }
                x_forward++;
                col_f = bmp.GetPixel(x_forward, y);
            }
            while (col_b == fill_col)
            {
                Color col_h_2 = bmp.GetPixel(x_back, y + 1);
                Color col_l_2 = bmp.GetPixel(x_back, y - 1);
                if (col_h_2 == start_col)
                {
                    FillRec(x_back, y + 1, ref bmp);
                }
                if (col_l_2 == start_col)
                {
                    FillRec(x_back, y - 1, ref bmp);
                }
                x_back--;
                col_b = bmp.GetPixel(x_back, y);
            }

            Color col_h = bmp.GetPixel(x, y + 1);
            Color col_l = bmp.GetPixel(x, y - 1);
            if (col_h == start_col)
            {
                FillRec(x, y + 1, ref bmp);
            }
            if (col_l == start_col)
            {
                FillRec(x, y - 1, ref bmp);
            }
        }

        private void FillLine(int x, int y, ref Bitmap bmp)
        {
            int x_forward = x;
            int x_back = x;
            Color start_col = bmp.GetPixel(x_forward, y);
            bmp.SetPixel(x_forward, y, Color.Red);
            x_forward++;
            x_back--;
            Color col_f = bmp.GetPixel(x_forward, y);
            Color col_b = bmp.GetPixel(x_back, y);
            while (col_f == start_col)
            {
                bmp.SetPixel(x_forward, y, Color.Red);
                x_forward++;
                col_f = bmp.GetPixel(x_forward, y);
            }
            while (col_b == start_col)
            {
                bmp.SetPixel(x_back, y, Color.Red);
                x_back--;
                col_b = bmp.GetPixel(x_back, y);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.draw_border = true;
            this.fill = false;
            this.fill_with_photo = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.draw_border = false;
            this.fill = true;
            this.fill_with_photo = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.draw_border = false;
            this.fill = false;
            this.fill_with_photo = true;
        }
    }

    public struct Point
    {
        public int x;
        public int y;
    }
}
