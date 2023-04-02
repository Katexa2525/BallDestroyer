using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс блоков </summary>
  public class Block : DisplayObject
  {
    private Vector2f position;

    public List<Sprite> Blocks { get; set; } = new List<Sprite>();

    public Block(VideoMode mode) 
    {
      this.Sprite.Texture = TextureManager.BlockTexture; // рисунок блока
      this.SpriteWidth = this.Sprite.TextureRect.Width; // ширина блока
      this.SpriteHeight = this.Sprite.TextureRect.Height; // высота блока

      StartPosition(mode); // создаю блоки
    }

    public override void StartPosition(VideoMode mode) 
    {
      int n = 0;
      for (int i = 1; i <= mode.Width / this.SpriteWidth - 2; i++)
      {
        for (int j = 1; j <= 10; j++)
        {
          Blocks.Add(new Sprite(TextureManager.BlockTexture));
          Blocks[n].Position = new Vector2f(i * this.SpriteWidth, j * this.SpriteHeight);
          n++;
        }
      }
    }

    public override void Update(VideoMode mode)
    {
      Blocks.Clear();
      StartPosition(mode);
    }

    public override void Draw(RenderTarget window)
    {
      // вывод блоков на эклан
      for (int i = 0; i < Blocks.Count; i++)
      {
        window.Draw(Blocks[i]);
      }
    }

    public override void Draw(RenderTarget window, VideoMode mode) { }

  }
}
