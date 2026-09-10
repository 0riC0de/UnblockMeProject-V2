using Priority_Queue;
using System;
using System.Collections.Generic;

namespace UnblockMeProject
{
    public class AStarSearcher
    {
        public GameState Solver(GameState gameState, int depth)
        {
            SimplePriorityQueue<GameState, int> openSet = new SimplePriorityQueue<GameState, int>();
            Dictionary<GameState, int> gScore = new Dictionary<GameState, int>();
            HashSet<GameState> closedSet = new HashSet<GameState>();
            Dictionary<GameState, GameState> canonicalOpenNodes = new Dictionary<GameState, GameState>();

            int weight = Math.Max(1, depth);

            gameState.CalculateCost();
            gScore[gameState] = 0;
            openSet.Enqueue(gameState, weight * gameState.Cost);
            canonicalOpenNodes[gameState] = gameState;

            while (openSet.Count > 0)
            {
                GameState current = openSet.Dequeue();
                canonicalOpenNodes.Remove(current);

                if (closedSet.Contains(current))
                    continue;

                closedSet.Add(current);

                if (current.IsGoal())
                {
                    // If Red is not yet at col 5, advance step by step to the exit
                    while (current.State.GetRed() < 5)
                    {
                        GameState next = null;
                        foreach (var succ in current.GetSuccessorStates())
                        {
                            if (succ.State.GetRed() > current.State.GetRed())
                            {
                                next = succ;
                                break;
                            }
                        }
                        if (next != null)
                            current = next;
                        else
                            break;
                    }
                    return current;
                }

                int currentG = gScore[current];
                foreach (var neighbor in current.GetSuccessorStates())
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    int tentativeGScore = currentG + 1;
                    int currentNeighborGScore = gScore.TryGetValue(neighbor, out int knownScore) ? knownScore : int.MaxValue;

                    if (tentativeGScore < currentNeighborGScore)
                    {
                        gScore[neighbor] = tentativeGScore;
                        int neighborFScore = tentativeGScore + weight * neighbor.Cost;

                        if (canonicalOpenNodes.TryGetValue(neighbor, out GameState existing))
                        {
                            existing.Previous = current;
                            openSet.UpdatePriority(existing, neighborFScore);
                        }
                        else
                        {
                            canonicalOpenNodes[neighbor] = neighbor;
                            openSet.Enqueue(neighbor, neighborFScore);
                        }
                    }
                }
            }
            return null;
        }
    }
}
