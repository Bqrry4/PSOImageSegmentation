using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        //PSO parameters
        private readonly int clustersCount = 7;
        private int pointDimensions = 5; //x,y,r,g,b
        private int particlesCount = 5;
        //iterationCount
        private int tmax = 20;
        //constants
        private double w = 0.73;
        private double c1 = 2.0;
        private double c2 = 2.0;


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
        double FitnessFunction(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
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



        public Bitmap runPSO(Bitmap image)
        {

            //convert image to dataset !! MAKE IT A SETTER to free some memory
            BitmapData bData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int depth = Bitmap.GetPixelFormatSize(bData.PixelFormat) / 8;
            int size = depth * bData.Height * bData.Width;
            byte[] data = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);

            var dataset = data.Select((x, i) => (Index: i, Value: x))
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

            image.UnlockBits(bData);

            //needed for the random factor
            Random rnd = new Random();
            //an array of particles as it count doesnt change throughout the algorithm
            var particles = Enumerable.Range(0, particlesCount).Select(_ => new Particle()).ToArray();

            //init particles randomly
            //foreach(var particle in particles)
            Parallel.ForEach(particles, particle =>
            {
                do
                {
                    //init centroids and its velocity of current particle
                    particle.centroids = new List<Point>();
                    particle.velocity = new List<Point>();
                    for (int j = 0; j < clustersCount; ++j)
                    {
                        //append centroid
                        particle.centroids.Add(new Point());
                        //randomly set centroids within image values
                        particle.centroids.ElementAt(j).vec = new double[]
                        {
                            rnd.Next(image.Width), rnd.Next(image.Height), rnd.Next(255), rnd.Next(255), rnd.Next(255),
                            rnd.Next(255)
                        };

                        //init velocity with 0 or random within a given interval
                        particle.velocity.Add(new Point());
                        particle.velocity.ElementAt(j).vec = new double[] { 0, 0, 0, 0, 0, 0 };
                    }

                    //compute the initial cost of particle
                    particle.cost = ComputeFitnessForGivenParticle(particle, dataset);
                } while (Double.IsNaN(particle.cost) || Double.IsInfinity(particle.cost));

                //pbest as copy of self
                particle.pbest = particle.Clone();
            });

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
                    for (int i = 0; i < clustersCount; i++) //WHY IS THERE A BUG?? HOOW ON EARTH
                    {
                        //the random component in velocity equation
                        double r1 = rnd.NextDouble();
                        double r2 = rnd.NextDouble();

                        var index = i;
                        //update velocity
                        particle.velocity[index].vec = particle.velocity[index].vec
                            .Select((velocity, id) => w * velocity //weigth from previous velocity
                                + c1 * r1 * (particle.pbest.centroids[index].vec.ElementAt(id) - particle.centroids[index].vec.ElementAt(id)) //cognitive component
                                + c2 * r2 * (sbest.centroids[index].vec.ElementAt(id) - particle.centroids[index].vec.ElementAt(id)) //social component
                            ).ToArray();
                        //update centroids
                        particle.centroids[index].vec = particle.centroids[index].vec
                            .Select((point, id) => point + particle.velocity[index].vec.ElementAt(id)).ToArray();


                        //check particles for convergence
                        if (particle.velocity[index].vec.Min() < 0.1)
                        {
                            mux.WaitOne();
                            particlesStillMoving++;
                            mux.ReleaseMutex();
                        }
                    }

                    //calculate fitness for current particle in context of the image
                    particle.cost = ComputeFitnessForGivenParticle(particle, dataset);
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
            return ClusterTheImage(image, dataset, sbest.centroids);
        }

        Bitmap ClusterTheImage(Bitmap image, IEnumerable<Point> dataset, List<Point> centroids)
        {
            var clusteredImage = new Bitmap(image.Width, image.Height);

            var clusters = GetClusters(dataset, centroids);

            foreach (var (cluster, id) in clusters.Select((value, id) => (value, id)))
            {
                foreach (var point in cluster)
                {
                    clusteredImage.SetPixel(
                        (int)point.vec.ElementAt(0),
                        (int)point.vec.ElementAt(1),
                        Color.FromArgb(
                            (int)centroids[id].vec.ElementAt(2),
                            (int)centroids[id].vec.ElementAt(3),
                            (int)centroids[id].vec.ElementAt(4)
                            )
                        );
                }
            }
            return clusteredImage;
        }

    }
}
