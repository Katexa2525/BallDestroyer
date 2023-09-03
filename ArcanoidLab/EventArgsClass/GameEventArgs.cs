using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс EventArgs для хранения информации об объектах игры </summary>
  public class GameEventArgs
  {
    public VideoMode Mode { get; }

    public GameEventArgs(VideoMode mode)
    {
      Mode = mode;
    }
  }
}
