using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class Grid
    {
        Texture2D tex;
        SpriteFont text;
        public Node[,] nodes;
        MouseState mouseInput, oldMOuseInput;
        KeyboardState keyInput, oldKeyInput;
        public GameState gameState { get; set; }
        enum SetupStateSequence { SELECTPATHALGORITHM, SETUPSTART, SETUPTARGET, SETUPOBSTACLES, COMPLETE }
        SetupStateSequence currentSetupState;
        enum PathFindingSelector { NOTHING, ASTAR, DIJKRSTRAS, BREADTHFIRST, DEPTHFIRST }
        PathFindingSelector chosenPathFindingAlgorithm;
        public bool isSetUpComplete { get; set; }
        public bool doPathFinding;
        AStar AStar;
        Dijkstra dijkstra;
        public int gridSizeX { get; private set; }
        public int gridSizeY { get; private set; }

        double timer;
        int reset = 0, interval = 50;

        public bool IsSearching { get; set; }

        public Grid(Texture2D texture, int _gridSizeX, int _gridSizeY, GameState gameState, SpriteFont _text)
        {
            tex = texture;
            text = _text;
            nodes = new Node[_gridSizeY, _gridSizeX];
            gridSizeX = _gridSizeX;
            gridSizeY = _gridSizeY;
            this.gameState = gameState;
            currentSetupState = SetupStateSequence.SELECTPATHALGORITHM;
            ConstructGrid();
            AStar = new AStar(this);
            dijkstra = new Dijkstra(this);
        }

        void ConstructGrid()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node(tex, new Vector2(j * tex.Width, i * tex.Height), text);
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            oldMOuseInput = mouseInput;
            mouseInput = Mouse.GetState();

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    CheckGameState(nodes[i, j], gameTime);
                    nodes[i, j].Update();
                }
            }
        }
        void CheckGameState(Node node, GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.SETUP:
                    if (RunSetupStateSequence(node))
                        isSetUpComplete = true;
                    break;
                case GameState.RUN:
                    if (IsSearching)
                    {
                        DoAlgorithm(chosenPathFindingAlgorithm, gameTime);
                    }
                    break;
                case GameState.EXAMINE:
                    break;
                case GameState.REVERT:
                    break;
                default:
                    break;
            }
        }
        bool RunSetupStateSequence(Node node)
        {
            oldKeyInput = keyInput;
            keyInput = Keyboard.GetState();
            switch (currentSetupState)
            {
                case SetupStateSequence.SELECTPATHALGORITHM:
                    if (chosenPathFindingAlgorithm == 0)
                        chosenPathFindingAlgorithm = AlgorithmSelecter();
                    if (chosenPathFindingAlgorithm != 0)
                        currentSetupState = SetupStateSequence.SETUPSTART;
                    break;
                case SetupStateSequence.SETUPSTART:
                    if (!SetUpNodes(Color.Green, node))
                        currentSetupState = SetupStateSequence.SETUPTARGET;
                    break;
                case SetupStateSequence.SETUPTARGET:

                    if (!SetUpNodes(Color.Red, node))
                        currentSetupState = SetupStateSequence.SETUPOBSTACLES;
                    break;
                case SetupStateSequence.SETUPOBSTACLES:
                    if (!SetUpNodes(Color.Black, node))
                        currentSetupState = SetupStateSequence.COMPLETE;
                    break;
                case SetupStateSequence.COMPLETE:
                    IsSearching = true;
                    return true;
                default:
                    break;
            }
            return false;
        }

        PathFindingSelector AlgorithmSelecter()
        {
            if (keyInput.IsKeyDown(Keys.D1))
                return chosenPathFindingAlgorithm = PathFindingSelector.ASTAR;

            if (keyInput.IsKeyDown(Keys.D2))
                return chosenPathFindingAlgorithm = PathFindingSelector.DIJKRSTRAS;

            if (keyInput.IsKeyDown(Keys.D3))
                return chosenPathFindingAlgorithm = PathFindingSelector.BREADTHFIRST;

            if (keyInput.IsKeyDown(Keys.D4))
                return chosenPathFindingAlgorithm = PathFindingSelector.DEPTHFIRST;

            return 0;
        }

        void DoAlgorithm(PathFindingSelector chosenAlgorithm, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                switch (chosenPathFindingAlgorithm)
                {
                    case PathFindingSelector.ASTAR:
                        AStar.FindPath(FindStartNode(), FindTargetNode());
                        break;
                    case PathFindingSelector.DIJKRSTRAS:
                        dijkstra.FindPath(FindStartNode(), FindTargetNode());
                        break;
                    case PathFindingSelector.BREADTHFIRST:
                        break;
                    case PathFindingSelector.DEPTHFIRST:
                        break;
                    default:
                        break;
                }
                timer = reset;
            }
        }

        Node FindStartNode()
        {
            foreach (Node n in nodes)
            {
                if (n.Color == Color.Green)
                {
                    n.Start = true;
                    return n;
                }
            }
            Console.WriteLine("Couldn't find start node");
            return null;
        }

        Node FindTargetNode()
        {
            foreach (Node n in nodes)
            {
                if (n.Color == Color.Red)
                {
                    n.Target = true;
                    return n;
                }                   
            }
            Console.WriteLine("Couldn't find target node");
            return null;
        }

        bool SetUpNodes(Color switchToColor, Node node)
        {
            if (mouseInput.LeftButton == ButtonState.Pressed )
            {
                if (node.Hitbox.Contains(mouseInput.Position))
                    node.Color = switchToColor;

            }
            if (mouseInput.RightButton == ButtonState.Pressed)
            {
                if (node.Hitbox.Contains(mouseInput.Position) && node.Color == switchToColor)
                {
                    

                    node.Color = Color.White;
                }
            }
            if (node.Color == Color.Black)
                node.SetWalkable(false);
            if (node.Color != Color.Black)
                node.SetWalkable(true);

            if (keyInput.IsKeyDown(Keys.Space) && oldKeyInput.IsKeyUp(Keys.Space))
                return false;
            return true;
        }
        /// <summary>
        /// clamp01 , RoundToint worldPos, gridWorldSize, gridSize. Checks if Node is OUT OF BOUNDS
        /// </summary>
        /// <param name="nodePosition"></param>
        /// <returns></returns>
        public Node NodeFromWorldPoint(Vector2 nodePosition)
        {
            float percentX = (nodePosition.X + (gridSizeX * tex.Width) / 2) / (gridSizeX * tex.Width);
            float percentY = (nodePosition.Y + (gridSizeY * tex.Height) / 2) / (gridSizeY * tex.Height);
            percentX = MathHelper.Clamp(percentX, 0, 1);
            percentY = MathHelper.Clamp(percentY, 0, 1);
            int x = (int)((gridSizeX - 1) * percentX);
            int y = (int)((gridSizeY - 1) * percentY);
            return nodes[y, x];
        }

        public bool DoRevert()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j].IsWalkable = true;
                    nodes[i, j].Color = Color.White;
                    nodes[i, j].Weight = 1;
                }
            }
            isSetUpComplete = false;
            chosenPathFindingAlgorithm = PathFindingSelector.NOTHING;
            currentSetupState = SetupStateSequence.SELECTPATHALGORITHM;
            return true;
        }
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j].Draw(sb);
                }
            }
        }
    }
}
