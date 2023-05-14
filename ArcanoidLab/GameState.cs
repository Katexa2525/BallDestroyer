using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidLab
{
  /// <summary> Класс для сохранения состояния игры </summary>
  public class GameState
  {
    public Ball DisplayObject { get; set; }

    // свойства для сохранения настроек из класса GameSetting
    public int Score { get; set; }  // свойство для подсчета очков
    public bool IsStart { get; set; }  // свойство, что игра запущена
    public bool IsVisibleMenu { get; set; } // свойство, что меню видно

    public int LIFE_TOTAL { get; }  // общее кол-во жизней в игре
    public int LifeCount { get; set; }  // начальное кол-во жизней в игре
    public int SCORE_STEP { get; } // шаг для подсчета очков

    public float PLATFORM_SPEED { get; }  // скорость движения платформы

    public int BALL_DELTA_X { get; } // смещение шарика по оси х
    public int BALL_DELTA_Y { get; }  // смещение шарика по оси у
    //

    public GameState(Ball displayObject)
    {
      DisplayObject = displayObject;
      Score = GameSetting.Score;
      IsStart = GameSetting.IsStart;
      IsVisibleMenu = !GameSetting.IsVisibleMenu; // false - чтобы при загрузке не было меню, а сразу игра
      LIFE_TOTAL = GameSetting.LIFE_TOTAL;
      LifeCount = GameSetting.LifeCount;
      SCORE_STEP = GameSetting.SCORE_STEP;
      PLATFORM_SPEED = GameSetting.PLATFORM_SPEED;
      BALL_DELTA_X = GameSetting.BALL_DELTA_X;
      BALL_DELTA_Y = GameSetting.BALL_DELTA_Y;
    }
  }
}
