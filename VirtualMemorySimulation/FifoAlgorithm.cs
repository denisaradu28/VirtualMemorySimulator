using System;
using System.Collections.Generic;

namespace VirtualMemorySimulation
{
    public class FifoAlgorithm : IPageReplacementAlgorithm
    {
        public int FrameCount { get; private set; }
        public List<Frame> Frames { get; private set; }
        public Dictionary<int, PageTableEntry> PageTable { get; private set; }

        public int TotalAccesses { get; private set; }
        public int PageFaults { get; private set; }
        public double HitRate => TotalAccesses == 0 ? 0 : (double)(TotalAccesses - PageFaults) / TotalAccesses;
        public double MissRate => 1 - HitRate;

        private Queue<int> queue;

        public FifoAlgorithm(int frameCount)
        {
            FrameCount = frameCount;
            Frames = new List<Frame>();
            for (int i = 0; i < frameCount; i++)
            {
                Frames.Add(new Frame(i));
            }

            PageTable = new Dictionary<int, PageTableEntry>();
            queue = new Queue<int>();
            TotalAccesses = 0;
            PageFaults = 0;
        }

        public SimulatorResult AccessPage(int pageId)
        {
            TotalAccesses++;
            bool wasHit = PageTable.ContainsKey(pageId) && PageTable[pageId].Valid;
            int? evictedPageId = null;

            if (!wasHit)
            {
                PageFaults++;

                if (queue.Count < FrameCount)
                {
                    int freeIdx = -1;
                    for (int i = 0; i < FrameCount; i++)
                    {
                        if (!Frames[i].isValid)
                        {
                            freeIdx = i;
                            break;
                        }
                    }

                    if (freeIdx == -1)
                        freeIdx = 0;

                    Frames[freeIdx].PageId = pageId;
                    PageTable[pageId] = new PageTableEntry(pageId)
                    {
                        Valid = true,
                        FrameIdx = freeIdx
                    };
                    queue.Enqueue(pageId);
                }
                else
                {
                    int oldestPage = queue.Dequeue();
                    int victimFrameIdx = PageTable[oldestPage].FrameIdx ?? 0;
                    evictedPageId = oldestPage;

                    PageTable[oldestPage].Valid = false;
                    PageTable[oldestPage].FrameIdx = null;

                    Frames[victimFrameIdx].PageId = pageId;
                    PageTable[pageId] = new PageTableEntry(pageId)
                    {
                        Valid = true,
                        FrameIdx = victimFrameIdx
                    };
                    queue.Enqueue(pageId);
                }
            }

            List<int?> snapshot = Frames.ConvertAll(f => f.PageId);

            SimulatorResult result = new SimulatorResult
            {
                PageId = pageId,
                WasHit = wasHit,
                EvictedPageId = evictedPageId,
                TotalAccesses = TotalAccesses,
                PageFaults = PageFaults,
                HitRate = HitRate,
                MissRate = MissRate,
                FrameHistory = new List<List<int?>> { snapshot },
                FrameSnapshot = new List<Frame>()
            };

            foreach (var f in Frames)
                result.FrameSnapshot.Add(new Frame(f.Index) { PageId = f.PageId });

            return result;
        }

        public SimulatorResult Run(int framesCount, int[] referenceString)
        {
            FrameCount = framesCount;
            Frames = new List<Frame>();
            for (int i = 0; i < framesCount; i++)
            {
                Frames.Add(new Frame(i));
            }

            PageTable.Clear();
            queue.Clear();
            PageFaults = 0;
            TotalAccesses = 0;

            SimulatorResult lastResult = new SimulatorResult();

            foreach (int page in referenceString)
            {
                SimulatorResult stepResult = AccessPage(page);
                lastResult.FrameHistory.Add(stepResult.FrameHistory[0]);
                lastResult.TotalAccesses = stepResult.TotalAccesses;
                lastResult.PageFaults = stepResult.PageFaults;
                lastResult.HitRate = stepResult.HitRate;
                lastResult.MissRate = stepResult.MissRate;
    }

            return lastResult!;
        }

        public int? GetFrame(int idx)
        {
            if (idx < 0 || idx >= Frames.Count)
                return null;

            return Frames[idx].PageId;
        }

    }

}
