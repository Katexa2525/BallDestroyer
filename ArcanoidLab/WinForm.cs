using SFML.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public Form FormUser { get; set; }/* = new Form();*/

    public WinForm(Ball ball, Platform platform, Block block, VideoMode mode)
    {
      _ball = ball;
      _platform = platform;
      _block = block;
      _mode = mode;
      _saveLoadState = new SaveLoadState();

      FormUser = new Form();
      FormUser.AutoSize = true;
      FormUser.Hide();
      // Create two buttons to use as the accept and cancel buttons.
      Button buttonNewGame = new Button();
      Button buttonLoad = new Button();
      Button buttonSave = new Button();
      ComboBox comboBoxResol = new ComboBox();
      ComboBox comboBoxLevel = new ComboBox();
      Label labelComboResolt = new Label();
      Label labelComboLevel = new Label();
      Label labelComboPlayer = new Label();
      System.Windows.Forms.TextBox textBoxPlayer = new System.Windows.Forms.TextBox();

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

      labelComboResolt.Text = "Выберите разрешение экрана";
      labelComboResolt.AutoSize = true;
      labelComboResolt.Location = new Point(buttonSave.Left, buttonSave.Height + buttonSave.Top + 20);

      comboBoxResol.Location = new Point(labelComboResolt.Left, labelComboResolt.Height + labelComboResolt.Top + 1);
      comboBoxResol.Size = new Size(260, 30);
      comboBoxResol.Items.Insert(0, "800x600");
      comboBoxResol.Items.Insert(1, "1200x900");
      comboBoxResol.Items.Insert(2, "Fullscreen");

      labelComboLevel.Text = "Выберите уровень игры";
      labelComboLevel.AutoSize = true;
      labelComboLevel.Location = new Point(comboBoxResol.Left, comboBoxResol.Height + comboBoxResol.Top + 10);

      comboBoxLevel.Location = new Point(labelComboLevel.Left, labelComboLevel.Height + labelComboLevel.Top + 1);
      comboBoxLevel.Size = new Size(260, 30);
      comboBoxLevel.Items.Insert(0, "Лёгкий");
      comboBoxLevel.Items.Insert(1, "Средний");
      comboBoxLevel.Items.Insert(2, "Тяжелый");

      labelComboPlayer.Text = "Имя игрока";
      labelComboPlayer.AutoSize = true;
      labelComboPlayer.Location = new Point(comboBoxLevel.Left, comboBoxLevel.Height + comboBoxLevel.Top + 5);

      textBoxPlayer.Text = "Катя";
      textBoxPlayer.Size = new Size(260, 30);
      textBoxPlayer.Location = new Point(labelComboPlayer.Left, labelComboPlayer.Height + labelComboPlayer.Top + 1);


      FormUser.Text = "Форма выбора";
      FormUser.HelpButton = true;

      FormUser.FormBorderStyle = FormBorderStyle.FixedDialog;
      FormUser.MaximizeBox = false;
      FormUser.MinimizeBox = false;
      FormUser.StartPosition = FormStartPosition.CenterScreen;

      FormUser.Controls.Add(buttonNewGame);
      FormUser.Controls.Add(buttonLoad);
      FormUser.Controls.Add(buttonSave);
      FormUser.Controls.Add(labelComboResolt);
      FormUser.Controls.Add(comboBoxResol);
      FormUser.Controls.Add(labelComboLevel);
      FormUser.Controls.Add(comboBoxLevel);
      FormUser.Controls.Add(labelComboPlayer);
      FormUser.Controls.Add(textBoxPlayer);

      FormUser.ShowDialog();
    }

    //private void CreateForm

    private void buttonSave_Click(object sender, EventArgs e)
    {
      //MessageBox.Show("Привет сохранение!");
      _gameState = new GameState(_ball, _platform, _block.Blocks);
      _saveLoadState.SaveState(_gameState);
      FormUser.Hide();
    }

    private void buttonLoad_Click(object sender, EventArgs e)
    {
      _gameState = _saveLoadState.LoadState(_ball, _platform, _block, _mode);
      FormUser.Hide();
      //MessageBox.Show("Привет загрузка!");
    }

    private void buttonNewGame_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Привет новая игра!");
    }
  }
}
