﻿using bubbleTea;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace bubble_puzzle;

public class MainScene : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Texture2D _gameBoardTexture;

    public MainScene()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = GameConstants.GAME_WINDOW_WIDTH * GameConstants.TILE_SIZE;
        _graphics.PreferredBackBufferHeight = GameConstants.GAME_WINDOW_HEIGHT * GameConstants.TILE_SIZE;
        _graphics.ApplyChanges();

        Singleton.Instance.gameBoard.Position = GameConstants.BOARD_POSITION;
        Singleton.Instance.scoreObject.Position = GameConstants.SCORE_POSITION;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _gameBoardTexture = Content.Load<Texture2D>("image/game_asset_02");
        Singleton.Instance.gameBoard.texture = _gameBoardTexture;

        //load all of buble texture into array
        for (int i = 0; i < Singleton.Instance.gameBoard.bubbleTexture.Length; i++)
        {
            Singleton.Instance.gameBoard.bubbleTexture[i] = Content.Load<Texture2D>("image/game_asset_01");
        }
        Singleton.Instance.gameBoard.Reset();

        //load all of score texture into array
        for (int i = 0; i < Singleton.Instance.scoreObject.scoreTexture.Length; i++)
        {
            Singleton.Instance.scoreObject.scoreTexture[i] = Content.Load<Texture2D>("image/game_asset_03");
        }

        _font = Content.Load<SpriteFont>("GameFont");

        //create aim assistant texture
        Texture2D _react = new Texture2D(GraphicsDevice, 64, 64*5);
        Color[] data = new Color[64 * 64 * 5];
        for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
        _react.SetData(data);
        Singleton.Instance.gameBoard.aimAssistant.texture = _react;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Singleton.Instance.gameBoard.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        Singleton.Instance.gameBoard.Draw(_spriteBatch);
        Singleton.Instance.scoreObject.Draw(_spriteBatch);

        if(GameConstants.DEBUG_MODE)
        {
           Vector2 titleSize = _font.MeasureString("Debug Mode");
            _spriteBatch.DrawString(_font, "Debug Mode", GameConstants.DEBUG_POSITION - new Vector2(titleSize.X / 2, 0), Color.White);
           //parint array of board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2 position = new Vector2(GameConstants.DEBUG_POSITION.X + (j * 32), GameConstants.DEBUG_POSITION.Y + (i * 32) + 32);
                    _spriteBatch.DrawString(_font, Singleton.Instance.gameBoard.board[i, j].ToString(), position, Color.White);
                }
            }

        }

        _spriteBatch.End();
        _graphics.BeginDraw();

        base.Draw(gameTime);
    }
}
