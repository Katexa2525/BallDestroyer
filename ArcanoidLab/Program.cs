using SFML.Graphics;
using System;
using System.Timers;

namespace ArcanoidLab
{
  class Program
  {
    static void Main(string[] args)
    {
      Game game = new Game(1024, 768, "Арканоид");
      //Game game = new Game();
      game.Run();
    }
  
  }
}
