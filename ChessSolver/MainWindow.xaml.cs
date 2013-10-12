using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ChessSolver
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    Problem p;
    public MainWindow()
    {
      InitializeComponent();
      canvas.loadImages();
      string[] cmdArgs = Environment.GetCommandLineArgs();
      if (cmdArgs.Count() == 2)
      {
        p = Helper.ReadProblemFromFile(cmdArgs[1]);
        List<Solution> s = Solver.getSolutions(p);
        Helper.WriteSolutionToFile(s);
        Close();
      }
      p = Helper.ReadProblemFromFile();
      canvas.board = p.board;
      List<Move> steps = new List<Move>();
      moveList.ItemsSource = steps;
    }

    private void solveButton_Click(object sender, RoutedEventArgs e)
    {
      List<Solution> s = Solver.getSolutions(p);
      solutionList.ItemsSource = s;
    }

    private void solutionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Solution s = (Solution)e.AddedItems[0];
      moveList.ItemsSource = s.Moves;
    }
  }
}
