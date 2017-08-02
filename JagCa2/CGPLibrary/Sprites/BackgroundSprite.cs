using System.Collections.Generic;
using System.Linq;
using System.Text;
using JagCa2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CGPLibrary
{
    class BackgroundSprite : Sprite
    {

        public BackgroundSprite(Main game, Texture2D texture, Rectangle sourceRectangle, Vector2 translation,
            int rotation, Vector2 scale, Vector2 origin, Color color, float zDepth) :
            base(game, texture, sourceRectangle, translation, rotation, scale, origin, color, zDepth)
        {

        }


        public override void Update(GameTime gameTime)
        {
            
        }

    }

}
