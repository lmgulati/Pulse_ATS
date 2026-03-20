using System.Collections.Concurrent;

namespace TickGeneratorApp
{
    public static class TickStreamHub
    {
        public static BlockingCollection<Tick> TickQueue = new BlockingCollection<Tick>();
    }
}