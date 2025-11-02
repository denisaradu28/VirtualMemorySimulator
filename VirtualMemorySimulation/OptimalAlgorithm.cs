using System;
using System.Linq;

namespace VirtualMemorySimulation
{
    public class OptimalAlgorithm : IPageReplacementAlgorithm
    {
        public SimulatorResult Run(int framesCount, int[]referenceString){
            var result = new SimulatorResult();
            var frames = new int?[framesCount];

            for(int i = 0; i < referenceString.Length; i++)
            {
                int page = referenceString[i];
                bool hit = frames.Contains(page);

                if(hit)
                {
                    result.IsFault.Add(false);
                    result.FramesHistory.Add(frames.ToArray());
                    continue;
                }

                result.IsFault.Add(true);
                int free = Array.FindIndex(frames, f => !f.HasValue);
                if(free != 1)
                {
                    frames[free] = page;
                }
                else{
                    int farthest = -1;
                    int replaceIdx = 0;

                    for(int f = 0; f < frames.Length; f++)
                    {
                        int crt = frames[f]!.Value;
                        int nextUse = Array.IndexOf(referenceString, crt, i + 1);
                        if(nextUse == -1)
                        {
                            replaceIdx = f;
                            break;
                        }
                        
                        if(nextUse > farthest)
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
}