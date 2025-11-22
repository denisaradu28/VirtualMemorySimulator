using System;
 using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VirtualMemorySimulation
{
    public static class DataPaths
    {
        public static string RunsFolder
        {
            get
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VirtualMemorySimulation", "Runs");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public static string ChartsFolder
        {
            get
            {
                string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VirtualMemorySimulation", "Charts");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;

            }
        }
    }

    public static class RunFileHelper
    {
        public static int GetNextRunIndex()
        {
            string folder = DataPaths.RunsFolder;

            var files = Directory.GetFiles(folder, "run_*.csv");
            int maxIdx = 0;

            foreach (var file in files)
            {

                string name = Path.GetFileNameWithoutExtension(file);
                if (name.StartsWith("run_"))
                {
                    string numPart = name.Substring(4);
                    if (int.TryParse(numPart, out int idx))
                    {
                        if (idx > maxIdx)
                            maxIdx = idx;
                    }
                }
            }
            return maxIdx + 1;
        }
    }
}
