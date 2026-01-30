using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleAnimationNamespace;

namespace Assignment_01;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _spaceStation;
    private Texture2D _ship;

    private SpriteFont _arial;
    private string _output = "This is the string I want to output";

    private SimpleAnimation _walkingAnimation;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 640;
        _graphics.PreferredBackBufferHeight = 320;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _spaceStation = Content.Load<Texture2D>("Background");
        _ship = Content.Load<Texture2D>("UFO");

        _arial = Content.Load<SpriteFont>("Arial");

        // Assign the width based off the per frame width
        _walkingAnimation = new SimpleAnimation(
            Content.Load<Texture2D>("Walking"),
            1000/12,
            179,
            8,
            8
        );
    }

    protected override void Update(GameTime gameTime)
    {
        _walkingAnimation.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // background
        _spriteBatch.Draw(_spaceStation, Vector2.Zero, Color.White);

        // static sprite
        _spriteBatch.Draw(_ship, new Vector2(300, 140), Color.White);

        // text
        _spriteBatch.DrawString(_arial, _output, new Vector2(20, 20), Color.White);

        // animation
        _walkingAnimation.Draw(_spriteBatch, new Vector2(100, 200), SpriteEffects.None);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}