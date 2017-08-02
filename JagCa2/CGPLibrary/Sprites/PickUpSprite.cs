using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using JagCa2;

namespace CGPLibrary
{
    class PickUpSprite : Sprite
    {

        public PickUpSprite(Main game, Texture2D texture, Rectangle sourceRectangle, Vector2 translation, 
            int rotation, Vector2 scale, Vector2 origin, Color color, float zDepth) :
            base(game, texture, sourceRectangle, translation, rotation, scale, origin, color, zDepth)
        {

        }



    }
}
