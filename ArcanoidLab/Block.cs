using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс блоков </summary>
  public class Block : DisplayObject
  {
    private Vector2f position;

    public List<DisplayObject> Blocks { get; set; } = new List<DisplayObject>();

    public Block()
    {
      BeginSetting();
    }

    public Block(VideoMode mode) 
    {
      BeginSetting();
      StartPosition(mode); // создаю блоки
    }

    // начальные настройки класса
    private void BeginSetting()
    {
      // первоначально блок в левом нижнем углу игрового поля
      this.x1 = 0; this.y1 = 20; // координаты левого верхнего угла
      this.x2 = 42; this.y2 = 0; // координаты правого нижнего угла

      this.SpriteWidth = Math.Abs(this.x1 - this.x2); // ширина блока
      this.SpriteHeight = Math.Abs(this.y1 - this.y2); // высота блока

      this.Sprite.Texture = TextureManager.BlockTexture; // рисунок блока
    }

    public override void StartPosition(VideoMode mode) 
    {
      int n = 0;
      for (int i = 1; i <= mode.Width / this.SpriteWidth - 2; i++) // по оси х блоки
      {
        for (int j = 1; j <= 10; j++) // по оси у блоки
        {
          // заполняю массив объектов Block(), которые наследуют от DisplayObject
          Blocks.Add(new Block());
          // заполняю координаты блоков массива DisplayObject для последующего расчета пересечений
          Blocks[n].x1 = i * this.SpriteWidth; 
          Blocks[n].y1 = j * this.SpriteHeight;
          Blocks[n].x2 = Blocks[n].x1 + this.SpriteWidth; 
          Blocks[n].y2 = Blocks[n].y1 + this.SpriteHeight;
          // заполняю Sprite объектом из рисунка
          Blocks[n].Sprite = new Sprite(TextureManager.BlockTexture);
          Blocks[n].Sprite.Position = new Vector2f(i * Math.Abs(Blocks[n].x1 - Blocks[n].x2), j * Math.Abs(Blocks[n].y1 - Blocks[n].y2));
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
        window.Draw(Blocks[i].Sprite);
      }
    }

    public override void Draw(RenderTarget window, VideoMode mode) { }

  }
}
