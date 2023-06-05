using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
// подключаем атрибут DllImport
using System.Runtime.InteropServices;

namespace ArcanoidLab
{
  /// <summary> Класс для формирования меню и пунктов настроек игры </summary>
  public class GameMenu
  {
    private Game Game;
    RectangleShape rectangleForm;
    RectangleShape rectangleFormShadow;
    GameForm gameFormMess;
    GameForm gameForm;
    MessageForm messageFormOK;

    public List<ButtonMenu> ButtonMenus { get; set; } = new List<ButtonMenu>();
    public List<ButtonMenu> ButtonLevel { get; set; } = new List<ButtonMenu>();
    public List<ButtonMenu> ButtonResol { get; set; } = new List<ButtonMenu>();
    public TextBoxLabel TextBox { get; set; }

    public GameMenu(VideoMode mode, Game game)
    {
      gameForm = new GameForm(mode, 700, 300, Color.Cyan, Color.Black);

      // основное меню
      ButtonMenus.Add(new ButtonMenu(110, 30, "Играть F1", "play", 18, "FreeMonospacedBold", 650, 270, Color.Red, Color.Yellow, mode));
      ButtonMenus.Add(new ButtonMenu(150, 30, "Сохранить F2", "save", 18, "FreeMonospacedBold", 770, 270, Color.Red, Color.Yellow, mode));
      ButtonMenus.Add(new ButtonMenu(150, 30, "Загрузить F3", "load", 18, "FreeMonospacedBold", 930, 270, Color.Red, Color.Yellow, mode));
      ButtonMenus.Add(new ButtonMenu(110, 30, "Выход F12", "exit", 18, "FreeMonospacedBold", 1090, 270, Color.Red, Color.Yellow, mode));
      ButtonMenus.Add(new ButtonMenu(130, 30, "Новая игра", "newGame", 18, "FreeMonospacedBold", 1070, 490, Color.Red, Color.Yellow, mode));

      // кнопки для переключения уровня сложности
      ButtonLevel.Add(new ButtonMenu(130, 30, "Уровень 1(F4)", "level1", 15, "FreeMonospacedBold", 650, 330, Color.Red, Color.Green, mode));
      ButtonLevel.Add(new ButtonMenu(130, 30, "Уровень 2(F5)", "level2", 15, "FreeMonospacedBold", 650, 370, Color.Red, Color.Blue, mode));
      ButtonLevel.Add(new ButtonMenu(130, 30, "Уровень 3(F6)", "level3", 15, "FreeMonospacedBold", 650, 410, Color.Red, Color.Blue, mode));
      ButtonLevel.Add(new ButtonMenu(130, 30, "Уровень 4(F7)", "level4", 15, "FreeMonospacedBold", 650, 450, Color.Red, Color.Blue, mode));
      ButtonLevel.Add(new ButtonMenu(130, 30, "Уровень 5(F8)", "level5", 15, "FreeMonospacedBold", 650, 490, Color.Red, Color.Blue, mode));

      // рисую поле для ввода имени игрока
      TextBox = new TextBoxLabel("Игрок: ", "FreeMonospacedBold", 16, Color.Red, 1000, 330, //данные надписи
                                 "FreeMonospacedBold", 16, Color.Black, 1010, 370, // данные вводимого текста имени игрока
                                 200, 30, 1000, 370, Color.White); // данные прямоугольника

      // кнопки для переключения разрешения экрана
      ButtonResol.Add(new ButtonMenu(140, 30, "800х600  (F7)", "800", 15, "FreeMonospacedBold", 790, 330, Color.Red, Color.Blue, mode));
      ButtonResol.Add(new ButtonMenu(140, 30, "1152х864 (F8)", "1152", 15, "FreeMonospacedBold", 790, 370, Color.Red, Color.Blue, mode));
      ButtonResol.Add(new ButtonMenu(140, 30, "1280x1024(F9)", "1280", 15, "FreeMonospacedBold", 790, 410, Color.Red, Color.Blue, mode));
      ButtonResol.Add(new ButtonMenu(140, 30, "1360x768(F10)", "1360", 15, "FreeMonospacedBold", 790, 450, Color.Red, Color.Blue, mode));
      ButtonResol.Add(new ButtonMenu(140, 30, "Full    (F11)", "fullscreen", 15, "FreeMonospacedBold", 790, 490, Color.Red, Color.Green, mode));

      Game = game;
    }

