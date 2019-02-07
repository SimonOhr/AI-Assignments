using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PathFinding
{
    public enum GameState { SETUP, RUN, EXAMINE, REVERT }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D recTex;
        private SpriteFont text;
        private Grid grid;
        private const int screenSizeX = 1500, screenSizeY = 1000;
        private GameState gameState;
        private KeyboardState keyInput, oldKeyInput;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            gameState = GameState.SETUP;
            graphics.PreferredBackBufferWidth = screenSizeX;
            graphics.PreferredBackBufferHeight = screenSizeY;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            recTex = Content.Load<Texture2D>("25x25Rec");
            text = Content.Load<SpriteFont>("text");
            grid = new Grid(recTex, (screenSizeX / recTex.Width), (screenSizeY / recTex.Height), gameState, text);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            oldKeyInput = keyInput;
            keyInput = Keyboard.GetState();
            switch (gameState)
            {
                case GameState.SETUP:
                    grid.gameState = GameState.SETUP;
                    grid.Update(gameTime);
                    if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
                        if (grid.isSetUpComplete)
                            gameState = GameState.RUN;
                    break;
                case GameState.RUN:
                    grid.gameState = GameState.RUN;

                    grid.doPathFinding = true;
                    grid.Update(gameTime);
                    gameState = GameState.EXAMINE;
                    break;
                case GameState.EXAMINE:
                    Console.WriteLine("testing testing");
                    grid.Update(gameTime);
                    if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
                        gameState = GameState.REVERT;
                    break;
                case GameState.REVERT: // input rever logics
                    grid.gameState = GameState.REVERT;
                    grid.Update(gameTime);
                    if (RevertDone())
                        gameState = GameState.SETUP;
                    break;
                default:
                    System.Console.WriteLine("DefaultState, something wrong");
                    break;
            }
            base.Update(gameTime);
        }

        private bool RevertDone()
        {
            if (DoRevert())
            {
                return true;
            }
            return false;
        }

        private bool DoRevert()
        {
            grid.DoRevert();
            return true;
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.SETUP:
                    grid.Draw(spriteBatch);
                    break;
                case GameState.RUN:
                    grid.Draw(spriteBatch);
                    break;
                case GameState.EXAMINE:
                    grid.Draw(spriteBatch);
                    break;
                case GameState.REVERT:
                    grid.Draw(spriteBatch);
                    if (RevertDone())
                        gameState = GameState.SETUP;
                    break;
                default:
                    System.Console.WriteLine("DefaultState, something wrong");
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
