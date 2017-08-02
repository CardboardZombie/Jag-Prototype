using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JagCa2;
using Microsoft.Xna.Framework.Audio;

namespace CGPLibrary
{
    public class SpriteManager : DrawableGameComponent
    {
        public List<Sprite> spriteList;
        public Main game;
        public bool bPause = false;

        protected SpriteBatch spriteBatch;

        #region PROPERTIES
        public Sprite this[int index]
        {
            get
            {
                return spriteList[index];
            }
        }

        public SpriteBatch SPRITEBATCH
        {
            get
            {
                return spriteBatch;
            }
        }

        public bool PAUSE
        {
            get
            {
                return bPause;
            }
            set
            {
                bPause = value;
            }
        }
        #endregion

        public SpriteManager(Main game)
            : base(game)
        {
            Sprite.SPRITEMANAGER = this;
            this.game = game;
            this.spriteList = new List<Sprite>();
            this.spriteBatch = game.SPRITEBATCH;

        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public void add(Sprite theSprite)
        {
            spriteList.Add(theSprite);
        }

        public void remove(Sprite theSprite)
        {
            spriteList.Remove(theSprite);
            //tag for garbage collection
            theSprite = null;
        }

        public void clear()
        {
            spriteList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            if ((!bPause) && (game.currentState == Main.GameState.InGame))
            {
                for (int i = 0; i < spriteList.Count; i++)
                {
                    spriteList[i].Update(gameTime);
                }
                game.HUD1.Update(gameTime);
                //game.HUD2.Update(gameTime);
            }
            else 
            {
                game.MENUMANAGER.Update(gameTime);
            }


            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Camera2D camera, Main.GameState theState)
        {
            if (!bPause)//show pause menu
            {
                game.GraphicsDevice.Viewport = camera.VIEWPORT;
                DrawSprites(gameTime, camera, theState);
            }
            else
            {
                game.MENUMANAGER.Draw(gameTime);
            }
           
        }

        private void DrawSprites(GameTime gameTime, Camera2D camera, Main.GameState theState)
        {
            if (theState == Main.GameState.InGame)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null,
                    camera.TRANSFORM);

                for (int i = 0; i < spriteList.Count; i++)
                {
                    spriteList[i].Draw(gameTime);
                }
                spriteBatch.End();

                spriteBatch.Begin();
                game.HUD1.Draw(gameTime);
                //game.HUD2.Draw(gameTime);
                spriteBatch.End();
            }
            else if (theState == Main.GameState.Lose)
            {
                spriteBatch.Begin();
                //if (theState == game.currentState)
                //{
                    spriteBatch.Draw(game.loseTexture, camera.VIEWPORT.Bounds, Color.White);
                //}
                /*else
                {
                    spriteBatch.Draw(game.loseTexture,
                        new Rectangle(camera.VIEWPORT.Bounds.X, camera.VIEWPORT.Bounds.Y + 125, camera.VIEWPORT.Bounds.Width, camera.VIEWPORT.Bounds.Height), 
                        Color.White);
                }*/
                spriteBatch.End();
            }
            else if (theState == Main.GameState.Win)
            {
                spriteBatch.Begin();
               // if (theState == game.currentStateP1)
                //{
                    spriteBatch.Draw(game.winTexture, camera.VIEWPORT.Bounds, Color.White);
                //}
                /*else
                {
                    spriteBatch.Draw(game.winTexture,
                        new Rectangle(camera.VIEWPORT.Bounds.X, camera.VIEWPORT.Bounds.Y + 125, camera.VIEWPORT.Bounds.Width, camera.VIEWPORT.Bounds.Height),
                        Color.White);
                }*/
                spriteBatch.End();
            }
            /*else if (theState == Main.GameState.IsDead)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(game.isDeadTexture, camera.VIEWPORT.Bounds, Color.White);
                spriteBatch.End();
            }*/

        }

        public int size()
        {
            return spriteList.Count;
        }
    }
}