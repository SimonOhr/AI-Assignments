using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using PathFindingALgorithmsUI;


namespace PathFinding
{
    public enum GameState { WAITING, SETUP, RUN, EXAMINE, REVERT }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D recTex;
        private SpriteFont text;
        private Grid grid;
        private const int screenSizeX = 800, screenSizeY = 800;
        private GameState gameState;
        private KeyboardState keyInput, oldKeyInput;

        public static Stopwatch sw = new Stopwatch();

        MainWindow ui;

        System.Windows.Forms.Form frm;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            frm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            frm.Hide();
            ui = new MainWindow();
            ui.InitializeComponent();
            ui.Show();
           
            gameState = GameState.WAITING;
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            recTex = Content.Load<Texture2D>("25x25Rec");
            text = Content.Load<SpriteFont>("text");
            ui.OnChoiceSelected += OnUserSetup;
            setNewScreenWidth(0);
            // grid = new Grid(recTex, (screenSizeX / recTex.Width), (screenSizeY / recTex.Height), gameState, text);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            oldKeyInput = keyInput;
            keyInput = Keyboard.GetState();
            switch (gameState)
            {
                case GameState.WAITING:

                    break;
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

        private void OnUserSetup(object sender, NewSimPropertiesEventArgs args)
        {
            try
            {
                var gridSize = Int32.Parse(args.size);
                var simSpeed = Int32.Parse(args.speed);
                var algorithmSelected = args.selected;
                SetupGrid(gridSize, simSpeed, algorithmSelected);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SetupGrid(int _gridSize, int _simSpeed, Algorithms _selected)
        {
            gameState = GameState.SETUP;
            grid = new Grid(recTex, _gridSize, gameState, text, _simSpeed, _selected);
            setNewScreenWidth(_gridSize);
        }
        void setNewScreenWidth(int size)
        {
            graphics.PreferredBackBufferWidth = size * recTex.Width;
            graphics.PreferredBackBufferHeight = size * recTex.Height;
            graphics.ApplyChanges();
        }
    }
}
