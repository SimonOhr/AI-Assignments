using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class Node
    {
        Texture2D tex;
        SpriteFont text;
        Vector2 pos;
        public int GridCoordX { get; set; }
        public int GridCoordY { get; set; }
        Color defaultColor = Color.White;
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
        public bool Start { get; set; }
        public bool Target { get; set; }

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
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color);
            sb.DrawString(text, $"{Distance}", new Vector2(pos.X + 9, pos.Y + 9), Color.Black);
        }

        public void SetWalkable(bool _isWalkable)
        {
            IsWalkable = _isWalkable;
        }
    }
}
