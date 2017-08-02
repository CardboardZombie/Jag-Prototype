using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JagCa2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CGPLibrary
{
    class RubyPickUp : PickUpSprite
    {

        private bool win;

        public bool WIN
        {
            get
            {
                return win;
            }
        }

        public RubyPickUp(Main game, Texture2D texture, Rectangle sourceRectangle, Vector2 translation,
            int rotation, Vector2 scale, Vector2 origin, Color color, float zDepth, bool winGame) :
            base(game, texture, sourceRectangle, translation, rotation, scale, origin, color, zDepth)
        {
            this.win = winGame;
        }
    }
}
