using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnblockMeProject
{
    public class BoardModel
    {
        private Dictionary<string, string> occupiedPositions;

        public BoardModel()
        {
            occupiedPositions = new Dictionary<string, string>();
        }

        public void AddBlock(int row, int col, string color)
        {
            string key = $"{row},{col}";
            if (!occupiedPositions.ContainsKey(key))
            {
                occupiedPositions[key] = color;
            }
        }

        public void RemoveBlock(int row, int col)
        {
            string key = $"{row},{col}";
            if (occupiedPositions.ContainsKey(key))
            {
                occupiedPositions.Remove(key);
            }
        }

        public bool IsMoveValid(int newRow, int newCol)
        {
            string key = $"{newRow},{newCol}";
            return !occupiedPositions.ContainsKey(key);
        }

        public bool IsMoveValidRec(int row, int col , int span, bool isHorizontal)
        {
            if (isHorizontal)
                for (int i = col; i > col - span; i--)
                {
                    string key = $"{row},{i}";
                    if(occupiedPositions.ContainsKey(key))
                        return false;
                }
            else
                for (int i = row; i < row + span; i++)
                {
                    string key = $"{i},{col}";
                    if (occupiedPositions.ContainsKey(key))
                        return false;
                }
            return true;
        }
    }
}