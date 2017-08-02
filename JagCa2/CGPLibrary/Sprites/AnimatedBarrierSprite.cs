using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JagCa2;

namespace CGPLibrary
{
    class AnimatedBarrierSprite : AnimatedSprite
    {
        protected Color passColor;

        public Color PASSCOLOR
        {
            get
            {
                return passColor;
            }
        }

        public AnimatedBarrierSprite(Main game, Texture2D texture,
            Rectangle destinationRectangle, Color color, Color passColor,
            AnimatedSpriteFrameInfo frameInfo, float rotation, float layerDepth, bool bCollidable)
            : base(game, texture, destinationRectangle, color,
                         frameInfo, rotation, layerDepth, bCollidable)
        {
            this.passColor = passColor;
            //lock into one of 4 possible rotations
            this.rotation = GameData.setValidRotation(rotation);
        }
    }
}
