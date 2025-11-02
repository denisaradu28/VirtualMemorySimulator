using System;
using System.Linq;

namespace VirtualMemorySimulation
{
    internal class Program
    {
        static void Main()
        {
            int[] referenceString = { 7, 0, 1, 2, 0, 3, 0, 4, 2, 3, 0, 3 };
            int frames = 3;

            IPageReplacementAlgorithm alg = new LRUAlgorithm();
            var result = alg.Run(frames, referenceString);

            Console.WriteLine($"Page Faults: {result.Faults}");
            Console.WriteLine($"Hits: {result.Hits}");

            for (int i = 0; i < result.FramesHistory.Count; i++)
            {
                Console.Write($"Step {i + 1}: ");
                Console.WriteLine(string.Join(" ", result.FramesHistory[i].Select(p => p?.ToString() ?? "-")));
            }

            Console.WriteLine($"\nFifo:\n");

            IPageReplacementAlgorithm alg2 = new FifoAlgorithm();
            var result2 = alg2.Run(frames, referenceString);

            Console.WriteLine($"Page Faults: {result2.Faults}");
            Console.WriteLine($"Hits: {result2.Hits}");

            for (int i = 0; i < result.FramesHistory.Count; i++)
            {
                Console.Write($"Step {i + 1}: ");
                Console.WriteLine(string.Join(" ", result2.FramesHistory[i].Select(p => p?.ToString() ?? "-")));
            } 
        }
    }
}