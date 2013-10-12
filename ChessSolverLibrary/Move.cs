using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessSolver
{
  public class Position
  {
    public int i=0;
    public int j=0;
    public Position(int i_=0,int j_=0) { i = i_; j = j_; }
    public Position(String s)
    {
      int.TryParse(s[s.Length - 1].ToString(), out i);
      i = 8 - i; // fixme 8 to Board.Size
      j = s[s.Length - 2] - 'a';
    }
    public override String ToString()
    {
      return ((char)(j+'a')).ToString() + (8-i).ToString();
    }
    public bool isValid() { return i >= 0 && i < 8 && j >= 0 && j < 8; }
  }

  public class Move
  {
    Position fromPos;
    public Position FromPos { get { return fromPos; } }
    Position toPos;
    public Position ToPos { get { return toPos; } }
    public bool IsBeat; // ход бьет другую фигуру
    public Chessman figure;
    
    //public Move(String s)
    //{
    //  figureType = Helper.FigureTypeFromString(s);
    //  string[] strs = s.Split('-');
    //  fromPos = new Position(strs[0]);
    //  toPos = new Position(strs[1]);
    //}

    public Move(Move m)
    {
      toPos = new Position(m.toPos.i, m.toPos.j);
      fromPos = new Position(m.fromPos.i, m.fromPos.j);
      figure = Helper.createFigure(m.figure.ToString(), m.figure.color);
    }

    public Move(Chessman f, Position newPos, bool isBeat)
    {
      fromPos = new Position(f.pos.i, f.pos.j);
      figure = f;// Helper.createFigure(f.ToString(), f.color);
      toPos = newPos;
      IsBeat = isBeat;
    }

    public override String ToString()
    {
      return Helper.FigureTypeToString(figure.getType()) + fromPos.ToString() + "-" + toPos.ToString();
    }
  }
}
