using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace ArcanoidLab.EventArgsClass
{
  // <summary> Класс EventArgs для хранения информации об объектах для пересечения (столкновения),
  //или об объектах для начала нового раунда игры, для определения отскока после столкновения</summary>
  public class IntersectionEventArgs : EventArgs
  {
    public DisplayObject Ball { get; }
    public List<DisplayObject> Blocks { get; }
    public DisplayObject Platform { get; }
    public DisplayObject HeartScull { get; }
    public VideoMode Mode { get; }
    public RenderTarget Window { get; }
    public DisplayObject StaticObject { get; }
    public DisplayObject DynamicObject { get; }
    public Dictionary<DisplayObject, int> DOBonus { get; }

    public IntersectionEventArgs(DisplayObject ball, List<DisplayObject> blocks, DisplayObject platform, DisplayObject heartScull,
                                 Dictionary<DisplayObject, int> doBonus, VideoMode mode, RenderTarget window)
    {
      Ball = ball;
      Blocks = blocks;
      Platform = platform;
      HeartScull = heartScull;
      Mode = mode;
      Window = window;
      DOBonus = doBonus;
    }

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

    public IntersectionEventArgs (DisplayObject dynamicObject, VideoMode mode)
    {
      DynamicObject = dynamicObject;
      Mode = mode;
    }

    public IntersectionEventArgs(DisplayObject staticObject, DisplayObject dynamicObject, List<DisplayObject> staticDO)
    {
      DynamicObject = dynamicObject;
      StaticObject = staticObject;
      Blocks = staticDO;
    }
  }
}
