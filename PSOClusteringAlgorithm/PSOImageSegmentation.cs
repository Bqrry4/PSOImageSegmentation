using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{

    public class PSOImageSegmentation
    {
        private readonly PSOClusteringAlgorithm _psoAlgorithm = new PSOClusteringAlgorithm();

        //parameters of image to be processed, needed to reconstruct the ouptut clustered image
        private int _width;
        private int _height;
        private PixelFormat _pixelFormat;

        public PSOImageSegmentation(int clusterCount, int particleCount, int maxIterration)
        {
            _psoAlgorithm.ClustersCount = clusterCount;
            _psoAlgorithm.ParticlesCount = particleCount;
            _psoAlgorithm.tmax = maxIterration;

            _psoAlgorithm.DomainLimits = new List<(int, int)>
            {
                (0, 0), //x
                (0, 0), //y
                (0, 255), //r
                (0, 255), //g
                (0, 255), //b
                (255, 255), //a
                //those are default values, the order for rgba can vary for image format
            };
        }

        public void GenerateDataSetFromBitmap(Bitmap image)
        {
            //convert image to dataset
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int depth = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            int size = depth * bitmapData.Height * bitmapData.Width;
            //copy the internal data to a buffer
            byte[] data = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, data, 0, size);

            _psoAlgorithm.DataSet = data.Select((x, i) => (Index: i, Value: x))
                .GroupBy(x => x.Index / depth)
                .Select((value, index) =>
                {
                    //computing the in-matrix coords from index
                    var y = index / image.Width;
                    var x = index - image.Width * y;
                    var pixelAsVec = new List<double>() { x, y };
                    pixelAsVec.AddRange(value.Select(val => (double)val.Value));
                    return new Point { vec = pixelAsVec };
                })
                .ToList();

            _width = bitmapData.Width;
            _height = bitmapData.Height;
            _pixelFormat = bitmapData.PixelFormat;
            _psoAlgorithm.PointDimensions = depth + 2; //color + position
            //updating the domain limits for position
            _psoAlgorithm.DomainLimits[0] = (0, _width - 1);
            _psoAlgorithm.DomainLimits[1] = (0, _height - 1);

            image.UnlockBits(bitmapData);
        }

        //should be called before the RunPSO, made that way to keep the observable feature
        public void instanciateParticles()
        {
            _psoAlgorithm.instanciateParticles();
        }

        //should be called before the RunPSO if monitosing the particles is desired
        public IEnumerable<ParticleObservable> instanciateObservableParticles()
        {
            return _psoAlgorithm.instanciateObservableParticles();
        }

        public Bitmap RunPSO()
        {
            var result = _psoAlgorithm.RunPSO();
            return ClusteredDatasetToImage(_psoAlgorithm.DataSet, result.centroids);
        }

        Bitmap ClusteredDatasetToImage(IEnumerable<Point> dataset, List<Point> centroids)
        {
            var clusters = PSOClusteringAlgorithm.GetClusters(dataset, centroids);

            var clusteredImage = new Bitmap(_width, _height, _pixelFormat);

            BitmapData bitmapData = clusteredImage.LockBits(new Rectangle(0, 0, clusteredImage.Width, clusteredImage.Height), ImageLockMode.ReadWrite, clusteredImage.PixelFormat);
            int depth = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            int size = depth * bitmapData.Height * bitmapData.Width;
            //buffer for image
            byte[] data = new byte[size];

            foreach (var (cluster, id) in clusters.Select((value, id) => (value, id)))
            {
                foreach (var point in cluster)
                {
                    var inArrayPosition = ((int)point.vec.ElementAt(1) * bitmapData.Width + (int)point.vec.ElementAt(0)) * depth; // (y * width + x) * depth

                    for (int i = 0; i < depth; i++)
                    {
                        data[inArrayPosition + i] = (byte)centroids[id].vec.ElementAt(2 + i); //as first 2 points are the coords
                    }
                }
            }

            //write buffer to image
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
            clusteredImage.UnlockBits(bitmapData);

            return clusteredImage;
        }


    }
}
