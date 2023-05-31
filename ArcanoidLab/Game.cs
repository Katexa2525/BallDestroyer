using ArcanoidLab.EventArgsClass;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArcanoidLab
{
  /// <summary> Класс главного игрового процесса </summary>
  public class Game
  {
    private const int WIDTH = 1024;
    private const int HEIGHT = 768;
    private const string TITLE = "АРКАНОИД";
    private Sprite background;

    private Platform platform;
    private Block block;
    private Ball ball;
    private HeartScull heartScull;
    private TextManager textManager;
    private ButtonMenu buttonMainMenu;
    private WinForm winForm;

    public RenderWindow window { get; set; }
    public VideoMode mode { get; set; } //размер игрового окна

    public Secundomer Secundomer { get; set; }

    // конструктор по умолчанию
    public Game()
    {
      GameCreate(WIDTH, HEIGHT, TITLE);
    }

    // конструктор с заданными параметрами
    public Game(uint width, uint height, string title)
    {
      GameCreate(width, height, title);
    }

    private void GameCreate(uint width, uint height, string title)
    {
      mode = new VideoMode(width, height);
      this.window = new RenderWindow(this.mode, title, Styles.Fullscreen);

      // Создание области просмотра, начинающейся с (0, 0) и размером 800x600
      //View view = new View(new FloatRect(0, 0, width, height));

      // Установка области просмотра в окно
      //window.SetView(view);

      this.window.SetVerticalSyncEnabled(true);
      this.window.SetFramerateLimit(60);

      this.window.Closed += (sender, args) =>
      {
        if (MessageBox.Show("Выйти из игры", "Вопрос...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.window.Close();
      };

      // экземпляр класса для работы с текстом
      textManager = new TextManager();

      // загрузка изображений
      TextureManagerLoadTexture();

      // загрузка для работы со шрифтами
      TextureManager.LoadTexture();
      textManager.LoadFont("FreeMonospacedBold");
      textManager.TextBonusChanged += HandleTextBonusChanged;

      // создаю экземпляры классов
      platform = new Platform(mode);
      platform.PlatformMoveChanged += HandlePlatformMoveChanged;

      block = new Block(mode);

      ball = new Ball(mode);
      ball.DeltaChanged += HandleDeltaChanged; // подписка на событие 
      ball.IntersectionChanged += HandleIntersectionChanged; // подписка на событие
      ball.RoundGameChanged += HandleRoundGameChanged;
      ball.ReboundAfterScreenCollisionChanged += HandleReboundAfterScreenCollisionChanged; // подписка на событие определения смещения после отскока от рамок игрового экрана
      ball.ReboundAfterCollisionChanged += HandleReboundAfterCollisionChanged; // подписка на событие определения отскока после столкновения

      heartScull = new HeartScull();
      heartScull.HeartScullChanged += HandleHeartScullChanged;

      Secundomer = new Secundomer();
      winForm = new WinForm(ball, platform, block, mode, this);

      // в поле positionObject объекта DisplayObject заношу координаты шара
      ball.positionObject = ball.Sprite.Position;
      // кнопка для вызова меню на главном окне 
      buttonMainMenu = new ButtonMenu(80, 15, "Меню Esc", "main", 13, "FreeMonospacedBold", 620, 2, Color.Red, Color.Yellow, mode);
    }

    // обработчик события изменения скорости шарика
    private void HandleDeltaChanged(object sender, DeltaEventArgs e)
    {
      if (sender is Ball _ball)
        _ball.SetSpeedDO(e.DX, e.DY);
    }

    // обработчик события столкновения
    private void HandleIntersectionChanged(object sender, IntersectionEventArgs e)
    {
      if (sender is Ball _ball)
        _ball.ObjectIntersection(e.Ball, e.Blocks, e.Platform, e.HeartScull, e.DOBonus, e.Mode, e.Window);
    }

    // обработчик события обновления жизни игрока
    private void HandleHeartScullChanged(object sender, HeartScullEventArgs e)
    {
      if (sender is HeartScull _heartScull)
        _heartScull.HeartScullPositionScreen(GameSetting.LIFE_TOTAL, GameSetting.LifeCount, mode);
    }
    // обработчик события движения платформы
    private void HandlePlatformMoveChanged(object sender, PlatformEventArgs e)
    {
      if (sender is Platform _platform)
        _platform.PlatformMove(_platform.IsMove, _platform.MoveLeft, _platform.MoveRight, mode, GameSetting.PLATFORM_SPEED, _platform.SpriteWidth);
    }
    // обработчик события столкновения
    private void HandleRoundGameChanged(object sender, IntersectionEventArgs e)
    {
      if (sender is Ball _ball)
        _ball.RoundGameEndBegin(e.Ball, e.Blocks, e.Platform, e.HeartScull, e.Mode, e.Window);
    }
    // обработчик определения смещения после отскока от рамок игрового экрана
    private void HandleReboundAfterScreenCollisionChanged(object sender, IntersectionEventArgs e)
    {
      if (sender is Ball _ball)
        _ball.ReboundAfterScreenCollisionExec(_ball.DynamicDOCurrent, e.Mode);
    }
    // обработчик определения отскока после столкновения
    private void HandleReboundAfterCollisionChanged(object sender, IntersectionEventArgs e)
    {
      if (sender is Ball _ball)
        _ball.ReboundAfterCollisionExec(_ball.StaticDOCurrent, _ball.DynamicDOCurrent, _ball.StaticDOList);
    }
    // обработчик определения отскока после столкновения
    private void HandleTextBonusChanged(object sender, TextBonusEventArgs e)
    {
      if (sender is TextManager _textManager)
        _textManager.TypeText(e.Text, e.Value, e.FontSize, e.FontColor, e.Position);
    }

    // метод запуска игрового процесса
    public void Run()
    {
      while (this.window.IsOpen)
      {
        Secundomer.OnStart(); // запуск секундомера
        if (GameSetting.LifeCount > 0)
        {
          HandleEvents();
          KeyHandler();
          Update();
          Draw();
        }
        else
        {
          if (MessageBox.Show("Игра окончена! Начать новую игру?", "Вопрос...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            StartNewGame();
          else
            this.window.Close();
        }
      }
    }

    private void HandleEvents()
    {
      this.window.DispatchEvents();
    }
    private void Update()
    {
      KeyHandler();
      UpdateScore();
      // вызываю проверку коллизии, т.е. пересечения фигур
      ball.OnIntersectionChanged(new IntersectionEventArgs(ball, block.Blocks, platform, heartScull, block.BlocksBonus, mode, window)); // через событие
      //ball.ObjectIntersection(ball, block.Blocks, platform, heartScull, mode, window);
      // если шарик не попал в платформу, то стартовые позиции объектов шарик и платформа
      if (!GameSetting.IsStart) 
      {
        ball.StartPosition(mode);
        platform.StartPosition(mode);
        ball.positionObject = ball.Sprite.Position;
      }
      ball.Sprite.Position = ball.positionObject; // новые координаты шарика
    }

    // метод отрисовки объектов
    private void Draw()
    {
      this.window.Clear(Color.Blue);
      // доп данные
      textManager.TypeText("Игрок: ", GameSetting.PLAYER_NAME, 14, Color.Yellow, new Vector2f(100f, 0f));
      textManager.TypeText("", Secundomer.GetElapsedTime("Время:"), 14, Color.Yellow, new Vector2f(320f, 0f));
      textManager.TypeText("Уровень: ", GameSetting.LEVEL, 14, Color.Yellow, new Vector2f(450f, 0f));

      this.platform.Draw(this.window, mode);
      this.block.Draw(this.window);
      this.ball.Draw(this.window);
      this.textManager.Draw(this.window);
      this.heartScull.Draw(this.window, mode);
      this.buttonMainMenu.Draw(this.window);

      this.window.Display();
    }

    private void TextureManagerLoadTexture()
    {
      TextureManager.LoadTexture();

      background = new Sprite();
      background.Texture = TextureManager.BackgroundTexture;
    }

    // метод обработчик нажатия клавиш
    private void KeyHandler()
    {
      Vector2i mousePosition = Mouse.GetPosition(window); // координаты мыши

      if (!GameSetting.IsStart) // если игра не началась, то проверяем нажатие, иначе нет
        GameSetting.IsStart = Keyboard.IsKeyPressed(Keyboard.Key.Space);

      if (Keyboard.IsKeyPressed(Keyboard.Key.F5)) // новая игра
      {
        StartNewGame();
      }
      else if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) // вызов меню
      {
        winForm.FormUser.ShowDialog();
      }

      // проверяю, находится ли курсор мыши над прямоугольником главной кнопки меню
      if (buttonMainMenu.MenuItemRect.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
      {
        // курсор мыши находится над прямоугольником пункта меню 
        buttonMainMenu.SetColorButton(Color.Magenta); // меняю цвет пункта
        buttonMainMenu.SetColorTextButton(Color.Black); // меняю цвет текста
        // проверяю, было ли нажатие, тогда вызов экрана с пунктами меню
        if (Mouse.IsButtonPressed(Mouse.Button.Left) && buttonMainMenu.AliasButton == "main")
        {
          winForm.FormUser.ShowDialog();
        }
      }
      else
      {
        buttonMainMenu.SetColorButton(Color.Yellow);
        buttonMainMenu.SetColorTextButton(Color.Red);
      }
    }

    // метод обновления очков на экране через класс работы с текстом
    private void UpdateScore()
    {
      textManager.TypeText("Очки: ", GameSetting.Score.ToString(), 14, Color.White, new Vector2f(5f, 0f));
      if (ball.IsBonus_1) // в блок для бонусных очков попал шар
      {
        textManager.OnTextBonusChanged(new TextBonusEventArgs("+" + GameSetting.SCORE_BONUS_STEP.ToString(), "", 26, Color.Red, ball.positionObject /*new Vector2f(450f, 400f)*/));
      }
      if (ball.IsBonus_2) // в блок для бонуса платформы попал шар
      {
        textManager.OnTextBonusChanged(new TextBonusEventArgs("x" + GameSetting.BONUS_PLATFORM.ToString(), "", 26, Color.Red, ball.positionObject /*new Vector2f(450f, 400f)*/));
      }
    }

    private void StartNewGame()
    {
      GameSetting.IsStart = true;
      GameSetting.LifeCount = GameSetting.LIFE_TOTAL;
      GameSetting.Score = 0;
      block.Update(mode);
    }

  }
}
