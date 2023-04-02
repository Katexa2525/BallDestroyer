using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс блоков </summary>
  public class Block : DisplayObject
  {
    private Sprite sprite;
    private Vector2f position;
    private readonly int spriteWidth = 0;
    private readonly int spriteHeight = 0;

    public List<Sprite> Blocks { get; set; } = new List<Sprite>();

    public Block(VideoMode mode) 
    {
      this.sprite = new Sprite();
      this.sprite.Texture = TextureManager.BlockTexture; // рисунок блока
      this.spriteWidth = this.sprite.TextureRect.Width; // ширина блока
      this.spriteHeight = this.sprite.TextureRect.Height; // высота блока

      CreateBlocks(mode); // создаю блоки
    }

    public override void StartPosition(VideoMode mode) { }

    public override void Update(VideoMode mode)
    {
      Blocks.Clear();
      CreateBlocks(mode);
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

    private void CreateBlocks(VideoMode mode)
    {
      int n = 0;
      for (int i = 1; i <= mode.Width / this.spriteWidth - 2; i++)
      {
        for (int j = 1; j <= 10; j++)
        {
          Blocks.Add(new Sprite(TextureManager.BlockTexture));
          Blocks[n].Position = new Vector2f(i * this.spriteWidth, j * this.spriteHeight);
          n++;
        }
      }
    }

  }
}
