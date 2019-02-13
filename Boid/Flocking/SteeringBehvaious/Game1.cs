using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;

namespace SteeringBehaviour
{
    public class Game1 : Game
    {
        static public Texture2D DebugTex { get; set; }

        static public Random random = new Random();
        static public Rectangle Bounds { get { return new Rectangle(0, 0, 1720, 880); } }


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D boidTex;
        Boid[] boids;
        MouseState currentMouseState, oldMouseState;

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

            DebugTex = Content.Load<Texture2D>("ball");

            boidTex = Content.Load<Texture2D>("boidTex");
            boids = new Boid[100];
            for (int i = 0; i < boids.Length; i++)
            {
                boids[i] = new Boid(boidTex);
                Console.WriteLine($"Boid number {i} created");
            }
            Console.WriteLine($"sum of all boids created = {boids.Length}");

            FlockingBehaviour.Initialize(ref boids);
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
                //    foreach (Boid tempBoid in boids)
                //    {
                //        tempBoid.SetDirection(currentMouseState);

                //        tempBoid.SetCohesion(FlockingBehaviour.Rule1(tempBoid));
                //        tempBoid.SetSeperation(FlockingBehaviour.Rule2(tempBoid));
                //        tempBoid.SetAlignment(FlockingBehaviour.Rule3(tempBoid));

                //        tempBoid.Update();
                //    }

                for (int i = 0; i < boids.Length; i++)
                {
                    boids[i].SetDirection(currentMouseState);

                    boids[i].SetAlignment(FlockingBehaviour.GetAlignment(boids[i]));
                    boids[i].SetCohesion(FlockingBehaviour.GetCohesion(boids[i]));
                    boids[i].SetSeperation(FlockingBehaviour.GetSeperation(boids[i]));
                    //flockingBehaviour.Update(boids[i]);

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