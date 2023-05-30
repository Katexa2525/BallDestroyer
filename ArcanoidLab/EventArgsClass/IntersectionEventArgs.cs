using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ArcanoidLab.EventArgsClass
{
  // <summary> Класс EventArgs для хранения информации о объектах для пересечения (столкновения) </summary>
  public class IntersectionEventArgs : EventArgs
  {
    public DisplayObject Ball { get; }
    public List<DisplayObject> Blocks { get; }
    public DisplayObject Platform { get; }
    public DisplayObject HeartScull { get; }
    public VideoMode Mode { get; }
    public RenderTarget Window { get; }

    public IntersectionEventArgs(DisplayObject ball, List<DisplayObject> blocks, DisplayObject platform, DisplayObject heartScull,
                                 VideoMode mode, RenderTarget window)
    {
      Ball = ball;
      Blocks = blocks;
      Platform = platform;
      HeartScull = heartScull;
      Mode = mode;
      Window = window;
    }
  }
}
