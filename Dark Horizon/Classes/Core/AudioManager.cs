using System;
using System.IO;
using System.Windows.Media;

namespace Dark_Horizon.Classes.Core;

public static class AudioManager
{
    private static readonly MediaPlayer _bgmPlayer = new MediaPlayer();
    private static double _volume = 0.5; // Гучність за замовчуванням (від 0.0 до 1.0)
    private static string? _currentTrackPath;
    /// <param name="fileName">Назва файлу з папки Assets/Audio/ (наприклад, "main_menu.mp3")</param>
    public static void PlayBGM(string fileName)
    {
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Audio", fileName);

        // Якщо цей трек уже грає — нічого не робимо
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

            // Підписуємося на подію завершення треку для його зациклення
            _bgmPlayer.MediaEnded -= BgmPlayer_MediaEnded;
            _bgmPlayer.MediaEnded += BgmPlayer_MediaEnded;

            _bgmPlayer.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[AudioError] Помилка відтворення: {ex.Message}");
        }
    }

    /// <summary>
    /// Зміна гучності (передавати значення від 0.0 до 1.0)
    /// </summary>
    public static void SetVolume(double volume)
    {
        _volume = Math.Clamp(volume, 0.0, 1.0);
        _bgmPlayer.Volume = _volume;
    }

    /// <summary>
    /// Повністю зупинити музику
    /// </summary>
    public static void StopBGM()
    {
        _bgmPlayer.Stop();
        _currentTrackPath = null;
    }

    private static void BgmPlayer_MediaEnded(object? sender, EventArgs e)
    {
        // Коли трек закінчується, перемотуємо на початок і запускаємо знову
        _bgmPlayer.Position = TimeSpan.Zero;
        _bgmPlayer.Play();
    }
}