using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс для показа бонуса </summary>
  public class Bonus : DisplayObject
  {
    public TextBoxLabel TextBoxLabel { get; set; }

    public Bonus()
    {    }

    public Bonus(VideoMode mode, string textLabel, uint sizeText, Color color, float coorX, float coorY)
    {
      // Создаю текст с содержанием "+100", или что задам
      TextBoxLabel = new TextBoxLabel(textLabel, "FreeMonospacedBold", sizeText, color, coorX, coorY);
    }

    public override void Draw(RenderTarget window)
    {
      TextBoxLabel.Draw(window);
    }

    public override void StartPosition(VideoMode mode) 
    {
      this.positionObject = new Vector2f(-100, -100);
    }

    public override void Draw(RenderTarget window, VideoMode mode) { }

    public override void Update(VideoMode mode) {    }
  }
}
