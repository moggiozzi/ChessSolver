using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChessSolver
{
  public class Solver
  {
    static List<Solution> solutions;
    static List<Move> moveList;
    static int cnt;
    static Stopwatch stopwatch;
    static public List<Solution> getSolutions(Problem p)
    {
      stopwatch = new Stopwatch();
      stopwatch.Start();
      solutions = new List<Solution>();
      moveList = new List<Move>();
      cnt = 1;
      solv(p);
      return solutions;
    }

    static void solv(Problem p)
    {
      if (stopwatch.Elapsed.Seconds >= 2)
        return;
      List<Move> moves = p.board.getMoves();
      foreach (var m in moves)
      {
        p.board.doMove(m);
        p.depth--;
        moveList.Add(m);

        if (p.depth > 0)
          solv(p);
        else
        {
          if (p.board.isMate())
          {
            // запомним решение
            Solution solution = new Solution();
            solution.Name = cnt.ToString(); cnt++;
            foreach (var mm in moveList)
            {
              solution.Moves.Add(new Move(mm));
            }
            solutions.Add(solution);
          }
        }

        moveList.Remove(m);
        p.depth++;
        p.board.revertMove(m);
      }
    }
  }

  public class Solution
  {
    public String Name;
    public List<Move> Moves = new List<Move>();
    public override string ToString()
    {
      return Name;
    }
  }
}
