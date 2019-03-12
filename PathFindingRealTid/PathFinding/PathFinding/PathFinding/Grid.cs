using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PathFindingALgorithmsUI;
using System;
using System.Collections.Generic;

namespace PathFinding
{
    public enum PathFindingSelector { NOTHING, ASTAR, DIJKRSTRAS, BREADTHFIRST, DEPTHFIRST }

    internal class Grid
    {
        private Texture2D tex;
        private readonly SpriteFont text;
        public Node[,] nodes;
        private MouseState mouseInput, oldMouseInput;
        private KeyboardState keyInput, oldKeyInput;
        public GameState gameState { get; set; }

        private enum SetupStateSequence { SELECTPATHALGORITHM, SETUPSTART, SETUPTARGET, SETUPOBSTACLES, COMPLETE }

        private SetupStateSequence currentSetupState;

        private PathFindingSelector chosenPathFindingAlgorithm;
        public bool isSetUpComplete { get; set; }
        public bool doPathFinding;
        private AStar aStar;
        private BFS bfs;
        private DFS dfs;
        private Dijkstra dijkstra;
        public int gridSize { get; private set; }


        private double timer;
        private const int reset = 0;
        private int simSpeed = 10000;

        public bool IsSearching { get; set; }

        private Node startNode, targetNode;

        public int MaxSize { get { return gridSize * gridSize; } }

        Algorithms selectedAlgorithm;

        public Grid(Texture2D texture, int _gridSize, GameState gameState, SpriteFont _text, int _simSpeed, Algorithms _selected)
        {
            tex = texture;
            gridSize = _gridSize;
            text = _text;
            nodes = new Node[_gridSize, _gridSize];
            this.gameState = gameState;
            simSpeed /= _simSpeed;
            selectedAlgorithm = _selected;
            currentSetupState = SetupStateSequence.SETUPSTART;
            ConstructGrid();
            aStar = new AStar(this);
            bfs = new BFS(this);
            dfs = new DFS(this);
            dijkstra = new Dijkstra(this);
        }

        private void ConstructGrid()
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node(tex, new Vector2(j * tex.Width, i * tex.Height), text);
                }
            }
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j].AdjList = GetNeighbours(nodes[i, j]);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.GridCoordX + x;
                    int checkY = node.GridCoordY + y;

                    if (checkX >= 0 && checkX < gridSize && checkY >= 0 && checkY < gridSize)
                        neighbours.Add(nodes[checkY, checkX]);
                }
            }
            return neighbours;
        }

        public void Update(GameTime gameTime)
        {
            oldMouseInput = mouseInput;
            mouseInput = Mouse.GetState();

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    CheckGameState(nodes[i, j], gameTime);
                }
            }
        }

        private void CheckGameState(Node node, GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.SETUP:
                    if (RunSetupStateSequence(node))
                    {
                        isSetUpComplete = true;
                        gameState = GameState.RUN;
                    }
                    break;
                case GameState.RUN:
                    if (IsSearching)
                    {
                        Game1.sw.Start();

                        if (startNode == null)
                            startNode = FindStartNode();

                        if (targetNode == null)
                            targetNode = FindTargetNode();

                        DoAlgorithm(selectedAlgorithm, gameTime);
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

        private bool RunSetupStateSequence(Node node)
        {
            oldKeyInput = keyInput;
            keyInput = Keyboard.GetState();
            switch (currentSetupState)
            {
                case SetupStateSequence.SELECTPATHALGORITHM:
                    if (chosenPathFindingAlgorithm == 0)                       
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

        private void DoAlgorithm(Algorithms chosenAlgorithm, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > simSpeed)
            {
                switch (selectedAlgorithm)
                {
                    case Algorithms.ASTAR:
                        aStar.FindPath(startNode, targetNode);
                        break;
                    case Algorithms.DIJKSTRA:
                        dijkstra.FindPath(startNode, targetNode);
                        break;
                    case Algorithms.BFS:
                        bfs.FindPath(startNode, targetNode);
                        break;
                    case Algorithms.DFS:
                        dfs.RunDFS(startNode, targetNode);
                        break;
                    default:
                        break;
                }

                timer = reset;
            }           
        }

        private Node FindStartNode()
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

        private Node FindTargetNode()
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

        private bool SetUpNodes(Color switchToColor, Node node)
        {
            if (node.Hitbox.Contains(mouseInput.Position) &&                
                mouseInput.LeftButton == ButtonState.Pressed)
            {
                if (node.Color != Color.Green &&
                node.Color != Color.Red)
                    node.Color = switchToColor;
            }

            if (mouseInput.RightButton == ButtonState.Pressed &&
                node.Hitbox.Contains(mouseInput.Position) &&
                node.Color == switchToColor)
            {
                node.Color = Color.White;
            }

            if (node.Color == Color.Black)
                node.SetWalkable(false);
            if (node.Color != Color.Black)
                node.SetWalkable(true);

            if (keyInput.IsKeyDown(Keys.Enter) && oldKeyInput.IsKeyUp(Keys.Enter))
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
            float percentX = (nodePosition.X + (gridSize * tex.Width) / 2) / (gridSize * tex.Width);
            float percentY = (nodePosition.Y + (gridSize * tex.Height) / 2) / (gridSize * tex.Height);
            percentX = MathHelper.Clamp(percentX, 0, 1);
            percentY = MathHelper.Clamp(percentY, 0, 1);
            int x = (int)((gridSize - 1) * percentX);
            int y = (int)((gridSize - 1) * percentY);
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
