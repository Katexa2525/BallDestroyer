using SFML.Window;

namespace ArcanoidLab.EventArgsClass
{
  // <summary> Класс EventArgs для хранения информации о параметрах для движения платформы </summary>
  public class PlatformEventArgs
  {
    public bool IsMove { get; } // признак, если было движение 
    public bool MoveLeft { get; } // признак, если было движение влево
    public bool MoveRight { get; } // признак, если было движение вправо
    public VideoMode Mode { get; } // видео режим
    public float PlatformSpeed { get; } // скорость движения платформы
    public int SpriteWidth { get; } // ширина платформы

    public PlatformEventArgs(bool isMove, bool moveLeft, bool moveRight, VideoMode videoMode, float platformSpeed, int spriteWidth) 
    {
      IsMove = isMove;
      MoveLeft = moveLeft;
      MoveRight = moveRight;
      Mode = videoMode;
      PlatformSpeed = platformSpeed;
      SpriteWidth = spriteWidth;
    }
  }
}
