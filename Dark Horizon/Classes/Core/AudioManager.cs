using System;
using System.IO;
using System.Windows.Media;

namespace Dark_Horizon.Classes.Core;

public static class AudioManager
{
    private static readonly MediaPlayer _bgmPlayer = new MediaPlayer();
    private static double _volume = 0.5;
    private static string? _currentTrackPath;
    /// <param name="fileName"></param>
    public static void PlayBGM(string fileName)
    {
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Audio", fileName);

        if (_currentTrackPath == fullPath) return;

        if (!File.Exists(fullPath))
        {
            System.Diagnostics.Debug.WriteLine($"[AudioError] Файл не знайдено: {fullPath}");
            return;
        }

        try
        {
            _currentTrackPath = fullPath;
            _bgmPlayer.Open(new Uri(fullPath, UriKind.Absolute));
            _bgmPlayer.Volume = _volume;

            _bgmPlayer.MediaEnded -= BgmPlayer_MediaEnded;
            _bgmPlayer.MediaEnded += BgmPlayer_MediaEnded;

            _bgmPlayer.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[AudioError] Помилка відтворення: {ex.Message}");
        }
    }

    public static void SetVolume(double volume)
    {
        _volume = Math.Clamp(volume, 0.0, 1.0);
        _bgmPlayer.Volume = _volume;
    }
    public static void StopBGM()
    {
        _bgmPlayer.Stop();
        _currentTrackPath = null;
    }

    private static void BgmPlayer_MediaEnded(object? sender, EventArgs e)
    {
        _bgmPlayer.Position = TimeSpan.Zero;
        _bgmPlayer.Play();
    }
}