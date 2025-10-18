using System.Windows.Input;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public ICommand? StartCommand { get; set; }        // Interface used for calling StartSession
        
        public void StartSession()   // open a new timer window with given parameters
        {
            TimerWindow timerwindow = new TimerWindow(WorkMinutes, BreakMinutes, TotalSessions);
            timerwindow.Show();
        }

        [JsonIgnore]
        public ICommand? DeleteCommand { get; set; }        // Interface used for calling StartSession


        // Constructor
        public TimerModel(int workMinutes, int breakMinutes, int totalSessions)
        {
            WorkMinutes = workMinutes;
            BreakMinutes = breakMinutes;
            TotalSessions = totalSessions;
        }
    }
}
