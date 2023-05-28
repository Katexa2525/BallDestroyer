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
    public string GetElapsedTime(string label)
    {
      return label + (elapsedTime.Hours > 9 ? elapsedTime.Hours.ToString() : "0"+ elapsedTime.Hours.ToString()) + ":" + 
                     (elapsedTime.Minutes > 9 ? elapsedTime.Minutes.ToString() : "0"+ elapsedTime.Minutes.ToString()) + ":" + 
                     (elapsedTime.Seconds > 9 ? elapsedTime.Seconds.ToString() : "0"+ elapsedTime.Seconds.ToString());
    }
  }
}
