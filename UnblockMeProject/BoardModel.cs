using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;

namespace UnblockMeProject
{
    public class BoardModel
    {
        public Dictionary<string, string> occupiedPositions;

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

        public bool IsMoveValidRec(int row, int col, int span, bool isHorizontal)
        {
            if (isHorizontal)
                for (int i = col; i > col - span; i--)
                {
                    string key = $"{row},{i}";
                    if (occupiedPositions.ContainsKey(key))
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
        public void PrintOccupiedPos()
        {
            foreach (var position in occupiedPositions)
            {
                Console.WriteLine($"Position: {position.Key}, Color: {position.Value}");
            }
        }
        public int GetRed()
        {
            int max = -1;
            foreach (var position in occupiedPositions)
            {
                if (position.Value.Contains("Red"))
                {
                    string[] numbers = position.Key.Split(',');
                    int colNumber = int.Parse(numbers[1]);

                    if (colNumber > max)
                        max = colNumber;
                }
            }
            return max;
        }
        public (int[],string) GetBlock(int row, int col)
        {
            int span = 0;
            int xCount = 0;
            int yCount = 0;
            int[] x = new int[4];
            int[] y = new int[4];
            string name = "";
            foreach (var position in occupiedPositions)
            {
                string key = row + "," + col;
                if (position.Key == key)
                {
                    name = position.Value;
                    break;
                }
            }
            foreach (var position in occupiedPositions)
            {
                if (position.Value == name)
                {
                    span++;
                    string[] numbers = position.Key.Split(',');
                    int rowNumber = int.Parse(numbers[0]);
                    int colNumber = int.Parse(numbers[1]);
                    x[xCount++] = colNumber;
                    y[yCount++] = rowNumber;

                }
            }
            int[] Return = new int[4];
            if (y[0] == y[1]) // not horizontal!
            {
                Return[3] = 1;
                Return[1] = x.Max();
                Return[0] = y.Max();
                Return[2] = span;
            }
            else
            {
                Return[3] = 0;
                Return[1] = x.Max();
                Return[0] = y.Max();
                Return[2] = span;
            }
            return (Return , name);
        }
        // method that will return a list of all of the rectangles on the board using the getblock format
        public List<(int[] , string)> GetAllBlocks()
        {
            List<(int[] , string)> blocks = new List<(int[] , string)>();
            HashSet<string> visited = new HashSet<string>();

            foreach (var position in occupiedPositions)
            {
                if (!visited.Contains(position.Key))
                {
                    string[] coordinates = position.Key.Split(',');
                    int row = int.Parse(coordinates[0]);
                    int col = int.Parse(coordinates[1]);

                    (int[] blockData , string blockName) = GetBlock(row, col);
                    blocks.Add((blockData , blockName));

                    // Mark all parts of the block as visited
                    for (int i = 0; i < blockData[2]; i++)
                    {
                        if (blockData[3] == 1) // Horizontal
                        {
                            visited.Add($"{row},{col + i}");
                        }
                        else // Vertical
                        {
                            visited.Add($"{row + i},{col}");
                        }
                    }
                }
            }
            return blocks;
        } 


    }
}