using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PathFinding
{
    internal class BFS
    {
        private Grid grid;       
        private Queue<Node> visit = new Queue<Node>();
        private Queue<Node> visited = new Queue<Node>();
        private List<Node> path = new List<Node>();      

        public BFS(Grid _grid)
        {
            grid = _grid;
        }
        /// <summary>
        /// used to reset the nodes and lists between runs and algorithms
        /// </summary>
        private void Reset()
        {           
            visit = new Queue<Node>();
            visited = new Queue<Node>();
            path = new List<Node>();
        }

        public List<Node> FindPath(Node _startNode, Node _targetNode)
        {         
            if (_startNode == null && _targetNode == null)
            {
                _startNode.Distance = 0;
                _startNode.Weight = 0;                
                Reset();
            }
            
            visit.Enqueue(_startNode);

            if (visit.Count != 0)
            {
                Node currentNode = visit.Dequeue();               

                EvaluateNeighbors(currentNode, visit);

                if (currentNode == _targetNode)
                {
                    path.Add(currentNode);
                    while (currentNode.Previous != _startNode)
                    {
                        currentNode = currentNode.Previous;
                        path.Add(currentNode);
                    }
                    path.Add(_startNode);                    

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
            System.Console.WriteLine("NODE VISIT COUNT: " + visit.Count);
            return null;
        }               
       
        private void EvaluateNeighbors(Node currentNode, Queue<Node> visit)
        {
            foreach (Node neighbour in currentNode.AdjList)
            {              
                if (neighbour.IsWalkable && !neighbour.Visited && !neighbour.Visit)
                {                   
                    neighbour.Previous = currentNode;
                    visit.Enqueue(neighbour);
                    neighbour.Visit = true;
                    System.Console.WriteLine("NODE ADJ LIST: " + neighbour.AdjList.Count);
                }
            }
        }
    }
}
