using System.Linq;
using System.Windows;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace VirtualMemorySimulation.UI
{
    public partial class ChartsWindow : Window
    {
        public ChartsWindow()
        {
            InitializeComponent();
            LoadCharts();
        }

        private void LoadCharts()
        {
            var runs = CsvLoader.LoadAllRuns();

            var fifo = runs.Where(r => r.Algorithm == "FIFO").ToList();
            var lru = runs.Where(r => r.Algorithm == "LRU").ToList();
            var opt = runs.Where(r => r.Algorithm == "Optimal").ToList();

            if (runs.Count == 0)
            {
                MessageBox.Show("Nu există date salvate.");
                return;
            }

            DrawPageFaults(fifo, lru, opt);
            DrawFaultRate(fifo, lru, opt);
            DrawAMAT(fifo, lru, opt);
            DrawMemoryUsage(fifo, lru, opt);
        }

        private void DrawPageFaults(params List<RunSummary>[] algSets)
        {
            var model = new PlotModel { Title = "Average Page Faults" };

            var labels = new[] { "FIFO", "LRU", "OPT" };

            var series = new BarSeries
            {
                ItemsSource = new[]
                {
                    new BarItem(algSets[0].Count > 0 ? algSets[0].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[1].Count > 0 ? algSets[1].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[2].Count > 0 ? algSets[2].Average(a => a.PageFaults) : 0)
                }
            };

            model.Series.Add(series);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = labels
            });

            model.Axes.Add(new LinearAxis
            {
                    Position = AxisPosition.Bottom,
                    Minimum = 0
            });

            PageFaultsPlot.Model = model;
        }

        private void DrawFaultRate(params List<RunSummary>[] algSets)
        {
            var model = new PlotModel { Title = "Average Fault Rate (%)" };

            var labels = new[] { "FIFO", "LRU", "OPT" };

            var series = new BarSeries
            {
                ItemsSource = new[]
                {
                    new BarItem(algSets[0].Count > 0 ? algSets[0].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[1].Count > 0 ? algSets[1].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[2].Count > 0 ? algSets[2].Average(a => a.PageFaults) : 0)
                }
            };
            model.Series.Add(series);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = labels
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0
            });

            FaultRatePlot.Model = model;
        }

        private void DrawAMAT(params List<RunSummary>[] algSets)
        {
            var model = new PlotModel { Title = "Average AMAT" };

            var labels = new[] { "FIFO", "LRU", "OPT" };

            var series = new BarSeries
            {
                ItemsSource = new[]
                {
                    new BarItem(algSets[0].Count > 0 ? algSets[0].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[1].Count > 0 ? algSets[1].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[2].Count > 0 ? algSets[2].Average(a => a.PageFaults) : 0)
                }
            };
            model.Series.Add(series);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = labels
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0
            });

            AmatPlot.Model = model;
        }

        private void DrawMemoryUsage(params List<RunSummary>[] algSets)
        {
            var model = new PlotModel { Title = "Average Memory Usage (%)" };

            var labels = new[] { "FIFO", "LRU", "OPT" };

            var series = new BarSeries
            {
                ItemsSource = new[]
                {
                    new BarItem(algSets[0].Count > 0 ? algSets[0].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[1].Count > 0 ? algSets[1].Average(a => a.PageFaults) : 0),
                    new BarItem(algSets[2].Count > 0 ? algSets[2].Average(a => a.PageFaults) : 0)
                }
            };
            model.Series.Add(series);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = labels
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0
            });

            MemoryUsagePlot.Model = model;
        }
    }
}
