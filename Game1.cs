using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame_3___Loops__Lists__Input
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        Rectangle mowerRect, window;
        Texture2D grassTexture, mowerTexture;

        SoundEffect mowerSound;
        SoundEffectInstance mowerSoundInstance;

        Vector2 mowerSpeed;

        List<Rectangle> grassTiles;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            window = new Rectangle(0, 0, 600, 500);
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            mowerRect = new Rectangle(100,100,30,30);

            grassTiles = new List<Rectangle>();
            for (int x = 0; x < window.Width; x += 2)
            {
                for (int y = 0; y < window.Height; y += 2)
                {
                    grassTiles.Add(new Rectangle(x, y, 2, 2));
                }
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            mowerTexture = Content.Load<Texture2D>("Images/mower");
            grassTexture = Content.Load<Texture2D>("Images/long_grass");
            mowerSound = Content.Load<SoundEffect>("Sounds/mower_sound");
            mowerSoundInstance = mowerSound.CreateInstance();
            mowerSoundInstance.IsLooped = true;
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mowerSpeed = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.W))
                mowerSpeed.Y -= 5;
            if (keyboardState.IsKeyDown(Keys.S))
                mowerSpeed.Y += 5;
            if (keyboardState.IsKeyDown(Keys.A))
                mowerSpeed.X -= 5;
            if (keyboardState.IsKeyDown(Keys.D))
                mowerSpeed.X += 5;

            mowerRect.Offset(mowerSpeed);

            for (int i = 0; i < grassTiles.Count; i++)
            {
                if (mowerRect.Contains(grassTiles[i]))
                {
                    grassTiles.Remove(grassTiles[i]);
                    i--;
                }
            }

            if (mowerRect.X < 0)
            {
                mowerRect.X = 0;
            }
            if (mowerRect.X > 570)
            {
                mowerRect.X = 570;
            }
            if (mowerRect.Y < 0)
            {
                mowerRect.Y = 0;
            }
            if (mowerRect.Y > 470)
            {
                mowerRect.Y = 470;
            }

            if (mowerSpeed == Vector2.Zero)
            {
                mowerSoundInstance.Stop();
            }
            else
            {
                mowerSoundInstance.Play();
            }

            if (grassTiles.Count == 0)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (Rectangle grass in grassTiles)
            {
                _spriteBatch.Draw(grassTexture, grass, Color.White);
            }
            _spriteBatch.Draw(mowerTexture, mowerRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
