using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JagCa2;

namespace CGPLibrary
{
    public class ProgressSprite : Sprite
    {
        protected Rectangle sourceRectangle;
        protected int progressValue;
        protected AnimatedPlayerSprite player;
        protected SpriteFont font;
        protected String hud;

        // whats this? new Rectangle(0, 0, 30, 20)

        public ProgressSprite(Main game, Texture2D texture, Rectangle rectangle, Vector2 translation, 
            int rotationInDegrees, Vector2 scale, Vector2 origin, Color color, float zDepth, AnimatedPlayerSprite player, String font)
            : base(game, texture, rectangle , translation, rotationInDegrees, scale, origin, color, zDepth)
        {
            this.player = player;
            this.progressValue = 0;
            this.font = game.Content.Load<SpriteFont>(@"" + font);
            this.hud = "Lives: " + game.LIVES + "\n Score: " + game.SCORE;

        }
        public override void Update(GameTime gameTime)
        {

            hud = "Lives: " + game.LIVES + "\n Score: " + game.SCORE;

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (this.player == game.PLAYER1)
            {
                game.SPRITEBATCH.DrawString(font, hud, new Vector2(0, 0), Color.Red);
            }
            else 
            {
                game.SPRITEBATCH.DrawString(font, hud, new Vector2(0, game.Window.ClientBounds.Height/2), Color.Red);
            }
        }
    }
}
