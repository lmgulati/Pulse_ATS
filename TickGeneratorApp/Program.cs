using System;
using System.Threading.Tasks;

namespace TickGeneratorApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var generator = new ControlledTickGenerator();

            Console.WriteLine("=== Tick Generator ===");
            Console.WriteLine("Available Scenarios:");
            Console.WriteLine("1 - Random");
            Console.WriteLine("2 - TrendUp");
            Console.WriteLine("3 - TrendDown");
            Console.WriteLine("4 - Range");
            Console.WriteLine("5 - Spike");
            Console.WriteLine("6 - Trap");
            Console.WriteLine("Q - Quit");

            while (true)
            {
                Console.Write("\nSelect Scenario: ");
                var input = Console.ReadLine();

                if (input.ToLower() == "q")
                    break;

                string scenario;

                switch (input)
                {
                    case "1":
                        scenario = "random";
                        break;
                    case "2":
                        scenario = "trendup";
                        break;
                    case "3":
                        scenario = "trenddown";
                        break;
                    case "4":
                        scenario = "range";
                        break;
                    case "5":
                        scenario = "spike";
                        break;
                    case "6":
                        scenario = "trap";
                        break;
                    default:
                        scenario = "random";
                        break;
                }

                Console.WriteLine($"\nRunning: {scenario}\n");

                var task = generator.RunScenarioAsync(scenario);

                Console.WriteLine("Press any key to stop scenario...");
                Console.ReadKey();

                generator.Stop();
                await Task.Delay(500);
            }
        }
    }
}