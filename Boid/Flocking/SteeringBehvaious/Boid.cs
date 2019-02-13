using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehaviour
{
    class Boid
    {
        public Vector2 Pos { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Direction { get; private set; }

        static readonly float speed = 0.8f;

        Texture2D tex;
        Vector2 alignment, cohesion, seperation;

        public Boid(Texture2D texture)
        {
            tex = texture;
            Pos = new Vector2(Game1.random.Next(0, Game1.Bounds.Width), Game1.random.Next(0, Game1.Bounds.Height));
            Velocity = Vector2.Zero;
        }

        public void Update()
        {
            Vector2 flockingDirection = alignment + seperation + alignment + Direction;
            Velocity += flockingDirection;
            Velocity *= speed;
            Pos += Velocity;
        }

        public void SetDirection(MouseState mouse)
        {
            Vector2 temp = mouse.Position.ToVector2() - Pos;
            Direction = Vector2.Normalize(temp);
        }

        public void SetAlignment(Vector2 newAlignment)
        {
            alignment = newAlignment;
        }

        public void SetCohesion(Vector2 newCohesion)
        {
            cohesion = newCohesion;
        }

        public void SetSeperation(Vector2 newSeperation)
        {
            seperation = newSeperation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
            spriteBatch.Draw(tex, Pos, null, Color.White, rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }

        //public void DebugDraw(SpriteBatch spriteBatch)
        //{
        //    float rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
        //    spriteBatch.Draw(Game1.DebugTex, new Rectangle((int)Pos.X, (int)Pos.Y, FlockingBehaviour.neighbourRange, FlockingBehaviour.neighbourRange),
        //    null, Color.LightGray, rotation, new Vector2(FlockingBehaviour.neighbourRange / 4, FlockingBehaviour.neighbourRange / 4), SpriteEffects.None, 0);
        //}
    }
}