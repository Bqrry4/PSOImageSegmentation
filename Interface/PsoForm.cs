using PSOClusteringAlgorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace Interface
{
    public partial class PsoForm : Form, IObserver<ParticleObservable>
    {
        public PsoForm()
        {
            InitializeComponent();
        }


        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();
        private PictureBox[] _particleViews;

        private List<IDisposable> subscriptions = new List<IDisposable>();

        private Bitmap _image;
        private PSOImageSegmentation _pso;

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
            _pso = new PSOImageSegmentation(numberOfClusters, numberOfParticles, numberOfIterations);
            _pso.GenerateDataSetFromBitmap(_image);

            /*Set the strategy*/
            //_pso.CentroidSpawner = new SpawnInDatasetValues(_pso.DataSet);
            _pso.CentroidSpawner = new SpawnWithKMeansSeed(_pso.DataSet, _pso.tmax);

            particlePanel.Controls.Clear();
            if (observableCheckBox.Checked)
            {
                _particleViews = Enumerable.Range(0, numberOfParticles)
                    .Select(i => new PictureBox
                    {
                        Height = 80,
                        Width = 80,
                        Visible = true,
                        SizeMode = PictureBoxSizeMode.Zoom
                    }).ToArray();

                particlePanel.Controls.AddRange(_particleViews);

                var particles = Enumerable.Range(0, numberOfParticles).Select(index => new ParticleObservable(index)).ToArray();

                foreach (var particleObservable in particles)
                {
                    subscriptions.Add(particleObservable.Subscribe(this));
                }

                _pso.Particles = particles;
            }
/*            else
            {
                _pso.instanciateParticles();
            }*/

            var image = await Task.Run(() => _pso.RunPSO());

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

        public void OnNext(ParticleObservable value)
        {
            var image = _pso.ClusteredDatasetToImage(value.Centroids);
            _particleViews[value.Index].Image = image;
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}