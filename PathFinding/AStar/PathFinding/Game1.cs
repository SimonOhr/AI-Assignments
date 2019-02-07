using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PathFinding
{
    public enum GameState { SETUP, RUN, EXAMINE, REVERT }
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D recTex;
        SpriteFont text;
        Grid grid;
        int screenSizeX = 1500, screenSizeY = 1000;
        GameState gameState;
        KeyboardState keyInput, oldKeyInput;

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


        protected override void UnloadContent()
        {

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
                    grid.Update();
                    if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
                        if (grid.isSetUpComplete)
                            gameState = GameState.RUN;
                    break;
                case GameState.RUN:
                    grid.gameState = GameState.RUN;

                    grid.doPathFinding = true;
                    grid.Update();
                    gameState = GameState.EXAMINE;
                    break;
                case GameState.EXAMINE:
                    Console.WriteLine("testing testing");
                    grid.Update();
                    if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
                        gameState = GameState.REVERT;
                    break;
                case GameState.REVERT: // input rever logics
                    grid.gameState = GameState.REVERT;
                    grid.Update();
                    if (RevertDone())
                        gameState = GameState.SETUP;
                    break;
                default:
                    System.Console.WriteLine("DefaultState, something wrong");
                    break;
            }
            base.Update(gameTime);
        }

        bool RevertDone()
        {
            if (DoRevert())
            {
                return true;
            }
            return false;
        }

        bool DoRevert()
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
                    //if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
                    //    gameState = GameState.Run;
                    break;
                case GameState.RUN:
                    grid.Draw(spriteBatch);
                    //if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
                    //    gameState = GameState.Revert;
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
