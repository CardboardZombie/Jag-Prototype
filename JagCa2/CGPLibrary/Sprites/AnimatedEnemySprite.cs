using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JagCa2;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CGPLibrary
{
    class AnimatedEnemySprite : AnimatedSprite
    {

        private List<AnimatedSpriteFrameInfo> frameInfoList = new List<AnimatedSpriteFrameInfo>();

        public List<AnimatedSpriteFrameInfo> LIST
        {
            get
            {
                return frameInfoList;
            }

        }

        public List<AnimatedSpriteFrameInfo> this[int i]
        {
            set
            {
                base.frameInfo = frameInfoList[i];
            }
        }

        public AnimatedEnemySprite(Main game, Texture2D texture,
            Rectangle destinationRectangle, Color color,
            List<AnimatedSpriteFrameInfo> frameInfo, float rotation, float layerDepth, bool bCollidable)
            : base(game, texture, destinationRectangle, color,
                         frameInfo[0], rotation, layerDepth, bCollidable)
        {
            for (int i = 0; i < frameInfoList.Count; i++)
            {
                this.frameInfoList.Add(frameInfoList[i]);
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }


}