    public void Update(RenderTarget window, DisplayObject displayObject)
    {
      this.KeyHandler(window, displayObject);
    }

    /// <summary> Отображаю кнопки на экране </summary>
    public void Draw(RenderTarget window, DisplayObject displayObject)
    {
      Update(window, displayObject);

      gameForm.Draw(window);

      // Рисую меню
      for (int i = 0; i < ButtonMenus.Count; i++)
      {
        ButtonMenus[i].Draw(window);
      }
      for (int i = 0; i < ButtonLevel.Count; i++)
      {
        ButtonLevel[i].Draw(window);
      }
      for (int i = 0; i < ButtonResol.Count; i++)
      {
        ButtonResol[i].Draw(window);
      }
      TextBox.Draw(window);
    }

    private void KeyHandler(RenderTarget window, DisplayObject displayObject)
    {
      // Пересчет координат фигуры с использованием MapPixelToCoords
      Vector2i mousePosition = Mouse.GetPosition((Window)window); // координаты мыши
      Vector2f worldMouseCoords = window.MapPixelToCoords(mousePosition);
      FloatRect localBounds, globalBounds;

      if (Keyboard.IsKeyPressed(Keyboard.Key.F4) && ButtonLevel[0].GetColorButton() != Color.Green) // продолжаем играть
        SetColorPressLevel(ButtonLevel[0], ButtonLevel, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F5) && ButtonLevel[1].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonLevel[1], ButtonLevel, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F6) && ButtonLevel[2].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonLevel[2], ButtonLevel, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.Num7) && ButtonLevel[2].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonLevel[3], ButtonLevel, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.Num8) && ButtonLevel[2].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonLevel[4], ButtonLevel, displayObject);

      else if (Keyboard.IsKeyPressed(Keyboard.Key.F7) && ButtonResol[0].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonResol[0], ButtonResol, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F8) && ButtonResol[0].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonResol[1], ButtonResol, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F9) && ButtonResol[0].GetColorButton() != Color.Green) 
        SetColorPressLevel(ButtonResol[2], ButtonResol, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F10) && ButtonResol[1].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonResol[3], ButtonResol, displayObject);
      else if (Keyboard.IsKeyPressed(Keyboard.Key.F11) && ButtonResol[2].GetColorButton() != Color.Green)
        SetColorPressLevel(ButtonResol[4], ButtonResol, displayObject);

      /////////////// Работа с кнопками уровней //////////////////////////////////////////
      // проверка, наведена ли мышь на кнопки изменения уровня
      for (int i = 0; i < ButtonLevel.Count; i++)
      {
        // Пересчет границ фигуры с использованием TransformRect
        localBounds = ButtonLevel[i].MenuItemRect.GetLocalBounds();
        globalBounds = ButtonLevel[i].MenuItemRect.Transform.TransformRect(localBounds);
        // проверяю, находится ли курсор мыши над прямоугольником меню
        //if (ButtonLevel[i].MenuItemRect.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y) &&
        //    Mouse.IsButtonPressed(Mouse.Button.Left) && ButtonLevel[i].GetColorButton() != Color.Green)
        if (globalBounds.Contains(worldMouseCoords.X, worldMouseCoords.Y) &&
            Mouse.IsButtonPressed(Mouse.Button.Left) && ButtonLevel[i].GetColorButton() != Color.Green)
        {
          // проверяю, было ли нажатие, тогда меняю цвета кнопок - одна нажата (зеленый), другие нет (синий)
          SetColorPressLevel(ButtonLevel[i], ButtonLevel, displayObject);
        }
      }

      /////////////// Работа с кнопками разрешения //////////////////////////////////////////
      // проверка, наведена ли мышь на кнопки изменения уровня
      for (int i = 0; i < ButtonResol.Count; i++)
      {
        localBounds = ButtonResol[i].MenuItemRect.GetLocalBounds();
        globalBounds = ButtonResol[i].MenuItemRect.Transform.TransformRect(localBounds);
        // проверяю, находится ли курсор мыши над прямоугольником меню
        if (globalBounds.Contains(worldMouseCoords.X, worldMouseCoords.Y) &&
            Mouse.IsButtonPressed(Mouse.Button.Left) && ButtonResol[i].GetColorButton() != Color.Green)
        {
          // проверяю, было ли нажатие, тогда меняю цвета кнопок - одна нажата (зеленый), другие нет (синий)
          SetColorPressLevel(ButtonResol[i], ButtonResol, displayObject);
        }
      }
    }

    // устанавливаю цвета кнопок уровня
    public void SetColorPressLevel(ButtonMenu buttonMenu, List<ButtonMenu> buttonList, DisplayObject displayObject)
    {
      // заношу цвет в словарь 
      if (buttonMenu.AliasButton == "level1" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Green);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 2; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 1; // смещение дельта y
        GameSetting.PLATFORM_SPEED = 15f; // скорость платформы
        GameSetting.LEVEL = "Уровень 1";
      }
      else if (buttonMenu.AliasButton == "level2" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Green);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 4; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 3; // смещение дельта y
        GameSetting.PLATFORM_SPEED = 12f; // скорость платформы
        GameSetting.LEVEL = "Уровень 2";
      }
      else if (buttonMenu.AliasButton == "level3" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Green);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 6; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 5; // смещение дельта y
        GameSetting.PLATFORM_SPEED = 10f; // скорость платформы
        GameSetting.LEVEL = "Уровень 3";
      }
      else if (buttonMenu.AliasButton == "level4" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Green);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.BALL_DELTA_X = 8; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 7; // смещение дельта y
        GameSetting.PLATFORM_SPEED = 9f; // скорость платформы
        GameSetting.LEVEL = "Уровень 4";
      }
      else if (buttonMenu.AliasButton == "level5" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Green);
        GameSetting.BALL_DELTA_X = 10; // смещение дельта х
        GameSetting.BALL_DELTA_Y = 9; // смещение дельта y
        GameSetting.PLATFORM_SPEED = 8f; // скорость платформы
        GameSetting.LEVEL = "Уровень 5";
      }
      else if (buttonMenu.AliasButton == "800" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Green);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.IsVisibleMenu = false;
        GameSetting.IsVisibleMessageForm = false;
        Game.window.Size = new Vector2u(800, 600);
      }
      else if (buttonMenu.AliasButton == "1152" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Green);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.IsVisibleMenu = false;
        GameSetting.IsVisibleMessageForm = false;

        Game.window.Size = new Vector2u(1152, 864);
      }
      else if (buttonMenu.AliasButton == "1280" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Green);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.IsVisibleMenu = false;
        GameSetting.IsVisibleMessageForm = false;
        Game.window.Size = new Vector2u(1280, 960);

        //float aspectRatio = (float)1280 / 960;
        //GameSetting.ChangeResolution(Game.window, new Vector2u(1280, 960));
      }
      else if (buttonMenu.AliasButton == "1360" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Green);
        buttonList[4].SetColorButton(Color.Blue);
        GameSetting.IsVisibleMenu = false;
        GameSetting.IsVisibleMessageForm = false;
        Game.window.Size = new Vector2u(1360, 768);
      }
      else if (buttonMenu.AliasButton == "fullscreen" && buttonMenu.GetColorButton() != Color.Green)
      {
        buttonList[0].SetColorButton(Color.Blue);
        buttonList[1].SetColorButton(Color.Blue);
        buttonList[2].SetColorButton(Color.Blue);
        buttonList[3].SetColorButton(Color.Blue);
        buttonList[4].SetColorButton(Color.Green);
        GameSetting.IsVisibleMenu = false;
        GameSetting.IsVisibleMessageForm = false;
        // Получение массива доступных разрешений экрана по убыванию
        VideoMode[] modes = VideoMode.FullscreenModes;
        //modes[0] - максимальное разрешение экрана
        Game.window.Size = new Vector2u(modes[0].Width, modes[0].Height);
      }
      displayObject.SetSpeedDO(GameSetting.BALL_DELTA_X, GameSetting.BALL_DELTA_Y);
    }

  }
}
