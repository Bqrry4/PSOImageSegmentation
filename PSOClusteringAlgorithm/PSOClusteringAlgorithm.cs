using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{
    /// <summary>
    /// Point vector in n space dimension.
    /// Delegates an enumerable to keep the code clearer
    /// </summary>
    public class Point
    {
        //keeping the point more generic for more dimensions
        public IEnumerable<double> vec { get; set; }
    }

    /// <summary>
    /// A particle in PSO algorithm
    /// </summary>
    public class Particle
    {
        public List<Point> centroids;
        public double cost;
        //afferent velocity values for centroids points
        public List<Point> velocity;
        //personal best
        public Particle pbest;

        public virtual Particle Clone()
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

        /// <summary>
        /// Updates centroids with current velocities
        /// </summary>
        public virtual void Update()
        {

        }

    }


    public class PSOClusteringAlgorithm
    {

        #region StaticMethods
        /// <summary>
        /// Computes euclidian distance between two points in n space dimension
        /// </summary>
        /// <param name="zp">First vector</param>
        /// <param name="zw">Second vector</param>
        /// <exception cref="ArgumentException"></exception>
        public static double EuclidianDistance(IEnumerable<double> zp, IEnumerable<double> zw)
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
        /// <summary>
        /// Compute quantization error for given centroids and the patterns associated to its clusters
        /// </summary>
        /// <param name="centroids">The clusters centroids</param>
        /// <param name="clusters">The clusters asociated data</param>
        public static double QuantizationError(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
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
        /// <summary>
        /// Maximum average Euclidean distance of centroids and patterns associated to clusters
        /// </summary>
        /// <param name="centroids">The clusters centroids</param>
        /// <param name="clusters">The clusters asociated data</param>
        public static double Dmax(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return centroids
                .Select((centroid, k) => clusters.ElementAt(k).Sum(point => EuclidianDistance(point.vec, centroid.vec)) / clusters.ElementAt(k).Count())
                .Max();
        }
        /// <summary>
        /// Minimum Euclidean distance between any pair of centroids
        /// </summary>
        /// <param name="centroids">The clusters centroids</param>
        public static double Dmin(IEnumerable<Point> centroids)
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

        /// <summary>
        /// Minimisation of non-parametric fitness function
        /// </summary>
        /// <param name="centroids">The clusters centroids</param>
        /// <param name="clusters">The clusters asociated data</param>
        public static double FitnessFunction(IEnumerable<Point> centroids, IEnumerable<IEnumerable<Point>> clusters)
        {
            return (Dmax(centroids, clusters) + QuantizationError(centroids, clusters)) / Dmin(centroids);
        }

        /// <summary>
        /// Get a list of clusters from given centroids and dataset 
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="centroids"></param>
        /// <returns></returns>
        public static List<List<Point>> GetClusters(IEnumerable<Point> dataset, IEnumerable<Point> centroids)
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
        #endregion


        #region Constant
        /// <summary>
        /// Weight for exploitation
        /// </summary>
        public double W { get; set; } = 0.73;
        /// <summary>
        /// Constant for cognitive exploration
        /// </summary>
        public double C1 { get; set; } = 1.49;
        /// <summary>
        /// Constant for social exploration
        /// </summary>
        public double C2 { get; set; } = 1.49;
        #endregion

        #region PSOParameters

        public int ClustersCount { get; set; }
        //should be given when generating dataset
        public int PointDimensions { get; set; } // ex. (x,y,r,g,b,a) for an image 
        public int ParticlesCount { get; set; }
        /// <summary>
        /// Maximum iterration count
        /// </summary>
        public int tmax { get; set; }

        public List<Point> DataSet { get; set; }

        /// <summary>
        /// Value limits foreach dimension of point.
        /// Those should be updated when dataset is generated
        /// </summary>
        public List<(int min, int max)> DomainLimits { get; set; }
        #endregion


        double ComputeFitnessForGivenParticle(Particle particle, IEnumerable<Point> dataset)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster of a particle
            var particleClusters = GetClusters(dataset, particle.centroids);

            return FitnessFunction(particle.centroids, particleClusters);
        }


        #region PSOData

        //an array of particles as it count doesn't change throughout the algorithm
        private Particle[] particles;

        #endregion

        //should be called before the RunPSO, made that way to keep the observable feature
        public void instanciateParticles()
        {
            particles = Enumerable.Range(0, ParticlesCount).Select(_ => new Particle()).ToArray();
        }

        //should be called before the RunPSO if monitosing the particles is desired
        public IEnumerable<ParticleObservable> instanciateObservableParticles()
        {
            var observables = Enumerable.Range(0, ParticlesCount).Select(index => new ParticleObservable(index)).ToArray();
            particles = observables;
            return observables;
        }

        /// <summary>
        /// Runs PSO with setted parameters
        /// </summary>
        /// <returns>Best Particle</returns>
        public Particle RunPSO()
        {
            //needed for the random factor
            Random _rnd = new Random();

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
                    for (int j = 0; j < ClustersCount; ++j)
                    {
                        //append centroid
                        particle.centroids.Add(new Point());

                        /*//randomly set centroids within domain values
                        particle.centroids.ElementAt(j).vec = Enumerable.Range(0, PointDimensions)
                            .Select(index => (double)_rnd.Next(DomainLimits[index].min, DomainLimits[index].max)).ToArray();*/

                        //randomly set centroids with points in dataset, runs showing it does converge better
                        particle.centroids[j].vec = DataSet.ElementAt(_rnd.Next(0, DataSet.Count)).vec
                            .Select(x => x).ToArray(); //a copy of vec

                        //init velocity with 0 [or random within a given interval] -> won't do that for now
                        particle.velocity.Add(new Point());
                        particle.velocity.ElementAt(j).vec = Enumerable.Range(0, PointDimensions)
                            .Select(_ => 0.0).ToArray();
                    }

                    //compute the initial cost of particle
                    particle.cost = ComputeFitnessForGivenParticle(particle, DataSet);

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
                    for (int i = 0; i < ClustersCount; i++)
                    {
                        //the random component in velocity equation
                        double r1 = _rnd.NextDouble();
                        double r2 = _rnd.NextDouble();

                        var index = i;
                        //update velocity | we could limit velocity -> won't do for now
                        particle.velocity[index].vec = particle.velocity[index].vec
                            .Select((velocity, id) => W * velocity //weigth from previous velocity
                                + C1 * r1 * (particle.pbest.centroids[index].vec.ElementAt(id) - particle.centroids[index].vec.ElementAt(id)) //cognitive component
                                + C2 * r2 * (sbest.centroids[index].vec.ElementAt(id) - particle.centroids[index].vec.ElementAt(id)) //social component
                            ).ToArray();

                        //update centroids
                        particle.centroids[index].vec = particle.centroids[index].vec
                            .Select((point, id) =>
                                //limit the values to its domain
                                Math.Min(
                                    Math.Max(
                                        point + particle.velocity[index].vec.ElementAt(id),
                                        DomainLimits[id].min)
                                    , DomainLimits[id].max))
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
                    particle.cost = ComputeFitnessForGivenParticle(particle, DataSet);
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
                    particle.Update();
                });

                //if the particles converged finish
                if (particlesStillMoving == 0)
                {
                    break;
                }
            }
            //solution is sbest
            return sbest;
        }

    }
}
