﻿using ArcanoidLab.EventArgsClass;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
    private SFML.Graphics.View viewWindow;
    private SFML.Graphics.View viewMenu;

    private Platform platform;
    private Block block;
    private Ball ball;
    private HeartScull heartScull;
    private TextManager textManager;
    private ButtonMenu buttonMainMenu;
    private WinForm winForm;
    private GameMenu gameMenu;
    private GameState gameState;
    private SaveLoadState saveLoadState;

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
      // создание области просмотра для главного окна
      viewWindow = new SFML.Graphics.View(new FloatRect(0, 0, mode.Width, mode.Height));
      viewWindow.Viewport = new FloatRect(0, 0, 1, 1); // Отображение на всю область окна

      // создание области просмотра для главного окна
      viewMenu = new SFML.Graphics.View(new FloatRect(0, 0, mode.Width, mode.Height));
      viewMenu.Viewport = new FloatRect(0, 0, 1, 1); // Отображение на всю область окна

      // Установка области просмотра в окно
      window.SetView(viewWindow);

      this.window.SetVerticalSyncEnabled(true);
      this.window.SetFramerateLimit(60);

      this.window.Closed += (sender, args) =>
      {
        if (MessageBox.Show("Выйти из игры", "Вопрос...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.window.Close();
      };

      // проверка ввода символов
      this.window.TextEntered += (sender, args) =>
      {
        //преобразовать unicode в ascii, чтобы позже проверить диапазон
        string hexValue = (Encoding.ASCII.GetBytes(args.Unicode)[0].ToString("X"));
        int ascii = (int.Parse(hexValue, NumberStyles.HexNumber));

        if (args.Unicode == "\b" && gameMenu.TextBox.ContentText.Length > 0)
        {
          gameMenu.TextBox.ContentText = gameMenu.TextBox.ContentText.Remove(gameMenu.TextBox.ContentText.Length - 1);
        }
        //добавлять в строку имени, если фактический символ
        else if (ascii >= 32 && ascii < 128 && gameMenu.TextBox.ContentText.Length < 15)
        {
          gameMenu.TextBox.ContentText += args.Unicode;
        }
        gameMenu.TextBox.SetContentText(gameMenu.TextBox.ContentText, "FreeMonospacedBold", 16, Color.Black, 300, 152);
        gameMenu.Draw(window, ball);
        GameSetting.PLAYER_NAME = gameMenu.TextBox.ContentText;
      };

      gameMenu = new GameMenu(mode, this);
      saveLoadState = new SaveLoadState();

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
        if (GameSetting.IsVisibleMenu)
        {
          HandleEvents();
          KeyHandler();
          // Установка области просмотра меню в окно
          window.SetView(viewMenu);
          gameMenu.Draw(window, ball);
          window.Display();
        }
        else if (GameSetting.LifeCount > 0)
        {
          // Установка области просмотра главного игрового окна
          window.SetView(viewWindow);
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
      else if (Keyboard.IsKeyPressed(Keyboard.Key.Q)) // вызов меню в виде windows form
      {
        winForm.FormUser.ShowDialog();
      }
      else if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) // вызов разработанного меню 
      {
        GameSetting.IsVisibleMenu = true;
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

      /////////////// Работа с кнопками меню //////////////////////////////////////////
      // проверка, наведена ли мышь на пункт меню
      for (int i = 0; i < gameMenu.ButtonMenus.Count; i++)
      {
        if (Keyboard.IsKeyPressed(Keyboard.Key.F1)) // продолжаем играть
          GameSetting.IsVisibleMenu = false;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.F2))
        {
          gameState = new GameState(ball, platform, block.Blocks, block.BlocksBonus);
          saveLoadState.SaveState(gameState);
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.F3))
        {
          gameState = saveLoadState.LoadState(ball, platform, block, mode);
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.F12) && 
                 MessageBox.Show("Выйти из игры", "Вопрос...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          window.Close();
        }

        // проверяю, находится ли курсор мыши над прямоугольником меню
        if (gameMenu.ButtonMenus[i].MenuItemRect.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
        {
          // курсор мыши находится над прямоугольником пункта меню 
          gameMenu.ButtonMenus[i].SetColorButton(Color.Magenta); // меняю цвет пункта
          gameMenu.ButtonMenus[i].SetColorTextButton(Color.Black); // меняю цвет текста
          if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "play") // проверяю, было ли нажатие, тогда начало игры
          {
            GameSetting.IsVisibleMenu = false;
          }
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "exit" &&
                   MessageBox.Show("Выйти из игры", "Вопрос...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // выход из игры через меню
            window.Close();
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "save") // сохранение в json состояния игры
          {
            gameState = new GameState(ball, platform, block.Blocks, block.BlocksBonus);
            saveLoadState.SaveState(gameState);
          }
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "load") // загрузка из json состояния игры
          {
            gameState = saveLoadState.LoadState(ball, platform, block, mode);
          }
        }
        else
        {
          gameMenu.ButtonMenus[i].SetColorButton(Color.Yellow);
          gameMenu.ButtonMenus[i].SetColorTextButton(Color.Red);
        }
        gameMenu.ButtonMenus[i].Draw(window);
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
