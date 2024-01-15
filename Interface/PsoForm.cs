using PSOClusteringAlgorithm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private int _numberOfParticles = 10;
        private int _numberOfClusters = 8;
        private int _numberOfIterations = 10;
        private (SbestType type, int size) _socialStrategy = (SbestType.Social, 0);

        private async void startButton_Click(object sender, EventArgs e)
        {
            _pso = new PSOImageSegmentation(_numberOfClusters, _numberOfParticles, _numberOfIterations);
            _pso.GenerateDataSetFromBitmap(_image);

            /*Set the strategy*/
            switch (seedComboBox.SelectedIndex)
            {
                case 0:
                    _pso.CentroidSpawner = new SpawnInDomainValues(_pso.PointDimensions, _pso.DomainLimits);
                    break;
                case 2:
                    _pso.CentroidSpawner = new SpawnWithKMeansSeed(_pso.DataSet, _pso.tmax);
                    break;
                default:
                    _pso.CentroidSpawner = new SpawnInDatasetValues(_pso.DataSet);
                    break;
            }

            /*Set the version of algorithm*/
            _pso.VicinityType = _socialStrategy.type;
            _pso.VicinitySize = _socialStrategy.size;

            particlePanel.Controls.Clear();
            if (observableCheckBox.Checked)
            {
                _particleViews = Enumerable.Range(0, _numberOfParticles)
                    .Select(i => new PictureBox
                    {
                        Height = 80,
                        Width = 80,
                        Visible = true,
                        SizeMode = PictureBoxSizeMode.Zoom
                    }).ToArray();

                particlePanel.Controls.AddRange(_particleViews);

                var particles = Enumerable.Range(0, _numberOfParticles).Select(index => new ParticleObservable(index)).ToArray();

                foreach (var particleObservable in particles)
                {
                    subscriptions.Add(particleObservable.Subscribe(this));
                }

                _pso.Particles = particles;
            }

            var image = await Task.Run(() => _pso.RunImagePSO());

            resultPictureBox.Image = image;
        }

        private void clustersNumeric_ValueChanged(object sender, EventArgs e)
        {
            _numberOfClusters = (int)clustersNumeric.Value;
        }

        private void particleNumeric_ValueChanged(object sender, EventArgs e)
        {
            _numberOfParticles = (int)particleNumeric.Value;
        }

        private void iterationsNumeric_ValueChanged(object sender, EventArgs e)
        {
            _numberOfIterations = (int)iterationsNumeric.Value;
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

        private void socialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (socialComboBox.SelectedIndex)
            {
                case 1:
                    _socialStrategy = (SbestType.Social, 3);
                    break;
                case 2:
                    _socialStrategy = (SbestType.Geographic, 3);
                    break;
                default:
                    _socialStrategy = (SbestType.Social, 0);
                    break;
            }
        }
    }
}