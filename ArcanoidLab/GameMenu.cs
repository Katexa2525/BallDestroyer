using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
// подключаем атрибут DllImport
using System.Runtime.InteropServices;

namespace ArcanoidLab
{
  public class GameMenu
  {
    public List<ButtonMenu> ButtonMenus { get; set; } = new List<ButtonMenu>();
    public List<ButtonMenu> ButtonLevel { get; set; } = new List<ButtonMenu>();
    public TextBox TextBox { get; set; }

    // Импортирую библиотку user32.dll (содержит WinAPI функцию MessageBox)
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, int options); // объявляем метод на C#

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
      TextBox = new TextBox("Игрок: ", "FreeMonospacedBold", 16, Color.White, 200, 152, //данные надписи
                            "FreeMonospacedBold", 16, Color.Black, 300, 152, // данные вводимого текста имени игрона
                            200, 30, 300, 150, Color.White); // данные прямоугольника
    }

    public void Update(RenderTarget window, DisplayObject displayObject)
    {
      this.KeyHandler(window, displayObject);
    }

    /// <summary> Отображаю кнопки на экране </summary>
    public void Draw(RenderTarget window, DisplayObject displayObject)
    {
      Update(window, displayObject);
      // Рисую меню
      for (int i = 0; i < ButtonMenus.Count; i++)
      {
        ButtonMenus[i].Draw(window);
      }
      for (int i = 0; i < ButtonLevel.Count; i++)
      {
        ButtonLevel[i].Draw(window);
      }
      TextBox.Draw(window);
    }

    private void KeyHandler(RenderTarget window, DisplayObject displayObject)
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
          SetColorPressLevel(ButtonLevel[i], ButtonLevel, displayObject);
        }
      }
    }

    // устанавливаю цвета кнопок уровня
    public void SetColorPressLevel(ButtonMenu buttonMenu, List<ButtonMenu> buttonLevel, DisplayObject displayObject)
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
        MessageBox(IntPtr.Zero, "Уровень игры изменен на Лёгкий.", "Информация", 0);
      }
      else if (buttonMenu.AliasButton == "medium" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Blue);
        buttonLevel[1].SetColorButton(Color.Green);
        buttonLevel[2].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 6; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 5; // смещение дельта y
        GameSetting.LEVEL = "Средний";
        MessageBox(IntPtr.Zero, "Уровень игры изменен на Средний.", "Информация", 0);
      }
      else if (buttonMenu.AliasButton == "hard" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Blue);
        buttonLevel[1].SetColorButton(Color.Blue);
        buttonLevel[2].SetColorButton(Color.Green);
        GameSetting.BALL_DELTA_X = 9; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 8; // смещение дельта y
        GameSetting.LEVEL = "Тяжелый";
        MessageBox(IntPtr.Zero, "Уровень игры изменен на Тяжелый.", "Информация", 0);
      }
      displayObject.SetSpeedDO();
    }

  }
}
