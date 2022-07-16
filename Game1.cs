using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ShootingGallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;
        SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(300, 300);
        const int targetRadius = 45;
        const int crosshairRadius = 25;

        MouseState mState;
        
        bool mReleased = true;
        int score = 0;

        double timer = 10;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            targetSprite = Content.Load<Texture2D>("target");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timer > 0)
                timer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timer < 0)
                timer = 0;

            mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                float mouseTargetDist = Vector2.Distance(targetPosition, mState.Position.ToVector2());
                if (mouseTargetDist < targetRadius && timer > 0) 
                {
                    score++;

                    Random rand = new Random();

                    targetPosition.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                    targetPosition.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                }
                else if(timer > 0)
                    score--;

                mReleased = false;
            }

            mReleased = mState.LeftButton == ButtonState.Released ? mReleased = true : false;



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(gameFont, $"Score: {score.ToString()}", new Vector2(10, 0), Color.White);
            _spriteBatch.DrawString(gameFont, $"Time: {Math.Ceiling(timer).ToString()}", new Vector2(10, 50), Color.White);

            if (timer > 0)
            _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);

            _spriteBatch.Draw(crosshairsSprite, new Vector2(mState.X - crosshairRadius, mState.Y - crosshairRadius), Color.White);


            if (timer == 0)
            {
                Viewport viewport = _graphics.GraphicsDevice.Viewport;
                Vector2 screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
                Vector2 fontCenter = new Vector2(gameFont.Texture.Width / 2f, gameFont.Texture.Height / 2f);
                _spriteBatch.DrawString(gameFont, "Game Finished", screenCenter, Color.White, 0f, fontCenter, 1f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}