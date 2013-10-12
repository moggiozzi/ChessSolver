using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSolver
{
  public class Helper
  {
    public static Chessman.FigureType FigureTypeFromString(String s)
    {
      switch (s.Trim()[0])
      {
        case 'K': return Chessman.FigureType.KING;
        case 'Q': return Chessman.FigureType.QUEEN;
        case 'B': return Chessman.FigureType.BISHOP;
        case 'N': return Chessman.FigureType.KNIGHT;
        case 'R': return Chessman.FigureType.ROOK;
        case 'p': return Chessman.FigureType.PAWN;
      }
      return Chessman.FigureType.UNKNOWN;
    }

    public static String FigureTypeToString(Chessman.FigureType ft)
    {
      switch (ft)
      {
        case Chessman.FigureType.KING: return "K";
        case Chessman.FigureType.QUEEN: return "Q";
        case Chessman.FigureType.BISHOP: return "B";
        case Chessman.FigureType.KNIGHT: return "N";
        case Chessman.FigureType.ROOK: return "R";
        case Chessman.FigureType.PAWN: return "p";
      }
      return "";
    }

    public static Problem ReadProblemFromFile(String fileName = "input.txt")
    {
      Problem p = new Problem();
      if (File.Exists(fileName))
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
          using (TextReader r = new StreamReader(fs))
          {
            string s = r.ReadLine(); //N
            int.TryParse(s, out p.depth);

            // white
            s = r.ReadLine();
            string[] whiteFigures = s.Split();
            for (int i = 0; i < whiteFigures.Count(); i++)
            {
              if (whiteFigures[i].Trim().Length == 3)
                p.board.addFigure(Helper.createFigure(whiteFigures[i].Trim(), Chessman.Color.WHITE));
            }

            // black
            s = r.ReadLine();
            string[] blackFigures = s.Split();
            for (int i = 0; i < blackFigures.Count(); i++)
            {
              if (blackFigures[i].Trim().Length == 3)
                p.board.addFigure(Helper.createFigure(blackFigures[i].Trim(), Chessman.Color.BLACK));
            }
          }
        }
      return p;
    }

    void WriteAnswerToFile()
    {
      String fileName = "output.txt";
      using (FileStream fs = new FileStream(fileName, FileMode.CreateNew))
      {
        using (BinaryWriter w = new BinaryWriter(fs))
        {
          for (int i = 0; i < 11; i++)
          {
            w.Write(i);
          }
        }
      }
    }

    public static Chessman createFigure(String s, Chessman.Color c)
    {
      Chessman chessman = null;
      switch (s[0])
      {
        case 'K':
          chessman = new KingFigure(s, c);
          break;
        case 'Q':
          chessman = new QuinFigure(s, c);
          break;
        case 'R':
          chessman = new RookFigure(s, c);
          break;
        case 'N':
          chessman = new KnightFigure(s, c);
          break;
        case 'B':
          chessman = new BishopFigure(s, c);
          break;
        case 'p':
          chessman = new PawnFigure(s, c);
          break;
        default:
          // unknown
          break;
      }
      return chessman;
    }

    public static Chessman.Color getColor(ChessBoard.CellType cell)
    {
      switch (cell)
      {
        case ChessBoard.CellType.BBISHOP:
        case ChessBoard.CellType.BKING:
        case ChessBoard.CellType.BKNIGHT:
        case ChessBoard.CellType.BPAWN:
        case ChessBoard.CellType.BQUEEN:
        case ChessBoard.CellType.BROOK:
          return Chessman.Color.BLACK;
      }
      return Chessman.Color.WHITE;
    }

    public static void WriteSolutionToFile(List<Solution> ss, String fileName = "output.txt")
    {
      using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        using (TextWriter r = new StreamWriter(fs))
        {
          r.WriteLine(ss.Count); // количество решений
          foreach (var s in ss)
          {
            r.WriteLine((int)(s.Moves.Count+1)/2); // количество ходов
            for (int i = 0; i < s.Moves.Count; i+=2)
            {
              r.Write(s.Moves[i].ToString()); // ход белых
              if (i + 1 < s.Moves.Count)
              {
                r.Write(' ');
                r.WriteLine(s.Moves[i + 1].ToString()); // ход черных
              }
            }
          }
        }
      }
    }
  }
}
