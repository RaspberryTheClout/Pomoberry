using Pomoberry.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pomoberry.UI.Models
{
    /// <summary>
    /// Model for timers in the TimerListBox in the home window.
    /// </summary>
    public class TimerModel
    {
        public int WorkMinutes { get; set; }
        public int BreakMinutes { get; set; }
        public int TotalSessions { get; set; }

        public ICommand? StartCommand { get; set; }        // Interface used for calling StartSession
        
        public void StartSession()   // open a new timer window with given parameters
        {
            TimerWindow timerwindow = new TimerWindow(WorkMinutes, BreakMinutes, TotalSessions);
            timerwindow.Show();
        }
    }
}
