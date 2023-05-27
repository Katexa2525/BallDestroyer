using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;

namespace ArcanoidLab
{
  public class GameMenu
  {
    public List<ButtonMenu> ButtonMenus = new List<ButtonMenu>();
    public List<ButtonMenu> ButtonLevel = new List<ButtonMenu>();

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
    }

    public void KeyHandler(RenderTarget window)
    {
      Vector2i mousePosition = Mouse.GetPosition((Window)window); // координаты мыши
      /////////////// Работа с кнопками уровней //////////////////////////////////////////
      // проверка, наведена ли мышь на кнопки изменения уровня
      for (int i = 0; i < ButtonLevel.Count; i++)
      {
        // проверяю, находится ли курсор мыши над прямоугольником меню
        if (ButtonLevel[i].MenuItemRect.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
        {
          // проверяю, было ли нажатие, тогда меняю цвета кнопок - одна нажата (зеленый), другие нет (синий)
          if (Mouse.IsButtonPressed(Mouse.Button.Left) && ButtonLevel[i].GetColorButton() != Color.Green)
          {
            SetColorPressLevel(ButtonLevel[i], ButtonLevel);
          }
        }
      }
    }

    // устанавливаю цвета кнопок уровня
    private void SetColorPressLevel(ButtonMenu buttonMenu, List<ButtonMenu> buttonLevel)
    {
      // заношу цвет в словарь 
      if (buttonMenu.AliasButton == "easy" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Green);
        buttonLevel[1].SetColorButton(Color.Blue);
        buttonLevel[2].SetColorButton(Color.Blue);
      }
      else if (buttonMenu.AliasButton == "medium" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Blue);
        buttonLevel[1].SetColorButton(Color.Green);
        buttonLevel[2].SetColorButton(Color.Blue);
      }
      else if (buttonMenu.AliasButton == "hard" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonLevel[0].SetColorButton(Color.Blue);
        buttonLevel[1].SetColorButton(Color.Blue);
        buttonLevel[2].SetColorButton(Color.Green);
      }
    }

  }
}
