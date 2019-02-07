using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class Dijkstra
    {
        Grid grid;
        public Dijkstra(Grid _grid)
        {
            grid = _grid;
        }

        public List<Node> FindPath(Node startNode, Node targetNode)
        {
            Queue<Node> visit = new Queue<Node>();
            Queue<Node> visited = new Queue<Node>();
            // Queue<Node> path = new Queue<Node>();
            List<Node> path = new List<Node>();

            startNode.Distance = 0;
            startNode.Weight = 0;
            //start.SetStartNode(true);
            //start.SetToVisit();
            visit.Enqueue(startNode);

            while (visit.Count != 0)
            {
                Node currentNode = visit.Dequeue();
                // if (setDebuggOn) Console.WriteLine("Evaluated NodeYX: " + currentNode.Y + " " + currentNode.X);               
                EvaluateNeighbors(currentNode, visit, visited);

                if (currentNode == targetNode)
                {
                    //Node[] pathArray = new Node[81];
                  

                    //if (setDebuggOn)
                    //{
                    //    for (int i = 0; i < visited.Count; i++)
                    //    {
                    //        Console.WriteLine("path detected, nodes in queue: " + visited.Dequeue().Y + visited.Dequeue().X);
                    //        Console.WriteLine("Target found: " + target + " at index " + target.Y + " " + target.X);
                    //    }
                    //}

                    path.Add(currentNode);
                    while (currentNode.Previous != null)
                    {
                        currentNode = currentNode.Previous;
                        path.Add(currentNode);
                    }
                    path.Add(startNode);

                    //for (int i = 0; i < path.Count; i++)
                    //{
                    //    Node[] tempArray = path.ToArray();
                    //   // if (setDebuggOn) Console.WriteLine("path; " + tempArray[i].Y + " " + tempArray[i].X);
                    //}
                    //for (int i = path.Count - 1; i >= 0; --i)
                    //{
                    //    pathArray[i] = path.Dequeue();
                    //}
                    //int it = 0;

                    //while (pathArray[it] != null)
                    //{
                    //   // if (setDebuggOn) Console.WriteLine("Path after Reversal, Index " + it + " contains Node at index " + pathArray[it].Y + " " + pathArray[it].X);
                    //    it++;
                    //}
                    foreach (Node n in visited)
                    {
                        n.Color = Color.Purple;
                    }
                    foreach (Node n in visit)
                    {
                        n.Color = Color.Violet;
                    }
                    foreach (Node n in path)
                    {
                        n.Color = Color.Blue;
                    }
                    path.Reverse();
                    return path;
                }

               // currentNode.SetVisited();
                visited.Enqueue(currentNode);
            }
            return null;

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

        private void EvaluateNeighbors(Node currentNode, Queue<Node> visit, Queue<Node> visited)
        {
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                //if (!neighbour.IsWalkable || visited.Contains(neighbour))
                //{
                //    continue;
                //}
                int distance = currentNode.Weight + neighbour.Weight;
                //int edgeDistance = currentNode.Weight + neighbour.Weight;
                // int newDistance = currentNode.Distance + edgeDistance;
                if ( distance < neighbour.Distance && neighbour.Color != Color.Black)
                {
                    neighbour.Distance = distance;
                    neighbour.Weight = distance;
                    neighbour.Previous = currentNode;
                    visit.Enqueue(neighbour);
                }
            //    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
            //    {
            //        continue;
            //    }
                //int newMoveCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                //if (newMoveCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                //{
                //    neighbour.GCost = newMoveCostToNeighbour;
                //    neighbour.HCost = GetDistance(neighbour, targetNode);
                //    neighbour.Parent = currentNode;

                //    if (!openSet.Contains(neighbour))
                //    {
                //        openSet.Add(neighbour);
                //    }

             //   }
            }
            //foreach (Edge v in currentNode.adj)
            //{
            //    Node adj = v.Child;
            //   // if (setDebuggOn) Console.WriteLine("Found NodeYX " + adj.Y + " " + adj.X + " Adj to " + u.Y + " " + u.X);


            //    int edgeDistance = currentNode.weight + adj.weight;

            //    // int newDistance = u.distance + edgeDistance;
            //    if (edgeDistance < adj.distance && adj.weight < 2)
            //    {
            //       // if (setDebuggOn) Console.WriteLine("AdjNode edgeDistance " + edgeDistance + " : newDistance " + newDistance + " adj.distance = " + adj.distance);
            //        adj.distance = edgeDistance;
            //      //  adj.previous = u;
            //       // adj.SetToVisit();
            //        visit.Enqueue(adj);
            //      //  if (setDebuggOn) Console.WriteLine("AdjNode edgeDistance " + edgeDistance + " : newDistance " + newDistance + " new adj.distance = " + adj.distance);
            //    }
            //}
        }
    }
}
