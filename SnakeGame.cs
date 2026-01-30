using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SimpleSnake;

public class SnakeGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Texture and Grid constants
    private Texture2D _pixel;
    private const int TileSize = 20;
    private const int GridWidth = 40;
    private const int GridHeight = 24;

    // Game State
    private List<Vector2> _snake = new List<Vector2>();
    private Vector2 _direction = new Vector2(1, 0);
    private Vector2 _nextDirection = new Vector2(1, 0);
    private Vector2 _food;
    private Random _random = new Random();

    // Timing
    private float _moveTimer = 0;
    private float _moveInterval = 0.15f; // Speed of snake

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GridWidth * TileSize;
        _graphics.PreferredBackBufferHeight = GridHeight * TileSize;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _snake.Add(new Vector2(10, 10)); // Starting position
        SpawnFood();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a 1x1 white pixel texture programmatically
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        // 1. Handle Input
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Up) && _direction.Y == 0) _nextDirection = new Vector2(0, -1);
        if (state.IsKeyDown(Keys.Down) && _direction.Y == 0) _nextDirection = new Vector2(0, 1);
        if (state.IsKeyDown(Keys.Left) && _direction.X == 0) _nextDirection = new Vector2(-1, 0);
        if (state.IsKeyDown(Keys.Right) && _direction.X == 0) _nextDirection = new Vector2(1, 0);

        // 2. Movement Timer
        _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_moveTimer >= _moveInterval)
        {
            _direction = _nextDirection;
            MoveSnake();
            _moveTimer = 0;
        }

        base.Update(gameTime);
    }

    private void MoveSnake()
    {
        // Calculate new head position
        Vector2 newHead = _snake[0] + _direction;

        // Check Wall Collision
        if (newHead.X < 0 || newHead.X >= GridWidth || newHead.Y < 0 || newHead.Y >= GridHeight || _snake.Contains(newHead))
        {
            ResetGame();
            return;
        }

        _snake.Insert(0, newHead);

        // Check Food Collision
        if (newHead == _food)
        {
            SpawnFood();
        }
        else
        {
            _snake.RemoveAt(_snake.Count - 1);
        }
    }

    private void SpawnFood()
    {
        _food = new Vector2(_random.Next(0, GridWidth), _random.Next(0, GridHeight));
    }

    private void ResetGame()
    {
        _snake.Clear();
        _snake.Add(new Vector2(10, 10));
        _direction = new Vector2(1, 0);
        _nextDirection = new Vector2(1, 0);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        // Draw Food
        _spriteBatch.Draw(_pixel, new Rectangle((int)_food.X * TileSize, (int)_food.Y * TileSize, TileSize, TileSize), Color.Red);

        // Draw Snake
        foreach (var segment in _snake)
        {
            _spriteBatch.Draw(_pixel, new Rectangle((int)segment.X * TileSize, (int)segment.Y * TileSize, TileSize - 1, TileSize - 1), Color.LimeGreen);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
