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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class KeyboardManager : Microsoft.Xna.Framework.GameComponent
    {
        private Main game;
        protected KeyboardState newState, oldState;

        public KeyboardManager(Main game)
            : base(game)
        {
            // TODO: Construct any child components here
            this.game = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //store the old keyboard state for later comparison
            setOldState();
            //get the current state in THIS update
            newState = Keyboard.GetState();

            //execute general (i.e. not related to character control) keyboard behaviour
            executeBehaviour();

            base.Update(gameTime);
            //code?????
        }

        public void setOldState()
        {
            this.oldState = newState;
        }


        private void executeBehaviour()
        {
            // nmcg - menu - set/reset a boolean variable in sprite manager which pauses any new updates
            if (isFirstKeyPress(Keys.Escape))
            {
                //invert pause for 2nd press of the Esc button - pause on and pause off
                //if pause is true the SpriteManager::update() method will not be called and sprite state will not change
                Sprite.SPRITEMANAGER.PAUSE = !Sprite.SPRITEMANAGER.PAUSE;

                //if pause then add menu manager to the component list so update() and draw() will be called
                if ((Sprite.SPRITEMANAGER.PAUSE) && (!this.game.MENUMANAGER.VISIBLE)) //not showing menu
                {
                    this.game.Components.Add(this.game.MENUMANAGER);
                    this.game.MENUMANAGER.VISIBLE = true;
                }
                else if ((!Sprite.SPRITEMANAGER.PAUSE) && (this.game.MENUMANAGER.VISIBLE))  //showing menu
                {
                    this.game.Components.Remove(this.game.MENUMANAGER);
                    this.game.MENUMANAGER.VISIBLE = false;
                    //bug fix - main and audio menu items shown at the same time
                    this.game.MENUMANAGER.removeAll();
                }


            }
            //nmcg - 10 - switch between fullscreen and preset e.g. 800x600
            else if (isFirstKeyPress(Keys.F11))
            {
                this.game.GRAPHICS.ToggleFullScreen();
            }
        }

        /// <summary>
        /// Detects first press of a user-defined key
        /// </summary>
        /// <param name="key">Test this key for first press</param>
        /// <returns>true if first press, otherwise false</returns>
        public bool isFirstKeyPress(Keys key)
        {
            //is this the first press for this key????
            if (newState.IsKeyDown(key) && oldState.IsKeyUp(key))
                return true;
            else
                return false;
        }

        public bool isKeyDown(Keys key)
        {
            return newState.IsKeyDown(key);
        }
        public bool isStateChanged()
        {
            return (newState.Equals(oldState)) ? false : true;
            //return (age >= 21) ? "adult" : "not adult";
        }
    }
}
