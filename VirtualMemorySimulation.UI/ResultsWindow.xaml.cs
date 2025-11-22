using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VirtualMemorySimulation;
using System.Windows.Threading;
using System.Windows.Controls;
using System.IO;

namespace VirtualMemorySimulation.UI
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private readonly SimulatorResult _result;
        private readonly int _framesCount;

        private readonly DispatcherTimer _timer;
        private int _crtStep = -1;
        private bool _isPlaying = false;

        private List<int?>? _previousSnapshot;
        public ResultsWindow(string algorithmName, int frames, SimulatorResult result)
        {
            InitializeComponent();

            AlgorithmLabel.Text = algorithmName;
            FramesLabel.Text = frames.ToString();
            RefStringLabel.Text = "Reference String: " + string.Join(", ", result.ReferenceString);

            _result = result;
            _framesCount = frames;

            BuildFramesUI();
            ComputePerformanceIndicators();
            SaveRunSummaryToCsv();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(900)
            };
            _timer.Tick += Timer_Tick;

            if (_result.FrameHistory.Count > 0)
            {
                GoToStep(0);
            }
        }


        private void BuildFramesUI()
        {
            FramesPanel.Children.Clear();

            for (int i = 0; i < _framesCount; i++)
            {
                var border = new Border
                {
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(8),
                    CornerRadius = new CornerRadius(10),
                    Background = Brushes.White,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(230, 200, 240)),
                    BorderThickness = new Thickness(2)
                };

                var text = new TextBlock
                {
                    Text = "-",
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(74, 20, 140)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                border.Child = text;
                FramesPanel.Children.Add(border);
            }
        }

        private void ComputePerformanceIndicators()
        {
            PerfPageFaults.Text = _result.PageFaults.ToString();

            double faultRate = _result.MissRate * 100.0;
            PerfFaultRate.Text = faultRate.ToString("F2") + "%";

            double T_ram = 1.0;
            double T_penalty = 100.0;
            double amat = T_ram + _result.MissRate * T_penalty;
            PerfAMAT.Text = amat.ToString("F2");

            int lastStep = _result.FrameHistory.Count - 1;
            int usedFrames = _result.FrameHistory[lastStep].Count(v => v.HasValue);
            double util = (double)usedFrames / _framesCount * 100.0;
            PerfMemoryUsage.Text = util.ToString("F1") + "%";

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_crtStep < _result.FrameHistory.Count - 1)
            {
                GoToStep(_crtStep + 1);
            }
            else
            {
                _timer.Stop();
                _isPlaying = false;
            }
        }

        private void GoToStep(int idx)
        {
            if (idx < 0 || idx >= _result.FrameHistory.Count)
                return;

            var snapshot = _result.FrameHistory[idx];
            bool isFault = idx < _result.IsFault.Count && _result.IsFault[idx];
            int page = idx < _result.ReferenceString.Count ? _result.ReferenceString[idx] : -1;

            _crtStep = idx;

            StepLabel.Text = $"{idx + 1} / {_result.FrameHistory.Count}";
            PageLabel.Text = page >= 0 ? page.ToString() : "-";
            StatusLabel.Text = isFault ? "MISS" : "HIT";
            StatusLabel.Foreground = isFault ? Brushes.IndianRed : Brushes.ForestGreen;

            for (int i = 0; i < _framesCount && i < snapshot.Count; i++)
            {
                var border = FramesPanel.Children[i] as Border;
                if (border == null) continue;

                var text = border.Child as TextBlock;
                if (text == null) continue;

                text.Text = snapshot[i]?.ToString() ?? "-";

            }

            if (_previousSnapshot == null)
            {
                for (int i = 0; i < _framesCount && i < snapshot.Count; i++)
                {
                    var border = FramesPanel.Children[i] as Border;
                    if (border != null)
                        AnimateFrameBorder(border, isFault);
                }
            }
            else
            {
                for (int i = 0; i < _framesCount && i < snapshot.Count; i++)
                {
                    int? prev = i < _previousSnapshot.Count ? _previousSnapshot[i] : null;
                    int? crt = snapshot[i];

                    if (prev != crt)
                    {
                        var border = FramesPanel.Children[i] as Border;
                        if (border != null)
                            AnimateFrameBorder(border, isFault);
                    }
                    else if (!isFault)
                    {
                        var border = FramesPanel.Children[i] as Border;
                        if (border != null)
                            AnimateHit(border);
                    }
                }
            }

            _previousSnapshot = new List<int?>(snapshot);
        }

        private void AnimateHit(Border border)
        {
            Color hitColor = Color.FromRgb(204, 255, 204);

            var anim = new ColorAnimation
            {
                From = hitColor,
                To = Colors.White,
                Duration = TimeSpan.FromSeconds(0.4)
            };

            var brush = new SolidColorBrush(hitColor);
            border.Background = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
        }
        private void AnimateFrameBorder(Border border, bool isFault)
        {
            Color fromColor = isFault ? Color.FromRgb(255, 204, 203) : Color.FromRgb(204, 255, 204);

            var anim = new ColorAnimation
            {
                From = fromColor,
                To = Colors.White,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            var brush = new SolidColorBrush(fromColor);
            border.Background = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, anim);

        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (_result.FrameHistory.Count == 0)
                return;

            if(!_isPlaying)
            {
                _isPlaying = true;
                _timer.Start();
            }
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();
        }

        private void NextStepClick(object sender, RoutedEventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();

            int next = _crtStep + 1;
            if(next < _result.FrameHistory.Count)
            {
                GoToStep(next);
            }
        }

        private void PrevStepClick(object sender, RoutedEventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();

            int prev = _crtStep - 1;
            if(prev >= 0)
                GoToStep(prev);
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();
            _previousSnapshot = null;

            if (_result.FrameHistory.Count > 0)
                GoToStep(0);
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChartsClick(object sender, RoutedEventArgs e)
        {
            var win = new ChartsWindow();
            win.Show();
        }


        private void SaveRunSummaryToCsv()
        {
            string alg = AlgorithmLabel.Text;

            double faultRate = _result.MissRate;
            double amat = 1 + faultRate * 100;

            var finalSnapshot = _result.FrameHistory.Last();
            int usedFrames = finalSnapshot.Count(f => f.HasValue);
            double memUsage = (double)usedFrames / _framesCount * 100.0;

            string folder = DataPaths.RunsFolder;

            int index = Directory.GetFiles(folder, $"run_{alg}_*.csv").Length + 1;
            string file = Path.Combine(folder, $"run_{alg}_{index}.csv");

            using (var w = new StreamWriter(file))
            {
                w.WriteLine($"Algorithm,{alg}");
                w.WriteLine($"PageFaults,{_result.PageFaults}");
                w.WriteLine($"TotalAccesses,{_result.TotalAccesses}");
                w.WriteLine($"FaultRate,{faultRate}");
                w.WriteLine($"AMAT,{amat}");
                w.WriteLine($"MemoryUsage,{memUsage}");
            }
        }

    }
}
