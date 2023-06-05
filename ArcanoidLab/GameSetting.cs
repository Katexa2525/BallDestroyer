namespace ArcanoidLab
{
  public static class GameSetting
  {
    public static int Score { get; set; } = 0; // свойство для подсчета очков
    public static bool IsStart { get; set; } = false; // свойство, что игра запущена
    public static bool IsVisibleMenu { get; set; } = false; // свойство, что меню видно
    public static bool IsVisibleMessageForm { get; set; } = false; // свойство, видно ли форма с вопросом

    public static int LIFE_TOTAL { get; set; } = 3; // общее кол-во жизней в игре
    public static int LifeCount { get; set; } = 3; // начальное кол-во жизней в игре
    public static int SCORE_STEP { get; set; } = 10; // шаг для подсчета очков
    public static int SCORE_BONUS_STEP { get; set; } = 100; // бонус для очков
    public static float BONUS_PLATFORM { get; set; } = 1.05f; // бонус для платформы

    public static float PLATFORM_SPEED { get; set; } = 15f; // скорость движения платформы

    public static int BALL_DELTA_X { get; set; } = 2; // смещение шарика по оси х
    public static int BALL_DELTA_Y { get; set; } = 1; // смещение шарика по оси у
    public static string LEVEL { get; set; } = "Уровень 1"; // начальный уровень игры
    public static string PLAYER_NAME { get; set; } = "Катя"; // имя игрока

  }
}