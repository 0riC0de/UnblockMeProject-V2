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
        public BoardModel boardModel;
        public GameState gameState;
        public MainWindow()
        {
            InitializeComponent();
            boardModel = new BoardModel();
            redBlock = new RedBlock(GameBoard , this);
            AStarSearcher solver = new AStarSearcher();
            regularBlock = new RegularBlock(GameBoard, 5, 1, 1, 3, true , this ,"BlueH_1");
            regularBlock2 = new RegularBlock(GameBoard, 2, 2, 3, 1, false , this , "Blue_0");
            regularBlock3 = new RegularBlock(GameBoard, 3, 3, 2, 1, false, this, "Blue_2");
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

            boardModel.AddBlock(2, 0, "Red");
            boardModel.AddBlock(2, 1, "Red");

            gameState = new GameState();
            gameState.initializeState(boardModel);
            gameState.CalculateCost();
            solver.Solver(gameState);
            

        }
        public bool OnBlockMove(int newRow, int newCol, string color , int span , bool isHorizontal)
        {
            boardModel.PrintOccupiedPos();
           if (boardModel.IsMoveValidRec(newRow, newCol,span , isHorizontal))
            {
                Console.WriteLine(newCol +", " + newRow);
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
        public void RemoveRec(int row, int col , int span , bool isHorizontal)
        {
            if (!isHorizontal)
                for (int i = row; i < row + span; i++) 
                    boardModel.RemoveBlock(i, col);
            else
                for (int i = col; i < col + span; i++)
                    boardModel.RemoveBlock(row, i);
        }
    }
}
