using System;
using System.Linq;

namespace VirtualMemorySimulation
{
    internal class Program
    {
        static void Main()
        {
            int[] referenceString = { 2, 3, 2, 0, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 4, 3, 2, 0 };
            int frames = 3;

            Console.WriteLine("\nLRU\n");
            IPageReplacementAlgorithm lru = new LRUAlgorithm(frames);
            RunAlgorithm(lru, frames, referenceString);

            Console.WriteLine("\nFIFO\n");
            IPageReplacementAlgorithm fifo = new FifoAlgorithm(frames);
            RunAlgorithm(fifo, frames, referenceString);

            Console.WriteLine("\nOPTIMAL\n");
            IPageReplacementAlgorithm optimal = new OptimalAlgorithm(frames);
            RunAlgorithm(optimal, frames, referenceString);

        }

        static void RunAlgorithm(IPageReplacementAlgorithm alg, int frames, int[] referenceString)
        {
            var result = alg.Run(frames, referenceString);

            Console.WriteLine($"Page Faults: {result.PageFaults}");
            Console.WriteLine($"Hits: {result.TotalAccesses - result.PageFaults}");
            Console.WriteLine();

            for (int i = 0; i < result.FrameHistory.Count; i++)
            {
                Console.Write($"Step {i + 1}: ");
                Console.WriteLine(string.Join(" ", result.FrameHistory[i].Select(f => f?.ToString() ?? "-")));
            }

        }

    }
}