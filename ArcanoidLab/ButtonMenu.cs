using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.IO;

namespace ArcanoidLab
{
  /// <summary> Класс кнопки для игрового меню </summary>
  public class ButtonMenu
  {
    private readonly string FONT_PATH = Directory.GetCurrentDirectory() + @"\Assets\Fonts\FreeMono\";

    public RectangleShape MenuItemRect { get; set; }
    public Text MenuItemText { get; set; }
    public string AliasButton { get; set; } // алиас меню, например, "game" - кнопка Играть, "exit" - кнопка Выход и т.д.

    public Vector2f Position { get; set; }

    /// <summary> Версия конструктора создания кнопки меню с позицией по Х по центру </summary>
    /// <param name="VecX">Координата х прямоугольника</param>
    /// <param name="VecY">Координата y прямоугольника</param>
    /// <param name="textButton">Текст кнопки</param>
    /// <param name="alias">Алиас кнопки</param>
    /// <param name="fontSize">Размер шрифта текста кнопки</param>
    /// <param name="fontName">Название шрифта для текста кнопки</param>
    /// <param name="coorY">Расположение кнопки по оси у, т.е. чтобы можно было друг под другом распологать</param>
    /// <param name="colorText">Цвет текста кнопки</param>
    /// <param name="colorButton">Цвет фона кнопки</param>
    /// <param name="mode">Ссылка на видео режим</param>
    public ButtonMenu(float VecX, float VecY, string textButton, string alias, uint fontSize, string fontName, float coorY, 
                      Color colorText, Color colorButton, VideoMode mode)
    {
      // Созданию объектов текста и прямоугольников для каждого пункта меню
      //MenuItemRect = new RectangleShape(new Vector2f(250, 50));
      MenuItemRect = new RectangleShape(new Vector2f(VecX, VecY));
      MenuItemRect.FillColor = colorButton;
      MenuItemRect.OutlineColor = Color.Black;
      MenuItemRect.OutlineThickness = 2;
      MenuItemRect.Position = new Vector2f((int)((mode.Width / 2) - MenuItemRect.Size.X / 2), coorY);

      MenuItemText = new Text(textButton, new Font(FONT_PATH + fontName + ".ttf"), fontSize);
      MenuItemText.FillColor = colorText;
      MenuItemText.Position = new Vector2f((int)((mode.Width / 2) - MenuItemRect.Size.X / 2) + 5, coorY);

      AliasButton = alias;
    }

    /// <summary> Версия конструктора создания кнопки меню с ручной настройкой </summary>
    /// <param name="VecX">Координата х прямоугольника</param>
    /// <param name="VecY">Координата y прямоугольника</param>
    /// <param name="textButton">Текст кнопки</param>
    /// <param name="alias">Алиас кнопки</param>
    /// <param name="fontSize">Размер шрифта текста кнопки</param>
    /// <param name="fontName">Название шрифта для текста кнопки</param>
    /// <param name="coorX">Расположение кнопки по оси у, т.е. чтобы можно было друг под другом распологать</param>
    /// <param name="coorY">Расположение кнопки по оси у, т.е. чтобы можно было друг под другом распологать</param>
    /// <param name="colorText">Цвет текста кнопки</param>
    /// <param name="colorButton">Цвет фона кнопки</param>
    /// <param name="mode">Ссылка на видео режим</param>
    public ButtonMenu(float VecX, float VecY, string textButton, string alias, uint fontSize, string fontName, float coorX, float coorY,
                      Color colorText, Color colorButton, VideoMode mode)
    {
      // Созданию объектов текста и прямоугольников для каждого пункта меню
      MenuItemRect = new RectangleShape(new Vector2f(VecX, VecY));
      MenuItemRect.FillColor = colorButton;
      MenuItemRect.OutlineColor = Color.Black;
      MenuItemRect.OutlineThickness = 2;
      MenuItemRect.Position = new Vector2f(coorX, coorY);

      MenuItemText = new Text(textButton, new Font(FONT_PATH + fontName + ".ttf"), fontSize);
      MenuItemText.FillColor = colorText;
      MenuItemText.Position = new Vector2f(coorX + 5, coorY);

      AliasButton = alias;
    }

    /// <summary> Установка цвета для кнопки </summary>
    /// <param name="colorButton">Цвет кнопки</param>
    public void SetColorButton(Color colorButton)
    {
      MenuItemRect.FillColor = colorButton;
    }

    /// <summary> Получить цвет кнопки </summary>
    /// <param name="colorButton"></param>
    public Color GetColorButton()
    {
      return MenuItemRect.FillColor;
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