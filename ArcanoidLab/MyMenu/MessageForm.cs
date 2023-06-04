using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace ArcanoidLab
{
  /// <summary> Класс для создания формы с кнопками, типа Да, Нет </summary>
  public class MessageForm
  {
    GameForm gameForm;
    List<ButtonMenu> ButtonMenus  = new List<ButtonMenu>();
    TextBoxLabel textBoxLabel;
    private Game Game;

    public MessageForm(VideoMode mode, string textLabel, string textButtonYes, string textButtonNo, Game game)
    {
      // создаю форму для кнопок
      gameForm = new GameForm(mode, 400, 200, Color.White, Color.Black);
      // текст на форме
      textBoxLabel = new TextBoxLabel(textLabel, "FreeMonospacedBold", 25, Color.Red, (mode.Width / 2) - 150, (mode.Height / 2) - 152); //данные надписи
      // кнопки на форме
      ButtonMenus.Add(new ButtonMenu(70, 30, textButtonYes, "yes", 16, "FreeMonospacedBold", (mode.Width / 2) - (50 / 2), (mode.Height / 2) - 40, Color.Red, Color.Yellow, mode));
      ButtonMenus.Add(new ButtonMenu(70, 30, textButtonNo, "no", 16, "FreeMonospacedBold", (mode.Width / 2) - (50 / 2) + 100, (mode.Height / 2) - 40, Color.Red, Color.Yellow, mode));
      // ссылка на игру
      Game = game;
    }

    public MessageForm(VideoMode mode, string textLabel, string textButtonOK, Game game)
    {
      // создаю форму для кнопок
      gameForm = new GameForm(mode, 400, 200, Color.White, Color.Black);
      // текст на форме
      textBoxLabel = new TextBoxLabel(textLabel, "FreeMonospacedBold", 25, Color.Red, (mode.Width / 2) - 150, (mode.Height / 2) - 152); //данные надписи
      // кнопки на форме
      ButtonMenus.Add(new ButtonMenu(70, 30, textButtonOK, "no", 16, "FreeMonospacedBold", (mode.Width / 2) - (50 / 2) + 100, (mode.Height / 2) - 30 - 10, Color.Red, Color.Yellow, mode));
      // ссылка на игру
      Game = game;
    }

    public void Update(RenderTarget window)
    {
      this.KeyHandler(window);
    }

    /// <summary> Отображаю кнопки на экране </summary>
    public void Draw(RenderTarget window)
    {
      Update(window);

      // рисую форму
      gameForm.Draw(window);
      // рисую надпись
      textBoxLabel.Draw(window);

      // Рисую кнопки
      for (int i = 0; i < ButtonMenus.Count; i++)
      {
        ButtonMenus[i].Draw(window);
      }
    }

    private void KeyHandler(RenderTarget window)
    {
      Vector2i mousePosition = Mouse.GetPosition((Window)window); // координаты мыши

      /////////////// Работа с кнопками на форме сообщения //////////////////////////////////////////
      // проверка, наведена ли мышь на кнопки изменения уровня
      for (int i = 0; i < ButtonMenus.Count; i++)
      {
        // проверяю, находится ли курсор мыши над прямоугольником главной кнопки меню
        if (ButtonMenus[i].MenuItemRect.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
        {
          // курсор мыши находится над прямоугольником пункта меню 
          ButtonMenus[i].SetColorButton(Color.Magenta); // меняю цвет пункта
          ButtonMenus[i].SetColorTextButton(Color.Black); // меняю цвет текста
          // проверяю, было ли нажатие, тогда вызов экрана с пунктами меню
          if ((Mouse.IsButtonPressed(Mouse.Button.Left) || Keyboard.IsKeyPressed(Keyboard.Key.Y)) && ButtonMenus[i].AliasButton == "yes")
          {
            Game.window.Close(); // выход из игры GameSetting
          }
          else if ((Mouse.IsButtonPressed(Mouse.Button.Left) || Keyboard.IsKeyPressed(Keyboard.Key.N)) && ButtonMenus[i].AliasButton == "no")
          {
            GameSetting.IsVisibleMenu = false;
            GameSetting.IsVisibleMessageForm = false;
          }
        }
        else
        {
          ButtonMenus[i].SetColorButton(Color.Yellow);
          ButtonMenus[i].SetColorTextButton(Color.Red);
        }
      }
    }

  }
}
