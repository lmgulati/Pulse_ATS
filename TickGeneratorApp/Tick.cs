using System;

namespace TickGeneratorApp
{
    public class Tick
    {
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        public int Volume { get; set; }

        public override string ToString()
        {
            return $"{Time:HH:mm:ss.fff} | Price: {Price:F2} | Vol: {Volume}";
        }
    }
}