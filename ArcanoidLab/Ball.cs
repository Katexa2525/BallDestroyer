using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArcanoidLab
{
  /// <summary> Класс мячика </summary>
  public class Ball : DisplayObject
  {
    private Vector2f position;
    private Platform platform;

    public Ball(VideoMode mode)
    {
      this.Sprite.Texture = TextureManager.BallTexture; // рисунок мячика
      this.SpriteWidth = this.Sprite.TextureRect.Width; // ширина мячика
      this.SpriteHeight = this.Sprite.TextureRect.Height; // высота мячика
      // создаю экземпляры объектов
      this.platform = new Platform(mode);
      // начальная позиция мячика
      StartPosition(mode);
    }

    public override void StartPosition(VideoMode mode)
    {
      position.X = (mode.Width / 2) - (this.SpriteWidth / 2); // вычисляю позицию по оси Х, чтобы посередине мячик был
      position.Y = mode.Height - platform.SpriteHeight - this.SpriteHeight; // вычисляю позицию по оси Y, чтобы мячик над платформой был
      this.Sprite.Position = position;
    }

    public override void Update(VideoMode mode) {  }

    public override void Draw(RenderTarget window)
    {
      window.Draw(this.Sprite);
    }

    public override void Draw(RenderTarget window, VideoMode mode) { }
  }
}
