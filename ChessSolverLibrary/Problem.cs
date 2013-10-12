using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessSolver
{
  public class Problem
  {
    /// <summary>
    /// Глубина (количество ходов)
    /// </summary>
    public int depth;
    public ChessBoard board = new ChessBoard();
  }
}
