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
        public BoardModel boardModel;

        public MainWindow()
        {
            InitializeComponent();
            boardModel = new BoardModel();
            redBlock = new RedBlock(GameBoard , this);
            //regularBlock = new RegularBlock(GameBoard, 4, 1, 1, 3, true);
            regularBlock2 = new RegularBlock(GameBoard, 3, 2, 3, 1, false , this);
            boardModel.AddBlock(3, 3, "BlueNotHorizontal");
            boardModel.AddBlock(4, 3, "BlueNotHorizontal");
            boardModel.AddBlock(5, 3, "BlueNotHorizontal");
            boardModel.AddBlock(2, 0, "red");
            boardModel.AddBlock(2, 1, "red");


        }
        public void OnBlockMove(int newRow, int newCol, string color, bool isForward , int span)
        {
            if (boardModel.IsMoveValid(newRow, newCol))
            {
                // Update the board model with the new position
                boardModel.AddBlock(newRow, newCol, color);
                // Remove the old position if necessary
                //red
                if (color == "red" && isForward)
                {
                    boardModel.RemoveBlock(newRow, newCol - span);
                }
                else if (color == "red" &&  !isForward)
                {
                    boardModel.RemoveBlock(newRow, newCol + span);
                }
                //BlueHorizontal
                else if (color == "BlueHorizontal" && isForward)
                {
                    boardModel.RemoveBlock(newRow, newCol - span);
                }
                else if (color == "BlueHorizontal" && !isForward)
                {
                    boardModel.RemoveBlock(newRow, newCol + span);
                }
                //BlueNotHorizontal
                else if (color == "BlueNotHorizontal" && isForward)
                {
                    boardModel.RemoveBlock(newRow, newCol - span);
                }
                else if (color == "BlueNotHorizontal" && !isForward)
                {
                    boardModel.RemoveBlock(newRow, newCol + span);
                }
            }
            else
            {
                // Handle invalid move
                
            }
        }
    }
}
