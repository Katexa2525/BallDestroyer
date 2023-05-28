using SFML.Graphics;
using SFML.System;
using System.IO;

namespace ArcanoidLab
{
  public class TextBox
  {
    private TextManager textManager = new TextManager();

    public RectangleShape ItemRect { get; set; }

    public TextBox()
    {
      // загрузка для работы со шрифтами
      textManager.LoadFont("FreeMonospacedBold");
      textManager.TypeText("Имя игрока: ", "", 16, Color.Yellow, new Vector2f(50f, 50f));

      ItemRect = new RectangleShape(new Vector2f(200, 30));
      ItemRect.Position = new Vector2f(300, 150);
      ItemRect.OutlineColor = Color.White;
      ItemRect.OutlineThickness = 1;

    }

    /// <summary> Отображаю кнопку на экране </summary>
    public void Draw(RenderTarget window)
    {
      // Рисую прямоугольник и текст на кнопке
      window.Draw(ItemRect);
      textManager.Draw(window);
    }
  }
}
