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
using System.Windows.Shapes;

namespace Pomoberry.UI
{
    /// <summary>
    /// Interaction logic for NewTimer.xaml
    /// </summary>
    public partial class NewTimer : Window
    {
        public NewTimer()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int WorkMinutes = Convert.ToInt32(WorkMinutesTxtBox.Text);
            int BreakMinutes = Convert.ToInt32(BreakMinutesTxtBox.Text);
            int Sessions = Convert.ToInt32(SessionsTxtBox.Text);

            Home.AddTimer(WorkMinutes, BreakMinutes, Sessions);  // call the add timer method in home
            Close();
        }
    }
}
