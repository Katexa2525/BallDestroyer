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
    private RenderWindow window;
    private VideoMode mode; //размер игрового окна
    private Sprite background;
    private bool exitProgram = false;

    private Platform platform;
    private Block block;
    private Ball ball;
    private HeartScull heartScull;
    private BallManager ballManager;
    private TextManager textManager;

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

      // в поле positionObject объекта DisplayObject заношу координаты шара
      ball.positionObject = ball.Sprite.Position;
    }

    // метод запуска игрового процесса
    public void Run()
    {
      while (this.window.IsOpen)
      {
        if (heartScull.LifeCount > 0) 
        {
          HandleEvents();
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
      ball.CheckCollision(ball.Sprite.Position.X, ball.Sprite.Position.Y, block.Blocks, platform, heartScull, mode, window);
      if (!ball.IsStart) // если шарик не попал в платформу, то стартовые позиции объектов шарик и платформа
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
      
      this.platform.Draw(this.window, mode);
      this.block.Draw(this.window);
      this.ball.Draw(this.window);
      textManager.Draw(this.window);
      heartScull.Draw(this.window, mode);

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
      if (!ball.IsStart) // если игра не началась, то проверяем нажатие, иначе нет
        ball.IsStart = Keyboard.IsKeyPressed(Keyboard.Key.Space);

      if (Keyboard.IsKeyPressed(Keyboard.Key.F5)) // новая игра
      {
        ball.IsStart = true;
        heartScull.LifeCount = heartScull.LIFE_TOTAL;
        ball.Score = 0;
        block.Update(mode);
      }

      if (Keyboard.IsKeyPressed(Keyboard.Key.F12)) // выход
      {
        exitProgram = true;
      }
    }

    // метод обновления очков на экране через класс работы с текстом
    private void UpdateScore()
    {
      textManager.TypeText("Очки: ", ball.Score.ToString(), 14, Color.White, new Vector2f(5f, 0f));
    }
  }
}
