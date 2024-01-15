using System.Collections.Generic;
using System.Linq;

namespace PSOClusteringAlgorithm
{

    /// <summary>
    /// A particle in PSO clustering algorithm, working with an abstraction in case of nedeeng to change its behavior in some special situations
    /// </summary>
    public interface IParticle
    {
        /// <summary>
        /// Clusters Centroids, the "position" of particle in stadard algorithm
        /// </summary>
        List<Point> Centroids { get; set; }
        /// <summary>
        /// Particles Cost from FitnessFunction
        /// </summary>
        double Cost { get; set; }
        /// <summary>
        /// Afferent Velocity values for Centroids points
        /// </summary>
        List<Point> Velocity { get; set; }
        /// <summary>
        /// Personal best
        /// </summary>
        IParticle PBest { get; set; }

        /// <summary>
        /// Used to make a copy of self when "this" is PBest or sbest.
        /// </summary>
        IParticle Clone();
    }

    /// <summary>
    /// A simple particle
    /// </summary>
    public class Particle : IParticle
    {
        public List<Point> Centroids { get; set; }
        public double Cost { get; set; }
        public List<Point> Velocity { get; set; }
        public IParticle PBest { get; set; }

        public virtual IParticle Clone()
        {
            return new Particle
            {
                Cost = this.Cost,
                Velocity = this.Velocity,
                Centroids = new List<Point>(this.Centroids.Select(point => new Point { vec = point.vec.Select(value => value) })),
                PBest = this.PBest
            };
        }
    }
}
