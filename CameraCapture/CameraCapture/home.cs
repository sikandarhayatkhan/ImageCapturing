using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Drawing.Imaging;
namespace CameraCapture
{
    public partial class home : Form
    {
        Image orgimg;
        Bitmap bitmap;
        int brightness = 0;
        float contrast = 0;
        float gamma = 1;
        public home()
        {
            InitializeComponent();
            trackBar1.Hide();
            trackBar2.Hide();
            trackBar3.Hide();
            trackBar4.Hide();

        }

        private void home_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //browse bb = new browse();
            //this.Hide();
            //bb.Show();
            string imagelocation = "";
            try
            {
                //OpenFileDialog dialog = new OpenFileDialog();
                //dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                //if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    imagelocation = dialog.FileName;
                //    image1.ImageLocation = imagelocation;
                //    orgimg = image1.Image;
                //}
                using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false,ValidateNames = true,Filter = "JPEG files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*" })
                {
                    if(ofd.ShowDialog() == DialogResult.OK)
                    {
                        image1.Image = Image.FromFile(ofd.FileName);
                        orgimg = image1.Image;
                        bitmap = new Bitmap(Image.FromFile(ofd.FileName));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error hshs");
            }


        }

        Image zoom(Image img,Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CameraCapture cc = new CameraCapture();
            this.Hide();
            cc.Show();
        }

        private void contractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar3.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Show();
           
        }

        private void home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(image1.Image != null)
            {
                image1.Dispose();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar2.Hide();
            trackBar3.Hide();
            if (trackBar1.Value > 0)
            {
                image1.Image = zoom(orgimg, new Size(trackBar1.Value, trackBar1.Value));
            }
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            detail dd = new detail();
            dd.Show();
            
        }

        private void image1_MouseHover(object sender, EventArgs e)
        {

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            //Console.WriteLine("h"+height);
            //Console.WriteLine("w"+width);
            Color color;
            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color = bitmap.GetPixel(x, y);
                    int a = color.A;
                    int r = color.R;
                    int g = color.G;
                    int b = color.B;

                    int avg = (r + g + b) / 3;
                    bitmap.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }
            }

            image1.Image = bitmap;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            //Console.WriteLine("h"+height);
            //Console.WriteLine("w"+width);
            Color color;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    color = bitmap.GetPixel(x, y);
                    int r = color.R;
                    int g = color.G;
                    int b = color.B;

                    bitmap.SetPixel(x, y, Color.FromArgb(255 - r, 255 - g, 255 - b));
                }
            }
            image1.Image = bitmap;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar1.Hide();
            trackBar3.Hide();
            image1.Image = AdjustBrightness(bitmap, trackBar2.Value);
        }

        private void britnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar2.Show();
            
        }

        public static Bitmap AdjustBrightness(Bitmap Image, int Value)
        {

            Bitmap TempBitmap = Image;

            
            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            //Console.WriteLine("bb" + TempBitmap.Width);

            Graphics NewGraphics = Graphics.FromImage(NewBitmap);

            float FinalValue = ((float)Value / 255.0f);

            float[][] FloatColorMatrix = {

                    new float[] {1, 0, 0, 0, 0},

                    new float[] {0, 1, 0, 0, 0},

                    new float[] {0, 0, 1, 0, 0},

                    new float[] {0, 0, 0, 1, 0},

                    new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);

            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);

            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);

            Attributes.Dispose();

            NewGraphics.Dispose();

            return NewBitmap;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            trackBar2.Hide();
            trackBar1.Hide();
            contrast = 0.04f * trackBar3.Value;
            Bitmap bm = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics g = Graphics.FromImage(bm);

            ImageAttributes ia = new ImageAttributes();

            ColorMatrix cm = new ColorMatrix(new float[][] {
                        
                    new float[] {contrast, 0f, 0f, 0f, 0f},

                    new float[] {0f, contrast, 0f, 0f, 0f},

                    new float[] {0f, 0f, contrast, 0f, 0f},

                    new float[] {0f, 0f, 0f, 1, 0f},

                    new float[] {0.002f, 0.002f, 0.002f, 0f, 1f}
                });

            ia.SetColorMatrix(cm);

            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),0, 0,bitmap.Width, bitmap.Height,GraphicsUnit.Pixel,ia);

            g.Dispose();
            ia.Dispose();
            image1.Image = bm;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            trackBar1.Hide();
            trackBar2.Hide();
            trackBar3.Hide();
            gamma = 0.04f * trackBar4.Value;
            Bitmap bm = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics g = Graphics.FromImage(bm);

            ImageAttributes ia = new ImageAttributes();

            ia.SetGamma(gamma);

            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            ia.Dispose();
            image1.Image = bm;
        }

        private void gammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar4.Show();
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image1.Image = orgimg;
        }
    }
}
