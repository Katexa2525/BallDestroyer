using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс платформы для отбития мяча </summary>
  public class Platform : DisplayObject
  {
    private int delay = 0;
    private Vector2f position;
    private List<Bullet> bullets = new List<Bullet>();

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

      // для пуль на будущее, может пригодиться
      for (int i = 0; i < this.bullets.Count; i++)
      {
        this.bullets[i].Update();
        if (this.bullets[i].Position.Y < 0)
        {
          this.bullets.Remove(this.bullets[i]);
        }
      }
    }

    public override void Draw(RenderTarget window, VideoMode mode)
    {
      Update(mode);
      window.Draw(this.Sprite);

      foreach (var bullet in this.bullets)
      {
        window.Draw(bullet.RectangleBullet);
      }
    }

    public override void Draw(RenderTarget window) { }

    public void KeyHandler(VideoMode mode)
    {
      bool moveLeft = Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left);
      bool moveRight = Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right);
      bool moveUp = Keyboard.IsKeyPressed(Keyboard.Key.W);
      bool moveDown = Keyboard.IsKeyPressed(Keyboard.Key.S);

      bool isMove = moveLeft || moveRight || moveUp || moveDown;

      if (isMove)
      {
        if (moveLeft && position.X - GameSetting.PLATFORM_SPEED >= 0) 
          position.X -= GameSetting.PLATFORM_SPEED;
        if (moveRight && position.X + GameSetting.PLATFORM_SPEED < mode.Width - this.SpriteWidth)
          position.X += GameSetting.PLATFORM_SPEED;
      }

      // это при нажатии на пробел идет стрельба. В будущем можно бонус какой-нибудь добавить
      //bool isFire = Keyboard.IsKeyPressed(Keyboard.Key.Space);
      //if (isFire) this.Fire();
    }

    private void Fire()
    {
      this.delay++;
      if (this.delay >= 15)
      {
        this.bullets.Add(new Bullet(this.position));
        var positionOfSecondBullet = new Vector2f(this.position.X + 25, this.position.Y);
        this.bullets.Add(new Bullet(positionOfSecondBullet));
        this.delay = 0;
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
