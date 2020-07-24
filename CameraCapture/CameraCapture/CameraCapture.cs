using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO;
using System.Drawing.Imaging;

namespace CameraCapture
{
    public partial class CameraCapture : Form
    {

        private Capture capture;
        private bool captureInProgress;
        IImage orgimg;
        Bitmap bitmap;
        float  contrast = 0;
        float gamma = 1;

        public CameraCapture()
        {
            InitializeComponent();
            trackBar1.Hide();
            trackBar2.Hide();
            trackBar3.Hide();
            pictureBox1.Hide();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> ImageFrame = capture.QuerySmallFrame();


            CamImgBox.Image = ImageFrame;
             //Image<Gray, Byte> grayFrame = capture.QuerySmallFrame().Convert<Gray, Byte>();
             //grayBox.Image = grayFrame;
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            #region if capture is not created, create it now
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            #endregion

            if (capture != null)
            {
                if (captureInProgress)
                {
                    btnStart.Text = "Start!"; //
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    btnStart.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }
        }

        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void CameraCapture_Load(object sender, EventArgs e)
        {

        }

        private void Capturebtn_Click(object sender, EventArgs e)
        {
            CaptureBox.Image = CamImgBox.Image;
            orgimg = CaptureBox.Image;
            //CamImgBox.Image = CamImgBox.Image;
            bitmap = new Bitmap(CamImgBox.Image.Bitmap);
            
        }

        Image zoom(Image img, Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Show();
            pictureBox1.Image = AdjustBrightness(bitmap, trackBar1.Value);
            //Bitmap tempbitmap = new Bitmap(temp);
            //Emgu.CV.Image<Bgr, byte> img = new Emgu.CV.Image<Bgr, byte>(bitmap);
            //CaptureBox.Image = img;


        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> grayFrame = capture.QuerySmallFrame().Convert<Gray, Byte>();
            CaptureBox.Image = grayFrame;
            //int height = bitmap.Height;
            //int width = bitmap.Width;
            //Console.WriteLine("h"+height);
            //Console.WriteLine("w"+width);
            //Color color;
            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        color = bitmap.GetPixel(x, y);
            //        int a = color.A;
            //        int r = color.R;
            //        int g = color.G;
            //        int b = color.B;

            //        int avg = (r + g + b) / 3;
            //        bitmap.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
            //    }
            //}

            //image1.Image = bitmap;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
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
            Emgu.CV.Image<Bgr, byte> img = new Emgu.CV.Image<Bgr, byte>(bitmap);
            CaptureBox.Image = img;
        }

        private void britnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Show();
            trackBar2.Hide();
            trackBar3.Hide();
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

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Show();
            contrast = 0.04f * trackBar2.Value;
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

            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            ia.Dispose();
            pictureBox1.Image = bm;
        }

        private void contractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Hide();
            trackBar3.Hide();
            trackBar2.Show();
        }

        private void gammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Hide();
            trackBar2.Hide();
            trackBar3.Show();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

            trackBar3.Show();

            gamma = 0.04f * trackBar3.Value;
            Bitmap bm = new Bitmap(bitmap.Width, bitmap.Height);
            Graphics g = Graphics.FromImage(bm);

            ImageAttributes ia = new ImageAttributes();

            ia.SetGamma(gamma);

            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, ia);

            g.Dispose();
            ia.Dispose();
            pictureBox1.Image = bm;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            home hh = new home();
            hh.Show();
            this.Hide();
            
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap;
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            detail dd = new detail();
            dd.Show();
        }

        //private void Savebtn_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog dialog = new SaveFileDialog();
        //    dialog.Filter = "JPG(*.JPG|*.jpg";
        //    if (dialog.ShowDialog() == DialogResult.OK)
        //    {


        //        int width = Convert.ToInt32(CaptureBox.Width);
        //        int height = Convert.ToInt32(CaptureBox.Height);
        //        Bitmap bmp = new Bitmap(width, height);
        //        CaptureBox.DrawToBitmap(bmp, new Rectangle(0, 0, Width, Height));
        //        bmp.Save(dialog.FileName, ImageFormat.Jpeg);



        //    }

        //}
    }
}