using SFML.Graphics;
using SFML.Window;
using System;
using System.Timers;

namespace ArcanoidLab
{
  class Program
  {
    static void Main(string[] args)
    {
      VideoMode[] modes = VideoMode.FullscreenModes;
      Game game = new Game(modes[0].Width, modes[0].Height, "Арканоид");
      //Game game = new Game();
      game.Run();
    }
  
  }
}
