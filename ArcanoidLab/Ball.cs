﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace ArcanoidLab
{
  /// <summary> Класс мячика </summary>
  public class Ball : DisplayObject
  {
    private Vector2f position;
    private Platform platform;

    public Ball(VideoMode mode)
    {
      // первоначально мячик в левом нижнем углу игрового поля
      this.x1 = 0; this.y1 = 12; // координаты левого верхнего угла
      this.x2 = 12; this.y2 = 0; // координаты правого нижнего угла

      this.Sprite.Texture = TextureManager.BallTexture; // рисунок мячика
      //this.SpriteWidth = this.Sprite.TextureRect.Width; // ширина мячика
      //this.SpriteHeight = this.Sprite.TextureRect.Height; // высота мячика
      this.SpriteWidth = Math.Abs(this.x1 - this.x2); // ширина блока
      this.SpriteHeight = Math.Abs(this.y1 - this.y2); // высота блока
      // создаю экземпляры объектов
      this.platform = new Platform(mode);
      // начальная позиция мячика
      StartPosition(mode);
    }

    public override void StartPosition(VideoMode mode)
    {
      // ставлю мячик в середину игрового поля
      position.X = (mode.Width / 2) - (this.SpriteWidth / 2); // вычисляю позицию по оси Х, чтобы посередине мячик был
      position.Y = mode.Height - platform.SpriteHeight - this.SpriteHeight; // вычисляю позицию по оси Y, чтобы мячик над платформой был
      this.Sprite.Position = position;
      // устанавливаю координаты фигуры
      int xx1 = Convert.ToInt32(position.X);
      int yy1 = Convert.ToInt32(position.Y);
      int xx2 = Convert.ToInt32(position.X + this.SpriteWidth);
      int yy2 = Convert.ToInt32(position.Y + this.SpriteHeight);
      this.SetCoordinates(xx1, yy1, xx2, yy2);
    }

    public override void Update(VideoMode mode) {  }

    public override void Draw(RenderTarget window)
    {
      window.Draw(this.Sprite);
    }

    public override void Draw(RenderTarget window, VideoMode mode) { }
  }
}
