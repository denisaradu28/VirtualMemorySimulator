using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMemorySimulation
{
    public class FifoAlgorithm : IPageReplacementAlgorithm
    {
        public SimulatorResult Run(int framesCount, int[] referenceString){
            var result = new SimulatorResult();
            var frames = new int?[framesCount];
            var queue = new Queue<int>();

            foreach(int page in referenceString)
            {
                bool hit = frames.Contains(page);
                if(hit)
                {
                    result.IsFault.Add(false);
                    result.FramesHistory.Add(frames.ToArray());
                    continue;
                }

                result.IsFault.Add(true);
                if(queue.Count < framesCount)
                {
                    int free = Array.FindIndex(frames, f => !f.HasValue);
                    frames[free] = page;
                    queue.Enqueue(page);
                }
                else
                {
                    int victim = queue.Dequeue();
                    int idx = Array.IndexOf(frames, victim);
                    frames[idx] = page;
                    queue.Enqueue(page);
                }

                result.FramesHistory.Add(frames.ToArray());
            }

            return result;
        }
    }
}