namespace VirtualMemorySimulation
{
    public class PageTableEntry
    {
        public int PageId { get; set; }
        public bool Valid { get; set; }
        public int? FrameIdx { get; set; }

        public PageTableEntry(int pageId)
        {
            PageId = pageId;
            Valid = false;
            FrameIdx = null;
        }

        public override string ToString()
        {
            return $"Page {PageId} -> {(Valid ? $"Frame {FrameIdx}" : "Not in memory")}";
        }
    }
}