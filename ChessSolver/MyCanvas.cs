using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessSolver
{
  public class MyCanvas : Canvas
  {
    public BitmapImage[] FigImages = new BitmapImage[(int)ChessBoard.CellType.CELL_TYPE_COUNT];
    public BitmapImage BoardImage;
    public ChessBoard board;
    List<Move> moves;
    //public ObservableCollection<Move> List;

    public void loadImages()
    {
      BoardImage = new BitmapImage(new Uri("Resources/Board.png", UriKind.Relative));

      FigImages[(int)ChessBoard.CellType.EMPTY] = null;
      FigImages[(int)ChessBoard.CellType.BBISHOP] = new BitmapImage(new Uri("Resources/BBISHOP.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.BKING] = new BitmapImage(new Uri("Resources/BKING.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.BKNIGHT] = new BitmapImage(new Uri("Resources/BKNIGHT.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.BPAWN] = new BitmapImage(new Uri("Resources/BPAWN.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.BQUEEN] = new BitmapImage(new Uri("Resources/BQUEEN.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.BROOK] = new BitmapImage(new Uri("Resources/BROOK.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.WBISHOP] = new BitmapImage(new Uri("Resources/WBISHOP.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.WKING] = new BitmapImage(new Uri("Resources/WKING.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.WKNIGHT] = new BitmapImage(new Uri("Resources/WKNIGHT.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.WPAWN] = new BitmapImage(new Uri("Resources/WPAWN.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.WQUEEN] = new BitmapImage(new Uri("Resources/WQUEEN.png", UriKind.Relative));
      FigImages[(int)ChessBoard.CellType.WROOK] = new BitmapImage(new Uri("Resources/WROOK.png", UriKind.Relative));
    }

    protected override void OnRender(DrawingContext dc)
    {
      if ( ! ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)) )
        drawBoard(dc);
      base.OnRender(dc);
    }
    void drawBoard(DrawingContext dc)
    {
      dc.DrawImage(BoardImage, new Rect(this.RenderSize));
      int cellSize = (int) this.RenderSize.Width / 8;
      for (int i = 0; i < board.Size; i++)
        for (int j = 0; j < board.Size; j++)
        {
          ChessBoard.CellType cellType = board[i, j];
          dc.DrawImage(FigImages[(int)cellType],
            new Rect(j*cellSize, i*cellSize, cellSize, cellSize));
        }
      if (moves != null)
        foreach (var m in moves)
        {
          Pen pen;
          if (m.IsBeat)
            pen = new Pen(new SolidColorBrush(Colors.Yellow), 4.0);
          else
           pen = new Pen(new SolidColorBrush(Colors.Blue),2.0);
          dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(255,255,0)),pen,
            new Rect(
              m.ToPos.j * cellSize + cellSize/2,
              m.ToPos.i * cellSize + cellSize / 2,
              4,4
              ));
        }
    }
    protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
    {
      base.OnMouseMove(e);
    }
    protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
    {
      int cellSize = (int)this.RenderSize.Width / 8;
      Point p = e.GetPosition(this);
      int i = (int) p.Y / cellSize;
      int j = (int) p.X / cellSize;
      Chessman f = board.getFigure(i, j);
      if (f == null)
        moves = null;
      else
        moves = f.getMoves(board);
      base.OnMouseDown(e);
      this.InvalidateVisual();
    }
  }
}
