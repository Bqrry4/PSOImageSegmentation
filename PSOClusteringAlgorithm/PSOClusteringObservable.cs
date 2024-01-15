using System;
using System.Collections.Generic;
using System.Linq;

namespace PSOClusteringAlgorithm
{

    public class ParticleObservable : IParticle, IObservable<ParticleObservable>
    {

        //index in the swarn
        public int Index { get; set; }

        public ParticleObservable(int index)
        {
            Index = index;
        }

        HashSet<IObserver<ParticleObservable>> Observers { get; set; } =
            new HashSet<IObserver<ParticleObservable>>();

        public IDisposable Subscribe(IObserver<ParticleObservable> observer)
        {
            Observers.Add(observer);

            return new Unsubscriber(Observers, observer);
        }

        internal sealed class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<ParticleObservable>> _observers;
            private readonly IObserver<ParticleObservable> _observer;

            internal Unsubscriber(
                ISet<IObserver<ParticleObservable>> observers,
                IObserver<ParticleObservable> observer) => (_observers, _observer) = (observers, observer);

            public void Dispose() => _observers.Remove(_observer);
        }


        public List<Point> Centroids { get; set; }

        //Update observers when cost is updated
        private double _cost;
        public double Cost
        {
            get => _cost;
            set
            {
                _cost = value;

                foreach (var observer in Observers)
                {
                    observer.OnNext(this);
                }
            }
        }

        public List<Point> Velocity { get; set; }
        public IParticle PBest { get; set; }

        public IParticle Clone()
        {
            return new ParticleObservable(Index)
            {
                Cost = this.Cost,
                Velocity = this.Velocity,
                Centroids = new List<Point>(this.Centroids.Select(point => new Point { vec = point.vec.Select(value => value) })),
                Observers = Observers,
                PBest = this.PBest
            };
        }
    }

}



