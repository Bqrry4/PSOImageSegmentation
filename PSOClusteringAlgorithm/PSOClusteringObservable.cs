using System;
using System.Collections.Generic;
using System.Linq;

namespace PSOClusteringAlgorithm
{

    public class ParticleObservable : Particle, IObservable<ParticleObservable>
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

        public override void Update()
        {
            base.Update();

            foreach (var observer in Observers)
            {
                observer.OnNext(this);
            }
        }

        public override Particle Clone()
        {
            return new ParticleObservable(Index)
            {
                cost = this.cost,
                velocity = this.velocity,
                centroids = new List<Point>(this.centroids.Select(point => new Point { vec = point.vec.Select(value => value) })),
                Observers = Observers
                //can be ignored
                //pbest = ...;
            };
        }
    }
}



