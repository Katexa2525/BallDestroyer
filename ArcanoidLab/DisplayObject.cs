using SFML.Graphics;
using SFML.Window;

namespace ArcanoidLab
{
  public abstract class DisplayObject
  {
    public abstract void StartPosition(VideoMode mode);
    public abstract void Update(VideoMode mode);
    public abstract void Draw(RenderTarget window);
    public abstract void Draw(RenderTarget window, VideoMode mode);

  }
}
