using SFML.System;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс для сохранения состояния игры </summary>
  public class GameState
  {
    public Ball Ball { get; set; }
    public Platform Platform { get; set; }
    public List<DisplayObject> Blocks { get; set; } = new List<DisplayObject>();

    // свойства для сохранения настроек из класса GameSetting
    public int Score { get; set; }  // свойство для подсчета очков
    public bool IsStart { get; set; }  // свойство, что игра запущена
    public int LifeCount { get; set; }  // начальное кол-во жизней в игре
    public string Level { get; set; } // уровень игры
    public string PlayerName { get; set; } // имя игрока
    public int ScoreStep { get; set; } // шаг для подсчета очков
    public int ScoreBonusStep { get; set; } // шаг для подсчета очков
    public int LifeTotal { get; set; } // общее кол-во жизней в игре
    public int BallDeltaX { get; set; } // смещение шарика по оси х
    public int BallDeltaY { get; set; } // смещение шарика по оси у

    public GameState(Ball ball, Platform platform, List<DisplayObject> blocks)
    {
      Ball = ball;
      Platform = platform;
      Platform.positionObject = new Vector2f(platform.x1, platform.y1); // положение на экране платформы
      foreach (DisplayObject item in blocks)
      {
        if (item is Block)
        {
          item.positionObject = new Vector2f(item.x1, item.y1); // положение на экране блока
          Blocks.Add(item);
        }
      }
      Score = GameSetting.Score;
      IsStart = GameSetting.IsStart;
      LifeCount = GameSetting.LifeCount;
      Level = GameSetting.LEVEL;
      PlayerName = GameSetting.PLAYER_NAME;
      ScoreStep = GameSetting.SCORE_STEP;
      ScoreBonusStep = GameSetting.SCORE_BONUS_STEP;
      LifeTotal = GameSetting.LIFE_TOTAL;
      BallDeltaX = GameSetting.BALL_DELTA_X;
      BallDeltaY = GameSetting.BALL_DELTA_Y;
    }
  }
}
