using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс для рисования формы посередине экрана в зависимости от размеров формы </summary>
  public class GameForm
  {
    RectangleShape rectForm;
    RectangleShape rectFormShadow;
    private Vector2f position;

    public GameForm(VideoMode mode, float rectX, float rectY, Color colorForm, Color colorShadow)
    {
      rectForm = new RectangleShape(new Vector2f(rectX, rectY));
      
      rectForm.FillColor = colorForm;

      rectFormShadow = new RectangleShape(new Vector2f(rectX, rectY+5));
      rectFormShadow.FillColor = colorShadow;

      StartPosition(mode);
    }

    public void StartPosition(VideoMode mode)
    {
      position.X = (mode.Width / 2) - (rectForm.Size.X / 2); // вычисляю позицию по оси Х
      position.Y = (mode.Height / 2) - rectForm.Size.Y;      // вычисляю позицию по оси Y
      rectForm.Position = position;
      position.X = (mode.Width / 2) - (rectForm.Size.X / 2) + 5;
      position.Y = (mode.Height / 2) - rectForm.Size.Y + 3;
      rectFormShadow.Position = position;
    }

    /// <summary> Отображаю кнопку на экране </summary>
    public void Draw(RenderTarget window)
    {
      // Рисую прямоугольник формы
      window.Draw(rectFormShadow);
      window.Draw(rectForm);
      
    }
  }
}
