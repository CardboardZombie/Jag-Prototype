using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using JagCa2;

namespace CGPLibrary
{
    /// <summary>
    /// Manages all changes relating to screen resolution
    /// </summary>
    public class ScreenManager : Microsoft.Xna.Framework.GameComponent
    {
        protected Main game;

        //if screen resolution is changed then apply new changes and set new dimensions
        private bool bResolutionChange = false;


        #region PROPERTIES
        public bool RESOLUTIONHASCHANGED
        {
            get
            {
                return bResolutionChange;
            }
        }

        public int WINDOWWIDTH
        {
            get
            {
                return game.Window.ClientBounds.Width;
            }
            set
            {
                game.GRAPHICS.PreferredBackBufferWidth = value;
                bResolutionChange = true;
            }
        }
        public int WINDOWHEIGHT
        {
            get
            {
                return game.Window.ClientBounds.Height;
            }
            set
            {
                game.GRAPHICS.PreferredBackBufferHeight = value;
                bResolutionChange = true;
            }
        }
        public int HALFWINDOWWIDTH
        {
            get
            {
                return game.Window.ClientBounds.Width / 2;
            }
        }
        public int HALFWINDOWHEIGHT
        {
            get
            {
                return game.Window.ClientBounds.Height / 2;
            }
        }


        #endregion
        public ScreenManager(Main game /*noOfSectors*/)
            : base(game)
        {
            this.game = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if the WINDOWWIDTH or WINDOWHEIGHT are changed then apply changes
            if (bResolutionChange)
            {
                game.GRAPHICS.ApplyChanges();
                bResolutionChange = false;
            }
            /*
            //A test to change resolution...remove eventually
            if (game.KEYBOARDMANAGER.isFirstKeyPress(Keys.F1))
            {
                WINDOWWIDTH = 1024;
                WINDOWHEIGHT = 768;
            }
            */
            base.Update(gameTime);
        }

    }
}
