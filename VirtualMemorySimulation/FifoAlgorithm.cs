using System;
using System.Collections.Generic;
using System.Linq;
using VirtualMemorySimulation;

public class FifoAlgorithm : IPageReplacementAlgorithm
{
    public SimulatorResult Run(int framesCount, int[] referenceString)
    {
        var result = new SimulatorResult();
        var memory = new HashSet<int>(framesCount); 
        var queue = new Queue<int>();              
        var frames = new int?[framesCount];        

        foreach (int page in referenceString)
        {
            if (memory.Contains(page))
            {
                result.Hits++;
                result.IsFault.Add(false);
            }
            else
            {
                result.Faults++;
                result.IsFault.Add(true);

                if (memory.Count < framesCount)
                {
                    memory.Add(page);

                    int freeIdx = Array.FindIndex(frames, f => !f.HasValue);
                    frames[freeIdx] = page;
                    queue.Enqueue(page);
                }
                else
                {
                    int oldestPage = queue.Dequeue();
                    memory.Remove(oldestPage);

                    memory.Add(page);
                    queue.Enqueue(page);

                    int idx = Array.IndexOf(frames, oldestPage);
                    frames[idx] = page;
                }
            }

            result.FramesHistory.Add(frames.ToArray());
        }

        return result;
    }
}
