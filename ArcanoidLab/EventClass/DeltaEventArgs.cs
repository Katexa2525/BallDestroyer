using System;

namespace ArcanoidLab.EventClass
{
  /// <summary> Класс EventArgs для хранения информации скорости смещения dx, dy </summary>
  public class DeltaEventArgs : EventArgs
  {
    public int DX { get; } // смещение дельта х
    public int DY { get; } // смещение дельта y 

    public DeltaEventArgs(int dx, int dy)
    {
      this.DX = dx;
      this.DY = dy;
    }
  }
}
