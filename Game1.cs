using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleAnimationNamespace;

namespace Assignment_01;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    int _height = 500;
    int _width = 800;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;
    private Texture2D _ufo;
    Vector2 UFOPosition = new Vector2(300, 20);
    string ufoEdge = "top";

    private SpriteFont _arial;
    private string _message;

    private SimpleAnimation _walkingAnimation;
    private SimpleAnimation _shootingAnimation;

    private KeyboardState _kbPreviousState;
    Vector2 _PlayersInput;
    Vector2 PlayerLocation = new Vector2(300, 200);
    private SpriteEffects _facing = SpriteEffects.None;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = _width;
        _graphics.PreferredBackBufferHeight = _height;
        _graphics.ApplyChanges();

        _PlayersInput = new Vector2(400, 300);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("Background");
        _ufo = Content.Load<Texture2D>("UFO");

        _arial = Content.Load<SpriteFont>("Arial");

        _walkingAnimation = new SimpleAnimation(
            Content.Load<Texture2D>("Walking"),
            1529 / 8,
            167,
            8,
            8
        );

        _shootingAnimation = new SimpleAnimation(
            Content.Load<Texture2D>("Shooting"),
            760 / 4,
            147,
            4,
            8
        );
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        _walkingAnimation.Update(gameTime);
        _shootingAnimation.Update(gameTime);

        // Player Control
        KeyboardState kbCurrentState = Keyboard.GetState();
        _message = "";
        _PlayersInput = Vector2.Zero;

        if (kbCurrentState.IsKeyDown(Keys.Left))
        {
            if (PlayerLocation.X > 0 - (1529 / 20))
            {
                _PlayersInput += new Vector2(-1, 0);
                _facing = SpriteEffects.FlipHorizontally;
                _message += "Left ";
            }

        }
        if (kbCurrentState.IsKeyDown(Keys.Right))
        {
            if (PlayerLocation.X < _width - (1529 / 12))
            {
                _PlayersInput += new Vector2(1, 0);
                _facing = SpriteEffects.None;
                _message += "Right ";
            }

        }
        if (kbCurrentState.IsKeyDown(Keys.Up))
        {
            if (PlayerLocation.Y > 0)
            {
                _PlayersInput += new Vector2(0, -1);
                _facing = SpriteEffects.FlipVertically;
                _message += "Up ";
            }

        }
        if (kbCurrentState.IsKeyDown(Keys.Down))
        {
            if (PlayerLocation.Y < _height - 167)
            {
                _PlayersInput += new Vector2(0, 1);
                _facing = SpriteEffects.None;
                _message += "Down";
            }

        }

        PlayerLocation += _PlayersInput * 5;

        _kbPreviousState = kbCurrentState;

        // Automated Movement
        switch (ufoEdge)
        {
            case "top":
                UFOPosition.X += 5;
                if (UFOPosition.X + _ufo.Width >= _width)
                {
                    UFOPosition.X = _width - _ufo.Width;
                    ufoEdge = "right";
                }
                break;

            case "right":
                UFOPosition.Y += 5;
                if (UFOPosition.Y + _ufo.Height >= _height)
                {
                    UFOPosition.Y = _height - _ufo.Height;
                    ufoEdge = "bottom";
                }
                break;

            case "bottom":
                UFOPosition.X -= 5;
                if (UFOPosition.X <= 0)
                {
                    UFOPosition.X = 0;
                    ufoEdge = "left";
                }
                break;

            case "left":
                UFOPosition.Y -= 5;
                if (UFOPosition.Y <= 0)
                {
                    UFOPosition.Y = 0;
                    ufoEdge = "top";
                }
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();

        // background
        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);

        // static sprite
        _spriteBatch.Draw(_ufo, UFOPosition, Color.White);

        // text
        _spriteBatch.DrawString(_arial, _message, new Vector2(20, 20), Color.Red);

        // animation
        _walkingAnimation.Draw(_spriteBatch, PlayerLocation, _facing);
        _shootingAnimation.Draw(_spriteBatch, new Vector2(50, _height / 2), SpriteEffects.None);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}