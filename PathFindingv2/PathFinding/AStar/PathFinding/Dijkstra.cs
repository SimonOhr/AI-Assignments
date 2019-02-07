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
        Node startNode, targetNode;
        Queue<Node> visit = new Queue<Node>();
        Queue<Node> visited = new Queue<Node>();
        List<Node> path = new List<Node>();

        public Dijkstra(Grid _grid)
        {
            grid = _grid;          
        }

        void StartNodeReset()
        {
            startNode.Distance = 0;
            startNode.Weight = 0;
        }

        public List<Node> FindPath(Node start, Node target)
        {           
            // Queue<Node> path = new Queue<Node>();
           
            if(this.startNode == null && this.targetNode == null)
            {
                this.startNode = start;
                this.targetNode = target;
                StartNodeReset();
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
                    {
                        n.Color = Color.Blue;
                    }
                    path.Reverse();
                    grid.IsSearching = false;
                    StartNodeReset();

                    return path;
                }
                foreach (Node n in visited)
                {
                    n.Color = Color.Purple;
                }
                foreach (Node n in visit)
                {
                    n.Color = Color.Violet;
                }              
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
                int distance = currentNode.Weight + neighbour.Weight;
                
                if (distance < neighbour.Distance && neighbour.Color != Color.Black && neighbour.Color != Color.Purple)
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
