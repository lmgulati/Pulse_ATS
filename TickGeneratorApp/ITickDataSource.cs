using System;

namespace TickGeneratorApp
{
    public interface ITickDataSource
    {
        event Action<Tick> OnTick;
        void Start();
        void Stop();
    }
}