﻿using Microsoft.Xna.Framework;
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

        static readonly float speed = 2;

        Texture2D tex;
        Vector2 alignment, cohersion, seperation;
        float rotation;

        public Boid(Texture2D texture)
        {
            tex = texture;
            Pos = new Vector2(Game1.random.Next(0, 1200), Game1.random.Next(0, 1200));
            Velocity = new Vector2(1, 1);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 flockingDirection = alignment + cohersion + seperation + Direction;
            Velocity = flockingDirection * speed;
            Pos += Velocity;
        }

        public void SetDirection(MouseState mouse)
        {
            Vector2 temp = mouse.Position.ToVector2() - Pos;
            Direction = Vector2.Normalize(temp);
            SetRotation(mouse);
        }

        private void SetRotation(MouseState mouse) => rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);

        public void SetAlignment(Vector2 newAlignment)
        {
            alignment = newAlignment;
        }

        public void SetCohesion(Vector2 newCohersion)
        {
            cohersion = newCohersion;
        }

        public void SetSeperation(Vector2 newSeperation)
        {
            seperation = newSeperation;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, Pos, null, Color.White, rotation, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}