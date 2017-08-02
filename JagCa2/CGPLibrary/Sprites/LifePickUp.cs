using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JagCa2;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CGPLibrary
{

    class LifePickUp : PickUpSprite
    {
        private int score;

        public LifePickUp(Main game, Texture2D texture, Rectangle sourceRectangle, Vector2 translation,
            int rotation, Vector2 scale, Vector2 origin, Color color, float zDepth, int score) :
            base(game, texture, sourceRectangle, translation, rotation, scale, origin, color, zDepth)
        {
            this.score = score;
        }


    }
}
