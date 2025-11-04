using System;
using System.Collections.Generic;
using System.Linq;
using VirtualMemorySimulation;

public class OptimalAlgorithm : IPageReplacementAlgorithm
{
    public SimulatorResult Run(int framesCount, int[] referenceString)
    {
        var result = new SimulatorResult();
        var frames = new int?[framesCount];

        for (int i = 0; i < referenceString.Length; i++)
        {
            int page = referenceString[i];

            if (frames.Contains(page))
            {
                result.Hits++;
                result.IsFault.Add(false);
                result.FramesHistory.Add(frames.ToArray());
                continue;
            }

            result.Faults++;
            result.IsFault.Add(true);

            int freeIdx = Array.FindIndex(frames, f => !f.HasValue);
            if (freeIdx != -1)
            {
                frames[freeIdx] = page;
            }
            else
            {
                int replaceIdx = 0;
                int farthest = -1;

                for (int f = 0; f < frames.Length; f++)
                {
                    int crt = frames[f]!.Value;
                    int nextUse = Array.IndexOf(referenceString, crt, i + 1);

                    if (nextUse == -1)
                    {
                        replaceIdx = f;
                        break;
                    }
                    if (nextUse > farthest)
                    {
                        farthest = nextUse;
                        replaceIdx = f;
                    }
                }

                frames[replaceIdx] = page;
            }

            result.FramesHistory.Add(frames.ToArray());
        }

        return result;
    }
}
