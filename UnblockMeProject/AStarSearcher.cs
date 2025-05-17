
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
            gScore[gameState] = 0;
            var cameFrom = new Dictionary<GameState, GameState>();
            openSet.Enqueue(gameState, (int)gameState.Cost);
            int count = gameState.State.occupiedPositions.Count;
            HashSet<GameState> closedSet = new HashSet<GameState>();

            GameState current = new GameState();

            while (openSet.Count() > 0)
            {
                current = openSet.Dequeue();

                if (current.Cost <= 0)
                {
                    Console.WriteLine("Goal Found! Path reconstruction needed.");
                    Console.WriteLine(closedSet.Count);
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (var state in current.GetSuccessorStates())
                        {
                            int red = state.State.GetRed();
                            if (red > current.State.GetRed())
                            {
                                current = state;
                            }
                        }
                    }
                    return current;
                }
                closedSet.Add(current);

                // Generate triple-depth successors
                if (count > 24)
                {
                    foreach (var neighbor in TripleDepthSuccessors(current))
                    {
                        if (closedSet.Contains(neighbor))
                        {
                            continue;
                        }

                        int tentativeGScore = gScore[current] + 1;
                        int currentNeighborGScore = gScore.TryGetValue(neighbor, out int knownScore) ? knownScore : int.MaxValue;

                        if (tentativeGScore < currentNeighborGScore)
                        {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = tentativeGScore;
                            int neighborFScore = tentativeGScore + (int)neighbor.Cost;
                            openSet.Enqueue(neighbor, neighborFScore);
                        }
                    }
                }
                else if(count > 15)
                {
                    foreach (var neighbor in DoubleDepthSuccessors(current))
                    {
                        if (closedSet.Contains(neighbor))
                        {
                            continue;
                        }

                        int tentativeGScore = gScore[current] + 1;
                        int currentNeighborGScore = gScore.TryGetValue(neighbor, out int knownScore) ? knownScore : int.MaxValue;

                        if (tentativeGScore < currentNeighborGScore)
                        {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = tentativeGScore;
                            int neighborFScore = tentativeGScore + (int)neighbor.Cost;
                            openSet.Enqueue(neighbor, neighborFScore);
                        }
                    }
                }
                else if(count <= 15)
                {
                    foreach (var neighbor in current.GetSuccessorStates())
                    {
                        if (closedSet.Contains(neighbor))
                        {
                            continue;
                        }

                        int tentativeGScore = gScore[current] + 1;
                        int currentNeighborGScore = gScore.TryGetValue(neighbor, out int knownScore) ? knownScore : int.MaxValue;

                        if (tentativeGScore < currentNeighborGScore)
                        {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = tentativeGScore;
                            int neighborFScore = tentativeGScore + (int)neighbor.Cost;
                            openSet.Enqueue(neighbor, neighborFScore);
                        }
                    }
                }
            }
            return null;
        }

        // Triple depth successor generation
        private List<GameState> TripleDepthSuccessors(GameState gameState)
        {
            List<GameState> tripleSuccessors = new List<GameState>();

            foreach (var first in gameState.GetSuccessorStates())
            {
                foreach (var second in first.GetSuccessorStates())
                {
                    foreach (var third in second.GetSuccessorStates())
                    {
                        tripleSuccessors.Add(third);
                    }
                }
            }
            return tripleSuccessors;
        }

        private List<GameState> DoubleDepthSuccessors(GameState gameState)
        {
            List<GameState> doubleSuccessors = new List<GameState>();

            foreach (var first in gameState.GetSuccessorStates())
            {
                foreach (var second in first.GetSuccessorStates())
                {
                    doubleSuccessors.Add(second);
                }
            }
            return doubleSuccessors;
        }
    }
}

