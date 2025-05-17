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

        public BoardModel State;
        private GameState Previous { get; set; }
        public int Cost { get; set; }



        // gives the block if given one position. output if horizontal arr[Max(x) , y , span , 1 -horizontal]
        // output if NotHorizontal arr[max(y) , x , span , 0 - Nothorizontal]
        public void CalculateCost()
        {
            this.Cost = 0;
            int start = State.GetRed(); // gives the last column of red rec

            // HashSet to track visited blocks to prevent counting the same block multiple times
            HashSet<string> visitedBlocks = new HashSet<string>();

            // Count blocks directly blocking red
            int directBlockersCount = 0;
            int manhattanDistance = 5 - (start + 1);
            this.Cost += manhattanDistance/2;
            for (int i = start + 1; i <= 5; i++)
            {
                if (!State.IsMoveValid(2, i)) // 2 is the row of red
                {
                    directBlockersCount++;
                    // Get the block at this position
                    (int[] block, string blockName) = State.GetBlock(2, i);
                    visitedBlocks.Add(blockName);

                    if (block[3] == 0) // Vertical block blocking red
                    {
                        // First layer Less Priority
                        this.Cost += 5;

                        // Check recursive blockage
                        ExamineBlockMobility(block, blockName, visitedBlocks, 1);
                    }
                }
            }

            // Base cost on direct blockers - minimum cost is always at least equal to direct blockers
            this.Cost = Math.Max(this.Cost, directBlockersCount);
        }

        private void ExamineBlockMobility(int[] block, string blockName, HashSet<string> visitedBlocks, int depth)
        {
            if (depth >= 7) // Maximum depth of 7
                return;

            int mobilityScore = 2; // Start with assumption of mobility

            if (block[3] == 0) // Vertical block
            {
                bool canMoveUp = false;
                bool canMoveDown = false;

                // Check if the block can move down
                if (block[X] + 1 <= 5) // Not at the bottom edge
                {
                    if (State.IsMoveValid(block[X] + 1, block[Y]))
                    {
                        canMoveDown = true;
                    }
                    else
                    {
                        // Calculate cost for the downward blocker
                        (int[] blockerBlock, string blockerName) = State.GetBlock(block[X] + 1, block[Y]);

                        if (!visitedBlocks.Contains(blockerName))
                        {
                            visitedBlocks.Add(blockerName);
                            this.Cost += depth; // Higher weight for blocks at lower depths
                            ExamineBlockMobility(blockerBlock, blockerName, visitedBlocks, depth + 1);
                        }
                    }
                }

                // Check if the block can move up
                if (block[X] - block[Span] >= 0) // Not at the top edge
                {
                    if (State.IsMoveValid(block[X] - block[Span], block[Y]))
                    {
                        canMoveUp = true;
                    }
                    else
                    {
                        // Calculate cost for the upward blocker
                        (int[] blockerBlock, string blockerName) = State.GetBlock(block[X] - block[Span], block[Y]);

                        if (!visitedBlocks.Contains(blockerName))
                        {
                            visitedBlocks.Add(blockerName);
                            this.Cost += depth;
                            ExamineBlockMobility(blockerBlock, blockerName, visitedBlocks, depth + 1);
                        }
                    }
                }

                // Adjust mobility score based on freedom of movement
                if (canMoveUp || canMoveDown)
                {
                    mobilityScore = 0; // Good mobility
                }
                else
                {
                    mobilityScore = 3; // Poor mobility
                }
            }
            else // Horizontal block
            {
                bool canMoveLeft = false;
                bool canMoveRight = false;

                // Check if the block can move left
                if (block[Y] - 1 >= 0) // Not at the left edge
                {
                    if (State.IsMoveValid(block[X], block[Y] - 1))
                    {
                        canMoveLeft = true;
                    }
                    else
                    {
                        // Calculate cost for the leftward blocker
                        (int[] blockerBlock, string blockerName) = State.GetBlock(block[X], block[Y] - 1);

                        if (!visitedBlocks.Contains(blockerName))
                        {
                            visitedBlocks.Add(blockerName);
                            this.Cost += depth;
                            ExamineBlockMobility(blockerBlock, blockerName, visitedBlocks, depth + 1);
                        }
                    }
                }

                // Check if the block can move right
                if (block[Y] + block[Span] <= 5) // Not at the right edge
                {
                    if (State.IsMoveValid(block[X], block[Y] + block[Span]))
                    {
                        canMoveRight = true;
                    }
                    else
                    {
                        // Calculate cost for the rightward blocker
                        (int[] blockerBlock, string blockerName) = State.GetBlock(block[X], block[Y] + block[Span]);

                        if (!visitedBlocks.Contains(blockerName))
                        {
                            visitedBlocks.Add(blockerName);
                            this.Cost +=  depth;
                            ExamineBlockMobility(blockerBlock, blockerName, visitedBlocks, depth + 1);
                        }
                    }
                }

                // Adjust mobility score based on freedom of movement
                if (canMoveLeft || canMoveRight)
                {
                    mobilityScore = 0; // Good mobility
                }
                else
                {
                    mobilityScore = 3; // Poor mobility
                }
            }

            // Add mobility score to total cost
            this.Cost += mobilityScore;
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
            // gets a list of tuples with (blockData, blockName)
            List<(int[], string)> recs = gameState.State.GetAllBlocks();

            List<GameState> Seccessors = new List<GameState>();
            foreach ((int[] rec, string blockName) in recs)
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
                        newState.State.AddBlock(rec[X] + 1, rec[Y], blockName); // Use the real name!
                        newState.State.RemoveBlock(rec[X] - rec[Span] + 1, rec[Y]);
                        newState.CalculateCost();
                        newState.Previous = gameState;
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
                        newState.State.AddBlock(rec[X] - rec[Span], rec[Y], blockName); // Use the real name!
                        newState.State.RemoveBlock(rec[X], rec[Y]);
                        newState.CalculateCost();
                        newState.Previous = gameState;
                        Seccessors.Add(newState);
                    }
                }
                else
                {
                    if (gameState.State.IsMoveValid(rec[X], rec[Y] + 1) && rec[Y] + 1 <= 5) // can go right?
                    {
                        GameState newState = new GameState();
                        newState.State = new BoardModel();
                        foreach (var position in gameState.State.occupiedPositions)
                        {
                            newState.State.occupiedPositions.Add(position.Key, position.Value);
                        }
                        newState.State.AddBlock(rec[X], rec[Y] + 1, blockName); // Use the real name!
                        newState.State.RemoveBlock(rec[X], rec[Y] - rec[Span] + 1);
                        newState.CalculateCost();
                        newState.Previous = gameState;
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
                        newState.State.AddBlock(rec[X], rec[Y] - rec[Span], blockName); // Use the real name!
                        newState.State.RemoveBlock(rec[X], rec[Y]);
                        newState.CalculateCost();
                        newState.Previous = gameState;
                        Seccessors.Add(newState);
                    }
                }
            }
            return Seccessors;
        }
        public List<GameState> ShowPath()
        {
            GameState gameState = this;
            List<GameState> states = new List<GameState>();
            while (gameState.Previous != null)
            {
                states.Add(gameState);
                gameState = gameState.Previous;
            }
            states.Add(gameState);
            return states;
        }
    }
}