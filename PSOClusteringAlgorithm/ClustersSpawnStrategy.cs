using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSOClusteringAlgorithm
{
    /// <summary>
    /// The "strategy to spawn the centroids
    /// </summary>
    public interface IClusterSpawner
    {
        Point PlaceCentroid();
    }

    /// <summary>
    /// randomly set Centroids within domain values
    /// </summary>
    public class SpawnInDomainValues: IClusterSpawner
    {
        private readonly int _pointDimensions;
        private readonly List<(int min, int max)> _domainLimits;
        private readonly Random _rnd = new Random();

        public SpawnInDomainValues(int pointDimensions, List<(int min, int max)> domainLimits)
        {
            _pointDimensions = pointDimensions;
            _domainLimits = domainLimits;
        }

        public Point PlaceCentroid()
        {

            return new Point
            {
                vec = Enumerable.Range(0, _pointDimensions)
                    .Select(index => (double)_rnd.Next(_domainLimits[index].min, _domainLimits[index].max)).ToArray()
            };
        }
    }
    /// <summary>
    /// randomly set Centroids with points in dataset, runs showing it does converge better than inDomain
    /// </summary>
    public class SpawnInDatasetValues : IClusterSpawner
    {
        private readonly List<Point> _dataSet;
        private readonly Random _rnd = new Random();

        public SpawnInDatasetValues(List<Point> dataSet)
        {
            _dataSet = dataSet;
        }
        public Point PlaceCentroid()
        {
            return new Point
            {
                vec = _dataSet.ElementAt(_rnd.Next(0, _dataSet.Count)).vec
                    .Select(x => x).ToArray()
            };
        }
    }
}
