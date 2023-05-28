using Newtonsoft.Json;
using SFML.Window;
using System;
using System.IO;
// подключаем атрибут DllImport
using System.Runtime.InteropServices;

namespace ArcanoidLab
{
  /// <summary> Класс для сохранения состояния игры </summary>
  public class SaveLoadState
  {
    private string jsonFilePath = Directory.GetCurrentDirectory() + @"\ball.json";
    private string txtFilePath = Directory.GetCurrentDirectory() + @"\ball.txt";

    // Импортирую библиотку user32.dll (содержит WinAPI функцию MessageBox)
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options); // объявляем метод на C#
    
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
        // Вызываю MessageBox (вызовется функция Windows WinAPI)
        MessageBox(IntPtr.Zero, "Сохранено!", "Информация", 0);
      }
      catch (Exception ex)
      {
        MessageBox(IntPtr.Zero, "Ошибка сохранения: " + ex.Message, "Ошибка", 0);
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
        foreach (var item in gameState.Blocks)
        {
          item.Sprite.Position = item.positionObject;
          block.Blocks.Add(item);
        }
        GameSetting.Score = gameState.Score;
        GameSetting.IsStart = gameState.IsStart;
        GameSetting.IsVisibleMenu = gameState.IsVisibleMenu;
        GameSetting.LifeCount = gameState.LifeCount;
        // Вызываю MessageBox (вызовется функция Windows WinAPI)
        MessageBox(IntPtr.Zero, "Состояние игры загружено!", "Информация", 0);
      }
      catch (Exception ex)
      {
        MessageBox(IntPtr.Zero, "Ошибка восстановления: " + ex.Message, "Ошибка", 0);
      }
      return gameState;
    }

  }
}
