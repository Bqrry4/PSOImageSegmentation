using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PSOimseg
{
    internal class PSOImage
    {

        //keeping the point more generic for more dimensions
        class Point
        {
            public IEnumerable<double> vec { get; set; }
        }

        class Particle
        {
            public List<Point> centroids;
            public double cost;
            //afferent velocity values for centroids points
            public List<Point> velocity;
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
        private int clustersCount = 7;
        private int pointDimensions = 5; //x,y,r,g,b
        private int particlesCount = 20;
        //iterationCount
        private int tmax = 100;
        //constants
        private double w = 0.0;
        private double c1 = 0.0;
        private double c2 = 0.0;


        double EuclidianDistance(IEnumerable<double> zp, IEnumerable<double> zw)
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
        double QuantizationError(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            double summ = 0;
            for (int k = 0; k < clustersCount; ++k)
            {
                summ += clusters.ElementAt(k)
                    .Sum(pattern => EuclidianDistance(pattern.vec, centroids.ElementAt(k).vec))
                    / clusters.ElementAt(k).Count();
            }
            return summ / clustersCount;
        }

        //maximum average Euclidean distance of centroids and patterns(pixels) associated to clusters
        double Dmax(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return centroids
                .Select((centroid, k) => clusters.ElementAt(k).Sum(point => EuclidianDistance(point.vec, centroid.vec)) / clusters.ElementAt(k).Count())
                .Max();
        }
        //minimum Euclidean distance between any pair of centroids
        double Dmin(IEnumerable<Point> centroids)
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
        /// Get a list of clusters from given centroids and image
        /// </summary>
        List<List<Point>> GetClusters(Bitmap image, List<Point> centroids)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster
            var clusters = new List<List<Point>>();
            for (int i = 0; i < clustersCount; ++i)
            {
                clusters.Add(new List<Point>());
            }

            //assign each pixel to a cluster
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    var pixelValue = image.GetPixel(x, y);
                    var pixelAsVec = new double[] { x, y, pixelValue.R, pixelValue.G, pixelValue.B };

                    //find the closest centroid to pixel
                    int idk = centroids
                        .Select((centroid, index) => (id: index, distance: EuclidianDistance(pixelAsVec, centroid.vec)))
                        .Aggregate((min, current) => min.distance < current.distance ? min : current).id;

                    //assign pixel to particle's closest cluster
                    clusters[idk].Add(new Point { vec = pixelAsVec });
                }
            }
            return clusters;
        }


        double ComputeFitnessForGivenParticle(Particle particle, Bitmap image)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster of a particle
            var particleClusters = GetClusters(image, particle.centroids);

            return FitnessFunction(particle.centroids, particleClusters);
        }

        public void PSOimage(Bitmap image)
        {
            //an array of particles as it count doesnt change throughout the algorithm
            var particles = new Particle[particlesCount];
            Random rnd = new Random();

            //init particles randomly
            foreach (var particle in particles)
            {
                //init centroids and its velocity of current particle
                particle.centroids = new List<Point>();
                particle.velocity = new List<Point>();
                for (int j = 0; j < clustersCount; ++j)
                {
                    //append centroid
                    particle.centroids.Add(new Point());
                    //randomly set centroids within image values
                    particle.centroids.ElementAt(j).vec = new double[] { rnd.Next(image.Width), rnd.Next(image.Height), rnd.Next(255), rnd.Next(255), rnd.Next(255) };
                    //compute the initial cost of particle
                    particle.cost = ComputeFitnessForGivenParticle(particle, image);
                    //init velocity with 0 or random within a given interval
                    particle.velocity.ElementAt(j).vec = new double[] { 0, 0, 0, 0, 0 };
                    //pbest as copy of self
                    particle.pbest = particle.Clone();
                }
            }

            var gbest = particles.Aggregate((min, current) => min.cost < current.cost ? min : current).Clone();
            for (int t = 0; t < tmax; t++)
            {
                foreach (var particle in particles)
                {
                    //foreach centroid
                    //compute velocity and move centroids
                    for (int i = 0; i < clustersCount; ++i)
                    {
                        //the random component in velocity equation
                        double r1 = rnd.NextDouble();
                        double r2 = rnd.NextDouble();

                        //update velocity
                        particle.velocity[i].vec = particle.velocity[i].vec
                            .Select((velocity, id) => w * velocity //weigth from previous velocity
                            + c1 * r1 * (particle.pbest.centroids[i].vec.ElementAt(id) - particle.centroids[i].vec.ElementAt(id)) //cognitive component
                            + c2 * r2 * (gbest.centroids[i].vec.ElementAt(id) - particle.centroids[i].vec.ElementAt(id)) //social component
                            );

                        //update centroids
                        particle.centroids[i].vec = particle.centroids[i].vec
                            .Select((point, id) => point + particle.velocity[i].vec.ElementAt(id));
                    }


                    //calculate fitness for current particle in context of the image
                    particle.cost = ComputeFitnessForGivenParticle(particle, image);
                    //updating pbest
                    if (particle.cost < particle.pbest.cost)
                    {
                        //save a copy of current particle as pbest
                        particle.pbest = particle.Clone();
                        //updating gbest
                        if (particle.cost < gbest.cost)
                        {
                            gbest = particle.Clone();
                        }
                    }
                }
            }


            //solution is gbest
            displayClusters(image, gbest.centroids);
        }

        void displayClusters(Bitmap image, List<Point> centroids)
        {
            var clusteredImage = new Bitmap(image.Width, image.Height);

            var clusters = GetClusters(image, centroids);

            foreach (var cluster in clusters)
            {
                foreach (var point in cluster)
                {
                    clusteredImage.SetPixel(
                        (int)point.vec.ElementAt(0),
                        (int)point.vec.ElementAt(1),
                        Color.FromArgb(
                            (int)point.vec.ElementAt(2),
                            (int)point.vec.ElementAt(3),
                            (int)point.vec.ElementAt(4)
                            )
                        );
                }
            }


        }

        static void Main(string[] args)
        {

            /*            string imagePath = "";
                        Bitmap image = new Bitmap(imagePath);*/
        }
    }
}
