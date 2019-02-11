using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehvaious
{
    class Boid
    {
        public Vector2 Pos { get { return pos; } }
        public Vector2 Velocity { get { return velocity; } }

        static float speed = 2;

        Texture2D tex;
        Vector2 pos, velocity, direction, alignment, cohersion, seperation;
        float rotation;

        public Boid(Texture2D texture)
        {
            tex = texture;
            pos.X = Game1.random.Next(0, 1200);
            pos.Y = Game1.random.Next(0, 1000);
            velocity = new Vector2(1, 1);
        }

        public void Update(GameTime gameTime)
        {
            var flockingDirection = alignment + cohersion + seperation + direction;
            velocity = flockingDirection * speed;
            pos += velocity;
        }

        public void SetDirection(MouseState mouse)
        {
            Vector2 temp = mouse.Position.ToVector2() - pos;
            direction = Vector2.Normalize(temp);
            SetRotation(mouse);
        }

        private void SetRotation(MouseState mouse) => rotation = (float)Math.Atan2(velocity.Y, velocity.X);

        public void SetAlignment(Vector2 newAlignment)
        {
            alignment = newAlignment;
        }

        public void SetCohersion(Vector2 newCohersion)
        {
            cohersion = newCohersion;
        }

        public void SetSeperation(Vector2 newSeperation)
        {
            seperation = newSeperation;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, null, Color.White, rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}