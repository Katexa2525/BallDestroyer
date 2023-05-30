using SFML.Graphics;
using SFML.System;


namespace ArcanoidLab.EventArgsClass
{
  // <summary> Класс EventArgs для хранения информации об объектах для рисования текста бонуса </summary>
  public class TextBonusEventArgs
  {
    public string Text {get;}
    public string Value {get;}
    public uint FontSize {get;}
    public Color FontColor {get;}
    public Vector2f Position {  get; }

    public TextBonusEventArgs(string text, string value, uint fontSize, Color fontColor, Vector2f position) 
    {
      Text = text;
      Value = value;
      FontSize = fontSize;
      FontColor = fontColor;
      Position = position;
    }
  }
}
