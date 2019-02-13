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
        private Grid grid;
        private Node startNode, targetNode;
        private Heap<Node> visit;
        private Queue<Node> visited = new Queue<Node>();
        private List<Node> path = new List<Node>();
        private Heap<Node> heap;
        Node currentNode;
        public Dijkstra(Grid _grid)
        {
            grid = _grid;
            visit = new Heap<Node>(_grid.MaxSize);
            heap = new Heap<Node>(_grid.MaxSize);           
        }

        private void Reset()
        {
            startNode.Distance = 0;
            startNode.Weight = 0;
           
            visited = new Queue<Node>();
            path = new List<Node>();
        }

        public List<Node> FindPath(Node _startNode, Node _targetNode)
        {          
            if (targetNode == null)
            {
                foreach (Node item in grid.nodes)
                {
                    item.Distance = int.MaxValue;
                    item.Previous = null;
                    heap.Add(item);
                }
                targetNode = _targetNode;
                //Reset();
                //heap.Add(_startNode);
            }           

            if (heap.Count != 0 && heap.GetCurrent() != null)
            {
                currentNode = heap.RemoveFirst();
                
                EvaluateNeighbors(currentNode, heap, visited);

                if (currentNode == targetNode)
                {
                    path.Add(currentNode);
                    while (currentNode.Previous != null)
                    {
                        currentNode = currentNode.Previous;
                        path.Add(currentNode);
                    }
                    path.Add(startNode);

                    foreach (Node n in path)
                        n.Color = Color.Blue;

                    path.Reverse();
                    grid.IsSearching = false;
                    Reset();

                    return path;
                }
                visited.Enqueue(currentNode);
            }
            foreach (Node n in visited)
                n.Color = Color.Purple;

            for (int i = 0; i < visit.GetCollection().Length; i++)           
            {
                if (visit.GetCollection()[i] == null)
                    break;
                visit.GetCollection()[i].Color = Color.Violet;
            }
            Console.WriteLine("heap count: " + visit.Count);
            return null;
        }

        //public List<Node> GetNeighbours(Node node)
        //{
        //    List<Node> neighbours = new List<Node>();

        //    for (int x = -1; x <= 1; x++)
        //    {
        //        for (int y = -1; y <= 1; y++)
        //        {
        //            if (x == 0 && y == 0)
        //                continue;

        //            int checkX = node.GridCoordX + x;
        //            int checkY = node.GridCoordY + y;

        //            if (checkX >= 0 && checkX < grid.gridSizeX && checkY >= 0 && checkY < grid.gridSizeY)
        //                neighbours.Add(grid.nodes[checkY, checkX]);
        //        }
        //    }
        //    return neighbours;
        //}

        private void EvaluateNeighbors(Node currentNode, Heap<Node> visit, Queue<Node> visited)
        {
            foreach (Node neighbour in currentNode.AdjList)
            {
                int distance = currentNode.Weight + neighbour.Weight;

                if (distance < neighbour.Distance && neighbour.IsWalkable && !neighbour.Visited)
                {
                    neighbour.Distance = distance;
                    neighbour.Weight = distance;
                    neighbour.Previous = currentNode;
                    heap.Add(neighbour);
                }
            }
        }
    }
}
