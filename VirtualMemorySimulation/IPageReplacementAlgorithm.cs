using System.Runtime.Versioning;

namespace VirtualMemorySimulation
{
    public interface IPageReplacementAlgorithm
    {
        int FrameCount { get; }
        int TotalAccesses { get; }
        int PageFaults { get; }
        double HitRate { get; }
        double MissRate { get; }

        List<Frame> Frames { get; }
        Dictionary<int, PageTableEntry> pageTable { get; }

        SimulatorResult AccessPage(int pageId); //ruleaza paginile pe rand (simularea din interfata)
        int? GetFrame(int idx); 
        public SimulatorResult Run(int framesCount, int[] referenceString); //ruleaza tot odata (simulare consola)
    }
}