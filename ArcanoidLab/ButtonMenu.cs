using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс кнопки для игрового меню </summary>
  public class ButtonMenu
  {
    private RectangleShape shape;
    private Text label;

    public ButtonMenu(string text, Font font, uint fontSize)
    {
      // Создаем фигуру прямоугольника для кнопки
      shape = new RectangleShape(new Vector2f(200, 50));
      shape.Origin = new Vector2f(shape.Size.X / 2, shape.Size.Y / 2);
      shape.FillColor = Color.Green;
      shape.OutlineColor = Color.Black;
      shape.OutlineThickness = 2;
      shape.Position = new Vector2f(0, 0);

      // Создаем текст для надписи на кнопке
      label = new Text(text, font, fontSize);
      label.Position = new Vector2f(shape.Position.X - label.GetGlobalBounds().Width / 2, shape.Position.Y - label.GetGlobalBounds().Height / 2);
    }
    
    /// <summary> Получаю размер кнопки </summary>
    /// <returns></returns>
    public Vector2f GetSize()
    {
      return shape.Size;
    }

    /// <summary> Проверяю, нажата ли кнопка  </summary>
    /// <returns></returns>
    public bool IsPressed()
    {
      // Получаем позицию курсора
      Vector2i mousePosition = Mouse.GetPosition();

      // Проверяем, был ли клик мышью на кнопке
      if (shape.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y) && Mouse.IsButtonPressed(Mouse.Button.Left))
      {
        return true;
      }

      return false;
    }

    /// <summary> Отображаю кнопку на экране </summary>
    /// <param name="target"></param>
    /// <param name="states"></param>
    public void Draw(RenderTarget target, RenderStates states)
    {
      // Рисую прямоугольник и текст на кнопке
      target.Draw(shape, states);
      target.Draw(label, states);
    }
  }
}

/*
namespace GameMenu
{
  class Program
  {
    static void Main(string[] args)
    {
      // Создаем окно с размерами 800x600 и заголовком "Игровое меню"
      RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Игровое меню");

      // Создаем шрифт для текста
      Font font = new Font("Arial.ttf");

      // Создаем текст для заголовка
      Text title = new Text("ИГРОВОЕ МЕНЮ", font, 48);
      title.Position = new Vector2f(window.Size.X / 2 - title.GetGlobalBounds().Width / 2, 50);

      // Создаем кнопку для новой игры
      ButtonMenu newGameButton = new ButtonMenu("НОВАЯ ИГРА", font, 36);
      newGameButton.Position = new Vector2f(window.Size.X / 2 - newGameButton.GetSize().X / 2, 200);

      // Создаем кнопку для выхода из приложения
      ButtonMenu quitButton = new ButtonMenu("ВЫХОД", font, 36);
      quitButton.Position = new Vector2f(window.Size.X / 2 - quitButton.GetSize().X / 2, 300);

      // Основной цикл приложения
      while (window.IsOpen)
      {
        // Обрабатываем события ввода
        window.DispatchEvents();

        // Очищаем экран
        window.Clear(Color.White);

        // Рисуем заголовок и кнопки
        window.Draw(title);
        window.Draw(newGameButton);
        window.Draw(quitButton);

        // Обновляем экран
        window.Display();

        // Обработка нажатий кнопок
        if (newGameButton.IsPressed())
        {
          // Запуск новой игры
          Console.WriteLine("Запуск новой игры...");
        }

        if (quitButton.IsPressed())
        {
          // Выход из приложения
          window.Close();
        }
      }
    }
  }

  */
