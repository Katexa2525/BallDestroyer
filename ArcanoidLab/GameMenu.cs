using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.IO;

namespace ArcanoidLab
{
  public class GameMenu
  {
    public List<ButtonMenu> ButtonMenus = new List<ButtonMenu>();
    public List<ButtonMenu> ButtonLevel = new List<ButtonMenu>();
    private ButtonMenu buttonPlayer;
    private TextBox textBox;
    private Text nameText;
    Font font = new Font(Directory.GetCurrentDirectory() + @"\Assets\Fonts\FreeMono\FreeMonospacedBold.ttf");

    public GameMenu(VideoMode mode)
    {
      // основное меню
      ButtonMenus.Add(new ButtonMenu(100, 30, "Играть", "play", 20, "FreeMonospacedBold", 120, 5, Color.Red, Color.Green, mode));
      ButtonMenus.Add(new ButtonMenu(140, 30, "Сохранить", "save", 20, "FreeMonospacedBold", 230, 5, Color.Red, Color.Green, mode));
      ButtonMenus.Add(new ButtonMenu(140, 30, "Загрузить", "load", 20, "FreeMonospacedBold", 380, 5, Color.Red, Color.Green, mode));
      ButtonMenus.Add(new ButtonMenu(100, 30, "Выход", "exit", 20, "FreeMonospacedBold", 530, 5, Color.Red, Color.Green, mode));

      // кнопки для переключения уровня сложности
      ButtonLevel.Add(new ButtonMenu(200, 30, "Легкий уровень ", "easy", 20, "FreeMonospacedBold", 80, 75, Color.Red, Color.Green, mode));
      ButtonLevel.Add(new ButtonMenu(200, 30, "Средний уровень", "medium", 20, "FreeMonospacedBold", 300, 75, Color.Red, Color.Blue, mode));
      ButtonLevel.Add(new ButtonMenu(200, 30, "Тяжелый уровень", "hard", 20, "FreeMonospacedBold", 520, 75, Color.Red, Color.Blue, mode));

      // рисую поле для ввода имени игрока
      //buttonPlayer = new ButtonMenu(200, 30, "Игрок: "+ GameSetting.PLAYER_NAME, "namePlayer", 20, "FreeMonospacedBold", 80, 150, Color.Red, Color.Transparent, mode);
      textBox = new TextBox();
      nameText = new Text(GameSetting.PLAYER_NAME, font, 16);
      nameText.Position = new Vector2f(300, 152);
      nameText.Color = Color.Black;
    }

    public void Update(RenderTarget window)
    {
      this.KeyHandler(window);
    }

    /// <summary> Отображаю кнопки на экране </summary>
    public void Draw(RenderTarget window)
    {
      Update(window);
      // Рисую меню
      for (int i = 0; i < ButtonMenus.Count; i++)
      {
        ButtonMenus[i].Draw(window);
      }
      for (int i = 0; i < ButtonLevel.Count; i++)
      {
        ButtonLevel[i].Draw(window);
      }
      //buttonPlayer.Draw(window);
      textBox.Draw(window);
      window.Draw(nameText);
    }

    public void KeyHandler(RenderTarget window)
    {
      Vector2i mousePosition = Mouse.GetPosition((Window)window); // координаты мыши

      /////////////// Работа с кнопками уровней //////////////////////////////////////////
      // проверка, наведена ли мышь на кнопки изменения уровня
      for (int i = 0; i < ButtonLevel.Count; i++)
      {
        // проверяю, находится ли курсор мыши над прямоугольником меню
        if (ButtonLevel[i].MenuItemRect.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y) &&
            Mouse.IsButtonPressed(Mouse.Button.Left) && ButtonLevel[i].GetColorButton() != Color.Green)
        {
          // проверяю, было ли нажатие, тогда меняю цвета кнопок - одна нажата (зеленый), другие нет (синий)
          SetColorPressLevel(ButtonLevel[i], ButtonLevel);
        }
      }
    }

    // устанавливаю цвета кнопок уровня
    public void SetColorPressLevel(ButtonMenu buttonMenu, List<ButtonMenu> buttonLevel)
    {
      // заношу цвет в словарь 
      if (buttonMenu.AliasButton == "easy" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Green);
        buttonLevel[1].SetColorButton(Color.Blue);
        buttonLevel[2].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 2; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 1; // смещение дельта y
        GameSetting.LEVEL = "Лёгкий";
  }
      else if (buttonMenu.AliasButton == "medium" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Blue);
        buttonLevel[1].SetColorButton(Color.Green);
        buttonLevel[2].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 6; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 5; // смещение дельта y
        GameSetting.LEVEL = "Средний";
      }
      else if (buttonMenu.AliasButton == "hard" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Blue);
        buttonLevel[1].SetColorButton(Color.Blue);
        buttonLevel[2].SetColorButton(Color.Green);
        GameSetting.BALL_DELTA_X = 9; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 8; // смещение дельта y
        GameSetting.LEVEL = "Тяжелый";
      }
    }

  }
}
