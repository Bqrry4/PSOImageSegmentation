using PSOImageSegmentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PSOImageSegmentation.PSOimage;

namespace Interface
{
    public partial class PsoForm : Form
    {
        public PsoForm()
        {
            InitializeComponent();
        }


        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();
        private List<PictureBox> _particleViews;
        private List<IObservable<IEnumerable<PSOimage.Point>>> _particleObservables;

        private Bitmap _image;

        private int numberOfParticles = 10;
        private int numberOfClusters = 8;
        private int numberOfIterations = 10;

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

        private async void startButton_Click(object sender, EventArgs e)
        {
            var pso = new PSOimage(numberOfClusters, numberOfParticles, numberOfIterations);
            pso.GenerateDataSetFromBitmap(_image);

            _particleViews = Enumerable.Range(0, numberOfParticles).Select(_ => new PictureBox()).ToList();
            _particleObservables = new List<IObservable<IEnumerable<PSOimage.Point>>>(numberOfIterations);

            var (image, _) = await Task.Run(() => pso.RunPSO());

            resultPictureBox.Image = image;
        }

        private void clustersNumeric_ValueChanged(object sender, EventArgs e)
        {
            numberOfClusters = (int)clustersNumeric.Value;
        }

        private void particleNumeric_ValueChanged(object sender, EventArgs e)
        {
            numberOfParticles = (int)particleNumeric.Value;
        }

        private void iterationsNumeric_ValueChanged(object sender, EventArgs e)
        {
            numberOfIterations = (int)iterationsNumeric.Value;
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                //var sr = new StreamReader(_openFileDialog.FileName);
                _image = new Bitmap(Image.FromFile(_openFileDialog.FileName));
                initialPictureBox.Image = _image;
                startButton.Enabled = true;
            }
            catch (SecurityException ex)
            {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                                $"Details:\n\n{ex.StackTrace}");
            }
        }
    }
}