using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;

namespace SteeringBehvaious
{
    public class Game1 : Game
    {
        static public Random random = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D boidTex;
        Boid[] boids;
        FlockingBehaviour fb;
        MouseState mouseInput;
        KeyboardState keyInput;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 1900;
            graphics.PreferredBackBufferWidth = 2500;
            graphics.IsFullScreen = true;
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
                Thread.Sleep(10);
                Console.WriteLine($"Boid number {i} created");
            }
            Console.WriteLine($"sum of all boids created = {boids.GetLength(0)}");
            fb = new FlockingBehaviour(boids);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseInput = Mouse.GetState();
            keyInput = Keyboard.GetState();

            if (mouseInput.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < boids.GetLength(0); i++)
                {
                    boids[i].SetDirection(mouseInput);
                    boids[i].SetAlignment(fb.GetAlignment(boids[i]));
                    boids[i].SetCohersion(fb.GetCohesion(boids[i]));
                    boids[i].SetSeperation(fb.GetSeperation(boids[i]));
                    boids[i].Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < boids.GetLength(0); i++)
            {
                boids[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
