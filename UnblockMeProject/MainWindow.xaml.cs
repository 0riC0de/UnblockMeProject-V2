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
            //regularBlock = new RegularBlock(GameBoard, 4, 1, 1, 3, true , this);
            regularBlock2 = new RegularBlock(GameBoard, 3, 2, 3, 1, false , this);
            boardModel.AddBlock(3, 2, "Blue");
            boardModel.AddBlock(4, 2, "Blue");
            boardModel.AddBlock(5, 2, "Blue");
            boardModel.AddBlock(2, 0, "Red");
            boardModel.AddBlock(2, 1, "Red");

        }
        public bool OnBlockMove(int newRow, int newCol, string color , int span , bool isHorizontal)
        {
           if (boardModel.IsMoveValidRec(newRow, newCol,span , isHorizontal))
            {
                Console.WriteLine(newCol +", " + newRow);
                // Remove the old position if necessary
                //red
                if (color == "Red")
                {
                    for (int i = newCol; i > newCol - span; i--)
                    {
                        boardModel.AddBlock(newRow, i, "Red");
                    }
                }
                //BlueHorizontal
                else if (color == "Blue" && isHorizontal)
                {
                    for (int i = newCol; i > newCol - span; i--)
                    {
                        boardModel.AddBlock(newRow, i, "Blue");
                    }
                }
                //BlueNotHorizontal
                else if (color == "Blue")
                {
                    for (int i = newRow; i < newRow + span; i++)
                    {
                        boardModel.AddBlock(i, newCol, "Blue");
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
