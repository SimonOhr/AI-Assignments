using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class AStar
    {        
        Grid grid;
        public AStar(Grid _grid)
        {
            grid = _grid;
        }

        public List<Node> FindPath(Node _startNode, Node _targetNode)
        {
            //Node startNode = grid.NodeFromWorldPoint(startPos);
            //Node targetNode = grid.NodeFromWorldPoint(targetPos);
            Node startNode = _startNode;
            Node targetNode = _targetNode;

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                Console.WriteLine("Number of nodes in openSet " + openSet.Count);
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    foreach (Node n in closedSet)
                    {
                        n.Color = Color.Purple;
                    }
                    foreach (Node n in openSet)
                    {
                        n.Color = Color.Purple;
                    }
                    grid.IsSearching = false;
                    return RetracePath(startNode, targetNode);
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
            Console.WriteLine("Found no path");
            return null;
        }

        List<Node> RetracePath(Node startNode, Node targetNode)
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

        int GetDistance(Node nodeA, Node nodeB)
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
