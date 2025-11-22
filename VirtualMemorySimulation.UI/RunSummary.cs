using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VirtualMemorySimulation.UI
{
    public class RunSummary
    {
        public string Algorithm { get; set; } = "";
        public int PageFaults { get; set; }
        public int TotalAccesses { get; set; }
        public double FaultRate { get; set; }
        public double AMAT { get; set; }
        public double MemoryUsagePercent { get; set; }
    }

    public static class CsvLoader
    {
        public static List<RunSummary> LoadAllRuns()
        {
            List<RunSummary> list = new();

            string folder = DataPaths.RunsFolder;
            if (!Directory.Exists(folder))
                return list;

            var files = Directory.GetFiles(folder, "run_*.csv");

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                if (lines.Length < 6) continue;

                string alg = lines[0].Split(',')[1].Trim();
                int faults = int.Parse(lines[1].Split(',')[1]);
                int accesses = int.Parse(lines[2].Split(',')[1]);
                double faultRate = double.Parse(lines[3].Split(',')[1]);
                double amat = double.Parse(lines[4].Split(',')[1]);
                double usage = double.Parse(lines[5].Split(',')[1]);

                list.Add(new RunSummary
                {
                    Algorithm = alg,
                    PageFaults = faults,
                    TotalAccesses = accesses,
                    FaultRate = faultRate,
                    AMAT = amat,
                    MemoryUsagePercent = usage
                });
            }

            return list;
        }
    }
}
