using Pomoberry.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pomoberry.UI
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private static Models.HomeViewModel TimerData;    //Non nullable???
        public Home()
        {
            InitializeComponent();
            TimerData = new Models.HomeViewModel();
            DataContext = TimerData;
        }

        private void NewTimerButton_Click(object sender, RoutedEventArgs e)
        {
            NewTimer newTimer = new NewTimer();
            newTimer.Show();
        }

        public static void AddTimer(int WorkMinutes, int BreakMinutes, int Sessions)          // Access AddTimer method in TimerData
        {
            TimerData.AddTimer(WorkMinutes, BreakMinutes, Sessions);
            TimerData.SaveTimers(); //Save Timers to File

        }


    }
}
