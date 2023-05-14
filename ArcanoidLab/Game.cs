using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;

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
    private TextManager textManager; 
    private Secundomer secundomer;
    private GameMenu gameMenu;
    private GameState gameState;

    private string jsonFilePath = Directory.GetCurrentDirectory() + @"\ball.json";

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
      gameMenu = new GameMenu(mode);

      // в поле positionObject объекта DisplayObject заношу координаты шара
      ball.positionObject = ball.Sprite.Position;
      secundomer = new Secundomer();
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
      textManager.TypeText("Игрок: ", "Катя", 14, Color.Yellow, new Vector2f(100f, 0f));
      textManager.TypeText("", secundomer.GetElapsedTime(), 14, Color.Yellow, new Vector2f(220f, 0f));
      textManager.TypeText("Уровень: ", "Easy", 14, Color.Yellow, new Vector2f(450f, 0f));

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

      // проверка, наведена ли мышь на пункт меню
      for (int i = 0; i < gameMenu.ButtonMenus.Count; i++)
      {
        // проверяем, находится ли курсор мыши над прямоугольником меню
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
            gameState = new GameState(ball);
            // Сериализация объекта в формат JSON
            var json = JsonConvert.SerializeObject(gameState);
            // Запись JSON-строки в файл
            File.WriteAllText(jsonFilePath, json);
          }
          else if (Mouse.IsButtonPressed(Mouse.Button.Left) && gameMenu.ButtonMenus[i].AliasButton == "load") // загрузка из json состояния игры
          {
            // Чтение JSON-строки из файла
            string json = File.ReadAllText(jsonFilePath);
            // Десериализация JSON-строки в объект класса GameState
            gameState = JsonConvert.DeserializeObject<GameState>(json);
            ball = gameState.DisplayObject;
            GameSetting.IsVisibleMenu = false;
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
  }
}
