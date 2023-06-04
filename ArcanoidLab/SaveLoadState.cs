using Newtonsoft.Json;
using SFML.Window;
using System;
using System.IO;
using System.Windows.Forms;

namespace ArcanoidLab
{
  /// <summary> Класс для сохранения состояния игры </summary>
  public class SaveLoadState
  {
    private readonly string jsonFilePath = Directory.GetCurrentDirectory() + @"\ball.json";
    private readonly string txtFilePath = Directory.GetCurrentDirectory() + @"\ball.txt";
   
    /// <summary> Метод сохранения в json и txt файлы состояния игры </summary>
    /// <param name="gameState">Экземпляр класса для состояния</param>
    public void SaveState(GameState gameState)
    {
      try
      {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
          Formatting = Formatting.Indented
        };
        var json = JsonConvert.SerializeObject(gameState, settings);
        //Запись JSON-строки в файл
        File.WriteAllText(jsonFilePath, json);
        File.WriteAllText(txtFilePath, json);
        //MessageBox.Show("Сохранено!");
      }
      catch (Exception ex)
      {
        MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка");
      }
    }

    public GameState LoadState(Ball ball, Platform platform, Block block, VideoMode mode)
    {
      GameState gameState = null;
      try
      {
        // Чтение JSON-строки из файла
        string json = File.ReadAllText(jsonFilePath);
        JsonConverter[] converters = { new DOConverter() };
        // Десериализация JSON-строки в объект класса GameState
        gameState = JsonConvert.DeserializeObject<GameState>(json, new JsonSerializerSettings() { Converters = converters });
        // восстановление данных
        ball = gameState.Ball;
        platform = gameState.Platform;
        platform.Sprite.Position = gameState.Platform.positionObject;
        platform.StartPosition(mode);
        block.Blocks.Clear();
        block.BlocksBonus.Clear();
        foreach (Block item in gameState.Blocks)
        {
          if (item.BonusBlockId == 1) // если бонусный блок, то подсовываю ему картинку нужную
          {
            item.Sprite.Texture = TextureManager.BlockBonus1Texture; // картинка бонуса для блока
            block.BlocksBonus.Add(item, 1);
          }
          else if (item.BonusBlockId == 2) // если бонусный блок, то подсовываю ему картинку нужную
          {
            item.Sprite.Texture = TextureManager.BlockBonus2Texture; // картинка бонуса для платформы
            block.BlocksBonus.Add(item, 2);
          }
          else
            block.BlocksBonus.Add(item, 0);
          item.Sprite.Position = item.positionObject;
          block.Blocks.Add(item);
        }
        GameSetting.Score = gameState.Score;
        GameSetting.IsStart = gameState.IsStart;
        GameSetting.LifeCount = gameState.LifeCount;
        GameSetting.LEVEL = gameState.Level;
        GameSetting.PLAYER_NAME = gameState.PlayerName;
        GameSetting.SCORE_STEP = gameState.ScoreStep;
        GameSetting.SCORE_BONUS_STEP = gameState.ScoreBonusStep;
        GameSetting.BONUS_PLATFORM = gameState.BonusPlatform;
        GameSetting.LIFE_TOTAL = gameState.LifeTotal;
        GameSetting.BALL_DELTA_X = gameState.BallDeltaX;
        GameSetting.BALL_DELTA_Y = gameState.BallDeltaY;

        //MessageBox.Show("Состояние игры загружено!", "Информация");
      }
      catch (Exception ex)
      {
        //MessageBox.Show("Ошибка восстановления: " + ex.Message, "Ошибка");
      }
      return gameState;
    }

  }
}
