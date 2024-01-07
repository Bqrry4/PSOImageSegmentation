using PSOImageSegmentation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Interface
{
    public partial class PsoForm : Form
    {
        public PsoForm()
        {
            InitializeComponent();
        }


        

        public static Bitmap MakeGrayscale2(Bitmap original)
        {
            int x, y;

            // Loop through the images pixels to reset color.
            for (x = 0; x < original.Width; x++)
            {
                for (y = 0; y < original.Height; y++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    original.SetPixel(x, y, newColor); // Now greyscale
                }
            }

            return original;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var pso = new PSOimage(4);
            var image = PSOimage.MakeGrayscale3(new Bitmap(Image.FromFile("./toji.jpg")));
            initialPictureBox.Image = image;
            pso.GenerateDataSetFromBitmap(image);
            var psoed = pso.RunPSO();
            resultPictureBox.Image = psoed.image;
        }
    }
}
