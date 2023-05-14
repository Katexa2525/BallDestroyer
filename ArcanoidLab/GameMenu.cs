﻿using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace ArcanoidLab
{
  public class GameMenu
  {
    public List<ButtonMenu> ButtonMenus = new List<ButtonMenu>();

    public GameMenu(VideoMode mode)
    {
      ButtonMenus.Add(new ButtonMenu("Играть    ", "play", 30, "FreeMonospacedBold", 100, Color.Red, Color.Green, mode));
      ButtonMenus.Add(new ButtonMenu("Сохранить ", "save", 30, "FreeMonospacedBold", 200, Color.Red, Color.Green, mode));
      ButtonMenus.Add(new ButtonMenu("Загрузить ", "load", 30, "FreeMonospacedBold", 300, Color.Red, Color.Green, mode));
      ButtonMenus.Add(new ButtonMenu("Выход     ", "exit", 30, "FreeMonospacedBold", 400, Color.Red, Color.Green, mode));
    }

    /// <summary> Отображаю кнопку на экране </summary>
    public void Draw(RenderTarget window)
    {
      // Рисую меню
      for (int i = 0; i < ButtonMenus.Count; i++)
      {
        ButtonMenus[i].Draw(window);
      }
    }
  }
}