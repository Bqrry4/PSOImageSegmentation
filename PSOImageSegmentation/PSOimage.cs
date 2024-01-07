using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PSOImageSegmentation
{
    public class PSOimage
    {
        /// <summary>
        /// Point vector in n space dimension.
        /// Delegates an enumerable to keep the code clearer
        /// </summary>
        class Point
        {
            //keeping the point more generic for more dimensions
            public IEnumerable<double> vec { get; set; }
        }

        /// <summary>
        /// A particle in PSO algorithm
        /// </summary>
        class Particle
        {
            public List<Point> centroids;
            public double cost;
            //afferent velocity values for centroids points
            public List<Point> velocity;
            //personal best
            public Particle pbest;

            public Particle Clone()
            {
                return new Particle
                {
                    cost = this.cost,
                    velocity = this.velocity,
                    centroids = new List<Point>(this.centroids.Select(point => new Point { vec = point.vec.Select(value => value) }))

                    //can be ignored
                    //pbest = ...;
                };
            }

        }


        static double EuclidianDistance(IEnumerable<double> zp, IEnumerable<double> zw)
        {
            if (zp.Count() != zw.Count())
            {
                throw new ArgumentException("vectors of different dimensions");
            }

            double summ = 0;
            for (int i = 0; i < zp.Count(); ++i)
            {
                var p = zp.ElementAt(i) - zw.ElementAt(i);
                summ += p * p;
            }
            return Math.Sqrt(summ);
        }

        //Compute quantization error for given centroids and the pixels associated to its clusters
        static double QuantizationError(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            double summ = 0;
            for (int k = 0; k < clusters.Count(); ++k)
            {
                summ += clusters.ElementAt(k)
                    .Sum(pattern => EuclidianDistance(pattern.vec, centroids.ElementAt(k).vec))
                    / clusters.ElementAt(k).Count();
            }
            return summ / clusters.Count();
        }

        //maximum average Euclidean distance of centroids and patterns(pixels) associated to clusters
        static double Dmax(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return centroids
                .Select((centroid, k) => clusters.ElementAt(k).Sum(point => EuclidianDistance(point.vec, centroid.vec)) / clusters.ElementAt(k).Count())
                .Max();
        }
        //minimum Euclidean distance between any pair of centroids
        static double Dmin(IEnumerable<Point> centroids)
        {
            var distanceMin = Double.MaxValue;
            for (int i = 0; i < centroids.Count(); ++i)
            {
                for (int j = i + 1; j < centroids.Count(); ++j)
                {
                    var distanceBetween = EuclidianDistance(centroids.ElementAt(i).vec, centroids.ElementAt(j).vec);
                    if (distanceBetween < distanceMin)
                        distanceMin = distanceBetween;
                }
            }

            return distanceMin;
        }

        //Minimisation of non parametric fitness function
        static double FitnessFunction(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return (Dmax(centroids, clusters) + QuantizationError(centroids, clusters)) / Dmin(centroids);
        }

        /// <summary>
        /// Get a list of clusters from given centroids and dataset 
        /// </summary>
        static List<List<Point>> GetClusters(IEnumerable<Point> dataset, IEnumerable<Point> centroids)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster
            var clusters = new List<List<Point>>();
            for (int i = 0; i < centroids.Count(); ++i)
            {
                clusters.Add(new List<Point>());
            }

            //assign each pattern to a cluster
            foreach (var point in dataset)
            {
                //find the closest centroid to pattern
                int idk = centroids
                    .Select((centroid, index) => (id: index, distance: EuclidianDistance(point.vec, centroid.vec)))
                    .Aggregate((min, current) => min.distance < current.distance ? min : current).id;

                //assign pattern to the closest cluster
                clusters[idk].Add(new Point { vec = point.vec });
            }

            return clusters;
        }


        double ComputeFitnessForGivenParticle(Particle particle, IEnumerable<Point> dataset)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster of a particle
            var particleClusters = GetClusters(dataset, particle.centroids);

            return FitnessFunction(particle.centroids, particleClusters);
        }

        //associated dataset to the image
        private List<Point> _dataSet;
        //size of image to be processed, needed to reconstruct the ouptut clustered image
        private int _width;
        private int _height;
        private PixelFormat _pixelFormat;

        //TODO: support for more images types, it works fine with jpg
        public void GenerateDataSetFromBitmap(Bitmap image)
        {
            //convert image to dataset
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int depth = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
            int size = depth * bitmapData.Height * bitmapData.Width;
            //copy the internal data to a buffer
            byte[] data = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, data, 0, size);

            _dataSet = data.Select((x, i) => (Index: i, Value: x))
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
            _pointDimensions = depth + 2; //color + position
            //updating the domain limits for position
            _domainLimits[0] = (0, _width - 1);
            _domainLimits[1] = (0, _height - 1);

            image.UnlockBits(bitmapData);
        }

        //PSO parameters
        private int _clustersCount = 12;
        //should be given when generating dataset
        private int _pointDimensions; //x,y,r,g,b,a
        private int _particlesCount = 50;
        //iterationCount
        private int tmax = 50;
        //constants
        private double w = 0.73;
        private double c1 = 1.49;
        private double c2 = 2.0;

        /// <summary>
        /// value limits for a rgba image, values should be updated when dataset is generated
        /// </summary>
        private readonly List<(int min, int max)> _domainLimits = new List<(int, int)>
        {
            (0, 0), //x
            (0, 0), //y
            (0, 255), //r
            (0, 255), //g
            (0, 255), //b
            (255, 255), //a
            //those are default values, the order for rgba can vary for image format
        };

        public Bitmap RunPSO()
        {
            //needed for the random factor
            Random rnd = new Random();
            //an array of particles as it count doesn't change throughout the algorithm
            var particles = Enumerable.Range(0, _particlesCount).Select(_ => new Particle()).ToArray();

            //init particles randomly
            Parallel.ForEach(particles, particle =>
            {
                //regenerate particle if its cost was NaN or infinite
                //caused by spawn of 2 centroids very close to each other, can happen if image is very uniform (ex. white background)
                do
                {
                    //init centroids and its velocity of current particle
                    particle.centroids = new List<Point>();
                    particle.velocity = new List<Point>();
                    for (int j = 0; j < _clustersCount; ++j)
                    {
                        //append centroid
                        particle.centroids.Add(new Point());

                        /*//randomly set centroids within domain values
                        particle.centroids.ElementAt(j).vec = Enumerable.Range(0, _pointDimensions)
                            .Select(index => (double)rnd.Next(_domainLimits[index].min, _domainLimits[index].max)).ToArray();*/

                        //randomly set centroids with points in dataset, runs showing it does converge better
                        particle.centroids[j].vec = _dataSet.ElementAt(rnd.Next(0, _dataSet.Count())).vec
                            .Select(x => x).ToArray(); //a copy of vec

                        //init velocity with 0 [or random within a given interval] -> won't do that for now
                        particle.velocity.Add(new Point());
                        particle.velocity.ElementAt(j).vec = Enumerable.Range(0, _pointDimensions)
                            .Select(_ => 0.0).ToArray();
                    }

                    //compute the initial cost of particle
                    particle.cost = ComputeFitnessForGivenParticle(particle, _dataSet);

                } while (Double.IsNaN(particle.cost) || Double.IsInfinity(particle.cost));

                //pbest as copy of self
                particle.pbest = particle.Clone();
            });

            //social best, current implementation - star topology as global best
            var sbest = particles.Aggregate((min, current) => min.cost < current.cost ? min : current).Clone();
            for (int t = 0; t < tmax; t++)
            {
                //mutex for parallel updating the sbest /and particlesStillMoving counter
                var mux = new Mutex();
                //needed to check convergency
                int particlesStillMoving = 0;
                //foreach (var particle in particles)
                Parallel.ForEach(particles, particle =>
                {
                    //foreach centroid
                    //compute velocity and move centroids
                    for (int i = 0; i < _clustersCount; i++)
                    {
                        //the random component in velocity equation
                        double r1 = rnd.NextDouble();
                        double r2 = rnd.NextDouble();

                        var index = i;
                        //update velocity | we could limit velocity -> won't do for now
                        particle.velocity[index].vec = particle.velocity[index].vec
                            .Select((velocity, id) => w * velocity //weigth from previous velocity
                                + c1 * r1 * (particle.pbest.centroids[index].vec.ElementAt(id) - particle.centroids[index].vec.ElementAt(id)) //cognitive component
                                + c2 * r2 * (sbest.centroids[index].vec.ElementAt(id) - particle.centroids[index].vec.ElementAt(id)) //social component
                            ).ToArray();
                        //update centroids
                        particle.centroids[index].vec = particle.centroids[index].vec
                            .Select((point, id) => 
                                //limit the values to its domain
                                Math.Min(
                                    Math.Max(
                                        point + particle.velocity[index].vec.ElementAt(id),
                                        _domainLimits[id].min)
                                    , _domainLimits[id].max))
                            .ToArray();


                        //check particles for convergence
                        if (particle.velocity[index].vec.Select(Math.Abs).Max() < 0.1) //if velocities -> [-0.1; 0.1]
                        {
                            mux.WaitOne();
                            particlesStillMoving++;
                            mux.ReleaseMutex();
                        }
                    }

                    //calculate fitness for current particle in context of the image
                    particle.cost = ComputeFitnessForGivenParticle(particle, _dataSet);
                    //updating pbest
                    if (particle.cost < particle.pbest.cost)
                    {
                        //save a copy of current particle as pbest
                        particle.pbest = particle.Clone();
                        //updating sbest
                        mux.WaitOne();
                        if (particle.cost < sbest.cost)
                        {
                            sbest = particle.Clone();
                        }
                        mux.ReleaseMutex();
                    }
                }
                );

                //if the particles converged finish
                if (particlesStillMoving == 0)
                {
                    break;
                }
            }
            //solution is sbest
            return ClusteredDatasetToImage(_dataSet, sbest.centroids);
        }

        Bitmap ClusteredDatasetToImage(IEnumerable<Point> dataset, List<Point> centroids)
        {

            var clusters = GetClusters(dataset, centroids);


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
