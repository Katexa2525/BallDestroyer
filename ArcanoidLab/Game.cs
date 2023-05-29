using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс главного игрового процесса </summary>
  public class Game
  {
    private const int WIDTH = 640;
    private const int HEIGHT = 480;
    private const string TITLE = "АРКАНОИД";
    private bool exitProgram = false; // признак выхода из игры
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
      this.window = new RenderWindow(this.mode, title/*, Styles.Fullscreen*/);

      // Создание области просмотра, начинающейся с (0, 0) и размером 800x600
      //View view = new View(new FloatRect(0, 0, width, height));

      // Установка области просмотра в окно
      //window.SetView(view);

      this.window.SetVerticalSyncEnabled(true);
      this.window.SetFramerateLimit(60);

      this.window.Closed += (sender, args) =>
      {
        this.window.Close();
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
      Secundomer = new Secundomer();
      winForm = new WinForm(ball, platform, block, mode, this);

      // в поле positionObject объекта DisplayObject заношу координаты шара
      ball.positionObject = ball.Sprite.Position;
      // кнопка для вызова меню на главном окне 
      buttonMainMenu = new ButtonMenu(80, 15, "Меню Esc", "main", 13, "FreeMonospacedBold", 620, 2, Color.Red, Color.Yellow, mode);
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
          winForm.FormUser.ShowDialog();
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
    }

  }
}
