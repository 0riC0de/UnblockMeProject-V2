using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnblockMeProject
{
    public class GameState : IEquatable<GameState>
    {
        private const int X = 0;
        private const int Y = 1;
        private const int Span = 2;
        private const int IsHorizontal = 3;

        public BoardModel State { get; set; }
        public GameState Previous { get; set; }
        public int Cost { get; set; }

        private string _stateKey;
        private int _hashCode;
        private bool _hasHash;

        public string GetStateKey()
        {
            if (_stateKey == null && State != null)
            {
                _stateKey = State.GetStateKey();
            }
            return _stateKey;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GameState);
        }

        public bool Equals(GameState other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            return string.Equals(this.GetStateKey(), other.GetStateKey(), StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            if (!_hasHash)
            {
                _hashCode = GetStateKey() != null ? GetStateKey().GetHashCode() : 0;
                _hasHash = true;
            }
            return _hashCode;
        }

        public bool IsGoal()
        {
            if (State == null) return false;
            int red = State.GetRed();
            if (red >= 5) return true;
            for (int col = red + 1; col <= 5; col++)
            {
                if (!State.IsMoveValid(2, col))
                    return false;
            }
            return true;
        }

        public void CalculateCost()
        {
            if (State == null)
            {
                this.Cost = 0;
                return;
            }

            int red = State.GetRed();
            int h = Math.Max(0, 5 - red);
            HashSet<string> visitedBlockers = new HashSet<string>();

            for (int col = red + 1; col <= 5; col++)
            {
                if (!State.IsMoveValid(2, col))
                {
                    (int[] blockData, string blockName) = State.GetBlock(2, col);
                    if (visitedBlockers.Add(blockName))
                    {
                        h += 1;

                        if (blockData[IsHorizontal] == 0) // vertical blocker
                        {
                            int maxRow = blockData[X];
                            int span = blockData[Span];
                            int c = blockData[Y];

                            bool canMoveUp = (maxRow - span >= 0) && State.IsMoveValid(maxRow - span, c);
                            bool canMoveDown = (maxRow + 1 <= 5) && State.IsMoveValid(maxRow + 1, c);

                            if (!canMoveUp && !canMoveDown)
                            {
                                h += 1; // Double blocked obstacle
                            }
                        }
                    }
                }
            }
            this.Cost = h;
        }

        public void initializeState(BoardModel state)
        {
            this.State = state;
            this._stateKey = null;
            this._hasHash = false;
        }

        public void initializePrevious(GameState prev)
        {
            this.Previous = prev;
        }

        public List<GameState> GetSuccessorStates()
        {
            GameState gameState = this;
            List<(int[], string)> recs = gameState.State.GetAllBlocks();
            List<GameState> successors = new List<GameState>();

            foreach ((int[] rec, string blockName) in recs)
            {
                if (rec[IsHorizontal] == 0) // Vertical block
                {
                    // Can go down?
                    if (rec[X] + 1 <= 5 && gameState.State.IsMoveValid(rec[X] + 1, rec[Y]))
                    {
                        GameState newState = new GameState();
                        newState.State = gameState.State.Clone();
                        newState.State.AddBlock(rec[X] + 1, rec[Y], blockName);
                        newState.State.RemoveBlock(rec[X] - rec[Span] + 1, rec[Y]);
                        newState.CalculateCost();
                        newState.Previous = gameState;
                        successors.Add(newState);
                    }
                    // Can go up?
                    if (rec[X] - rec[Span] >= 0 && gameState.State.IsMoveValid(rec[X] - rec[Span], rec[Y]))
                    {
                        GameState newState = new GameState();
                        newState.State = gameState.State.Clone();
                        newState.State.AddBlock(rec[X] - rec[Span], rec[Y], blockName);
                        newState.State.RemoveBlock(rec[X], rec[Y]);
                        newState.CalculateCost();
                        newState.Previous = gameState;
                        successors.Add(newState);
                    }
                }
                else // Horizontal block
                {
                    // Can go right?
                    if (rec[Y] + 1 <= 5 && gameState.State.IsMoveValid(rec[X], rec[Y] + 1))
                    {
                        GameState newState = new GameState();
                        newState.State = gameState.State.Clone();
                        newState.State.AddBlock(rec[X], rec[Y] + 1, blockName);
                        newState.State.RemoveBlock(rec[X], rec[Y] - rec[Span] + 1);
                        newState.CalculateCost();
                        newState.Previous = gameState;
                        successors.Add(newState);
                    }
                    // Can go left?
                    if (rec[Y] - rec[Span] >= 0 && gameState.State.IsMoveValid(rec[X], rec[Y] - rec[Span]))
                    {
                        GameState newState = new GameState();
                        newState.State = gameState.State.Clone();
                        newState.State.AddBlock(rec[X], rec[Y] - rec[Span], blockName);
                        newState.State.RemoveBlock(rec[X], rec[Y]);
                        newState.CalculateCost();
                        newState.Previous = gameState;
                        successors.Add(newState);
                    }
                }
            }
            return successors;
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