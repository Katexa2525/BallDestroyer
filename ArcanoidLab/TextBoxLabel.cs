using SFML.Graphics;
using SFML.System;
using System.IO;

namespace ArcanoidLab
{
  /// <summary> Класс для рисования поля для ввода и подписи этого поля </summary>
  public class TextBoxLabel
  {
    private readonly string FONT_PATH = Directory.GetCurrentDirectory() + @"\Assets\Fonts\FreeMono\";
    private readonly Text LabelText;
    private Text NameText;

    public RectangleShape ItemRect { get; set; }
    public string ContentText { get; set; } = GameSetting.PLAYER_NAME;

    public TextBoxLabel(string textLabel, string fontNameLabel, uint fontSizeLabel, Color colorTextLabel, float coorXLabel, float coorYLabel,
                   string fontNameText, uint fontSizeText, Color colorTextText, float coorXText, float coorYText,
                   float sizeXRect, float sizeYRect, float coorXRect, float coorYRect, Color OutlineRect)
    {
      LabelText = new Text("Игрок: ", new Font(FONT_PATH + fontNameLabel + ".ttf"), fontSizeLabel);
      LabelText.Position = new Vector2f(coorXLabel, coorYLabel);
      LabelText.FillColor = colorTextLabel;

      ItemRect = new RectangleShape(new Vector2f(sizeXRect, sizeYRect));
      ItemRect.Position = new Vector2f(coorXRect, coorYRect);
      ItemRect.OutlineColor = OutlineRect;
      ItemRect.OutlineThickness = 1;

      NameText = SetContentText(ContentText, fontNameText, fontSizeText, colorTextText, coorXText, coorYText);
    }

    /// <summary> Устанавливаю новый текст для элемента Text </summary>
    public Text SetContentText(string textText, string fontNameText, uint fontSizeText, Color colorTextText, float coorXText, float coorYText)
    {
      NameText = new Text(textText, new Font(FONT_PATH + fontNameText + ".ttf"), 16);
      NameText.Position = new Vector2f(coorXText, coorYText);
      NameText.FillColor = colorTextText;

      return NameText;
    }

    /// <summary> Отображаю кнопку на экране </summary>
    public void Draw(RenderTarget window)
    {
      // Рисую прямоугольник и текст на кнопке
      window.Draw(ItemRect);
      window.Draw(NameText);
      window.Draw(LabelText);
    }

  }
}
