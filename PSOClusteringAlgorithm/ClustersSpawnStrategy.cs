using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{
    /// <summary>
    /// The "strategy" to spawn the swarm [only the position should be affected]
    /// </summary>
    public interface IClusterSpawner
    {
        List<Point> PlaceCentroids(int clusterCount);
        void SpawnSwarm(IParticle[] particles, int clusterCount);
    }

    /// <summary>
    /// Randomly set Centroids within domain values
    /// </summary>
    public class SpawnInDomainValues : IClusterSpawner
    {
        private readonly int _pointDimensions;
        private readonly List<(int min, int max)> _domainLimits;
        private readonly Random _rnd = new Random();

        public SpawnInDomainValues(int pointDimensions, List<(int min, int max)> domainLimits)
        {
            _pointDimensions = pointDimensions;
            _domainLimits = domainLimits;
        }

        public List<Point> PlaceCentroids(int clusterCount)
        {
            return Enumerable.Range(0, clusterCount).Select(_ => new Point
            {
                vec = Enumerable.Range(0, _pointDimensions)
                    .Select(index => (double)_rnd.Next(_domainLimits[index].min, _domainLimits[index].max)).ToArray()
            }).ToList();
        }

        public void SpawnSwarm(IParticle[] particles, int clusterCount)
        {
            Parallel.ForEach(particles, particle =>
            {
                particle.Centroids = Enumerable.Range(0, clusterCount).Select(_ => new Point
                {
                    vec = Enumerable.Range(0, _pointDimensions)
                        .Select(index => (double)_rnd.Next(_domainLimits[index].min, _domainLimits[index].max))
                        .ToArray()
                }).ToList();
            });

        }
    }
    /// <summary>
    /// The Forgy method.
    /// Randomly set Centroids with points in dataset, runs showing it does converge better than inDomain
    /// </summary>
    public class SpawnInDatasetValues : IClusterSpawner
    {
        private readonly List<Point> _dataSet;
        private readonly Random _rnd = new Random();

        public SpawnInDatasetValues(List<Point> dataSet)
        {
            _dataSet = dataSet;
        }
        public List<Point> PlaceCentroids(int clusterCount)
        {
            return Enumerable.Range(0, clusterCount).Select(_ => new Point
            {
                vec = _dataSet.ElementAt(_rnd.Next(0, _dataSet.Count)).vec
                    .Select(x => x).ToArray()
            }).ToList();
        }
        public void SpawnSwarm(IParticle[] particles, int clusterCount)
        {
            Parallel.ForEach(particles, particle =>
            {
                particle.Centroids = Enumerable.Range(0, clusterCount).Select(_ => new Point
                {
                    vec = _dataSet.ElementAt(_rnd.Next(0, _dataSet.Count)).vec
                        .Select(x => x).ToArray()
                }).ToList();
            });
        }
    }
    /// <summary>
    /// Using default Kmeans implementation with Forgy init method(in dataset)
    /// </summary>
    public class SpawnWithKMeansSeed : IClusterSpawner
    {
        private readonly KMeansClustering _kMeans = new KMeansClustering();
        private readonly SpawnInDatasetValues secondSpawner;

        public SpawnWithKMeansSeed(List<Point> dataSet, int maxIterration)
        {
            _kMeans.DataSet = dataSet;
            _kMeans.tmax = maxIterration;
            secondSpawner = new SpawnInDatasetValues(dataSet);
        }
        public List<Point> PlaceCentroids(int clusterCount)
        {
            _kMeans.ClustersCount = clusterCount;

            return _kMeans.RunKMeans();
        }

        public void SpawnSwarm(IParticle[] particles, int clusterCount)
        {
            _kMeans.ClustersCount = clusterCount;

            particles[0].Centroids = _kMeans.RunKMeans();

            secondSpawner.SpawnSwarm(particles.Skip(1).ToArray(), clusterCount);

        }
    }
}
