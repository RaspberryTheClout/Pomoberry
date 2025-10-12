using Pomoberry.Core;
using System.Windows;

namespace Pomoberry.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PomodoroSessionManager _manager;

        public MainWindow()
        {
            InitializeComponent();
            _manager = new PomodoroSessionManager();

            // Subscribe to events from the core logic
            _manager.TimeChanged += Manager_TimeChanged;
            _manager.SessionStarted += Manager_SessionStarted;
        }

        private void Manager_SessionStarted(object? sender, SessionEventArgs e)
        {
            // The timer's events run on a background thread.
            // To update the UI, we must use the Dispatcher to send the work to the UI thread.
            Application.Current.Dispatcher.Invoke(() =>
            {
                SessionDisplay.Text = e.SessionType.ToString();
            });
        }

        private void Manager_TimeChanged(object? sender, TimeChangedEventArgs e)
        {
            // Use the Dispatcher here as well for thread safety.
            Application.Current.Dispatcher.Invoke(() =>
            {
                TimeDisplay.Text = e.TimeLeft.ToString(@"mm\:ss");
            });
        }

        // --- Button Click Handlers ---

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Pause();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Reset();
        }
    }
}