using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PathFinding
{
    internal class AStar
    {
        private Grid grid;
        private Node currentNode;
        private Node targetNode;
        private Heap<Node> openSet;
        private HashSet<Node> closedSet = new HashSet<Node>();



        public AStar(Grid _grid)
        {
            grid = _grid;
            openSet = new Heap<Node>(grid.MaxSize);

        }

        private void Reset()
        {
            // Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            //  HashSet<Node> closedSet = new HashSet<Node>();
        }

        public List<Node> FindPath(Node _startNode, Node _targetNode)
        {
           
            if (currentNode == null && targetNode == null)
            {
                targetNode = _targetNode;
                Reset();
                openSet.Add(_startNode);
            }
                  
            if (openSet.Count > 0)
            {
                currentNode = openSet.RemoveFirst();               
                Console.WriteLine("Number of nodes in openSet " + openSet.Count);               
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    Game1.sw.Stop();
                    Console.WriteLine("path found in: " + Game1.sw.ElapsedMilliseconds + " ms");
                    grid.IsSearching = false;
                    Game1.sw.Reset();
                    return RetracePath(_startNode, targetNode);
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    int newMoveCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMoveCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMoveCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }          

            foreach (Node n in closedSet)
            {
                n.Color = Color.Purple;
            }
            for (int i = 0; i < openSet.GetCollection().Length; i++)
            {
                if (openSet.GetCollection()[i] == null)
                {
                    break;
                }
                openSet.GetCollection()[i].Color = Color.Violet;
            }
            Console.WriteLine("Found no path");
            return null;
        }

        private List<Node> RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currenNode = targetNode;
            while (currenNode != startNode)
            {
                path.Add(currenNode);
                currenNode = currenNode.Parent;
                currenNode.Color = Color.Blue;
            }
            path.Reverse();
            return path;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Math.Abs(nodeA.GridCoordX - nodeB.GridCoordX);
            int distY = Math.Abs(nodeA.GridCoordY - nodeB.GridCoordY);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);

            return 14 * distX + 10 * (distY - distX);
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = node.GridCoordX + x;
                    int checkY = node.GridCoordY + y;

                    if (checkX >= 0 && checkX < grid.gridSizeX && checkY >= 0 && checkY < grid.gridSizeY)
                        neighbours.Add(grid.nodes[checkY, checkX]);
                }
            }
            return neighbours;
        }
    }
}
