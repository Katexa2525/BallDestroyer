using SFML.Graphics;
using SFML.System;

namespace ArcanoidLab
{
  /// <summary>Класс пуль для играка </summary>
  public class Bullet
  {
    private RectangleShape rectangle;    
    private Vector2f position;
    private Vector2f size = new Vector2f(5, 10);

    public const float BULLET_SPEED = 2f;

    public Vector2f Position { get { return position; } }
    public RectangleShape RectangleBullet { get { return this.rectangle; } }

    public Bullet(Vector2f position)
    {
      this.rectangle = new RectangleShape(size);
      this.rectangle.FillColor = Color.White;
      this.rectangle.Position = position;
      this.position = position;
    }

    public void Update()
    {
      this.position.Y -= BULLET_SPEED;
      this.rectangle.Position = this.position;
    }
  }
}
