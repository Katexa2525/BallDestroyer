namespace ArcanoidLab
{
  public class GameSetting
  {
    public static int Score { get; set; } = 0; // свойство для подсчета очков
    public static bool IsStart { get; set; } = false; // свойство, что игра запущена

    public static int LIFE_TOTAL { get; } = 3; // общее кол-во жизней в игре
    public static int LifeCount { get; set; } = 3; // начальное кол-во жизней в игре
    public static int SCORE_STEP = 10; // шаг для подсчета очков

    public static float PLATFORM_SPEED = 6f; // скорость движения платформы
  }
}
