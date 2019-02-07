using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PathFinding
{
    internal class Dijkstra
    {
        private Grid grid;
        private Node startNode, targetNode;
        private Queue<Node> visit = new Queue<Node>();
        private Queue<Node> visited = new Queue<Node>();
        private List<Node> path = new List<Node>();

        public Dijkstra(Grid _grid)
        {
            grid = _grid;
        }

        private void Reset()
        {
            startNode.Distance = 0;
            startNode.Weight = 0;

            visit = new Queue<Node>();
            visited = new Queue<Node>();
            path = new List<Node>();
        }

        public List<Node> FindPath(Node _startNode, Node _targetNode)
        {
            // Queue<Node> path = new Queue<Node>();

            if (startNode == null && targetNode == null)
            {
                startNode = _startNode;
                targetNode = _targetNode;
                Reset();
            }

            visit.Enqueue(startNode);

            if (visit.Count != 0)
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

            foreach (Node n in visit)
                n.Color = Color.Violet;

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
                int distance = currentNode.Weight + neighbour.Weight;

                if (distance < neighbour.Distance && neighbour.IsWalkable && !neighbour.Visited)
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
