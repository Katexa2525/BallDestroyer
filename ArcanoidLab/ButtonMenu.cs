using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.IO;

namespace ArcanoidLab
{
  /// <summary> Класс кнопки для игрового меню </summary>
  public class ButtonMenu
  {
    private string FONT_PATH = Directory.GetCurrentDirectory() + @"\Assets\Fonts\FreeMono\";

    public RectangleShape MenuItemRect { get; set; }
    public Text MenuItemText { get; set; } 

    public Vector2f Position { get; set; }

    public ButtonMenu(string textButton, uint fontSize, string fontName, float coorY, Color colorText, Color colorButton, VideoMode mode)
    {
      // Созданию объектов текста и прямоугольников для каждого пункта меню
      MenuItemRect = new RectangleShape(new Vector2f(250, 50));
      MenuItemRect.FillColor = colorButton;
      MenuItemRect.OutlineColor = Color.Black;
      MenuItemRect.OutlineThickness = 2;
      MenuItemRect.Position = new Vector2f((int)((mode.Width / 2) - MenuItemRect.Size.X / 2), coorY);

      MenuItemText = new Text(textButton, new Font(FONT_PATH + fontName + ".ttf"), fontSize);
      MenuItemText.FillColor = colorText;
      MenuItemText.Position = new Vector2f((int)((mode.Width / 2) - MenuItemRect.Size.X / 2) + 30, coorY);
    }

    /// <summary> Установка цвета для кнопки </summary>
    /// <param name="colorButton">Цвет кнопки</param>
    public void SetColorButton(Color colorButton)
    {
      MenuItemRect.FillColor = colorButton;
    }

    /// <summary> Установка цвета для текста кнопки </summary>
    /// <param name="colorText">Цвет текста</param>
    public void SetColorTextButton(Color colorText)
    {
      MenuItemText.FillColor = colorText;
    }

    /// <summary> Отображаю кнопку на экране </summary>
    public void Draw(RenderTarget window)
    {
      // Рисую прямоугольник и текст на кнопке
      window.Draw(MenuItemRect);
      window.Draw(MenuItemText);
    }
  }
}