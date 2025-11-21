using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualMemorySimulation.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int[] exampleRef = { 2, 3, 2, 0, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 4, 3, 2, 0 };
        private const int exampleFrames = 3;
        public MainWindow()
        {
            InitializeComponent();

            TryExampleRB.Checked += ModeChanged;
            UserInputRB.Checked += ModeChanged;

            TryExampleRB.IsChecked = true;
        }

        private void ModeChanged(object sender, RoutedEventArgs e)
        {
            if (TryExampleRB.IsChecked == true)
            {
                ExamplePanel.Visibility = Visibility.Visible;
                UserInputPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                ExamplePanel.Visibility = Visibility.Collapsed;
                UserInputPanel.Visibility = Visibility.Visible;
            }
        }

        private void RunSimulation(object sender, RoutedEventArgs e)
        {
            try
            {

                int[] referenceString;
                int frames;


                if (TryExampleRB.IsChecked == true)
                {
                    referenceString = exampleRef;
                    frames = exampleFrames;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ReferenceStringBox.Text) || string.IsNullOrWhiteSpace(FramesBox.Text))
                    {
                        MessageBox.Show("Please fill all fields.");
                        return;
                    }
                    try
                    {
                        referenceString = ReferenceStringBox.Text.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                        frames = int.Parse(FramesBox.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid input.");
                        return;
                    }
                }
                IPageReplacementAlgorithm algorithm = AlgorithmBox.SelectedIndex switch
                {
                    0 => new FifoAlgorithm(frames),
                    1 => new LRUAlgorithm(frames),
                    2 => new OptimalAlgorithm(frames),
                    _ => new FifoAlgorithm(frames)
                };

                var result = algorithm.Run(frames, referenceString);



                var win = new ResultsWindow(
                    AlgorithmBox.Text,
                    frames,
                    result
                );

                win.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}