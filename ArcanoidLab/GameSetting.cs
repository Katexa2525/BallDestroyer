using SFML.Graphics;
using SFML.System;

namespace ArcanoidLab
{
  public static class GameSetting
  {
    public static int Score { get; set; } = 0; // свойство для подсчета очков
    public static bool IsStart { get; set; } = false; // свойство, что игра запущена
    public static bool IsVisibleMenu { get; set; } = false; // свойство, что меню видно

    public static int LIFE_TOTAL { get; } = 3; // общее кол-во жизней в игре
    public static int LifeCount { get; set; } = 3; // начальное кол-во жизней в игре
    public static int SCORE_STEP { get; } = 10; // шаг для подсчета очков

    public static float PLATFORM_SPEED { get; set; } = 6f; // скорость движения платформы

    public static int BALL_DELTA_X { get; set; } = 2; // смещение шарика по оси х
    public static int BALL_DELTA_Y { get; set; } = 1; // смещение шарика по оси у
    public static string LEVEL { get; set; } = "Лёгкий"; // начальный уровень игры
    public static string PLAYER_NAME { get; set; } = "Катя"; // имя игрока

    // Метод для изменения разрешения и обновления области просмотра
    public static void ChangeResolution(RenderWindow window, Vector2u resolution)
    {
      window.Size = resolution;

      // Получение текущей области просмотра
      View currentView = window.GetView();

      // Вычисление новых границ области просмотра с учетом нового разрешения
      float aspectRatio = (float)resolution.X / resolution.Y;
      float viewWidth = currentView.Size.Y * aspectRatio;
      float viewHeight = currentView.Size.Y;
      float viewLeft = (currentView.Size.X - viewWidth) / 2;
      float viewTop = 0;

      // Создание новой области просмотра
      View newView = new View(new FloatRect(viewLeft, viewTop, viewWidth, viewHeight));

      // Установка новой области просмотра в окно
      window.SetView(newView);
    }
  }
}