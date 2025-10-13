using System.Media;
using System.IO;
using System.Runtime.Versioning;

namespace Pomoberry.Core
{
    [SupportedOSPlatform("windows")]
    public class AudioManager : IDisposable
    {
        private readonly SoundPlayer _player;

        public AudioManager(string soundFileName)
        {
            // Expects the sound file to be in the same directory as the executable.
            string soundPath = Path.Combine(AppContext.BaseDirectory, soundFileName);

            if (File.Exists(soundPath))
            {
                _player = new SoundPlayer(soundPath);
                _player.Load(); // Pre-load the sound file for faster playback
            }
            else
            {
                // If the file is not found, create a player that does nothing.
                // Prevents the app from crashing if the sound file is missing.
                _player = new SoundPlayer();
            }
        }

        public void PlayMusic()
        {
            // Loop the sound so it plays continuously during the work session.
            _player?.PlayLooping();
        }

        public void StopMusic()
        {
            _player?.Stop();
        }

        public void Dispose()
        {
            _player?.Dispose();
        }
    }
}