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
    
    public float x { get; set; } = 0; // // координата х фигуры для метода пересечения
    public float y { get; set; } = 0; // координата у фигуры для метода пересечения

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
        int n = blocks.Count; // кол-во блоков

        ball.x1 += dx; //ball.x2 = ball.x1 + ball.SpriteWidth + dx;
        ball.y1 -= dy; //ball.y2 -= ball.y1 + ball.SpriteHeight - dy;

        // если столкновение о стенки игрового экрана слева и справа
        if (ball.x1 < 0)
        {
          dx = dx < 0 ? -dx : dx;
        }
        if (ball.x2 > mode.Width)
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
        if (ball.y2 > platform.y1 /*&& ball.x2 >= platform.x1 && ball.x2 <= platform.x2*/)
          dy = (random.Next() % 5 + 2);


        // новые координаты объекта в поле 
        positionObject = new Vector2f(ball.x1, ball.y1);
        int xx1 = Convert.ToInt32(positionObject.X);
        int yy1 = Convert.ToInt32(positionObject.Y);
        int xx2 = Convert.ToInt32(positionObject.X + ball.SpriteWidth);
        int yy2 = Convert.ToInt32(positionObject.Y + ball.SpriteHeight);
        ball.SetCoordinates(xx1, yy1, xx2, yy2);
      }
    }

    // virtual - чтобы можно было в классах наследниках или переопределить этот метод, или пользоваться базовым, т.е. этим
    public virtual void CheckCollision(float X, float Y, List<DisplayObject> Blocks, DisplayObject platform, DisplayObject heartScull,
                                       VideoMode mode, RenderTarget window)
    {
      x = X; // позиция спрайта (шарика) по х
      y = Y; // позиция спрайта (шарика) по у
      Random random = new Random(); 

      if (GameSetting.IsStart)
      {
        int n = Blocks.Count;

        // определение пересечения шарика с координатами (x,y) с блоком со своими координатами из массива блоков для оси х
        x += dx;
        for (int i = 0; i < n; i++)
        {
          // если есть пересечение, то удаление блока из коллекции, определение dx, т.е. отскока по оси х
          if (new FloatRect(x + 3, y + 3, 6, 6).Intersects(Blocks[i].Sprite.GetGlobalBounds()))
          {
            Blocks[i].Sprite.Position = new Vector2f(-100, 0);
            Blocks.RemoveAt(i);
            n = Blocks.Count;
            dx = -dx;
            GameSetting.Score += GameSetting.SCORE_STEP; // вывод результата
          }
        }

        // определение пересечения шарика с координатами (x,y) с блоком со своими координатами из массива блоков для оси у
        y += dy;
        for (int i = 0; i < n; i++)
        {
          // если есть пересечение, то удаление блока из коллекции, определение dy, т.е. отскока по оси y
          if (new FloatRect(x + 3, y + 3, 6, 6).Intersects(Blocks[i].Sprite.GetGlobalBounds()))
          {
            Blocks[i].Sprite.Position = new Vector2f(-100, 0);
            Blocks.RemoveAt(i);
            n = Blocks.Count;
            dy = -dy;
            GameSetting.Score += GameSetting.SCORE_STEP; // вывод результата
          }
        }

        // если столкновение о стенки игрового экрана слева и справа
        if (x < 0 || x > mode.Width) dx = -dx; 

        // если столкновение о верх игрового экрана
        if (y < 0) dy = -dy;

        // если выбиты все блоки, или промах мимо платформы, т.е. столкновение о низ игрового экрана
        if (y > mode.Height || Blocks.Count == 0)
        {
          GameSetting.IsStart = false;
          dx = 6; dy = 5;
          x = X; y = Y;
          GameSetting.LifeCount--;// минус жизнь
          heartScull.Draw(window, mode); // перерисовываю после минусования жизни
        }

        // определение отскока dу при пересечении с платформой
        if (new FloatRect(x, y, 12, 12).Intersects(platform.Sprite.GetGlobalBounds()))
          dy = -(random.Next() % 5 + 2);

        // новые координаты объекта в поле 
        positionObject = new Vector2f(x, y);
      }
    }

  }
}
