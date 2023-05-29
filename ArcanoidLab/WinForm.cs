using SFML.System;
using SFML.Window;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArcanoidLab
{
  public class WinForm
  {
    private Ball _ball;
    private Platform _platform;
    private Block _block;
    private VideoMode _mode;
    private SaveLoadState _saveLoadState;
    private GameState _gameState;
    private Game _game;

    // Экземпляры объектов управления виндоус форм
    Button buttonNewGame = new Button();
    Button buttonLoad = new Button();
    Button buttonSave = new Button();
    Button buttonExit = new Button();
    Button buttonOkResolt = new Button();
    Button buttonOkLevel = new Button();
    ComboBox comboBoxResol = new ComboBox();
    ComboBox comboBoxLevel = new ComboBox();
    Label labelComboResolt = new Label();
    Label labelComboLevel = new Label();
    Label labelComboPlayer = new Label();
    TextBox textBoxPlayer = new TextBox();

    // Получение массива доступных разрешений экрана по убыванию
    VideoMode[] modes = VideoMode.FullscreenModes;

    public Form FormUser { get; set; } = new Form();

    public WinForm(Ball ball, Platform platform, Block block, VideoMode mode, Game game)
    {
      _ball = ball;
      _platform = platform;
      _block = block;
      _mode = mode;
      _saveLoadState = new SaveLoadState();
      _game = game;

      FormUser.AutoSize = true;
      FormUser.Hide();
      
      buttonNewGame.Text = "Новая игра";
      buttonNewGame.Location = new Point(10, 10);
      buttonNewGame.Size = new Size(260, 30);
      buttonNewGame.Click += buttonNewGame_Click;

      buttonLoad.Text = "Загрузить";
      buttonLoad.Location = new Point(buttonNewGame.Left, buttonNewGame.Height + buttonNewGame.Top + 10);
      buttonLoad.Size = new Size(260, 30);
      buttonLoad.Click += buttonLoad_Click;

      buttonSave.Text = "Сохранить";
      buttonSave.Location = new Point(buttonLoad.Left, buttonLoad.Height + buttonLoad.Top + 10);
      buttonSave.Size = new Size(260, 30);
      buttonSave.Click += buttonSave_Click;

      buttonExit.Text = "Выйти из игры";
      buttonExit.Location = new Point(buttonSave.Left, buttonSave.Height + buttonSave.Top + 10);
      buttonExit.Size = new Size(260, 30);
      buttonExit.Click += buttonExit_Exit;

      labelComboResolt.Text = "Выберите разрешение экрана";
      labelComboResolt.AutoSize = true;
      labelComboResolt.Location = new Point(buttonExit.Left, buttonExit.Height + buttonExit.Top + 20);

      comboBoxResol.Location = new Point(labelComboResolt.Left, labelComboResolt.Height + labelComboResolt.Top + 1);
      comboBoxResol.Size = new Size(180, 30);
      int countItem = 0; // кол-во элементов добавленных в выпадающий список
      for (int i = 0; i < modes.Length; i++)
      {
        if (modes[i].Width >= _game.mode.Width)
        {
          comboBoxResol.Items.Insert(i, modes[i].Width.ToString() + "x" + modes[i].Height.ToString());// формирую список разрешений экрана
          countItem++;
        }
      }
      comboBoxResol.SelectedIndex = countItem - 1; // максимальное значение это наше разрешение, его и вывожу на экран на форме

      buttonOkResolt.Text = "Применить";
      buttonOkResolt.Location = new Point(comboBoxResol.Right, comboBoxResol.Width + 18);
      buttonOkResolt.AutoSize = true;
      buttonOkResolt.Click += buttonOkResolt_Click;

      labelComboLevel.Text = "Выберите уровень игры";
      labelComboLevel.AutoSize = true;
      labelComboLevel.Location = new Point(comboBoxResol.Left, comboBoxResol.Height + comboBoxResol.Top + 10);

      comboBoxLevel.Location = new Point(labelComboLevel.Left, labelComboLevel.Height + labelComboLevel.Top + 1);
      comboBoxLevel.Size = new Size(180, 30);
      comboBoxLevel.Items.Insert(0, "Лёгкий");
      comboBoxLevel.Items.Insert(1, "Средний");
      comboBoxLevel.Items.Insert(2, "Тяжелый");
      comboBoxLevel.SelectedIndex = comboBoxLevel.FindString(GameSetting.LEVEL); // установка значения по умолчанию из настроек игры

      buttonOkLevel.Text = "Применить";
      buttonOkLevel.Location = new Point(comboBoxLevel.Right, comboBoxLevel.Width + 75);
      buttonOkLevel.AutoSize = true;
      buttonOkLevel.Click += buttonOkLevel_Click;

      labelComboPlayer.Text = "Имя игрока";
      labelComboPlayer.AutoSize = true;
      labelComboPlayer.Location = new Point(comboBoxLevel.Left, comboBoxLevel.Height + comboBoxLevel.Top + 5);

      textBoxPlayer.Text = "Катя";
      textBoxPlayer.Size = new Size(260, 30);
      textBoxPlayer.Location = new Point(labelComboPlayer.Left, labelComboPlayer.Height + labelComboPlayer.Top + 1);


      FormUser.Text = "Форма настроек";
      FormUser.HelpButton = true;

      FormUser.FormBorderStyle = FormBorderStyle.FixedDialog;
      FormUser.MaximizeBox = false;
      FormUser.MinimizeBox = false;
      FormUser.StartPosition = FormStartPosition.CenterScreen;

      FormUser.Controls.Add(buttonNewGame);
      FormUser.Controls.Add(buttonLoad);
      FormUser.Controls.Add(buttonSave);
      FormUser.Controls.Add(buttonExit);
      FormUser.Controls.Add(buttonOkResolt);
      FormUser.Controls.Add(buttonOkLevel);
      FormUser.Controls.Add(labelComboResolt);
      FormUser.Controls.Add(comboBoxResol);
      FormUser.Controls.Add(labelComboLevel);
      FormUser.Controls.Add(comboBoxLevel);
      FormUser.Controls.Add(labelComboPlayer);
      FormUser.Controls.Add(textBoxPlayer);

      //FormUser.ShowDialog();
      FormUser.FormClosing += FormUser_FormClosing;
    }

    private void FormUser_FormClosing(object sender, FormClosingEventArgs e)
    {
      GameSetting.PLAYER_NAME = textBoxPlayer.Text;
    }

    private void HideForm()
    {
      GameSetting.PLAYER_NAME = textBoxPlayer.Text;
      FormUser.Hide();
    }

    private void buttonExit_Exit(object sender, EventArgs e)
    {
      _game.window.Close();
      HideForm();
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      //MessageBox.Show("Привет сохранение!");
      _gameState = new GameState(_ball, _platform, _block.Blocks);
      _saveLoadState.SaveState(_gameState);
      HideForm();
    }

    private void buttonLoad_Click(object sender, EventArgs e)
    {
      _gameState = _saveLoadState.LoadState(_ball, _platform, _block, _mode);
      HideForm();
      //MessageBox.Show("Привет загрузка!");
    }

    private void buttonNewGame_Click(object sender, EventArgs e)
    {
      GameSetting.IsStart = true;
      GameSetting.LifeCount = GameSetting.LIFE_TOTAL;
      GameSetting.Score = 0;
      _block.Update(_mode);
      //_game.Secundomer.OnStart();
      HideForm();
    }

    private void buttonOkResolt_Click(object sender, EventArgs e)
    {
      int selectedIndex = comboBoxResol.SelectedIndex;
      //object selectedItem = comboBoxResol.SelectedItem;

      if (selectedIndex != -1) // если выбрано значение
      {
        //GameSetting.ChangeResolution(_game.window, new Vector2u(modes[selectedIndex].Width, modes[selectedIndex].Height)); // изменение разрешения
        //_game.mode = new VideoMode(modes[selectedIndex].Width, modes[selectedIndex].Height); // установка разрешения для игры окна
        _game.window.Size = new Vector2u(modes[selectedIndex].Width, modes[selectedIndex].Height);
        //var ga = _game.mode;
        HideForm();
      }
      else
        MessageBox.Show("Не выбрано значение разрешения экрана.", "Предупреждение");
    }

    private void buttonOkLevel_Click(object sender, EventArgs e)
    {
      int selectedIndex = comboBoxLevel.SelectedIndex;

      if (selectedIndex != -1) // если выбрано значение
      {
        switch (selectedIndex)
        {
          case 0:
            GameSetting.BALL_DELTA_X = 2; // смещение дельта х
            GameSetting.BALL_DELTA_Y = 1; // смещение дельта y
            GameSetting.LEVEL = "Лёгкий";
            break;
          case 1:
            GameSetting.BALL_DELTA_X = 6; // смещение дельта х
            GameSetting.BALL_DELTA_Y = 5; // смещение дельта y
            GameSetting.LEVEL = "Средний";
            break;
          case 2:
            GameSetting.BALL_DELTA_X = 9; // смещение дельта х
            GameSetting.BALL_DELTA_Y = 8; // смещение дельта y
            GameSetting.LEVEL = "Тяжелый";
            break;
        }
        _ball.SetSpeedDO(); // установка значений скорости шарика
        HideForm();
      }
      else
        MessageBox.Show("Не выбрано значение уровня игры.", "Предупреждение");
    }

  }
}
