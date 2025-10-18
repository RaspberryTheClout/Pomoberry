using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Pomoberry.UI.Models
{
    public class HomeViewModel
    {
        /// <summary>
        /// Model for the TimersListBox
        /// </summary>
        /// 

        public string timersFile = "UserData/timers.json";

        // Function to load the saved timers from storage.
        public void LoadTimers()
        {
            
            if (File.Exists(timersFile))
            {
                // Read json file and convert to the collection format
                var json = File.ReadAllText(timersFile);
                var timers = JsonSerializer.Deserialize<ObservableCollection<TimerModel>>(json);
                if (timers != null)
                {
                    Timers.Clear();
                    // Add the timers to the list
                    foreach (var timer in timers) { Timers.Add(timer); }
                }
            }
        }

        // Save the current timers collection to storage
        public void SaveTimers()
        {
            var json = JsonSerializer.Serialize(Timers);
            Directory.CreateDirectory(Path.GetDirectoryName(timersFile)!);
            File.WriteAllText(timersFile, json);
        }

        public void AddTimer(int WorkMinutes, int BreakMinutes, int Sessions)          // to add a timer using new timer window
        {
            TimerModel timer = new TimerModel(WorkMinutes, BreakMinutes, Sessions);
            Timers.Add(timer);
            timer.StartCommand = new RelayCommand(_ => timer.StartSession());
            timer.DeleteCommand = new RelayCommand(_ => DeleteTimer(timer));
        }

        public void DeleteTimer(TimerModel timer)          // to add a timer using new timer window
        {
            Timers.Remove(timer);
            SaveTimers();   // write the change to the jason file
        }


        public ObservableCollection<TimerModel> Timers { get; set; } = new();

        // Constructor
        public HomeViewModel()
        {
            LoadTimers();   // Load timers from storage
            // Example: Add a sample timer
            Timers.Add(new TimerModel(25,5,4 ));
            Timers.Add(new TimerModel(30,7,3));

            // Assign Relay Command that calls StartSession for each timer
            foreach (var timer in Timers) 
            {
                timer.StartCommand = new RelayCommand(_ => timer.StartSession());  //lambda function
                timer.DeleteCommand = new RelayCommand(_ => DeleteTimer(timer));  // Bind the delete timer method in this class to the delete timer interface in the timer model
            }
            //SaveTimers();
        }

        //private void StartTimer(object parameter)
        //{
            // Timer start logic here
        //}
    }
}
