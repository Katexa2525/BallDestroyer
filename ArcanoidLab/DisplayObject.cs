using SFML.Graphics;
using SFML.Window;

namespace ArcanoidLab
{
  public abstract class DisplayObject
  {
    // свойства, общие для всех наследуемых объектов
    public int SpriteWidth { get; set; } = 0; // свойство ширины объекта
    public int SpriteHeight { get; set; } = 0; // свойство высоты объекта
    public Sprite Sprite { get; set; } = new Sprite(); // сам объект (блок, шарик, платформа)
    public int Score { get; set; } = 0; // свойство для подсчета очков

    public abstract void StartPosition(VideoMode mode);
    public abstract void Update(VideoMode mode);
    public abstract void Draw(RenderTarget window);
    public abstract void Draw(RenderTarget window, VideoMode mode);

  }
}
