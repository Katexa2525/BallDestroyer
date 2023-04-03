﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс для создание картинок жизни и потери жизни в виде сердца и черепа соответсвенно </summary>
  public class HeartScull : DisplayObject
  {
    private List<Sprite> sprite;
    private Vector2f position;

    public override void StartPosition(VideoMode mode) { }

    private Sprite HeartScullPosition(VideoMode mode, int pos)
    {
      Sprite spriteHS = new Sprite();
      if (LIFE_TOTAL - LifeCount == 0 || LIFE_TOTAL - LifeCount < pos)
        spriteHS.Texture = TextureManager.HeartTexture; // рисунок сердца
      else
        spriteHS.Texture = TextureManager.ScullTexture; // рисунок черепа
      position.X = mode.Width - (spriteHS.TextureRect.Width * pos + 3); // вычисляю позицию по оси Х, чтобы сердце-череп были справа
      position.Y = 0; // вычисляю позицию по оси Y
      spriteHS.Position = position;
      return spriteHS;
    }

    public override void Update(VideoMode mode)
    {
      sprite = new List<Sprite>();
      for (int i = 1; i <= LIFE_TOTAL; i++)
      {
        sprite.Add(HeartScullPosition(mode, i));
      }
    }

    public override void Draw(RenderTarget window, VideoMode mode)
    {
      Update(mode);
      foreach (var item in sprite)
      {
        window.Draw(item);
      }
    }

    public override void Draw(RenderTarget window)
    {  }
  }
}
