using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;

namespace SteeringBehaviour
{
    public class Game1 : Game
    {
        readonly bool halvedFrameRate = true;

        static public Random random = new Random();
        static public Rectangle Bounds { get { return new Rectangle(0, 0, 1720, 880); } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D boidTex;
        Boid[] boids;
        MouseState currentMouseState, oldMouseState;

        int updatingHalf;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = Bounds.Height;
            graphics.PreferredBackBufferWidth = Bounds.Width;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            boidTex = Content.Load<Texture2D>("boidTex");
            boids = new Boid[halvedFrameRate ? 200 : 150];
            for (int i = 0; i < boids.Length; i++)
            {
                boids[i] = new Boid(boidTex);
                Console.WriteLine($"Boid number {i} created");
            }
            Console.WriteLine($"sum of all boids created = {boids.Length}");

            FlockingBehaviour.Initialize(ref boids);

            updatingHalf = halvedFrameRate ? 0 : 2;
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

            if (currentMouseState.LeftButton == ButtonState.Pressed /*&& oldMouseState.LeftButton == ButtonState.Pressed*/)
            {
                int start = 0, stop = 0;
                switch (updatingHalf)
                {
                    case 0:
                        start = 0;
                        stop = boids.Length / 2;
                        updatingHalf = 1;
                        break;
                    case 1:
                        start = boids.Length / 2;
                        stop = boids.Length;
                        updatingHalf = 0;
                        break;
                    case 2:
                        start = 0;
                        stop = boids.Length;
                        break;
                }

                for (int i = start; i < stop; i++)
                {
                    boids[i].SetDirection(currentMouseState);

                    FlockingBehaviour.Update(boids[i]);

                    boids[i].Update();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            //foreach (Boid tempBoid in boids)
            //    tempBoid.DebugDraw(spriteBatch);

            foreach (Boid tempBoid in boids)
                tempBoid.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}