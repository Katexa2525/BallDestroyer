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
      ballManager = new BallManager(ball, heartScull);
    }

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
      ballManager.Update(block, ball, platform, heartScull, mode, this.window);
      
    }

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

    private void KeyHandler()
    {
      if (!ballManager.IsStart) // если игра не началась, то проверяем нажатие, иначе нет
        ballManager.IsStart = Keyboard.IsKeyPressed(Keyboard.Key.Space);

      if (Keyboard.IsKeyPressed(Keyboard.Key.F5)) // новая игра
      {
        ballManager.IsStart = true;
        heartScull.LifeCount = heartScull.LIFE_TOTAL;
        platform.Score = 0;
        block.Update(mode);
      }

      if (Keyboard.IsKeyPressed(Keyboard.Key.F12)) // выход
      {
        exitProgram = true;
      }
    }

    private void UpdateScore()
    {
      textManager.TypeText("Очки: ", platform.Score.ToString(), 14, Color.White, new Vector2f(5f, 0f));
    }
  }
}
