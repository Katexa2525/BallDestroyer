using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.IO;

namespace ArcanoidLab
{
  /// <summary> Класс для работы с текстом </summary>
  public class TextManager : DisplayObject
  {
    private readonly string FONT_PATH = Directory.GetCurrentDirectory() + @"\Assets\Fonts\FreeMono\";
    private Font Font;

    private List<Text> texts = new List<Text>();

    public void LoadFont(string fontFamily)
    {
      Font = new Font(FONT_PATH + fontFamily + ".ttf");
    }

    public void TypeText(string text, string value, uint fontSize, Color fontColor, Vector2f position)
    {
      Text textContent = new Text(text + value, Font, fontSize);
      textContent.Position = position;
      textContent.FillColor = fontColor;
      texts.Add(textContent);
    }

    public override void Draw(RenderTarget window)
    {
      for (int i = 0; i < texts.Count; i++)
      {
        window.Draw(texts[i]);
        texts.Remove(texts[i]);
      }
    }
    
    public override void StartPosition(VideoMode mode) { }
    public override void Update(VideoMode mode) { }
    public override void Draw(RenderTarget window, VideoMode mode) { }
  }
}
