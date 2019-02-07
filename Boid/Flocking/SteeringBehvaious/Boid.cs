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
        Texture2D tex;
        Vector2 pos, velocity, direction, alignment, cohersion, seperation;
        const float speed = 5;
        float rotation;

        public Boid(Texture2D texture)
        {
            tex = texture;
            pos.X = Game1.random.Next(0, 1200);
            pos.Y = Game1.random.Next(0, 1000);
            int rndSpeed = Game1.random.Next(1, 10);
            //velocity = new Vector2(rndSpeed, rndSpeed);
            velocity = new Vector2(1, 1);           
        }

        public void Update(GameTime gameTime)
        {
            var flocking = alignment + cohersion + seperation + direction;
            //direction += flocking;
            //velocity += direction * (5 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            velocity = flocking;
            // velocity += flocking;
            pos += velocity;

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, null, Color.White, rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }

        public void SetDirection(MouseState mouse)
        {
            Vector2 temp = mouse.Position.ToVector2() - pos;
            direction = Vector2.Normalize(temp);
            SetRotation(mouse);
        }

        public void SetRotation(MouseState mouse)
        {
            rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        }

        public Vector2 GetPos()
        {
            return pos;
        }

        public void SetPos(Vector2 newPos)
        {
            pos = newPos;
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

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
    }
}