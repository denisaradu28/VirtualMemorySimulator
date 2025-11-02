using System.Collections.Generic;

namespace VirtualMemorySimulation
{
    public class SimulatorResult
    {
        public int Faults { get; set; } = 0;
        public int Hits { get; set; } = 0;
        public List<bool> IsFault { get; } = new();
        public List<int?[]> FramesHistory { get; } = new();
    }
}
