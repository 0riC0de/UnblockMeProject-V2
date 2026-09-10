using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

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
            {
                for (int i = col; i > col - span; i--)
                {
                    string key = $"{row},{i}";
                    if (occupiedPositions.ContainsKey(key))
                        return false;
                }
            }
            else
            {
                for (int i = row; i < row + span; i++)
                {
                    string key = $"{i},{col}";
                    if (occupiedPositions.ContainsKey(key))
                        return false;
                }
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

        public (int[], string) GetBlock(int row, int col)
        {
            string targetKey = $"{row},{col}";
            if (!occupiedPositions.TryGetValue(targetKey, out string name) || string.IsNullOrEmpty(name))
            {
                return (new int[4], "");
            }

            int span = 0;
            int maxRow = -1;
            int maxCol = -1;
            int minRow = 6;
            int minCol = 6;

            foreach (var position in occupiedPositions)
            {
                if (position.Value == name)
                {
                    span++;
                    int comma = position.Key.IndexOf(',');
                    int r = position.Key[0] - '0';
                    int c = position.Key[comma + 1] - '0';
                    if (r > maxRow) maxRow = r;
                    if (r < minRow) minRow = r;
                    if (c > maxCol) maxCol = c;
                    if (c < minCol) minCol = c;
                }
            }

            int[] Return = new int[4];
            bool isHoriz = (minRow == maxRow);
            Return[0] = maxRow;
            Return[1] = maxCol;
            Return[2] = span;
            Return[3] = isHoriz ? 1 : 0;

            return (Return, name);
        }

        public Dictionary<string, char> blockCharMap = new Dictionary<string, char>();

        public BoardModel Clone()
        {
            BoardModel clone = new BoardModel();
            clone.occupiedPositions = new Dictionary<string, string>(this.occupiedPositions);
            clone.blockCharMap = this.blockCharMap;
            return clone;
        }

        public string GetStateKey()
        {
            char[] g = new char[36];
            for (int i = 0; i < 36; i++) g[i] = '.';
            foreach (var kvp in occupiedPositions)
            {
                int comma = kvp.Key.IndexOf(',');
                int r = kvp.Key[0] - '0';
                int c = kvp.Key[comma + 1] - '0';
                int idx = r * 6 + c;
                if (idx >= 0 && idx < 36)
                {
                    if (kvp.Value.Contains("Red"))
                    {
                        g[idx] = 'R';
                    }
                    else
                    {
                        if (!blockCharMap.TryGetValue(kvp.Value, out char ch))
                        {
                            int count = blockCharMap.Count;
                            if (count < 26)
                                ch = (char)('a' + count);
                            else
                                ch = (char)('A' + (count - 26) + (count - 26 >= 17 ? 1 : 0)); // Skip 'R' if needed
                            blockCharMap[kvp.Value] = ch;
                        }
                        g[idx] = ch;
                    }
                }
            }
            return new string(g);
        }

        // method that will return a list of all of the rectangles on the board using the getblock format
        public List<(int[], string)> GetAllBlocks()
        {
            List<(int[], string)> blocks = new List<(int[], string)>();
            HashSet<string> visitedNames = new HashSet<string>();

            foreach (var position in occupiedPositions)
            {
                if (visitedNames.Add(position.Value))
                {
                    int comma = position.Key.IndexOf(',');
                    int row = position.Key[0] - '0';
                    int col = position.Key[comma + 1] - '0';

                    (int[] blockData, string blockName) = GetBlock(row, col);
                    blocks.Add((blockData, blockName));
                }
            }
            return blocks;
        }

        public void DrawBoard(Grid GameBoard, MainWindow window)
        {
            RedBlock redBlock;
            RegularBlock regularBlock;
            GameBoard.Children.Clear();
            List<(int[], string)> blocks = this.GetAllBlocks();
            foreach (var block in blocks)
            {
                if (block.Item2.Contains("Red"))
                {
                    redBlock = new RedBlock(GameBoard, window, block.Item1[1] - block.Item1[2] + 1);
                }
                else
                {
                    if (block.Item1[3] == 0) // Vertical
                        regularBlock = new RegularBlock(GameBoard, block.Item1[0] - block.Item1[2] + 1, block.Item1[1], block.Item1[2], 1, false, window, block.Item2);
                    else // Horizontal
                        regularBlock = new RegularBlock(GameBoard, block.Item1[0], block.Item1[1] - block.Item1[2] + 1, 1, block.Item1[2], true, window, block.Item2);
                }
            }
        }
    }
}