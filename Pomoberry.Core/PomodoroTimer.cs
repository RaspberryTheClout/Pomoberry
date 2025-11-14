using System;
using System.Timers;

namespace Pomoberry.Core
{
    // Enum to indicate whether the session is Work or Break
    public enum SessionType { Work, Break }

    // Event args for sending the remaining time to UI
    public class TimeChangedEventArgs : EventArgs
    {
        public TimeSpan TimeLeft { get; }
        public TimeChangedEventArgs(TimeSpan timeLeft) => TimeLeft = timeLeft;
    }

    // Event args for when a session starts
    public class SessionEventArgs : EventArgs
    {
        public SessionType SessionType { get; }
        public int CurrentSession { get; }

        public SessionEventArgs(SessionType sessionType, int currentSession)
        {
            SessionType = sessionType;
            CurrentSession = currentSession;
        }
    }

    // Core Pomodoro timer logic
    public class PomodoroSessionManager : IDisposable
    {
        /// <summary>
        /// **This is the SessionManager class that handles the core logic of the Pomodoro timer.**
        /// </summary>

        private readonly System.Timers.Timer _timer; // System.Timers.Timer runs in a thread pool thread
        private readonly AudioManager _audioManager; // Manages audio playback (implemented elsewhere)

        // User-configurable settings
        public int WorkMinutes { get; private set; }
        public int BreakMinutes { get; private set; }
        public int TotalSessions { get; private set; }

        // State tracking
        public int CurrentSession { get; private set; }
        public SessionType CurrentSessionType { get; private set; }
        public TimeSpan TimeLeft { get; private set; }

        // Events to notify subscribers (UI)
        public event EventHandler<TimeChangedEventArgs>? TimeChanged; // fires every tick
        public event EventHandler<SessionEventArgs>? SessionStarted;  // fires when session switches
        public event EventHandler? AllSessionsCompleted;             // fires when all sessions finished

        public bool IsRunning => _timer.Enabled;

        // Constructor
        public PomodoroSessionManager(int workMinutes = 25, int breakMinutes = 5, int totalSessions = 4)
        {
            _audioManager = new AudioManager("lofi-loop.wav"); // Initialize audio manager 

            // Ensure valid values
            WorkMinutes = Math.Max(1, workMinutes);
            BreakMinutes = Math.Max(0, breakMinutes);
            TotalSessions = Math.Max(1, totalSessions);

            // Create timer, 1-second interval
            _timer = new System.Timers.Timer(1000)  // 1 second interval
            {
                AutoReset = true
            };
            _timer.Elapsed += Timer_Elapsed;

            // Initialize first session
            Reset();
        }

        // This is a new seperate constructor specifically for allowing unit tests to use short TimeSpans
        internal PomodoroSessionManager(TimeSpan workTime, TimeSpan breakTime, int totalSessions)
        {
            WorkMinutes = (int)workTime.TotalMinutes; // Still set these for compatibility
            BreakMinutes = (int)breakTime.TotalMinutes;
            TotalSessions = Math.Max(1, totalSessions);

            _timer = new System.Timers.Timer(100) { AutoReset = true }; // Use a faster tick for tests
            _timer.Elapsed += Timer_Elapsed;

            Reset(); // Call your existing Reset method

            // Override initial TimeLeft with the precise TimeSpan for testing
            TimeLeft = workTime;
        }

        // Start timer
        public void Start()
        {
            if (CurrentSession > TotalSessions) return; // all sessions completed
            _timer.Start();
            _audioManager.PlayMusic(); // Start background music

            // Immediately notify UI so it shows the current time
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(TimeLeft));
        }

        // Pause timer
        public void Pause()
        {  
            _timer.Stop();
            _audioManager.StopMusic(); // Pause background music
        }

        // Reset timer to first session
        public void Reset()
        {
            _timer.Stop(); // stop timer if running
            _audioManager.StopMusic(); // Stop background music
            CurrentSession = 1;
            CurrentSessionType = SessionType.Work;
            TimeLeft = TimeSpan.FromMinutes(WorkMinutes);

            // Notify UI of initial state
            SessionStarted?.Invoke(this, new SessionEventArgs(CurrentSessionType, CurrentSession));
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(TimeLeft));
        }

        // Update settings dynamically
        public void UpdateSettings(int? workMinutes = null, int? breakMinutes = null, int? totalSessions = null)
        {
            if (workMinutes.HasValue) WorkMinutes = Math.Max(1, workMinutes.Value);
            if (breakMinutes.HasValue) BreakMinutes = Math.Max(0, breakMinutes.Value);
            if (totalSessions.HasValue) TotalSessions = Math.Max(1, totalSessions.Value);

            // If timer is paused, update displayed time
            if (!IsRunning)
            {
                TimeLeft = CurrentSessionType == SessionType.Work
                    ? TimeSpan.FromMinutes(WorkMinutes)
                    : TimeSpan.FromMinutes(BreakMinutes);
            }

            TimeChanged?.Invoke(this, new TimeChangedEventArgs(TimeLeft));
        }

        // Called on every timer tick
        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            // Subtract 1 second
            TimeLeft = TimeLeft - TimeSpan.FromSeconds(1);

            // If session ended, switch
            if (TimeLeft.TotalSeconds <= 0)
            {
                SwitchSession();
            }

            // Notify UI of remaining time
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(TimeLeft));
        }

        // Switch between work and break sessions
        private void SwitchSession()
        {
            if (CurrentSessionType == SessionType.Work)
            {
                // Switch to break
                CurrentSessionType = SessionType.Break;
                _audioManager.StopMusic(); // Stop music during break
                TimeLeft = TimeSpan.FromMinutes(BreakMinutes);
                SessionStarted?.Invoke(this, new SessionEventArgs(CurrentSessionType, CurrentSession));
            }
            else
            {
                // Break finished → move to next work session
                CurrentSession++;
                if (CurrentSession > TotalSessions)
                {
                    _timer.Stop(); // All sessions completed
                    AllSessionsCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }

                CurrentSessionType = SessionType.Work;
                _audioManager.PlayMusic(); // Resume music for work session
                TimeLeft = TimeSpan.FromMinutes(WorkMinutes);
                SessionStarted?.Invoke(this, new SessionEventArgs(CurrentSessionType, CurrentSession));
            }
        }

        // Dispose timer
        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
