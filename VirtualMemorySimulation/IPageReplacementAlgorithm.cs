namespace VirtualMemorySimulation
{
    public interface IPageReplacementAlgorithm
    {
        public SimulatorResult Run(int framesCount, int[] referenceString);
    }
}