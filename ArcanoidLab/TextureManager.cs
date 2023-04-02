using SFML.Graphics;
using System.IO;

namespace ArcanoidLab
{
  /// <summary> Класс текстур игры </summary>
  public class TextureManager
  {
    static string ASSETS_PATH = Directory.GetCurrentDirectory() + @"\Assets\Textures\";
    static Texture playerTexture;
    static Texture ballTexture;
    static Texture backgroundTexture;
    static Texture blockTexture;
    static Texture heartTexture;
    static Texture scullTexture;
    static Sprite moonTexture;
    static Sprite cloudTexture;

    public static Texture PlayerTexture { get { return playerTexture; } }
    public static Texture BallTexture { get { return ballTexture; } }
    public static Texture BackgroundTexture { get { return backgroundTexture; } }
    public static Texture BlockTexture { get { return blockTexture; } }
    public static Texture HeartTexture { get { return heartTexture; } }
    public static Texture ScullTexture { get { return scullTexture; } }

    public static Sprite MoonTexture { get { return moonTexture; } }

    public static Sprite CloudTexture { get { return cloudTexture; } }

    public static void LoadTexture()
    {
      playerTexture = new Texture(ASSETS_PATH + "platform.png");
      ballTexture = new Texture(ASSETS_PATH + "ball.png");
      backgroundTexture = new Texture(ASSETS_PATH + "Asf3.png");
      blockTexture = new Texture(ASSETS_PATH + "block01.png");
      heartTexture = new Texture(ASSETS_PATH + "heart-with-pulse-16.png");
      scullTexture = new Texture(ASSETS_PATH + "scull-16.png");

      moonTexture = new Sprite(new Texture(ASSETS_PATH + "moon_full.png"));
      moonTexture.Position = new SFML.System.Vector2f(50, 250);
      cloudTexture = new Sprite(new Texture(ASSETS_PATH + "cloud.png"));
      cloudTexture.Position = new SFML.System.Vector2f(300, 50);
    }
  }
}
