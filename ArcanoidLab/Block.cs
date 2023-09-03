﻿using Newtonsoft.Json;
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
    public string Alias { set; get; } = "bl";
    public int BonusBlockId { set; get; } = 0;

    [JsonIgnore]
    public List<DisplayObject> Blocks { get; set; } = new List<DisplayObject>();
    [JsonIgnore]
    public Dictionary<DisplayObject, int> BlocksBonus { get; set; } = new Dictionary<DisplayObject, int>();  // словарь для с объектами обычными и бонусными (блоки) 

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
      this.Sprite.Texture = TextureManager.BlockTexture; // рисунок блока

      // первоначально блок в левом нижнем углу игрового поля
      this.x1 = 0; 
      this.y1 = (int)(this.Sprite.Texture.Size.Y * this.Scale); //20; // координаты левого верхнего угла
      this.x2 = (int)(this.Sprite.Texture.Size.X * this.Scale) /*42*/; 
      this.y2 = 0; // координаты правого нижнего угла

      this.SpriteWidth = Math.Abs(this.x1 - this.x2); // ширина блока
      this.SpriteHeight = Math.Abs(this.y1 - this.y2); // высота блока
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
          BlocksBonus.Add(Blocks[n], 0);

          n++;
        }
      }
      // разукрашиваю блоки с бонусами
      // 1 - бонус +100 очков, 2 - бонус увеличение платформы
      Random random = new Random();
      for (int i = 0; i < n; i++)
      {
        int randomBlock = random.Next(0, n-1);
        // новая текстура для бонусного блока
        Blocks[randomBlock].Sprite.Texture = TextureManager.BlockBonus1Texture;
        // устанавливаю бонус для блока
        Blocks[randomBlock].IsBonus = true;
        Blocks[randomBlock].BonusPoint = 100;
        // признак, что бонус для блока
        BlocksBonus[Blocks[randomBlock]] = 1; 
        if (i > n-50)
        {
          Blocks[randomBlock].Sprite.Texture = TextureManager.BlockBonus2Texture;
          BlocksBonus[Blocks[randomBlock]] = 2; // признак, что бонус для платформы
          Blocks[randomBlock].IsBonus = true;
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
