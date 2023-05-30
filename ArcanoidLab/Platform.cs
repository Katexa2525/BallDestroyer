using ArcanoidLab.EventArgsClass;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace ArcanoidLab
{
  /// <summary> Класс платформы для отбития мяча </summary>
  public class Platform : DisplayObject
  {
    private Vector2f position;

    public bool IsMove { get; set; } // признак, если было движение 
    public bool MoveLeft { get; set; } // признак, если было движение влево
    public bool MoveRight { get; set;  } // признак, если было движение вправо
    public bool MoveUp { get; set; } // признак, если было движение вверх 
    public bool MoveDown { get; set; } // признак, если было движение вниз

    public Platform(VideoMode mode)
    {
      this.Scale = 1.5f;
      this.SmoothTexture = true;
      this.Sprite.Texture = TextureManager.PlayerTexture; // рисунок платформы
      this.Sprite.Texture.Smooth = this.SmoothTexture; // сглаживание краев текстуры
      this.Sprite.Scale = new Vector2f(this.Scale, this.Scale);

      // первоначально платформа в левом нижнем углу игрового поля
      this.x1 = 0; 
      this.y1 = (int)(this.Sprite.Texture.Size.Y * this.Scale); //9; // координаты левого верхнего угла
      this.x2 = (int)(this.Sprite.Texture.Size.X * this.Scale) /*90*/;
      this.y2 = 0; // координаты правого нижнего угла

      this.SpriteWidth = Math.Abs(this.x1 - this.x2); // ширина блока
      this.SpriteHeight = Math.Abs(this.y1 - this.y2); // высота блока

      // начальная позиция платформы
      StartPosition(mode);
    }

    public override void OnPlatformMoveChanged(PlatformEventArgs e)
    {
      base.OnPlatformMoveChanged(e);  // Метод вызова события базового класса.
    }

    public override void StartPosition(VideoMode mode)
    {
      position.X = (mode.Width / 2) - (this.SpriteWidth / 2); // вычисляю позицию по оси Х, чтобы посередине платформа была
      position.Y = mode.Height - this.SpriteHeight - 4;      // вычисляю позицию по оси Y, чтобы платформа над нижней частью окна была
      this.Sprite.Position = position;
      // устанавливаю координаты фигуры
      Coordinates();
    }

    public override void Update(VideoMode mode)
    {
      this.KeyHandler(mode);
      this.Sprite.Position = position;
      // устанавливаю координаты фигуры
      Coordinates();
    }

    public override void Draw(RenderTarget window, VideoMode mode)
    {
      Update(mode);
      window.Draw(this.Sprite);
    }

    public override void Draw(RenderTarget window) { }

    public void KeyHandler(VideoMode mode)
    {
      MoveLeft = Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left);
      MoveRight = Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right);
      MoveUp = Keyboard.IsKeyPressed(Keyboard.Key.W);
      MoveDown = Keyboard.IsKeyPressed(Keyboard.Key.S);

      IsMove = MoveLeft || MoveRight || MoveUp || MoveDown;

      OnPlatformMoveChanged(new PlatformEventArgs(IsMove, MoveLeft, MoveRight, mode, GameSetting.PLATFORM_SPEED, SpriteWidth));
    }

    public void PlatformMove(bool isMove, bool moveLeft, bool moveRight, VideoMode mode, float platformSpeed, int spriteWidth)
    {
      if (isMove)
      {
        if (moveLeft && position.X - GameSetting.PLATFORM_SPEED >= 0)
          position.X -= GameSetting.PLATFORM_SPEED;
        if (moveRight && position.X + GameSetting.PLATFORM_SPEED < mode.Width - this.SpriteWidth)
          position.X += GameSetting.PLATFORM_SPEED;
      }
    }

    private void Coordinates()
    {
      // устанавливаю координаты фигуры
      int xx1 = Convert.ToInt32(position.X);
      int yy1 = Convert.ToInt32(position.Y);
      int xx2 = Convert.ToInt32(position.X + this.SpriteWidth);
      int yy2 = Convert.ToInt32(position.Y + this.SpriteHeight);
      this.SetCoordinates(xx1, yy1, xx2, yy2);
    }
  }
}
