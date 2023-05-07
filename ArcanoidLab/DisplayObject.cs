﻿using SFML.Graphics;
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
    public int y1 { get; set; } =  0; // координата у1 фигуры верхнего левого угла
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

    public bool CheckIntersection(List<DisplayObject> staticDO, List<DisplayObject> dynamicDO)
    {
      for (int i = 0; i < dynamicDO.Count; i++)
      {
        for (int k = 0; k < staticDO.Count; k++)
        {
          if ((dynamicDO[i].y1 <= staticDO[k].y2 && dynamicDO[i].y1 >= staticDO[k].y1 && dynamicDO[i].x1 >= staticDO[k].x1 && dynamicDO[i].x1 <= staticDO[k].x2) || // подлет снизу к объекту
              (dynamicDO[i].x1 <= staticDO[k].x2 && dynamicDO[i].x1 >= staticDO[k].x1 && dynamicDO[i].y1 >= staticDO[k].y1 && dynamicDO[i].y1 <= staticDO[k].y2) || // подлет к правой стенке объекту
              (dynamicDO[i].x2 >= staticDO[k].x1 && dynamicDO[i].x2 <= staticDO[k].x2 && dynamicDO[i].y1 >= staticDO[k].y1 && dynamicDO[i].y1 <= staticDO[k].y2) || // подлет к левой стенке объекту
              (dynamicDO[i].y2 >= staticDO[k].y1 && dynamicDO[i].y2 <= staticDO[k].y2 && dynamicDO[i].x2 >= staticDO[k].x1 && dynamicDO[i].x2 <= staticDO[k].x2))   // подлет сверху в вверх объекту
          {
            ReboundAfterCollision(staticDO[k], dynamicDO[i], staticDO);
            return true;
          }
          else if ((dynamicDO[i].x1 < 0) || //столкновение о стенки игрового экрана слева 
                   (dynamicDO[i].x2 > 800) || //столкновение о стенки игрового экрана справа
                   (dynamicDO[i].y1 < 0)) //столкновение о вверх игрового экрана
          {
            ReboundAfterScreenCollision(dynamicDO[i]);
            return true;
          }
        }
      }
      return false;
    }

    private void ReboundAfterScreenCollision(DisplayObject dynamicObject)
    {
      if (dynamicObject.x1 < 0) // если столкновение о стенки игрового экрана слева 
        dx = dx < 0 ? -dx : dx;
      if (dynamicObject.x2 > 800) // если столкновение о стенки игрового экрана справа
        dx = dx > 0 ? -dx : dx;
      if (dynamicObject.y1 < 0) // если столкновение о верх игрового экрана
        dy = -dy;
    }

    /// <summary> Метод для определения отскока после столкновения </summary>
    private void ReboundAfterCollision(DisplayObject staticObject, DisplayObject dynamicObject, List<DisplayObject> staticDO)
    {
      Random random = new Random();
      if (staticObject is Block)
      {
        staticObject.Sprite.Position = new Vector2f(-100, 0);
        if (dynamicObject.x1 < staticObject.x1) // левая стенка блока
          dx = dx > 0 ? -dx : dx;
        if (dynamicObject.x2 > staticObject.x2) // правая стенка блока
          dx = dx < 0 ? -dx : dx;
        if (dynamicObject.y1 < staticObject.y2) // если столкновение о нижнюю часть блока
          dy = -dy;
        // удаляю блок после столкновения из массива
        staticDO.Remove(staticObject);
        GameSetting.Score += GameSetting.SCORE_STEP; // вывод результата
      }
      else if (staticObject is Platform) 
        dy = (random.Next() % 5 + 2); // отскок шарика от платформы по оси у
    }

    /// <summary> Метод проверки пересечения объектов шара с блоками, платформой, стенками игрового экрана </summary>
    public virtual void ObjectIntersection_1(DisplayObject ball, List<DisplayObject> blocks, DisplayObject platform, DisplayObject heartScull,
                                             VideoMode mode, RenderTarget window)
    {
      Random random = new Random();

      if (GameSetting.IsStart)
      {
        int n = blocks.Count; // кол-во блоков в массиве blocks

        ball.x1 += dx;
        ball.y1 -= dy;
        SetNewSetCoordinates(ball.x1, ball.y1, ball);

        List<DisplayObject> dynamicDO = new List<DisplayObject>();
        dynamicDO.Add(ball);
        List<DisplayObject> staticDO;
        staticDO = blocks;
        staticDO.Add(platform);
        bool www = (CheckIntersection(staticDO, dynamicDO));

        // если столкновение о стенки игрового экрана слева и справа
        //if (ball.x1 < 0) // слева 
        //{
        //  dx = dx < 0 ? -dx : dx;
        //}
        //if (ball.x2 > mode.Width) // справа
        //{
        //  dx = dx > 0 ? -dx : dx;
        //}

        //// если столкновение о верх игрового экрана
        //if (ball.y1 < 0)
        //  dy = -dy;

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

        
        //if (ball.y2 >= platform.y1 && ball.y2 <= platform.y2 && ball.x2 >= platform.x1 && ball.x2 <= platform.x2)
        //  dy = (random.Next() % 5 + 2); // отскок шарика от платформы по оси у
      }
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
          if ((ball.y1 <= blocks[i].y2 && ball.y1 >= blocks[i].y1 && ball.x1 >= blocks[i].x1 && ball.x1 <= blocks[i].x2) || // подлет снизу к блоку
              (ball.x1 <= blocks[i].x2 && ball.x1 >= blocks[i].x1 && ball.y1 >= blocks[i].y1 && ball.y1 <= blocks[i].y2) || // подлет к правой стенке блока
              (ball.x2 >= blocks[i].x1 && ball.x2 <= blocks[i].x2 && ball.y1 >= blocks[i].y1 && ball.y1 <= blocks[i].y2) || // подлет к левой стенке блока
              (ball.y2 >= blocks[i].y1 && ball.y2 <= blocks[i].y2 && ball.x2 >= blocks[i].x1 && ball.x2 <= blocks[i].x2)  // подлет сверху в вверх блока
             )
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
        if (ball.y2 >= platform.y1 && ball.y2 <= platform.y2 && ball.x2 >= platform.x1 && ball.x2 <= platform.x2)
          dy = (random.Next() % 5 + 2); // отскок шарика от платформы по оси у
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
