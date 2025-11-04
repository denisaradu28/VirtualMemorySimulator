using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtualMemorySimulation
{
    public class LRUAlgorithm : IPageReplacementAlgorithm
    {
        public SimulatorResult Run(int framesCount, int[] referenceString)
        {
            var result = new SimulatorResult();
            var memory = new HashSet<int>(framesCount); 
            var lastUsed = new Dictionary<int, int>();  
            var frames = new int?[framesCount];         

            for (int i = 0; i < referenceString.Length; i++)
            {
                int page = referenceString[i];

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
                    }
                    else
                    {
                        int lruPage = memory
                            .OrderBy(p => lastUsed[p])
                            .First();

                        int idx = Array.IndexOf(frames, lruPage);
                        frames[idx] = page;

                        memory.Remove(lruPage);
                        memory.Add(page);
                        lastUsed.Remove(lruPage);
                    }
                }

                lastUsed[page] = i;

                result.FramesHistory.Add(frames.ToArray());
            }

            return result;
        }
    }
}
