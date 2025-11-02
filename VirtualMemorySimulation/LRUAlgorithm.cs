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
            var frames = new int?[framesCount];
            var lastUsed = new Dictionary<int, int>();

            for (int i = 0; i < referenceString.Length; i++)
            {
                int page = referenceString[i];
                bool hit = frames.Contains(page);

                if (hit)
                {
                    result.Hits++;
                    lastUsed[page] = i;
                    result.IsFault.Add(false);
                }
                else
                {
                    result.Faults++;
                    result.IsFault.Add(true);

                    int free = Array.FindIndex(frames, f => !f.HasValue);

                    if (free != -1)
                    {
                        frames[free] = page;
                    }
                    else
                    {
                        int lruPage = frames
                            .Where(f => f.HasValue)
                            .Select(f => f.Value)
                            .OrderBy(f => lastUsed[f])
                            .First();

                        int idx = Array.IndexOf(frames, lruPage);
                        frames[idx] = page;
                        lastUsed.Remove(lruPage);
                    }

                    lastUsed[page] = i;
                }

                result.FramesHistory.Add(frames.ToArray());
            }

            return result;
        }
    }
}
