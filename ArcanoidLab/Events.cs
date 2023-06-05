using ArcanoidLab.EventArgsClass;
using System;
using System.Timers;

namespace ArcanoidLab
{
  /// <summary> Событийный класс игры </summary>
  public class Events
  {
    public delegate void TimerHandler(object sender, ElapsedEventArgs e);
    public event TimerHandler TimerElapsed; // Cобытие Elapsed таймера

    //public event EventHandler TimerElapsed; // Cобытие Elapsed таймера

    public event EventHandler<DeltaEventArgs> DeltaChanged; //Событие на изменение скорости шарика
    public event EventHandler<IntersectionEventArgs> IntersectionChanged; //Событие на пересечение объектов
    public event EventHandler<HeartScullEventArgs> HeartScullChanged; //Событие на изменение жизни, перерисовка
    public event EventHandler<PlatformEventArgs> PlatformMoveChanged; //Событие на изменение положения платформы
    public event EventHandler<IntersectionEventArgs> RoundGameChanged; //Событие на окончание раунда игры или всей игры
    public event EventHandler<IntersectionEventArgs> ReboundAfterScreenCollisionChanged; //Событие на определение смещения после отскока от рамок игрового экрана
    public event EventHandler<IntersectionEventArgs> ReboundAfterCollisionChanged; // Событие на определение отскока после столкновения
    public event EventHandler<TextBonusEventArgs> TextBonusChanged; // Событие на столкновение показом текстового бонуса

    /// <summary> Cобытие Elapsed таймера </summary>
    /// <param name="e"></param>
    public void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
      TimerElapsed?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на изменение скорости шарика </summary>
    /// <param name="e"></param>
    public void OnDeltaChanged(object sender, DeltaEventArgs e)
    {
      DeltaChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на пересечение объектов </summary>
    /// <param name="e"></param>
    public void OnIntersectionChanged(object sender, IntersectionEventArgs e)
    {
      IntersectionChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на изменение жизни, перерисовка </summary>
    /// <param name="e"></param>
    public void OnHeartScullChanged(object sender, HeartScullEventArgs e)
    {
      HeartScullChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на изменение положения платформы </summary>
    /// <param name="e"></param>
    public void OnPlatformMoveChanged(object sender, PlatformEventArgs e)
    {
      PlatformMoveChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на окончание раунда игры или всей игры </summary>
    /// <param name="e"></param>
    public void OnRoundGameChanged(object sender, IntersectionEventArgs e)
    {
      RoundGameChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на определение смещения после отскока от рамок игрового экрана </summary>
    /// <param name="e"></param>
    public void OnReboundAfterScreenCollisionChanged(object sender, IntersectionEventArgs e)
    {
      ReboundAfterScreenCollisionChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на определение отскока после столкновения </summary>
    /// <param name="e"></param>
    public void OnReboundAfterCollisionChanged(object sender, IntersectionEventArgs e)
    {
      ReboundAfterCollisionChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }

    /// <summary> Событие на столкновение показом текстового бонуса </summary>
    /// <param name="e"></param>
    public void OnTextBonusChanged(object sender, TextBonusEventArgs e)
    {
      TextBonusChanged?.Invoke(sender, e);  // Безопасно поднять событие для всех подписчиков
    }
  }
}
