using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProgressBar
{
    class Program
    {
        private static int CurrentLength = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Press > to increase the speed.");
            Console.WriteLine("Press < to decrease the speed.");
            Console.WriteLine("Press + to increase the bar length.");
            Console.WriteLine("Press - to decrease the bar length.");
            Console.WriteLine("Press the space bar to exit.");
            Console.WriteLine("Press any other key to change the progress bar character.");
            RunBar().Wait();
            Console.WriteLine();
            Console.WriteLine("Done!");
        }

        private static async Task RunBar()
        {
            var config = new ProgressBarConfig();
            var displayTask = PrintBar(config);
            var configTask = GetInput(config);
            await Task.WhenAny(displayTask, configTask);
        }

        private static void UpdateBar(ProgressBarConfig config)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            int ProgressLength = config.Progress * config.BarLength / 100;
            string Bar = String.Concat(Enumerable.Repeat(config.Character, ProgressLength));
            string BarWhiteSpace = String.Concat(Enumerable.Repeat(" ", config.BarLength - ProgressLength));
            string ToPrint = $"[{Bar}{BarWhiteSpace}] {config.Progress}% ETA:{(100 - config.Progress) * config.TimeStep / 1000}s";
            string AppendedWhiteSpace = String.Concat(Enumerable.Repeat(" ", Math.Max(CurrentLength - ToPrint.Length, 0)));
            Console.Write($"{ToPrint}{AppendedWhiteSpace}");
            CurrentLength = ToPrint.Length;
        }


        private static async Task PrintBar(ProgressBarConfig config)
        {
            while (!config.Done)
            {
                UpdateBar(config);
                config.IncProgress();
                await Task.Delay(config.TimeStep);
            }
            config.SetDone();
        }

        private static async Task GetInput(ProgressBarConfig config)
        {
            Action work = () =>
            {
                while (!config.Done)
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                    {
                        config.UpdateTimeStep(-100);
                    }
                    else if (key.KeyChar == '<')
                    {
                        config.UpdateTimeStep(100);
                    }
                    else if (key.KeyChar == '+')
                    {
                        config.UpdateBarLength(5);
                    }
                    else if (key.KeyChar == '-')
                    {
                        config.UpdateBarLength(-5);
                    }
                    else if (key.KeyChar == ' ')
                    {
                        config.SetDone();
                    }
                    else if (!char.IsWhiteSpace(key.KeyChar))
                    {
                        config.SetChar(key.KeyChar);
                    }
                    UpdateBar(config);
                }
            };
            await Task.Run(work);
        }
    }
}
