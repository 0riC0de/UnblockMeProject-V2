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
using System.Windows.Threading;

namespace UnblockMeProject
{

    public partial class MainWindow : Window
    {
        private RedBlock redBlock;
        private RegularBlock regularBlock;
        private RegularBlock regularBlock2;
        private RegularBlock regularBlock3;
        private List<GameState> _GameStates;
        private DispatcherTimer _timer;
        private int _currentIndex = 0;
        public BoardModel boardModel;
        public GameState gameState;
        public MainWindow()
        {
            InitializeComponent();
            boardModel = new BoardModel();

            // Create the dialog
            BoardSelectionDialog dialog = new BoardSelectionDialog();

            // Show the dialog and wait for user to close it
            bool? result = dialog.ShowDialog();

            // Check if user clicked OK or Cancel
            if (result != true)
            {
                // User clicked Cancel or closed the window
                MessageBox.Show("No board selected. The application will exit.");
                Application.Current.Shutdown();
                return;
            }

            // NOW it's safe to access the selected values
            int option = dialog.SelectedBoard;
            int depth = dialog.SelectedDepth;

            // Continue with your existing code
            AStarSearcher solver = new AStarSearcher();

            // Rest of your code for board setup...
        
            if (option == 1)
            {
                boardModel.AddBlock(2, 2, "Blue_0");
                boardModel.AddBlock(3, 2, "Blue_0");
                boardModel.AddBlock(4, 2, "Blue_0");

                boardModel.AddBlock(2, 3, "Blue_2");
                boardModel.AddBlock(3, 3, "Blue_2");

                boardModel.AddBlock(4, 3, "BlueH_3");
                boardModel.AddBlock(4, 4, "BlueH_3");

                boardModel.AddBlock(1, 3, "BlueH_4");
                boardModel.AddBlock(1, 4, "BlueH_4");

                boardModel.AddBlock(5, 1, "BlueH_1");
                boardModel.AddBlock(5, 2, "BlueH_1");
                boardModel.AddBlock(5, 3, "BlueH_1");    // Blue -> color , H -> Horizontal , _1 -> id

                boardModel.AddBlock(1, 1, "BlueH_5");
                boardModel.AddBlock(1, 2, "BlueH_5");

                boardModel.AddBlock(4, 0, "Blue_6");
                boardModel.AddBlock(5, 0, "Blue_6");

                boardModel.AddBlock(2, 0, "Red");
                boardModel.AddBlock(2, 1, "Red");
            }

            if (option == 2)
            {
                
                boardModel.AddBlock(2, 1, "Red");
                boardModel.AddBlock(2, 2, "Red");

                boardModel.AddBlock(0, 1, "BlueH_0");
                boardModel.AddBlock(0, 2, "BlueH_0");

                boardModel.AddBlock(1, 0, "BlueH_1");
                boardModel.AddBlock(1, 1, "BlueH_1");

                boardModel.AddBlock(3, 0, "BlueH_2");
                boardModel.AddBlock(3, 1, "BlueH_2");

                boardModel.AddBlock(5, 3, "BlueH_3");
                boardModel.AddBlock(5, 4, "BlueH_3");
                boardModel.AddBlock(5, 5, "BlueH_3");

                boardModel.AddBlock(2, 3, "Blue_4");
                boardModel.AddBlock(3, 3, "Blue_4");
                boardModel.AddBlock(4, 3, "Blue_4");

                boardModel.AddBlock(2, 4, "Blue_5");
                boardModel.AddBlock(3, 4, "Blue_5");
                boardModel.AddBlock(4, 4, "Blue_5");

                boardModel.AddBlock(2, 5, "Blue_6");
                boardModel.AddBlock(3, 5, "Blue_6");
                boardModel.AddBlock(4, 5, "Blue_6");

                boardModel.AddBlock(0, 3, "Blue_7");
                boardModel.AddBlock(1, 3, "Blue_7");

                boardModel.AddBlock(4, 0, "Blue_8");
                boardModel.AddBlock(5, 0, "Blue_8");

                boardModel.AddBlock(3, 2, "Blue_9");
                boardModel.AddBlock(4, 2, "Blue_9");
                
            }

            if (option == 3)
            {

                boardModel.AddBlock(2, 1, "Red");
                boardModel.AddBlock(2, 2, "Red");

                boardModel.AddBlock(0, 0, "BlueH_0");
                boardModel.AddBlock(0, 1, "BlueH_0");

                boardModel.AddBlock(1, 3, "BlueH_1");
                boardModel.AddBlock(1, 4, "BlueH_1");

                boardModel.AddBlock(3, 0, "BlueH_2");
                boardModel.AddBlock(3, 1, "BlueH_2");

                boardModel.AddBlock(4, 3, "BlueH_3");
                boardModel.AddBlock(4, 4, "BlueH_3");

                boardModel.AddBlock(5, 0, "BlueH_4");
                boardModel.AddBlock(5, 1, "BlueH_4");

                boardModel.AddBlock(2, 3, "Blue_5");
                boardModel.AddBlock(3, 3, "Blue_5");
                boardModel.AddBlock(4, 3, "Blue_5");

                boardModel.AddBlock(2, 5, "Blue_6");
                boardModel.AddBlock(3, 5, "Blue_6");
                boardModel.AddBlock(4, 5, "Blue_6");
            }

            if (option == 4)
            {
                boardModel.AddBlock(2, 0, "Red");
                boardModel.AddBlock(2, 1, "Red");

                boardModel.AddBlock(3, 0, "BlueH_0");
                boardModel.AddBlock(3, 1, "BlueH_0");
                boardModel.AddBlock(3, 2, "BlueH_0");

                boardModel.AddBlock(0, 2, "Blue_1");
                boardModel.AddBlock(1, 2, "Blue_1");
                boardModel.AddBlock(2, 2, "Blue_1");

                boardModel.AddBlock(0, 5, "Blue_2");
                boardModel.AddBlock(1, 5, "Blue_2");
                boardModel.AddBlock(2, 5, "Blue_2");

            }
            if (option == 5)
            {
                boardModel.AddBlock(2, 0, "Red");
                boardModel.AddBlock(2, 1, "Red");

                boardModel.AddBlock(0, 3, "Blue_1");
                boardModel.AddBlock(1, 3, "Blue_1");
                boardModel.AddBlock(2, 3, "Blue_1");

                boardModel.AddBlock(3, 1, "BlueH_3");
                boardModel.AddBlock(3, 2, "BlueH_3");
                boardModel.AddBlock(3, 3, "BlueH_3");

                boardModel.AddBlock(3, 0, "Blue_4");
                boardModel.AddBlock(4, 0, "Blue_4");
                boardModel.AddBlock(5, 0, "Blue_4");

                boardModel.AddBlock(0, 0, "BlueH_5");
                boardModel.AddBlock(0, 1, "BlueH_5");

                boardModel.AddBlock(4, 4, "BlueH_6");
                boardModel.AddBlock(4, 5, "BlueH_6");
            }
            if (option == 6)
            {               
                boardModel.AddBlock(2, 1, "Red");
                boardModel.AddBlock(2, 2, "Red");

                boardModel.AddBlock(1, 0, "Blue_0");
                boardModel.AddBlock(2, 0, "Blue_0");

                boardModel.AddBlock(4, 5, "Blue_1");
                boardModel.AddBlock(5, 5, "Blue_1");

                boardModel.AddBlock(0, 2, "Blue_3");
                boardModel.AddBlock(1, 2, "Blue_3");

                boardModel.AddBlock(4, 0, "Blue_4");
                boardModel.AddBlock(5, 0, "Blue_4");

                boardModel.AddBlock(0, 3, "BlueH_5");
                boardModel.AddBlock(0, 4, "BlueH_5");
                boardModel.AddBlock(0, 5, "BlueH_5");

                boardModel.AddBlock(5, 1, "BlueH_6");
                boardModel.AddBlock(5, 2, "BlueH_6");
                boardModel.AddBlock(5, 3, "BlueH_6");

                boardModel.AddBlock(1, 5, "BlueH_7");
                boardModel.AddBlock(1, 4, "BlueH_7");

                boardModel.AddBlock(3, 4, "BlueH_8");
                boardModel.AddBlock(3, 5, "BlueH_8");

                boardModel.AddBlock(1, 3, "Blue_9");
                boardModel.AddBlock(2, 3, "Blue_9");
                boardModel.AddBlock(3, 3, "Blue_9");

                boardModel.AddBlock(4, 2, "BlueH_10");
                boardModel.AddBlock(4, 3, "BlueH_10");

                boardModel.AddBlock(4, 0, "Blue_2");
                boardModel.AddBlock(5, 0, "Blue_2");

            }






            gameState = new GameState();
            gameState.initializeState(boardModel);
            gameState.CalculateCost();
            gameState = solver.Solver(gameState , depth);
            List<GameState> states = gameState.ShowPath();
           


           StartVisualization(states);

        }
        public bool OnBlockMove(int newRow, int newCol, string color, int span, bool isHorizontal)
        {
            boardModel.PrintOccupiedPos();
            if (boardModel.IsMoveValidRec(newRow, newCol, span, isHorizontal))
            {
                Console.WriteLine(newCol + ", " + newRow);
                // Remove the old position if necessary
                //red
                if (color.Contains("Red"))
                {
                    RemoveRec(newRow, newCol, span, isHorizontal);
                    for (int i = newCol; i > newCol - span; i--)
                    {
                        boardModel.AddBlock(newRow, i, "Red");
                    }
                }
                //BlueHorizontal
                else if (color.Contains("Blue") && isHorizontal)
                {
                    RemoveRec(newRow, newCol, span, isHorizontal);

                    for (int i = newCol; i > newCol - span; i--)
                    {
                        boardModel.AddBlock(newRow, i, color);
                    }
                }
                //BlueNotHorizontal
                else if (color.Contains("Blue"))
                {
                    RemoveRec(newRow, newCol, span, isHorizontal);
                    for (int i = newRow; i < newRow + span; i++)
                    {
                        boardModel.AddBlock(i, newCol, color);
                    }
                }
            }
            else
            {
                // Handle invalid move
                Console.WriteLine("RecNotValid");
                return false;
            }
            return true;
        }
        public void RemoveRec(int row, int col, int span, bool isHorizontal)
        {
            if (!isHorizontal)
                for (int i = row; i < row + span; i++)
                    boardModel.RemoveBlock(i, col);
            else
                for (int i = col; i < col + span; i++)
                    boardModel.RemoveBlock(row, i);
        }
        public void StartVisualization(List<GameState> gameStates)
        {
            _currentIndex = gameStates.Count - 1;
            _GameStates = gameStates;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Change the interval if you want it faster or slower
            };

            _timer.Tick += UpdateBoard;
            _timer.Start();
        }
        private void UpdateBoard(object sender, EventArgs e)
        {
            if (_currentIndex < 0)
            {
                _timer.Stop(); // Stop the timer if we are at the end
                return;
            }

            // Clear the grid and draw the next state
            GameBoard.Children.Clear();
            _GameStates[_currentIndex].State.DrawBoard(GameBoard, this);

            _currentIndex--;
        }

    }
}
