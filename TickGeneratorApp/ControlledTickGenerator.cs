using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TickGeneratorApp
{
    public class ControlledTickGenerator : ITickDataSource
    {
        public event Action<Tick> OnTick;

        private bool _running;
        private decimal _price = 100;
        private Random _rand = new Random();

        public int DelayMs { get; set; } = 100;

        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }

        public async Task RunScenarioAsync(string scenario)
        {
            Start();

            switch (scenario.ToLower())
            {
                case "trendup":
                    await TrendUp();
                    break;

                case "trenddown":
                    await TrendDown();
                    break;

                case "range":
                    await Range();
                    break;

                case "spike":
                    await Spike();
                    break;

                case "trap":
                    await FakeBreakoutTrap();
                    break;

                case "random":
                default:
                    await RandomWalk();
                    break;
            }
        }

        private async Task EmitTick(decimal price)
        {
            var tick = new Tick
            {
                Time = DateTime.Now,
                Price = price,
                Volume = _rand.Next(1, 20)
            };

            // Event publish
            OnTick?.Invoke(tick);

            // Stream publish
            TickStreamHub.TickQueue.Add(tick);

            // Console output
            Console.WriteLine(tick);

            await Task.Delay(DelayMs);
        }

        // ---------- SCENARIOS ----------

        private async Task RandomWalk()
        {
            while (_running)
            {
                _price += (decimal)(_rand.NextDouble() - 0.5);
                await EmitTick(_price);
            }
        }

        private async Task TrendUp()
        {
            while (_running)
            {
                _price += (decimal)(0.2 + _rand.NextDouble());
                await EmitTick(_price);
            }
        }

        private async Task TrendDown()
        {
            while (_running)
            {
                _price -= (decimal)(0.2 + _rand.NextDouble());
                await EmitTick(_price);
            }
        }

        private async Task Range()
        {
            decimal basePrice = _price;

            while (_running)
            {
                _price = basePrice + (decimal)(_rand.NextDouble() * 2 - 1);
                await EmitTick(_price);
            }
        }

        private async Task Spike()
        {
            // calm
            for (int i = 0; i < 20; i++)
            {
                _price += (decimal)(_rand.NextDouble() - 0.5);
                await EmitTick(_price);
            }

            // sudden spike
            for (int i = 0; i < 10; i++)
            {
                _price += (decimal)(2 + _rand.NextDouble());
                await EmitTick(_price);
            }

            // continue random
            await RandomWalk();
        }

        private async Task FakeBreakoutTrap()
        {
            decimal basePrice = _price;

            // build range
            for (int i = 0; i < 20; i++)
            {
                _price = basePrice + (decimal)(_rand.NextDouble() - 0.5);
                await EmitTick(_price);
            }

            // breakout up
            for (int i = 0; i < 5; i++)
            {
                _price += (decimal)(1 + _rand.NextDouble());
                await EmitTick(_price);
            }

            // sharp reversal (trap)
            for (int i = 0; i < 10; i++)
            {
                _price -= (decimal)(1.5 + _rand.NextDouble());
                await EmitTick(_price);
            }

            await RandomWalk();
        }
    }
}