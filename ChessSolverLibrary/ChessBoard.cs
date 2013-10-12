using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSolver
{  
  public class ChessBoard
  {
    public enum CellType { EMPTY,
      WKING, WQUEEN, WROOK, WKNIGHT, WBISHOP, WPAWN,
      BKING, BQUEEN, BROOK, BKNIGHT, BBISHOP, BPAWN,
      CELL_TYPE_COUNT
    };

    const int size = 8;
    public int Size
    {
      get { return size; }
    }

    /// <summary>
    /// Ориентация доски. Находятся ли белые "снизу"?
    /// </summary>
    static bool isWhiteBottom()
    {
      return true;
    }

    enum BeatType { WHITE_BEAT = 1, BLACK_BEAT = 2, FREE = 0 };
    Byte[,] beatBoard = new Byte[size,size];

    CellType[,] board = new CellType[size, size];
    public CellType this[int i, int j]
    {
      get { return board[i,j]; }
      set { board[i, j] = value; }
    }

    List<Chessman> whiteFigures = new List<Chessman>();
    List<Chessman> blackFigures = new List<Chessman>();

    Chessman blackKing;
    
    public void addFigure(Chessman chessman)
    {
      CellType cellType = CellType.EMPTY;
      if (chessman.color == Chessman.Color.WHITE)
      {
        switch (chessman.getType())
        {
          case Chessman.FigureType.BISHOP: cellType = CellType.WBISHOP; break;
          case Chessman.FigureType.KING: cellType = CellType.WKING; break;
          case Chessman.FigureType.KNIGHT: cellType = CellType.WKNIGHT; break;
          case Chessman.FigureType.PAWN: cellType = CellType.WPAWN; break;
          case Chessman.FigureType.QUEEN: cellType = CellType.WQUEEN; break;
          case Chessman.FigureType.ROOK: cellType = CellType.WROOK; break;
        }
        whiteFigures.Add(chessman);
      }
      else
      {
        switch (chessman.getType())
        {
          case Chessman.FigureType.BISHOP: cellType = CellType.BBISHOP; break;
          case Chessman.FigureType.KING:
            {
              cellType = CellType.BKING;
              blackKing = chessman;
            } break;
          case Chessman.FigureType.KNIGHT: cellType = CellType.BKNIGHT; break;
          case Chessman.FigureType.PAWN: cellType = CellType.BPAWN; break;
          case Chessman.FigureType.QUEEN: cellType = CellType.BQUEEN; break;
          case Chessman.FigureType.ROOK: cellType = CellType.BROOK; break;
        }
        blackFigures.Add(chessman);
      }
      board[chessman.pos.i, chessman.pos.j] = cellType;
      calcBeats();
    }

    public void doMove(Move move)
    {
      board[move.ToPos.i, move.ToPos.j] = board[move.FromPos.i, move.FromPos.j];
      board[move.FromPos.i, move.FromPos.j] = CellType.EMPTY;
      move.figure.pos = move.ToPos;
    }

    public void revertMove(Move move)
    {
      board[move.FromPos.i, move.FromPos.j] = board[move.ToPos.i, move.ToPos.j];
      board[move.ToPos.i, move.ToPos.j] = CellType.EMPTY;
      move.figure.pos = move.FromPos;
    }
    
    public bool isMate()
    {
      calcBeats();
      if ((beatBoard[blackKing.pos.i, blackKing.pos.j] & (int)BeatType.WHITE_BEAT) != 0)
      {
        if (blackKing.getMoves(this).Count == 0)
          return true;
      }
      return false;
    }

    public List<Move> getMoves()
    {
      calcBeats();
      List<Move> moves = new List<Move>();
      foreach (var f in whiteFigures)
        moves.AddRange( f.getMoves(this) );
      return moves;
    }

    public Chessman getFigure(int i, int j)
    {
      calcBeats();
      CellType cellType = board[i,j];
      if (cellType != CellType.EMPTY)
      {
        if (Helper.getColor(cellType) == Chessman.Color.BLACK)
        {
          foreach (var f in blackFigures)
            if (f.pos.i == i && f.pos.j == j)
              return f;
        }
        else
        {
          foreach (var f in whiteFigures)
            if (f.pos.i == i && f.pos.j == j)
              return f;
        }
      }
      return null;
    }

    public bool isBeat(int i, int j, Chessman.Color c)
    {
      if (c == Chessman.Color.WHITE)
        return (beatBoard[i, j] & (int)BeatType.WHITE_BEAT) != 0;
      else
        return (beatBoard[i, j] & (int)BeatType.BLACK_BEAT) != 0;
    }

    public void calcBeats()
    {
      for (int i = 0; i < 8; i++)
        for (int j = 0; j < 8; j++)
          beatBoard[i, j] = 0;
      foreach (var f in whiteFigures)
      {
        List<Move> moves = f.getMoves(this,true);
        foreach (var m in moves)
          beatBoard[m.ToPos.i, m.ToPos.j] |= (int)BeatType.WHITE_BEAT;
      }
      foreach (var f in blackFigures)
      {
        List<Move> moves = f.getMoves(this, true);
        foreach (var m in moves)
          beatBoard[m.ToPos.i, m.ToPos.j] |= (int)BeatType.BLACK_BEAT;
      }
    }
  }
}
