using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PathFinding
{
    internal class DFS
    {
        private List<Node> path = new List<Node>();
        private readonly Stack<Node> closedStack = new Stack<Node>();
        private Node start;
        private Node current;
        private Node target;
        private Grid grid;

        public DFS(Grid _grid)
        {
            grid = _grid;
        }             

        public List<Node> RunDFS(Node _current, Node _target)
        {
            if (current == null)
                current = _current;
            if (target == null)
                target = _target;
            if (start == null)
                start = current;

            current.Visited = true;
            current.Color = Color.Purple;

            if (current == target)
            {
                path.Add(current);
                while (current.Parent != null && current.Parent != start)
                {
                    current = current.Parent;
                    path.Add(current);
                }
                path.Add(start);

                foreach (Node n in path)
                    n.Color = Color.Blue;

                path.Reverse();
                grid.IsSearching = false;          
                return path;
            }
            else
            {
                current.AdjList = GetNeighbours(current);
                foreach (Node item in current.AdjList)
                {
                    if (item.Parent == null && item.IsWalkable)
                    {
                        item.Parent = current;
                        item.Color = Color.Violet;
                    }
                }
                Node parent = current.Parent;              
                current = current.AdjList.Find(x => !x.Visited && x.Parent == current);               
                if (current == null)
                {
                    current = parent;
                }              
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
    }
}
