using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private readonly HashSet<IObserver<ParticleObservable>> _observers =
            new HashSet<IObserver<ParticleObservable>>();

        public IDisposable Subscribe(IObserver<ParticleObservable> observer)
        {
            _observers.Add(observer);

            return new Unsubscriber(_observers, observer);
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

            foreach (var observer in _observers)
            {
                observer.OnNext(this);
            }
        }
    }
}



