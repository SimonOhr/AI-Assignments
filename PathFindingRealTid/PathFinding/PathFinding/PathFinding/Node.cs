using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PathFinding
{
    internal class Node : IHeapItem<Node>
    {
        private Texture2D tex;
        private readonly SpriteFont text;
        private Vector2 pos;
        public int GridCoordX { get; set; }
        public int GridCoordY { get; set; }

        private Color defaultColor = Color.White;
        public Color Color { get; set; }
        public Rectangle Hitbox { get; private set; }
        // Dijkstra specific
        public Node Previous { get; set; }
        public int Weight { get; set; }
        public int Distance { get; set; }
        // Astar specific
        public Node Parent { get; set; }
        public bool IsWalkable { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost
        {
            get { return GCost + HCost; }
        }
        public bool Visited { get; set; }
        public bool Visit { get; set; }
        public bool Start { get; set; }
        public bool Target { get; set; }
        public List<Node> AdjList { get; set; }

        int heapIndex;

        public Node(Texture2D texture, Vector2 position, SpriteFont _text)
        {
            IsWalkable = true;
            tex = texture;
            text = _text;
            pos = position;
            Color = defaultColor;
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            GridCoordX = (int)pos.X / tex.Width;
            GridCoordY = (int)pos.Y / tex.Height;
            Weight = 1;
            Distance = int.MaxValue;
            AdjList = new List<Node>();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color);
            sb.DrawString(text, $"{Weight}", new Vector2(pos.X + 9, pos.Y + 9), Color.Black);
        }

        public void SetWalkable(bool _isWalkable)
        {
            IsWalkable = _isWalkable;
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }
            return -compare;
        }
    }
}
