using SFML.Window;

namespace ArcanoidLab.EventClass
{
  // <summary> Класс EventArgs для хранения информации о объектах для рисования жизней </summary>
  public class HeartScullEventArgs
  {
    public int LifeTotal { get; } // общее кол-во жизней
    public int LifeCount { get; } // кол-во оставшихся жизней
    public VideoMode Mode { get; } // видео режим
    public HeartScullEventArgs(int lifeTotal, int lifeCount, VideoMode videoMode)
    {
      LifeTotal = lifeTotal;
      LifeCount = lifeCount;
      Mode = videoMode;
    }
  }
}
