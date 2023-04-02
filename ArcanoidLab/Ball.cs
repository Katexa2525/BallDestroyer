using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс мячика </summary>
  public class Ball : DisplayObject
  {
    private Sprite sprite;
    private Vector2f position;
    private readonly int spriteWidth = 0;
    private readonly int spriteHeight = 0;
    private Platform platform;

    public Sprite Sprite { get { return sprite; } }

    public Ball(VideoMode mode)
    {
      this.sprite = new Sprite();
      this.sprite.Texture = TextureManager.BallTexture; // рисунок мячика
      this.spriteWidth = this.sprite.TextureRect.Width; // ширина мячика
      this.spriteHeight = this.sprite.TextureRect.Height; // высота мячика
      // создаю экземпляры объектов
      this.platform = new Platform(mode);
      // начальная позиция мячика
      StartPosition(mode);
    }

    public override void StartPosition(VideoMode mode)
    {
      position.X = (mode.Width / 2) - (this.spriteWidth / 2); // вычисляю позицию по оси Х, чтобы посередине мячик был
      position.Y = mode.Height - platform.SpriteHeight - this.spriteHeight; // вычисляю позицию по оси Y, чтобы мячик над платформой был
      this.sprite.Position = position;
    }

    public override void Update(VideoMode mode) {  }

    public override void Draw(RenderTarget window)
    {
      window.Draw(this.sprite);
    }

    public override void Draw(RenderTarget window, VideoMode mode) { }
  }
}
