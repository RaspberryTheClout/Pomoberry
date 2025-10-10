using Pomoberry.Core; // Allows to use the SessionType enum and PomodoroSessionManager class
using System.Threading.Tasks; // For async tests

namespace Pomoberry.Core.Tests
{
    /// <summary>
    /// Contains unit tests for the PomodoroSessionManager class.
    /// </summary>
    
    [TestClass]
    public class PomodoroSessionManagerTests
    {
        /// <summary>
        /// Verifies that the PomodoroSessionManager constructor correctly initializes
        /// the timer's state to the default starting values.
        /// </summary>
        [TestMethod]
        public void Constructor_WhenCalled_InitializesStateCorrectly()
        {
            // Arrange
            int workMinutes = 25;
            int breakMinutes = 5;
            int totalSessions = 4;

            // Act
            var manager = new PomodoroSessionManager(workMinutes, breakMinutes, totalSessions);

            // Assert
            Assert.AreEqual(1, manager.CurrentSession);
            Assert.AreEqual(SessionType.Work, manager.CurrentSessionType);
            Assert.AreEqual(TimeSpan.FromMinutes(workMinutes), manager.TimeLeft);
        }

        /// <summary>
        /// Verifies that calling the Start method correctly sets the
        /// IsRunning property to true.
        /// </summary>
        [TestMethod]
        public void Start_WhenCalled_SetsIsRunningToTrue()
        {
            // Arrange
            var manager = new PomodoroSessionManager();

            // Act
            manager.Start();

            // Assert
            Assert.IsTrue(manager.IsRunning, "IsRunning should be true after starting the timer.");
        }

        /// <summary>
        /// Verifies that calling the Pause method on a running timer
        /// correctly sets the IsRunning property to false.
        /// </summary>
        [TestMethod]
        public void Pause_WhenCalled_SetsIsRunningToFalse()
        {
            // Arrange
            var manager = new PomodoroSessionManager();
            manager.Start(); // Timer must be running before it can be paused

            // Act
            manager.Pause();

            // Assert
            Assert.IsFalse(manager.IsRunning, "IsRunning should be false after pausing the timer.");
        }

        /// <summary>
        /// Verifies that the UpdateSettings method correctly changes the
        /// session configuration properties.
        /// </summary>
        [TestMethod]
        public void UpdateSettings_WithNewValues_CorrectlyUpdatesProperties()
        {
            // Arrange
            var manager = new PomodoroSessionManager(); // Starts with default values
            int newWorkMinutes = 30;
            int newBreakMinutes = 10;
            int newTotalSessions = 8;

            // Act
            manager.UpdateSettings(newWorkMinutes, newBreakMinutes, newTotalSessions);

            // Assert
            Assert.AreEqual(newWorkMinutes, manager.WorkMinutes);
            Assert.AreEqual(newBreakMinutes, manager.BreakMinutes);
            Assert.AreEqual(newTotalSessions, manager.TotalSessions);
        }

        /// <summary>
        /// Verifies that the UpdateSettings method clamps invalid (negative)
        /// values to their allowed minimums.
        /// </summary>
        [TestMethod]
        public void UpdateSettings_WithInvalidValues_ClampsToMinimums()
        {
            // Arrange
            var manager = new PomodoroSessionManager();

            // Act
            manager.UpdateSettings(workMinutes: -5, breakMinutes: -10);

            // Assert
            Assert.AreEqual(1, manager.WorkMinutes, "Work minutes should be clamped to a minimum of 1.");
            Assert.AreEqual(0, manager.BreakMinutes, "Break minutes should be clamped to a minimum of 0.");
        }

        /// <summary>
        /// Verifies that the timer automatically transitions from a Work session
        /// to a Break session when the work time elapses.
        /// </summary>
        [TestMethod]
        public async Task Timer_WhenWorkSessionEnds_SwitchesToBreakSession()
        {
            // Arrange
            var workTime = TimeSpan.FromMilliseconds(200);
            var breakTime = TimeSpan.FromMilliseconds(100);
            var manager = new PomodoroSessionManager(workTime, breakTime, 4);

            bool breakSessionStarted = false;
            manager.SessionStarted += (sender, args) =>
            {
                if (args.SessionType == SessionType.Break)
                {
                    breakSessionStarted = true;
                }
            };

            // Act
            manager.Start();
            await Task.Delay(300); // Wait for a duration longer than the work time
            manager.Pause();

            // Assert
            Assert.IsTrue(breakSessionStarted, "The event for the break session should have fired.");
            Assert.AreEqual(SessionType.Break, manager.CurrentSessionType, "The session type should have switched to Break.");
        }
    }
}
