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
            List<Node> path = new List<Node>();
            startNode.Distance = 0;
            startNode.Weight = 0;
            visit.Enqueue(startNode);
            while (visit.Count != 0)
            {
                Node currentNode = visit.Dequeue();            
                EvaluateNeighbors(currentNode, visit, visited);
                if (currentNode == targetNode)
                {
                    path.Add(currentNode);
                    while (currentNode.Previous != null)
                    {
                        currentNode = currentNode.Previous;
                        path.Add(currentNode);
                    }
                    path.Add(startNode);
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
                visited.Enqueue(currentNode);
            }
            return null;
        }   

        private void EvaluateNeighbors(Node currentNode, Queue<Node> visit, Queue<Node> visited)
        {
            foreach (Node neighbour in currentNode.AdjList)
            {                           
                int distance = currentNode.Weight + neighbour.Weight;
                if (distance < neighbour.Distance && neighbour.Color != Color.Black)
                {
                    neighbour.Distance = distance;
                    neighbour.Weight = distance;
                    neighbour.Previous = currentNode;
                    visit.Enqueue(neighbour);
                }           
            }          
        }
    }
}

