using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ArcanoidLab
{
  public abstract class DisplayObject
  {
    private int dx = 6; // смещение дельта х
    private int dy = 5; // смещение дельта y

    public Vector2f positionObject; // поле для сохранения позиции объекта, например, шарика после смещения

    // свойства, общие для всех наследуемых объектов
    public int SpriteWidth { get; set; } = 0; // свойство ширины объекта
    public int SpriteHeight { get; set; } = 0; // свойство высоты объекта
    public Sprite Sprite { get; set; } = new Sprite(); // сам объект (блок, шарик, платформа)

    public int x1 { get; set; } = 0; // // координата х1 фигуры верхнего левого угла
    public int y1 { get; set; } = 0; // координата у1 фигуры верхнего левого угла
    public int x2 { get; set; } = 0; // // координата х2 фигуры нижнего правого угла
    public int y2 { get; set; } = 0; // координата у2 фигуры нижнего правого угла

    public abstract void StartPosition(VideoMode mode);
    public abstract void Update(VideoMode mode);
    public abstract void Draw(RenderTarget window);
    public abstract void Draw(RenderTarget window, VideoMode mode);

    /// <summary> Устанавливаю координаты фигуры  </summary>
    public virtual void SetCoordinates(int xx1, int yy1, int xx2, int yy2)
    {
      x1 = xx1; y1 = yy1; x2 = xx2; y2 = yy2;
    }

    /// <summary> Метод проверки пересечения объектов шара с блоками, платформой, стенками игрового экрана </summary>
    public virtual void ObjectIntersection(DisplayObject ball, List<DisplayObject> blocks, DisplayObject platform, DisplayObject heartScull,
                                           VideoMode mode, RenderTarget window)
    {
      Random random = new Random();

      if (GameSetting.IsStart)
      {
        int n = blocks.Count; // кол-во блоков в массиве blocks

        ball.x1 += dx; 
        ball.y1 -= dy;
        SetNewSetCoordinates(ball.x1, ball.y1, ball);
        for (int i = 0; i < n; i++)
        {
          // если есть пересечение, то удаление блока из коллекции, определение dx, т.е. отскока по оси х
          if (IsCrossing(ball.x1, ball.y1, ball.x2, ball.y2, 
                         blocks[i].x1, blocks[i].y1 + blocks[i].SpriteHeight, blocks[i].x2, blocks[i].y2) ||
              IsCrossing(ball.x1, ball.y1, ball.x2, ball.y2, 
                         blocks[i].x1 + blocks[i].SpriteWidth, blocks[i].y1, blocks[i].x2, blocks[i].y2) ||
              IsCrossing(ball.x1, ball.y1, ball.x2, ball.y2, 
                         blocks[i].x1, blocks[i].y1, blocks[i].x2, blocks[i].y2 - blocks[i].SpriteWidth) ||
              IsCrossing(ball.x1, ball.y1 + ball.SpriteHeight, ball.x2, ball.y2 - ball.SpriteHeight,
                         blocks[i].x1, blocks[i].y1, blocks[i].x2, blocks[i].y2 - blocks[i].SpriteHeight))
          {
            blocks[i].Sprite.Position = new Vector2f(-100, 0);
            if (ball.x1 < blocks[i].x1) // левая стенка блока
              dx = dx > 0 ? -dx : dx;
            if (ball.x2 > blocks[i].x2) // правая стенка блока
              dx = dx < 0 ? -dx : dx;
            if (ball.y1 < blocks[i].y2) // если столкновение о нижнюю часть блока
              dy = -dy;
            // удаляю блок после столкновения из массива
            blocks.RemoveAt(i);
            n = blocks.Count;
            GameSetting.Score += GameSetting.SCORE_STEP; // вывод результата
          }
        }

        // если столкновение о стенки игрового экрана слева и справа
        if (ball.x1 < 0) // слева 
        {
          dx = dx < 0 ? -dx : dx;
        }
        if (ball.x2 > mode.Width) // справа
        {
          dx = dx > 0 ? -dx : dx;
        }

        // если столкновение о верх игрового экрана
        if (ball.y1 < 0)
          dy = -dy;

        // если выбиты все блоки, или промах мимо платформы, т.е. столкновение о низ игрового экрана
        if (ball.y2 > mode.Height || blocks.Count == 0)
        {
          GameSetting.IsStart = false;
          dx = 6; dy = 5;
          // ставлю мячик в середину игрового поля
          ball.x1 = (int)(mode.Width / 2) - (ball.SpriteWidth / 2); // вычисляю позицию по оси Х, чтобы посередине мячик был
          ball.y1 = (int)mode.Height - platform.SpriteHeight - ball.SpriteHeight; // вычисляю позицию по оси Y, чтобы мячик над платформой был
          // минус жизнь
          GameSetting.LifeCount--;
          heartScull.Draw(window, mode); // перерисовываю после минусования жизни
        }

        // определение отскока dу при пересечении с платформой
        if (IsCrossing(ball.x1, ball.y1, ball.x2, ball.y2, 
                       platform.x1, platform.y1, platform.x2, platform.y2 - platform.SpriteHeight))
          dy = (random.Next() % 5 + 2); // отскок шарика от платформы по оси у
      }
    }

    /// <summary> Метод определения пересечения двух отрезков </summary>
    /// Параметры - координаты 2-х отрезков: 
    /// (x1,y1,x2,y2) - координаты прямой прямоугольника шарика
    /// (x3,y3,x4,y4) - координаты прямой прямоугольника блока или др объекта для столкновения с шариком
    /// Возврат: true - прямые пересекаются, или совпадают, т.е. есть столкновение; false - не пересекаются
    private bool IsCrossing(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
      float Ua, Ub, numerator_a, numerator_b, denominator;

      denominator = (y4 - y3) * (x1 - x2) - (x4 - x3) * (y1 - y2);

      if (denominator == 0)
      {
        if ((x1 * y2 - x2 * y1) * (x4 - x3) - (x3 * y4 - x4 * y3) * (x2 - x1) == 0 &&
            (x1 * y2 - x2 * y1) * (y4 - y3) - (x3 * y4 - x4 * y3) * (y2 - y1) == 0)
          return true;
        else
          return false;
      }
      else
      {
        numerator_a = (x4 - x2) * (y4 - y3) - (x4 - x3) * (y4 - y2);
        numerator_b = (x1 - x2) * (y4 - y2) - (x4 - x2) * (y1 - y2);
        Ua = numerator_a / denominator;
        Ub = numerator_b / denominator;

        if (Ua >= 0 && Ua <= 1 && Ub >= 0 && Ub <= 1)
          return true;
        return false;
      }
    }

    /// <summary> Установка новых координат объекта </summary>
    public void SetNewSetCoordinates(int x1, int y1, DisplayObject displayObject)
    {
      positionObject = new Vector2f(x1, y1);
      int xx1 = Convert.ToInt32(positionObject.X);
      int yy1 = Convert.ToInt32(positionObject.Y);
      int xx2 = Convert.ToInt32(positionObject.X + displayObject.SpriteWidth);
      int yy2 = Convert.ToInt32(positionObject.Y + displayObject.SpriteHeight);
      displayObject.SetCoordinates(xx1, yy1, xx2, yy2);
    }
  }
}
