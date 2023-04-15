using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace ArcanoidLab
{
  /// <summary> Класс для управления движением шарика и уничтожения блоков </summary>
  public class BallManager
  {
    public float dx { get; set; } = 6;
    public float dy { get; set; } = 5;
    public bool IsStart { get; set; } = false;

    public BallManager(Ball ball, HeartScull heartScull)
    {    }

    public void Update(Block block, Ball ball, Platform platform, HeartScull heartScull, VideoMode mode, RenderTarget window)
    {
      float x = ball.Sprite.Position.X, y = ball.Sprite.Position.Y;
      Random random = new Random();

      if (IsStart)
      {
        int n = block.Blocks.Count;
        x += dx;
        for (int i = 0; i < n; i++)
        {
          if (new FloatRect(x + 3, y + 3, 6, 6).Intersects(block.Blocks[i].GetGlobalBounds()))
          {
            block.Blocks[i].Position = new Vector2f(-100, 0);
            block.Blocks.RemoveAt(i);
            n = block.Blocks.Count;
            dx = -dx;
            //platform.Score += 10; // вывод результата
            GameSetting.Score += 10; // вывод результата
          }
        }

        y += dy;
        for (int i = 0; i < n; i++)
        {
          if (new FloatRect(x + 3, y + 3, 6, 6).Intersects(block.Blocks[i].GetGlobalBounds()))
          {
            block.Blocks[i].Position = new Vector2f(-100, 0);
            block.Blocks.RemoveAt(i);
            n = block.Blocks.Count;
            dy = -dy;
            //platform.Score += 10; // вывод результата
            GameSetting.Score += 10; // вывод результата
          }
        }

        //if (x < 0 || x > 640) dx = -dx;
        if (x < 0 || x > mode.Width) dx = -dx;
        //if (y < 0 || y > 480) dy = -dy;
        if (y < 0) dy = -dy;

        //if (y > 480 || block.Blocks.Count == 0)
        if (y > mode.Height || block.Blocks.Count == 0)
        {
          IsStart = false;
          ball.StartPosition(mode);
          platform.StartPosition(mode);
          dx = 6; dy = 5;
          x = ball.Sprite.Position.X; y = ball.Sprite.Position.Y;
          //heartScull.LifeCount--; // минус жизнь
          GameSetting.LifeCount += 10; // вывод результата
          heartScull.Draw(window, mode); // перерисовываю после минусования жизни
        }

        if (new FloatRect(x, y, 12, 12).Intersects(platform.Sprite.GetGlobalBounds()))
          dy = -(random.Next() % 5 + 2);

        ball.Sprite.Position = new Vector2f(x, y);
      }
    }

    public void Draw(RenderTarget window, Ball ball)
    {
      //window.Draw(ball.Sprite);
    }
  }
}
