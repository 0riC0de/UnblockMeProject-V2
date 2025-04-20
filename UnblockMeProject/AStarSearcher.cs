
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;


namespace UnblockMeProject
{
    internal class AStarSearcher
    {
        public GameState Solver(GameState gameState)
        {
            SimplePriorityQueue<GameState> openSet = new SimplePriorityQueue<GameState>();
            Dictionary<GameState, int> gScore = new Dictionary<GameState, int>();
            gScore[gameState] = 0;// the g of the AStar! the h of the aStar is cost
            var cameFrom = new Dictionary<GameState, GameState>();
            openSet.Enqueue(gameState , (int)gameState.Cost);
            HashSet<GameState> closedSet = new HashSet<GameState>();

            GameState current = new GameState();

            while (openSet.Count() > 0)
            {
                    current = openSet.Dequeue();

                    if (current.Cost == 0) // Or IsGoal(successor)
                    {
                        // GOAL FOUND!
                        // Now you would call a function that runs on the previous untill null!
                        Console.WriteLine("Goal Found! Path reconstruction needed.");
                        return current; 
                    }
                    closedSet.Add((GameState)current);

                foreach (var neighbor in current.GetSuccessorStates())
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    int tentativeGScore = gScore[current] + 1;  
                    int currentNeighborGScore = gScore.TryGetValue(neighbor, out int knownScore) ? knownScore : int.MaxValue; //retrive the gScore (if exist)
                    if (tentativeGScore < currentNeighborGScore)
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore; // Update the best g-score for 'neighbor'
                        float neighborFScore = tentativeGScore + (int)neighbor.Cost;
                        openSet.Enqueue(neighbor, neighborFScore);
                    }

                }
            }
            return null;
        }
    }
}
