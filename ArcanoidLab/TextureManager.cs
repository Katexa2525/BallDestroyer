using SFML.Graphics;
using System.IO;

namespace ArcanoidLab
{
  /// <summary> Класс текстур игры </summary>
  public class TextureManager
  {
    static readonly string ASSETS_PATH = Directory.GetCurrentDirectory() + @"\Assets\Textures\";
    static Texture playerTexture;
    static Texture ballTexture;
    static Texture backgroundTexture;
    static Texture blockTexture;
    static Texture blockBonus1Texture;
    static Texture blockBonus2Texture;
    static Texture heartTexture;
    static Texture scullTexture;
    static Sprite moonTexture;
    static Sprite cloudTexture;

    public static Texture PlayerTexture { get { return playerTexture; } }
    public static Texture BallTexture { get { return ballTexture; } }
    public static Texture BackgroundTexture { get { return backgroundTexture; } }
    public static Texture BlockTexture { get { return blockTexture; } }
    public static Texture BlockBonus1Texture { get { return blockBonus1Texture; } }
    public static Texture BlockBonus2Texture { get { return blockBonus2Texture; } }
    public static Texture HeartTexture { get { return heartTexture; } }
    public static Texture ScullTexture { get { return scullTexture; } }

    public static Sprite MoonTexture { get { return moonTexture; } }

    public static Sprite CloudTexture { get { return cloudTexture; } }

    public static void LoadTexture()
    {
      playerTexture = new Texture(ASSETS_PATH + "platform.png");
      ballTexture = new Texture(ASSETS_PATH + "ball1.png");
      backgroundTexture = new Texture(ASSETS_PATH + "Asf3.png");
      blockTexture = new Texture(ASSETS_PATH + "block01.png");
      blockBonus1Texture = new Texture(ASSETS_PATH + "block04.png");
      blockBonus2Texture = new Texture(ASSETS_PATH + "block03.png");
      heartTexture = new Texture(ASSETS_PATH + "heart-with-pulse-16.png");
      scullTexture = new Texture(ASSETS_PATH + "scull-16.png");

      moonTexture = new Sprite(new Texture(ASSETS_PATH + "moon_full.png"));
      moonTexture.Position = new SFML.System.Vector2f(50, 250);
      cloudTexture = new Sprite(new Texture(ASSETS_PATH + "cloud.png"));
      cloudTexture.Position = new SFML.System.Vector2f(300, 50);
    }
  }
}
