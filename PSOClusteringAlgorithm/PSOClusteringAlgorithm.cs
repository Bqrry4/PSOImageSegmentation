using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{
    /// <summary>
    /// Type of social best when using LBest
    /// </summary>
    public enum SbestType
    {
        Social, Geographic
    };


    public class PSOClusteringAlgorithm
    {


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

        double ComputeFitnessForGivenParticle(IParticle particle, IEnumerable<Point> dataset)
        {
            //clusterCount x pointsInCluster, holds the associated points to a cluster of a particle
            var particleClusters =
                ClusteringMethods.GetClusters(dataset, particle.Centroids, ClusteringMethods.EuclidianDistance);

            return ClusteringMethods.FitnessFunction(particle.Centroids, particleClusters);
        }

        #region PSOData

        //an array of Particles as it count doesn't change throughout the algorithm
        public IParticle[] Particles { get; set; }

        /// <summary>
        /// Needed when particles array is not provided prior to make an instance of default particles
        /// </summary>
        public int ParticlesCount { get; set; }

        public IClusterSpawner CentroidSpawner { get; set; }

        public SbestType VicinityType { get; set; }

        /// <summary>
        /// When 0, it is supossed to run in gbest social mode
        /// </summary>
        public int VicinitySize { get; set; } = 0;

        #endregion


        /// <summary>
        /// Get best from the vecinity for LBest
        /// </summary>
        /// <param name="particleIndex">Index in the swarm of particle</param>
        /// <returns>The best in its vecinity</returns>
        internal IParticle GetBestInParticleVicinity(int particleIndex)
        {
            IParticle sbest = Particles[particleIndex];
            switch (VicinityType)
            {
                //vicinity by index
                case SbestType.Social:

                    var leftNeighbours = (VicinitySize - 1) / 2;
                    var rightNeighbours = (VicinitySize - 1) - leftNeighbours;

                    //take the right neighbours
                    for (int i = 0; i < rightNeighbours; ++i)
                    {
                        var neighbour = Particles[(particleIndex + i + 1) % Particles.Length];
                        if (neighbour.Cost < sbest.Cost )
                        {
                            sbest = neighbour;
                        }
                    }
                    //except myself and take one shifted
                    for (int i = 0; i < leftNeighbours; ++i)
                    {
                        var neighbour = Particles[(particleIndex + i + 1) % Particles.Length];
                        if (neighbour.Cost < sbest.Cost)
                        {
                            sbest = neighbour;
                        }
                    }

                    break;
                //vicinity by position
                case SbestType.Geographic:

                    var vicinity = new List<(double distance, IParticle particle)>();
                    vicinity.Sort();
                    for (int i = 0; i < Particles.Length; ++i)
                    {
                        //we know that the distance to self is zero so yeah
                        if (i == particleIndex)
                        {
                            continue;
                        }

                        double distance = 0;
                        for (int j = 0; j < sbest.Centroids.Count; ++j)
                        {
                            distance += ClusteringMethods.EuclidianDistance(Particles[particleIndex].Centroids[j].vec, Particles[i].Centroids[j].vec);
                        }
                        distance /= sbest.Centroids.Count;

                        vicinity.Add((distance, Particles[i]));
                    }

                    sbest = vicinity.OrderBy(value => value.distance) //sorting to get the closer ones
                        .Take(VicinitySize)
                        .Select(value => value.particle).Append(Particles[particleIndex])
                        .Aggregate((min, current) => min.Cost < current.Cost ? min : current);

                    break;

            }

            return sbest.Clone();
        }


        /// <summary>
        /// Runs PSO with setted parameters
        /// </summary>
        /// <returns>Best Particle</returns>
        public IParticle RunPSO()
        {
            //instantiate as basic particles
            if (Particles == null)
            {
                Particles = Enumerable.Range(0, ParticlesCount).Select(_ => new Particle()).ToArray();
            }

            //init positions(centroids)
            CentroidSpawner.SpawnSwarm(Particles, ClustersCount);

            //init other values
            Parallel.ForEach(Particles, particle =>
            {
                //compute the initial Cost of particle
                particle.Cost = ComputeFitnessForGivenParticle(particle, DataSet);

                //init Velocity with 0 [or random within a given interval] -> won't do that for now as it does not make a much bigger difference
                particle.Velocity = Enumerable.Range(0, ClustersCount).Select(_ => new Point
                {
                    vec = Enumerable.Range(0, PointDimensions)
                        .Select(__ => 0.0).ToArray()
                }).ToList();

                //PBest as copy of self
                particle.PBest = particle.Clone();
            });


            //social best
            IParticle sbest = null;
            //if GBest is choosen
            if (VicinitySize == 0)
            {
                sbest = Particles.Aggregate((min, current) => min.Cost < current.Cost ? min : current).Clone();
            }

            //needed for the random factor
            Random _rnd = new Random();
            //mutex for parallel updating the sbest /and particlesStillMoving counter
            var mux = new Mutex();

            for (int t = 0; t < tmax; t++)
            {

                //needed to check convergency
                int particlesStillMoving = 0;
                //foreach (var particle in Particles)
                Parallel.ForEach(Particles, (particle, state, particleIndex) =>
                {
                    //find best in particle's vicinity
                    if (VicinitySize != 0)
                    {
                        mux.WaitOne();
                        sbest = GetBestInParticleVicinity((int)particleIndex);
                        mux.ReleaseMutex();
                    }

                    //foreach centroid
                    //compute Velocity and move Centroids
                    for (int i = 0; i < ClustersCount; i++)
                    {
                        //the random component in Velocity equation
                        double r1 = _rnd.NextDouble();
                        double r2 = _rnd.NextDouble();

                        //because of the closure
                        var index = i;
                        //update Velocity | we could limit Velocity -> won't do for now
                        particle.Velocity[index].vec = particle.Velocity[index].vec
                            .Select((velocity, id) => W * velocity //weigth from previous Velocity
                                + C1 * r1 * (particle.PBest.Centroids[index].vec.ElementAt(id) - particle.Centroids[index].vec.ElementAt(id)) //cognitive component
                                + C2 * r2 * (sbest.Centroids[index].vec.ElementAt(id) - particle.Centroids[index].vec.ElementAt(id)) //social component
                            ).ToArray();

                        //update Centroids
                        particle.Centroids[index].vec = particle.Centroids[index].vec
                            .Select((point, id) =>
                                //limit the values to its domain
                                Math.Min(
                                    Math.Max(
                                        point + particle.Velocity[index].vec.ElementAt(id),
                                        DomainLimits[id].min)
                                    , DomainLimits[id].max))
                            .ToArray();

                        //check Particles for convergence
                        if (particle.Velocity[index].vec.Select(Math.Abs).Max() < 0.1) //if velocities -> [-0.1; 0.1]
                        {
                            mux.WaitOne();
                            particlesStillMoving++;
                            mux.ReleaseMutex();
                        }
                    }

                    //calculate fitness for current particle in context of the image
                    particle.Cost = ComputeFitnessForGivenParticle(particle, DataSet);
                    //updating PBest
                    if (particle.Cost < particle.PBest.Cost)
                    {
                        //save a copy of current particle as PBest
                        particle.PBest = particle.Clone();
                        //updating right away when gbest
                        if (VicinitySize == 0)
                        {
                            //updating sbest
                            mux.WaitOne();
                            if (particle.Cost < sbest.Cost)
                            {
                                sbest = particle.Clone();
                            }
                            mux.ReleaseMutex();
                        }
                    }
                });

                //if the Particles converged finish
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
