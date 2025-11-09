using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMemorySimulation;

public class OptimalAlgorithm : IPageReplacementAlgorithm
{
    public SimulatorResult Run(int framesCount, int[] referenceString)
    {
        var result = new SimulatorResult();
        var memory = new HashSet<int>(framesCount);
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
                    int pageToReplace = FindPageToReplace(frames, referenceString, i + 1);

                    memory.Remove(pageToReplace);
                    memory.Add(page);

                    int replaceIdx = Array.IndexOf(frames, pageToReplace);
                    frames[replaceIdx] = page;
                }
            }

            result.FramesHistory.Add(frames.ToArray());
        }

        return result;
    }
    

    private int FindPageToReplace(int?[] frames, int[] referenceString, int startIndex)
    {
        int farthestIdx = -1;
        int pageToReplace = -1;

        foreach (var frame in frames)
        {
            if (!frame.HasValue)
                continue;

            int nextUse = Array.IndexOf(referenceString, frame.Value, startIndex);

            if (nextUse == -1)
            {
                return frame.Value;
            }

            if (nextUse > farthestIdx)
            {
                farthestIdx = nextUse;
                pageToReplace = frame.Value;
            }
        }
        return pageToReplace;
    }

}
