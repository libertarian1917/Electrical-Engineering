using System.Windows;

namespace FrequencyTimer
{
    /// <summary>
    /// MainWindow for FrequencyTimer
    /// </summary>
    public partial class MainWindow : Window
    {

        CancellationTokenSource cts;

        public MainWindow()
        {
            InitializeComponent();
        }



        private async void Button_Start(object sender, RoutedEventArgs e)
        {
            int hz;
            try
            {
                hz = int.Parse(Input.Text);
                if (hz > 1000 || hz < 1) throw new Exception();
            }
            catch
            {
                Result.Content = "Invalid input";
                return;
            }
            int sleepDuration = 1000 / hz;
            int count = 1;


            StopButton.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Hidden;
            cts = new CancellationTokenSource();

            while (true)
            {
                if (count > hz) count = 1;
                Task.Delay(sleepDuration);
                Result.Content = count;
                count += 1;

                try {
                    await Task.Delay(sleepDuration, cts.Token);
                }
                catch (TaskCanceledException) {
                    break;
                }
                if (cts.Token.IsCancellationRequested) { break; }
            }
            Result.Content = "";
        }

        private async void Button_Stop(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            StopButton.Visibility = Visibility.Hidden;
            StartButton.Visibility = Visibility.Visible;
        }


    }
}