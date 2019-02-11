using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;

namespace SteeringBehaviour
{
    public class Game1 : Game
    {
        static public Random random = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D boidTex;
        Boid[] boids;
        FlockingBehaviour flockingBehaviour;
        MouseState currentMouseState, oldMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 1900;
            graphics.PreferredBackBufferWidth = 2500;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            boidTex = Content.Load<Texture2D>("boidTex");
            boids = new Boid[100];
            for (int i = 0; i < 100; i++)
            {
                boids[i] = new Boid(boidTex);
                Console.WriteLine($"Boid number {i} created");
            }
            Console.WriteLine($"sum of all boids created = {boids.Length}");

            flockingBehaviour = new FlockingBehaviour(boids);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < boids.GetLength(0); i++)
                {
                    boids[i].SetDirection(currentMouseState);

                    boids[i].SetAlignment(flockingBehaviour.GetAlignment(boids[i]));
                    boids[i].SetCohersion(flockingBehaviour.GetCohesion(boids[i]));
                    boids[i].SetSeperation(flockingBehaviour.GetSeperation(boids[i]));

                    boids[i].Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (Boid tempBoid in boids)
                tempBoid.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}