using ArcanoidLab.EventArgsClass;
using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ArcanoidLab
{
  public abstract class DisplayObject
  {
    private int dx = GameSetting.BALL_DELTA_X; // смещение дельта х
    private int dy = GameSetting.BALL_DELTA_Y; // смещение дельта y

    public Vector2f positionObject; // поле для сохранения позиции объекта, например, шарика после смещения

    // свойства, общие для всех наследуемых объектов
    public int SpriteWidth { get; set; } = 0; // свойство ширины объекта
    public int SpriteHeight { get; set; } = 0; // свойство высоты объекта
    public float Scale { get; set; } = 1; // масштаб фигуры во сколько раз увеличить
    public bool SmoothTexture { get; set; } = false; // сглаживание текстуры по умолчанию
    public bool IsIntersection { get; set; } = false; // есть пересечение
    public bool IsBonus_1 { get; set; } = false; // есть пересечение
    public bool IsBonus_2 { get; set; } = false; // есть пересечение
    [JsonIgnore]
    public Sprite Sprite { get; set; } = new Sprite(); // сам объект (блок, шарик, платформа)
    [JsonIgnore]
    public DisplayObject DynamicDOCurrent { get; set; } // динамический объект, с которым было пересечение, нужно для событий
    [JsonIgnore]
    public DisplayObject StaticDOCurrent { get; set; } // статический объект, с которым было пересечение, нужно для событий
    [JsonIgnore]
    public List<DisplayObject> StaticDOList { get; set; } // статические объекты, с которыми подразумевается пересечение, нужно для событий
    [JsonIgnore]
    public Dictionary<DisplayObject, int> DOBonus { get; set; } = new Dictionary<DisplayObject, int>();  // словарь для с объектами обычными и бонусными (блоки) 

    public int x1 { get; set; } = 0;  // координата х1 фигуры верхнего левого угла
    public int y1 { get; set; } =  0; // координата у1 фигуры верхнего левого угла
    public int x2 { get; set; } = 0;  // координата х2 фигуры нижнего правого угла
    public int y2 { get; set; } = 0;  // координата у2 фигуры нижнего правого угла

    public abstract void StartPosition(VideoMode mode);
    public abstract void Update(VideoMode mode);
    public abstract void Draw(RenderTarget window);
    public abstract void Draw(RenderTarget window, VideoMode mode);

    //События базового класса 
    public event EventHandler<DeltaEventArgs> DeltaChanged; //Событие на изменение скорости шарика
    public event EventHandler<IntersectionEventArgs> IntersectionChanged; //Событие на пересечение объектов
    public event EventHandler<HeartScullEventArgs> HeartScullChanged; //Событие на изменение жизни, перерисовка
    public event EventHandler<PlatformEventArgs> PlatformMoveChanged; //Событие на изменение положения платформы
    public event EventHandler<IntersectionEventArgs> RoundGameChanged; //Событие на окончание раунда игры или всей игры
    public event EventHandler<IntersectionEventArgs> ReboundAfterScreenCollisionChanged; //Событие на определение смещения после отскока от рамок игрового экрана
    public event EventHandler<IntersectionEventArgs> ReboundAfterCollisionChanged; // Событие на определение отскока после столкновения
    public event EventHandler<TextBonusEventArgs> TextBonusChanged; // Событие на столкновение показом текстового бонуса

    //Методы вызова события, который производные классы могут переопределить.
    public virtual void OnDeltaChanged(DeltaEventArgs e)
    {
      DeltaChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnIntersectionChanged(IntersectionEventArgs e)
    {
      IntersectionChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnHeartScullChanged(HeartScullEventArgs e)
    {
      HeartScullChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnPlatformMoveChanged(PlatformEventArgs e)
    {
      PlatformMoveChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnRoundGameChanged(IntersectionEventArgs e)
    {
      RoundGameChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnReboundAfterScreenCollisionChanged(IntersectionEventArgs e)
    {
      ReboundAfterScreenCollisionChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnReboundAfterCollisionChanged(IntersectionEventArgs e)
    {
      ReboundAfterCollisionChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    public virtual void OnTextBonusChanged(TextBonusEventArgs e)
    {
      TextBonusChanged?.Invoke(this, e);  // Безопасно поднять событие для всех подписчиков
    }
    //

    /// <summary> Устанавливаю координаты фигуры  </summary>
    public virtual void SetCoordinates(int xx1, int yy1, int xx2, int yy2)
    {
      x1 = xx1; y1 = yy1; x2 = xx2; y2 = yy2;
    }

    public bool CheckIntersection(List<DisplayObject> staticDO, List<DisplayObject> dynamicDO, VideoMode mode)
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
            DynamicDOCurrent = dynamicDO[i];
            StaticDOCurrent = staticDO[k];
            StaticDOList = staticDO;
            ReboundAfterCollision(staticDO[k], dynamicDO[i], staticDO);
            return true;
          }
          else if ((dynamicDO[i].x1 < 0) || //столкновение о стенки игрового экрана слева 
                   (dynamicDO[i].x2 > mode.Width) || //столкновение о стенки игрового экрана справа
                   (dynamicDO[i].y1 < 0)) //столкновение о вверх игрового экрана
          {
            DynamicDOCurrent = dynamicDO[i];
            ReboundAfterScreenCollision(dynamicDO[i], mode);
            return true;
          }
        }
      }
      return false;
    }

    /// <summary> Метод для определения смещения после отскока от рамок игрового экрана </summary>
    private void ReboundAfterScreenCollision(DisplayObject dynamicObject, VideoMode mode)
    {
      OnReboundAfterScreenCollisionChanged(new IntersectionEventArgs(dynamicObject, mode));
    }

    /// <summary> Метод для определения смещения после отскока от рамок игрового экрана </summary>
    public void ReboundAfterScreenCollisionExec(DisplayObject dynamicObject, VideoMode mode)
    {
      if (dynamicObject.x1 < 0) // если столкновение о стенки игрового экрана слева 
        dx = dx < 0 ? -dx : dx;
      if (dynamicObject.x2 > mode.Width) // если столкновение о стенки игрового экрана справа
        dx = dx > 0 ? -dx : dx;
      if (dynamicObject.y1 < 0) // если столкновение о верх игрового экрана
        dy = -dy;
    }

    /// <summary> Метод для определения отскока после столкновения </summary>
    private void ReboundAfterCollision(DisplayObject staticObject, DisplayObject dynamicObject, List<DisplayObject> staticDO)
    {
      OnReboundAfterCollisionChanged(new IntersectionEventArgs(staticObject, dynamicObject, staticDO));
    }

    /// <summary> Метод для определения отскока после столкновения </summary>
    public void ReboundAfterCollisionExec(DisplayObject staticObject, DisplayObject dynamicObject, List<DisplayObject> staticDO)
    {
      Random random = new Random();
      if (staticObject is Block staticBlock)
      {
        staticObject.Sprite.Position = new Vector2f(-100, 0);
        if (dynamicObject.x1 < staticObject.x1) // левая стенка блока
          dx = dx > 0 ? -dx : dx;
        if (dynamicObject.x2 > staticObject.x2) // правая стенка блока
          dx = dx < 0 ? -dx : dx;
        if (dynamicObject.y1 < staticObject.y2) // если столкновение о нижнюю часть блока
          dy = -dy;
        // проверяю, является ли блок бонусным
        IsBonus_1 = (DOBonus.ContainsKey(staticObject) && DOBonus[staticObject] == 1);
        IsBonus_2 = (DOBonus.ContainsKey(staticObject) && DOBonus[staticObject] == 2);
        // удаляю блок после столкновения из массива
        staticDO.Remove(staticObject);
        // считаю очки
        if (IsBonus_1)
          GameSetting.Score += GameSetting.SCORE_BONUS_STEP; // добавление к результату бонусных очков
        else 
          GameSetting.Score += GameSetting.SCORE_STEP; // вывод результата
      }
      else if (staticObject is Platform)
        dy = (random.Next() % 5 + GameSetting.BALL_DELTA_Y); // отскок шарика от платформы по оси у
    }

    /// <summary> Метод проверки пересечения объектов шара с блоками, платформой, стенками игрового экрана </summary>
    public virtual void ObjectIntersection(DisplayObject ball, List<DisplayObject> blocks, DisplayObject platform, DisplayObject heartScull,
                                           Dictionary<DisplayObject, int> doBonus, VideoMode mode, RenderTarget window)
    {
      if (GameSetting.IsStart)
      {
        ball.x1 += dx;
        ball.y1 -= dy;
        SetNewSetCoordinates(ball.x1, ball.y1, ball);

        // формирую спиские статических и динамических объектов для проверки на столкновение
        List<DisplayObject> dynamicDO = new List<DisplayObject>();
        dynamicDO.Add(ball);
        List<DisplayObject> staticDO = blocks;
        staticDO.Add(platform);
        DOBonus = doBonus;

        IsIntersection = CheckIntersection(staticDO, dynamicDO, mode);

        if (IsBonus_2) // бонус для платформы
        {
          platform.Scale += GameSetting.BONUS_PLATFORM;
          platform.Sprite.Scale = new Vector2f(platform.Scale, platform.Scale);
          IsBonus_2 = false; // чтобы бесконечно не увеличивалось
        }

        OnRoundGameChanged(new IntersectionEventArgs(ball, blocks, platform, heartScull, mode, window));
      }
    }

    /// <summary> Метод действия для события, если выбиты все блоки, или промах мимо платформы, т.е. столкновение о низ игрового экрана </summary>
    public void RoundGameEndBegin(DisplayObject ball, List<DisplayObject> blocks, DisplayObject platform, DisplayObject heartScull,
                                  VideoMode mode, RenderTarget window)
    {
      if (ball.y2 > mode.Height || blocks.Count == 0)
      {
        GameSetting.IsStart = false;
        dx = GameSetting.BALL_DELTA_X;
        dy = GameSetting.BALL_DELTA_Y;
        // ставлю мячик в середину игрового поля
        ball.x1 = (int)(mode.Width / 2) - (ball.SpriteWidth / 2); // вычисляю позицию по оси Х, чтобы посередине мячик был
        ball.y1 = (int)mode.Height - platform.SpriteHeight - ball.SpriteHeight; // вычисляю позицию по оси Y, чтобы мячик над платформой был
        GameSetting.LifeCount--; // минус жизнь
        heartScull.Draw(window, mode); // перерисовываю после минусования жизни
      }
    }

    /// <summary> Установка новых координат объекта </summary>
    private void SetNewSetCoordinates(int x1, int y1, DisplayObject displayObject)
    {
      positionObject = new Vector2f(x1, y1);
      int xx1 = Convert.ToInt32(positionObject.X);
      int yy1 = Convert.ToInt32(positionObject.Y);
      int xx2 = Convert.ToInt32(positionObject.X + displayObject.SpriteWidth);
      int yy2 = Convert.ToInt32(positionObject.Y + displayObject.SpriteHeight);
      displayObject.SetCoordinates(xx1, yy1, xx2, yy2);
    }

    /// <summary>Установка скорости движения объекта </summary>
    public void SetSpeedDO(int _x, int _y)
    {
      dx = _x; dy = _y;
    }
  }
}
