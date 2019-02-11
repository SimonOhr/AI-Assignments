﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PathFinding
{
    internal class Grid
    {
        private Texture2D tex;
        private readonly SpriteFont text;
        public Node[,] nodes;
        private MouseState mouseInput, oldMOuseInput;
        private KeyboardState keyInput, oldKeyInput;
        public GameState gameState { get; set; }

        private enum SetupStateSequence { SELECTPATHALGORITHM, SETUPSTART, SETUPTARGET, SETUPOBSTACLES, COMPLETE }

        private SetupStateSequence currentSetupState;

        private enum PathFindingSelector { NOTHING, ASTAR, DIJKRSTRAS, BREADTHFIRST, DEPTHFIRST }

        private PathFindingSelector chosenPathFindingAlgorithm;
        public bool isSetUpComplete { get; set; }
        public bool doPathFinding;
        private AStar aStar;
        private BFS bfs;
        private DFS dfs;
        public int gridSizeX { get; private set; }
        public int gridSizeY { get; private set; }

        private double timer;
        private const int reset = 0, interval = 10000;

        public bool IsSearching { get; set; }

        private Node startNode, targetNode;

        
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
            aStar = new AStar(this);
            bfs = new BFS(this);
            dfs = new DFS(this);
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
                }
            }
        }

        private void CheckGameState(Node node, GameTime gameTime)
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
                        if (startNode == null)
                        {
                            startNode = FindStartNode();
                        }
                        if (targetNode == null)
                        {
                            targetNode = FindTargetNode();
                        }
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

        private bool RunSetupStateSequence(Node node)
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

        private PathFindingSelector AlgorithmSelecter()
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

        private void DoAlgorithm(PathFindingSelector chosenAlgorithm, GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                switch (chosenPathFindingAlgorithm)
                {
                    case PathFindingSelector.ASTAR:
                        aStar.FindPath(startNode, targetNode);
                        break;
                    case PathFindingSelector.DIJKRSTRAS:
                        bfs.FindPath(startNode, targetNode);
                        break;
                    case PathFindingSelector.BREADTHFIRST:
                        break;
                    case PathFindingSelector.DEPTHFIRST:
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
            if (mouseInput.LeftButton == ButtonState.Pressed)
            {
                if (node.Hitbox.Contains(mouseInput.Position) && node.Color != Color.Green && node.Color != Color.Red)
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
