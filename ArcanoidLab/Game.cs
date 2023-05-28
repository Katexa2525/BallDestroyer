using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Globalization;
// подключаем атрибут DllImport
using System.Runtime.InteropServices;

namespace ArcanoidLab
{
  /// <summary> Класс главного игрового процесса </summary>
  public class Game
  {
    private const int WIDTH = 640;
    private const int HEIGHT = 480;
    private const string TITLE = "АРКАНОИД";
    private RenderWindow window;
    private VideoMode mode; //размер игрового окна
    private Sprite background;
    private bool exitProgram = false;
    private string InputText = "";

    private Platform platform;
    private Block block;
    private Ball ball;
    private HeartScull heartScull;
    private TextManager textManager; 
    private Secundomer secundomer;
    private GameMenu gameMenu;
    private GameState gameState;
    private ButtonMenu buttonMainMenu;
    private SaveLoadState saveLoadState;

    // Импортирую библиотку user32.dll (содержит WinAPI функцию MessageBox)
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, int options); // объявляем метод на C#

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
      this.window = new RenderWindow(this.mode, title);

      this.window.SetVerticalSyncEnabled(true); 
      this.window.SetFramerateLimit(60);

      this.window.Closed += (sender, args) =>
      {
        this.window.Close();
      };

      // проверка ввода символов
      this.window.TextEntered += (sender, args) =>
      {
        //GameSetting.PLAYER_NAME = GameSetting.PLAYER_NAME.Insert(GameSetting.PLAYER_NAME.Length, args.Unicode);
        OnTextEntered(sender, args);
        GameSetting.PLAYER_NAME = InputText;
      };

      // экземпляр класса для работы с текстом
      textManager = new TextManager();

      // загрузка изображений
      TextureManagerLoadTexture();

      // загрузка для работы со шрифтами
      TextureManager.LoadTexture();
      textManager.LoadFont("FreeMonospacedBold");

      // создаю экземпляры классов
      platform = new Platform(mode);
      block = new Block(mode);
      ball = new Ball(mode);
      heartScull = new HeartScull();
      secundomer = new Secundomer();
      gameMenu = new GameMenu(mode);
      saveLoadState = new SaveLoadState();

      // в поле positionObject объекта DisplayObject заношу координаты шара
      ball.positionObject = ball.Sprite.Position;
      // кнопка для вызова меню на главном окне 
      buttonMainMenu = new ButtonMenu(70, 15, "Меню...", "main", 13, "FreeMonospacedBold", 620, 2, Color.Red, Color.Yellow, mode);
    }

    // метод запуска игрового процесса
    public void Run()
    {
      while (this.window.IsOpen)
      {
        secundomer.OnStart(); // запуск секундомера
        // отрисовка элементов меню, если меню отображается
        if (GameSetting.IsVisibleMenu)
        {
          HandleEvents();
          KeyHandler();
          window.Clear();
          gameMenu.Draw(window);
          window.Display();
        }
        else if (GameSetting.LifeCount > 0) 
        {
          HandleEvents();
          KeyHandler();
          Update();
          Draw();
        }
        else
        {
          KeyHandler();
          string strFinish = "Игра окончена!\nДля новой игры нажмите F5\nВыход - F12";
          textManager.TypeText(strFinish, "", 20, Color.Red, new Vector2f(mode.Width / 2 - 150, mode.Height / 2 -100 ));
          Draw();
          if (exitProgram) break; // выход из игры
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
      ball.ObjectIntersection(ball, block.Blocks, platform, heartScull, mode, window);
      if (!GameSetting.IsStart) // если шарик не попал в платформу, то стартовые позиции объектов шарик и платформа
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
      textManager.TypeText("", secundomer.GetElapsedTime(), 14, Color.Yellow, new Vector2f(220f, 0f));
      textManager.TypeText("Уровень: ", GameSetting.LEVEL, 14, Color.Yellow, new Vector2f(450f, 0f));

      this.platform.Draw(this.window, mode);
      this.block.Draw(this.window);
      this.ball.Draw(this.window);
      textManager.Draw(this.window);
      heartScull.Draw(this.window, mode);
      buttonMainMenu.Draw(this.window);

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
        GameSetting.IsStart = true;
        GameSetting.LifeCount = GameSetting.LIFE_TOTAL;
        GameSetting.Score = 0;
        block.Update(mode);
      }
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F12)) // выход
      {
        exitProgram = true;
      }
      else if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) // вызов меню
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
          GameSetting.IsVisibleMenu = true;
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
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "exit") // выход из игры через меню
            window.Close();
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "save") // сохранение в json состояния игры
          {
            gameState = new GameState(ball, platform, block.Blocks);
            saveLoadState.SaveState(gameState);
          }
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "load") // загрузка из json состояния игры
          {
            gameState = saveLoadState.LoadState(ball, platform, block, mode);
          }
        }
        else
        {
          gameMenu.ButtonMenus[i].SetColorButton(Color.Green);
          gameMenu.ButtonMenus[i].SetColorTextButton(Color.Red);
        }
        gameMenu.ButtonMenus[i].Draw(window);
      }
    }

    // метод обновления очков на экране через класс работы с текстом
    private void UpdateScore()
    {
      textManager.TypeText("Очки: ", GameSetting.Score.ToString(), 14, Color.White, new Vector2f(5f, 0f));
    }

    void OnTextEntered(object sender, TextEventArgs e)
    {
      //convert unicode to ascii to check range later
      string hexValue = (System.Text.Encoding.ASCII.GetBytes(e.Unicode)[0].ToString("X"));
      int ascii = (int.Parse(hexValue, NumberStyles.HexNumber));

      if (e.Unicode == "\b")
      {
        if (InputText.Length > 0)
        {
          InputText = InputText.Remove(InputText.Length - 1, 1);
        }
      }
      else if (e.Unicode == "\r")
      {
        InputText += "\n";
      }
      else if (ascii >= 32 && ascii < 128)
      { //only add to keystring if actual character
        InputText += e.Unicode;
      }
    }

  }
}
