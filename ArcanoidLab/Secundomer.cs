using System;
using System.Timers;

namespace ArcanoidLab
{
  /// <summary> Класс секундомера для игры </summary>
  public class Secundomer
  {
    public Timer timer { get; set; } = new Timer(1000); // Создаю таймер с интервалом в 1 секунду
    public static DateTime startTime { get; set; } = DateTime.Now;
    public static TimeSpan elapsedTime { get; set; }

    public void OnStart()
    {
      // Подписываюсь на событие Elapsed таймера
      timer.Elapsed += OnTimerElapsed;

      // Запускаю таймер
      timer.Start();
    }

    // Событие таймера
    static void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
      // Вычисляю прошедшее время
      elapsedTime = DateTime.Now - startTime;
    }

    /// <summary> Вывод прошедшего времени по-секундно</summary>
    public string GetElapsedTime()
    {
      return "Прошло времени: " + elapsedTime.Hours.ToString()+":"+ elapsedTime.Minutes.ToString()+":"+ elapsedTime.Seconds.ToString();
    }
  }
}
