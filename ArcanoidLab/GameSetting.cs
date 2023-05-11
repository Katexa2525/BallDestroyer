namespace ArcanoidLab
{
  public static class GameSetting
  {
    public static int Score { get; set; } = 0; // свойство для подсчета очков
    public static bool IsStart { get; set; } = false; // свойство, что игра запущена

    public static int LIFE_TOTAL { get; } = 3; // общее кол-во жизней в игре
    public static int LifeCount { get; set; } = 3; // начальное кол-во жизней в игре
    public static int SCORE_STEP { get; } = 10; // шаг для подсчета очков

    public static float PLATFORM_SPEED { get; } = 6f; // скорость движения платформы

    public static int BALL_DELTA_X { get; } = 2; // смещение шарика по оси х
    public static int BALL_DELTA_Y { get; } = 1; // смещение шарика по оси у
  }
}
