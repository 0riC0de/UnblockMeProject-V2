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
        // for readability of the format from the GetBlock Func 
        private const int X = 0;
        private const int Y = 1;
        private const int Span = 2;
        private const int IsHorizontal = 3;

        private BoardModel State;
        private GameState Previous {  get; set; }
        private double Cost { get; set; }



        public void CalculateCost()
        {
            this.Cost = 0;
            bool checkd = false; //this bool checks if i can just make a move that makes the cost less 
            int start = State.GetRed(); // gives the last colunm of red rec
            for (int i = start + 1; i <= 5; i++)
            {
                if (!State.IsMoveValid(2, i))      // put 2 because thats the row of red
                {
                    this.Cost++;
                    // gives the block if given one position. output if horizontal arr[Max(x) , y , span , 1 -horizontal]
                    // output if NotHorizontal arr[x , Max(y) , span , 0 - Nothorizontal]
                    int[] Block = State.GetBlock(2, i);
                    if (Block[3] == 0) // not horizontal because red cant be blocked by a horizontal rec!
                    {
                        if (Block[2] == 3) // check if the block can even clear going down!
                        {
                            if (!State.IsMoveValid(Block[X] + 1, Block[Y]))
                            { 
                                this.Cost++;
                                checkd = true;
                            }
                            if (!State.IsMoveValid(5, Block[Y]) && Block[X] + 1 != 5)
                                this.Cost++;
                        }
                        if (Block[2] < 3) //check if the block can even clear going up!
                        {
                            if (!State.IsMoveValid(Block[X] - Block[Span], Block[Y]))
                            {
                                this.Cost++;
                            }
                            else
                                if (checkd)
                                    { Cost--; }
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

        // will need to change it and make the name id's!!!!!!!! IMPORTANT
        public List<GameState> GetSuccessorStates()
        {
            GameState gameState = this;
            // gets a list of arrays with all of the blocks like the GetBlock format
            List<int[]> recs = gameState.State.GetAllBlocks();

            List<GameState> Seccessors = new List<GameState>();
            foreach (int[] rec in recs)
            {
                if (rec[IsHorizontal] == 0) // NotHorizontal
                {
                    if (gameState.State.IsMoveValid(rec[X] + 1, rec[Y]) && rec[X] + 1 <= 5) // Can go down?
                    {
                        GameState newState = new GameState();
                        newState.State = new BoardModel();
                        foreach (var position in gameState.State.occupiedPositions)
                        {
                            newState.State.occupiedPositions.Add(position.Key, position.Value);
                        }
                        newState.State.AddBlock(rec[X] + 1, rec[Y], "Blue"); // will need to change it and make the name id's!!
                        newState.State.RemoveBlock(rec[X] - rec[Span] + 1, rec[Y]);
                        newState.CalculateCost(); // calc the cost
                        newState.Previous = gameState; // set previous
                        Seccessors.Add(newState);
                    }
                    if (gameState.State.IsMoveValid(rec[X] - rec[Span], rec[Y]) && rec[X] - rec[Span] >= 0) // Can go up?
                    {
                        GameState newState = new GameState();
                        newState.State = new BoardModel();
                        foreach (var position in gameState.State.occupiedPositions)
                        {
                            newState.State.occupiedPositions.Add(position.Key, position.Value);
                        }
                        newState.State.AddBlock(rec[X] - rec[Span], rec[Y], "Blue"); // will need to change it and make the name id's!!
                        newState.State.RemoveBlock(rec[X], rec[Y]);
                        newState.CalculateCost(); // calc the cost
                        newState.Previous = gameState; // set previous
                        Seccessors.Add(newState);
                    }
                }
                else
                {
                    string name = "";
                    if (rec[X] == 2)
                        name = "Red";
                    else
                        name = "BlueH";

                    if (gameState.State.IsMoveValid(rec[X], rec[Y] + 1) && rec[Y] + 1 <= 5) // can go right?
                    {
                        GameState newState = new GameState();
                        newState.State = new BoardModel();
                        foreach (var position in gameState.State.occupiedPositions)
                        {
                            newState.State.occupiedPositions.Add(position.Key, position.Value);
                        }
                        newState.State.AddBlock(rec[X], rec[Y] + 1, name); // will need to change it and make the name id's!!
                        newState.State.RemoveBlock(rec[X], rec[Y] - rec[Span] + 1);
                        newState.CalculateCost(); // calc the cost
                        newState.Previous = gameState; // set previous
                        Seccessors.Add(newState);
                    }
                    if (gameState.State.IsMoveValid(rec[X], rec[Y] - rec[Span]) && rec[Y] - rec[Span] >= 0) // can go left?
                    {
                        GameState newState = new GameState();
                        newState.State = new BoardModel();
                        foreach (var position in gameState.State.occupiedPositions)
                        {
                            newState.State.occupiedPositions.Add(position.Key, position.Value);
                        }
                        newState.State.AddBlock(rec[X], rec[Y] - rec[Span], name); // will need to change it and make the name id's!!
                        newState.State.RemoveBlock(rec[X], rec[Y]);
                        newState.CalculateCost(); // calc the cost
                        newState.Previous = gameState; // set previous
                        Seccessors.Add(newState);
                    }
                }
            }
            return Seccessors;
        }

    }

}
