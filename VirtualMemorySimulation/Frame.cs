namespace VirtualMemorySimulation
{
    public class Frame
    {
        public int Index { get; set; }
        public int? PageId { get; set; }
        public bool isValid => PageId.HasValue;
        public int LoadOrder { get; set; }
        public int LastUsedOrder { get; set; }

        public Frame(int idx)
        {
            Index = idx;
            PageId = null;
            LastUsedOrder = -1;
            LoadOrder = -1;
        }

        public override string ToString()
        {
            return PageId?.ToString() ?? "-";
        }

    }
}