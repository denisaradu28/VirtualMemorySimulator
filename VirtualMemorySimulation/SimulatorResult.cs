using System.Collections.Generic;

namespace VirtualMemorySimulation
{
    public class SimulatorResult
    {
        public int PageId { get; set; }
        public bool WasHit { get; set; }
        public int? EvictedPageId { get; set; }
        public List<Frame> FrameSnapshot { get; set; } = new List<Frame>();
        public int TotalAccesses { get; set; }
        public int PageFaults { get; set; }
        public double HitRate { get; set; }
        public double MissRate { get; set; }

        public List<List<int?>> FrameHistory { get; set; } = new List<List<int?>>();

        public override string ToString()
        {
            string evicted = EvictedPageId.HasValue ? $"(Evicted {EvictedPageId})" : "";
            return $"{PageId}: {(WasHit ? "Hit" : "Fault")} {evicted}";
        }
    }
}
