using SFML.Graphics;
using System;

namespace ArcanoidLab
{
  class Program
  {
    static void Main(string[] args)
    {
      Game game = new Game(800, 600, "Арканоид");
      //Game game = new Game();
      game.Run();
    }
  }
}
