using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace UnblockMeProject
{
    public class GameState
    {
        private BoardModel State;
        private GameState Previous;
        private double Cost { get; set; }



        public void CalculateCost()
        {
            this.Cost = 0;
            int start = State.GetRed(); // gives the last colunm of red rec
            for (int i = start + 1; i <= 5; i++)
            {
                if (!State.IsMoveValid(2, i))      // put 2 because thats the row of red
                {
                    this.Cost++;
                    // gives the block if given one position. output if horizontal arr[Max(x) , y , span , 1 -horizontal]
                    // output if NotHorizontal arr[x , Max(y) , span , 0 - horizontal]
                    int[] Block = State.GetBlock(2, i);
                    if (Block[3] == 0) // not horizontal because red cant be blocked by a horizontal rec!
                    {
                        if (Block[2] < 4) // check if the block can even clear going down!
                        {
                            if (!State.IsMoveValid(Block[0], Block[1] + 1))
                            { this.Cost++; }
                        }
                        if (Block[2] < 3) //check if the block can even clear going up!
                        {
                            if (!State.IsMoveValid(Block[0], Block[1] - Block[2]))
                            { this.Cost++; }
                        }
                    }
                }

            }
        }
        public void initializeState(BoardModel state)
        {
            this.State = state;
        }
        public void initializePrevious(GameState prev)
        {
            this.Previous = prev;
        }
    }
    
}
