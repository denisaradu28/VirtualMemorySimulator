using System;
using System.Collections.Generic;
using System.Globalization;


namespace VirtualMemorySimulation
{
    public class OptimalAlgorithm : IPageReplacementAlgorithm
    {
        public int FrameCount { get; private set; }
        public List<Frame> Frames { get; private set; }
        public Dictionary<int, PageTableEntry> PageTable { get; private set; }

        public int TotalAccesses { get; private set; }
        public int PageFaults { get; private set; }
        public double HitRate => TotalAccesses == 0 ? 0 : (double)(TotalAccesses - PageFaults) / TotalAccesses;
        public double MissRate => 1 - HitRate;

        private int[]? referenceString;

        private int crtTime = 0;
    
        public OptimalAlgorithm(int frameCount)
        {
            FrameCount = frameCount;
            Frames = new List<Frame>();
            for (int i = 0; i < frameCount; i++)
            {
                Frames.Add(new Frame(i));
            }

            PageTable = new Dictionary<int, PageTableEntry>();
            TotalAccesses = 0;
            PageFaults = 0;
        }

        public SimulatorResult AccessPage(int pageId)
        {
            throw new InvalidOperationException("Optimal needs a reference string to work");
        }

        public SimulatorResult Run(int framesCount, int[] referenceString)
        {
            FrameCount = framesCount;
            this.referenceString = referenceString;

            Frames = new List<Frame>();
            for (int i = 0; i < framesCount; i++)
                Frames.Add(new Frame(i));

            PageTable.Clear();
            TotalAccesses = 0;
            PageFaults = 0;
            crtTime = 0;

            SimulatorResult lastResult = new SimulatorResult();

            for (int i = 0; i < referenceString.Length; i++)
            {
                int page = referenceString[i];
                TotalAccesses++;

                bool wasHit = PageTable.ContainsKey(page) && PageTable[page].Valid;
                int? evictedPageId = null;

                crtTime++;

                if(wasHit)
                {
                    int frameIdx = PageTable[page].FrameIdx ?? 0;
                    Frames[frameIdx].LastUsedOrder = crtTime;
                }
                else
                {
                    PageFaults++;

                    int freeIdx = FindFreeFrame();
                    if (freeIdx != -1)
                    {
                        Frames[freeIdx].PageId = page;
                        Frames[freeIdx].LastUsedOrder = crtTime;

                        PageTable[page] = new PageTableEntry(page)
                        {
                            Valid = true,
                            FrameIdx = freeIdx
                        };
                    }
                    else
                    {
                        int pageToReplace = FindPageToReplace(i + 1);

                        int victimFrameIdx = PageTable[pageToReplace].FrameIdx ?? 0;
                        evictedPageId = pageToReplace;

                        PageTable[pageToReplace].Valid = false;
                        PageTable[pageToReplace].FrameIdx = null;

                        Frames[victimFrameIdx].PageId = page;
                        Frames[victimFrameIdx].LastUsedOrder = crtTime;
                        PageTable[page] = new PageTableEntry(page)
                        {
                            Valid = true,
                            FrameIdx = victimFrameIdx
                        };
                    }
                }

                List<int?> snapshot = Frames.Select(f => f.PageId).ToList();
                lastResult.FrameHistory.Add(snapshot);
                lastResult.ReferenceString.Add(page);
                lastResult.IsFault.Add(!wasHit);

            }
            
            lastResult.TotalAccesses = TotalAccesses;
            lastResult.PageFaults = PageFaults;
            lastResult.HitRate = HitRate;
            lastResult.MissRate = MissRate;

            lastResult.FrameSnapshot = new List<Frame>();
            foreach (var f in Frames)
            {
                lastResult.FrameSnapshot.Add(new Frame(f.Index)
                {
                    PageId = f.PageId,
                    LastUsedOrder = f.LastUsedOrder,
                    LoadOrder = f.LoadOrder
                });
            }


            return lastResult;
        }

        public int? GetFrame(int idx)
        {
            if (idx < 0 || idx >= Frames.Count)
                return null;

            return Frames[idx].PageId;
        }

        private int FindFreeFrame()
        {
            for (int i = 0; i < Frames.Count; i++)
            {
                if (!Frames[i].isValid)
                    return i;
            }
            return -1;
        }

        private int FindPageToReplace(int startIdx)
        {
            if(startIdx >= referenceString!.Length)
            {
                int oldest = int.MaxValue;
                int lruPage = -1;

                foreach(var frame in Frames)
                {
                    if(frame.PageId.HasValue && frame.LastUsedOrder < oldest)
                    {
                        oldest = frame.LastUsedOrder;
                        lruPage = frame.PageId.Value;
                    }
                }
                return lruPage;
            }



            int farthestUse = -1;
            int pageToReplace = -1;

            for (int i = 0; i < Frames.Count; i++)
            {
                int? crtPage = Frames[i].PageId;
                if (!crtPage.HasValue)
                    continue;

                int nextUse = -1;
                for (int j = startIdx; j < referenceString!.Length; j++)
                {
                    if (referenceString[j] == crtPage.Value)
                    {
                        nextUse = j;
                        break;
                    }
                }

                if (nextUse == -1)
                    return crtPage.Value;

                if (nextUse > farthestUse)
                {
                    farthestUse = nextUse;
                    pageToReplace = crtPage.Value;
                }
            }
            return pageToReplace;
        }

    }
}