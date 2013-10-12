using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessSolver
{
  public abstract class Chessman
  {
    /// <summary>
    /// король, ферзь, ладья(тура), конь(рыцарь), слон, пешка
    /// </summary>
    public enum FigureType { KING, QUEEN, ROOK, KNIGHT, BISHOP, PAWN, UNKNOWN};
    public enum Color { BLACK, WHITE, COLOR_COUNT };
    public static Color ReverseColor(Color c)
    {
      if (c == Color.BLACK)
        return Color.WHITE;
      return Color.BLACK;
    }
    public Position pos;
    public Color color;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    /// <param name="allMoves">все ходы(включая "самоубийство" короля)</param>
    /// <returns></returns>
    public abstract List<Move> getMoves(ChessBoard b, bool allMoves = false);
    public virtual List<Move> getBeatMoves(ChessBoard b)
    {
      return getMoves(b);
    }
    public abstract FigureType getType();
    public override string ToString()
    {
      String s = "";
      switch (getType())
      {
        case FigureType.BISHOP: s += "B"; break;
        case FigureType.KING: s += "K"; break;
        case FigureType.KNIGHT: s += "N"; break;
        case FigureType.PAWN: s += "p"; break;
        case FigureType.QUEEN: s += "Q"; break;
        case FigureType.ROOK: s += "R"; break;
      }
      s += pos.ToString();
      return s;
    }
  }

  public class KingFigure : Chessman
  {
    public KingFigure(String s, Color c) { pos = new Position(s); color = c; }
    public override FigureType getType() { return FigureType.KING; }
    public override List<Move> getMoves(ChessBoard b, bool allMoves)
    {
      List<Move> moveList = new List<Move>();
      int[] di={-1, -1, 0, 1, 1, 1, 0,-1};
      int[] dj={ 0,  1, 1, 1, 0,-1,-1,-1};
      for(int k=0;k<di.Count();k++)
      {
        Position npos = new Position(pos.i+di[k],pos.j+dj[k]);
        if ( npos.isValid() )
        {
          ////проверим присутствие другого короля
          //bool isKingNear = false;
          //ChessBoard.CellType enemyKingCell = (color == Color.WHITE) ? ChessBoard.CellType.BKING : ChessBoard.CellType.WKING;
          //for (int z = 0; z < di.Count() && !isKingNear; z++)
          //{
          //  Position kpos = new Position(npos.i + di[z], npos.j + dj[z]);
          //  if (kpos.isValid() && b[kpos.i, kpos.j] == enemyKingCell)
          //    isKingNear = true;
          //}
          //if (isKingNear)
          //  continue;
          if (allMoves || !b.isBeat(npos.i, npos.j, ReverseColor(color)))
          {
            if (b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
            {
              moveList.Add(new Move(this, npos, false));
            }
            else
            {
              if (Helper.getColor(b[npos.i, npos.j]) != color || allMoves)
                moveList.Add(new Move(this, npos, true));
            }
          }
        }
      }
      return moveList;
    }
  }
  
  public class QuinFigure : Chessman
  {
    public QuinFigure(String s, Color c) { pos = new Position(s); color = c; }
    public override FigureType getType() { return FigureType.QUEEN; }
    public override List<Move> getMoves(ChessBoard b, bool allMoves)
    {
      List<Move> moveList = new List<Move>();
      int[] di = {-1,-1, 0, 1, 1, 1, 0, -1};
      int[] dj = { 0, 1, 1, 1, 0, -1,-1,-1};      
      for(int k=0;k<di.Count();k++)
      {
        for (int z = 1; z < 8; z++)
        {
          Position npos = new Position(pos.i + z*di[k], pos.j + z*dj[k]);
          if (npos.isValid())
          {
            if (b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
              moveList.Add(new Move(this, npos, false));
            else
            {
              if (Helper.getColor(b[npos.i, npos.j]) != color || allMoves)
                moveList.Add(new Move(this, npos, true));
              break;
            }
          }
          else
            break;
        }
      }
      return moveList;
    }
  }
  
  public class RookFigure : Chessman
  {
    public RookFigure(String s, Color c) { pos = new Position(s); color = c; }
    public override FigureType getType() { return FigureType.ROOK; }
    public override List<Move> getMoves(ChessBoard b, bool allMoves)
    {
      List<Move> moveList = new List<Move>();
      int[] di = { 1,-1, 0, 0};
      int[] dj = { 0, 0, 1,-1};
      for (int k = 0; k < di.Count(); k++)
      {
        for (int z = 1; z < 8; z++)
        {
          Position npos = new Position(pos.i + z * di[k], pos.j + z * dj[k]);
          if (npos.isValid())
          {
            if (b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
              moveList.Add(new Move(this, npos, false));
            else
            {
              if (Helper.getColor(b[npos.i, npos.j]) != color || allMoves)
                moveList.Add(new Move(this, npos, true));
              break;
            }
          }
          else
            break;
        }
      }
      return moveList;
    }
  }

  public class KnightFigure : Chessman
  {
    public KnightFigure(String s, Color c) { pos = new Position(s); color = c; }
    public override FigureType getType() { return FigureType.KNIGHT; }
    public override List<Move> getMoves(ChessBoard b, bool allMoves)
    {
      List<Move> moveList = new List<Move>();
      int[] di = {-2,-1, 1, 2, 2, 1,-1,-2};
      int[] dj = { 1, 2, 2, 1,-1,-2,-2,-1};
      for (int k = 0; k < di.Count(); k++)
      {
        Position npos = new Position(pos.i + di[k], pos.j + dj[k]);
        if (npos.isValid())
        {
          if (b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
            moveList.Add(new Move(this, npos, false));
          else
          {
            if (Helper.getColor(b[npos.i, npos.j]) != color || allMoves)
              moveList.Add(new Move(this, npos, true));
          }
        }
      }
      return moveList;
    }
  }

  public class BishopFigure : Chessman
  {
    public BishopFigure(String s, Color c) { pos = new Position(s); color = c; }
    public override FigureType getType() { return FigureType.BISHOP; }
    public override List<Move> getMoves(ChessBoard b, bool allMoves)
    {
      List<Move> moveList = new List<Move>();
      int[] di = {-1, 1, 1,-1 };
      int[] dj = { 1, 1,-1,-1 };
      for (int k = 0; k < di.Count(); k++)
      {
        for (int z = 1; z < 8; z++)
        {
          Position npos = new Position(pos.i + z * di[k], pos.j + z * dj[k]);
          if (npos.isValid())
          {
            if (b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
              moveList.Add(new Move(this, npos, false));
            else
            {
              if (Helper.getColor(b[npos.i, npos.j]) != color || allMoves)
                moveList.Add(new Move(this, npos, true));
              break;
            }
          }
          else
            break;
        }
      }
      return moveList;
    }
  }

  public class PawnFigure : Chessman
  {
    public PawnFigure(String s, Color c) { pos = new Position(s); color = c; }
    public override FigureType getType() { return FigureType.PAWN; }
    public override List<Move> getMoves(ChessBoard b, bool allMoves)
    {
      List<Move> moveList = new List<Move>();
      // todo if (ChessBoard.isWhiteBottom)
      if (color == Color.WHITE)
      {
        Position npos = new Position(pos.i - 1, pos.j);
        if (npos.isValid())
        {
          if (b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
            moveList.Add(new Move(this, npos,false));
        }
        if (pos.i == 6 && b[5,pos.j] == ChessBoard.CellType.EMPTY)
        {
          Position npos2 = new Position(pos.i - 2, pos.j);
          if (b[npos2.i, npos2.j] == ChessBoard.CellType.EMPTY)
            moveList.Add(new Move(this, npos2, false));
        }
      }
      else
      {
        Position npos = new Position(pos.i + 1, pos.j);
        if (npos.isValid())
        {
          if(b[npos.i, npos.j] == ChessBoard.CellType.EMPTY)
            moveList.Add(new Move(this, npos,false));
        }
        if (pos.i == 1 && b[2, pos.j] == ChessBoard.CellType.EMPTY)
        {
          Position npos2 = new Position(pos.i + 2, pos.j);
          if (b[npos2.i, npos2.j] == ChessBoard.CellType.EMPTY)
            moveList.Add(new Move(this, npos2, false));
        }
      }

      int d = 1; // вниз
      if (color == Color.WHITE) d = -1; // вверх
      int[] di = { d, d };
      int[] dj = { -1, 1 };
      for (int k = 0; k < di.Count(); k++)
      {
        Position npos = new Position(pos.i + di[k], pos.j + dj[k]);
        if (npos.isValid())
          if( allMoves ||
              (b[npos.i, npos.j] != ChessBoard.CellType.EMPTY && Helper.getColor(b[npos.i, npos.j]) != color))
            moveList.Add(new Move(this, npos, true));
      }
      return moveList;
    }
  }
}
