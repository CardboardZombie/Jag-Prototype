using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JagCa2;

namespace CGPLibrary
{
    public class AnimatedSprite : CollidableSprite
    {
        protected SpriteEffects flip;
        protected AnimatedSpriteFrameInfo frameInfo;
        protected int currentFrameNumber, currentFrameRateCount, maxFrameNumber, startFrameNumber;
        private double totalElapsedSinceLastFrame;
        public int scoreValue { get; protected set; }

        #region PROPERTIES
        public override Rectangle SOURCERECTANGLE
        {
            set
            {
                sourceRectangle = value;
                //if frame changes then source rectangle changes
                setTextureColorData2DFromSourceRectangle();
            }
            get
            {
                return sourceRectangle;
            }
        }
        //see AnimatedSprite::Update() to see why we need these two X/Y PROPERTY methods
        public override int SOURCERECTANGLEX
        {
            set
            {
                sourceRectangle.X = value;
                //if frame changes then source rectangle changes
                setTextureColorData2DFromSourceRectangle();
            }
            get
            {
                return sourceRectangle.X;
            }
        }
        public int SOURCERECTANGLEY
        {
            set
            {
                sourceRectangle.Y = value;
                //if frame changes then source rectangle changes
                setTextureColorData2DFromSourceRectangle();
            }
            get
            {
                return sourceRectangle.Y;
            }
        }
        #endregion

        public AnimatedSprite(Main game, Rectangle sourceRectangle,
            Vector2 translation, int rotationInDegrees, Vector2 scale, Vector2 origin, Color color,
            float zDepth, AnimatedSpriteFrameInfo frameInfo, int scoreValue)
            /*pass null for texture later we will set with AnimationData*/
            : base(game, null, sourceRectangle, translation, rotationInDegrees, scale, origin, color, zDepth)
        {
                
                this.scoreValue = scoreValue;
                this.frameInfo = frameInfo;
                //(Starting Frame * Frame Width, Dimension of spriteSheet(1D = 0), Width, Height)
                this.sourceRectangle = new Rectangle(frameInfo.FRAMESTARTNUMBER * frameInfo.FRAMEWIDTH,
                                        0, frameInfo.FRAMEWIDTH, frameInfo.FRAMEHEIGHT);
                //need to set the total number of frames in a single row - subtract 1 because its ZERO based (i.e. 0 - 15)
                SetFrameInfo(frameInfo);
        }

        public void SetFrameInfo(AnimatedSpriteFrameInfo frameInfo)
        {
            if (frameInfo.ENDFRAME == -1)
                this.maxFrameNumber = (int)(texture.Width / frameInfo.FRAMEWIDTH) - 1;
            else
                this.maxFrameNumber = frameInfo.ENDFRAME;

            this.startFrameNumber = frameInfo.FRAMESTARTNUMBER;
            this.currentFrameNumber = startFrameNumber;
        }
        public override void Update(GameTime gameTime)
        {
            //perform animation
            
                //Change the current frame number
                if (currentFrameNumber < maxFrameNumber)
                {
                    currentFrameNumber++;
                }
                else
                {
                    //stop InGame the animation
                    //remove animation from the sprite manager
                    //repeat the animation
                    currentFrameNumber = startFrameNumber;
                }

                totalElapsedSinceLastFrame = 0; //reset the count until next increment
           
            animate(gameTime);
        }

        protected virtual void animate(GameTime gameTime)
        {
            totalElapsedSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (totalElapsedSinceLastFrame > frameInfo.FRAMEDURATIONINSECSONDS)
            {
                if (currentFrameNumber < maxFrameNumber)
                {
                    SOURCERECTANGLEX += frameInfo.FRAMEWIDTH;
                    currentFrameNumber++;
                }
                else
                {
                    //stop InGame the animation
                    //remove animation from the sprite manager
                    //repeat the animation
                    currentFrameNumber = startFrameNumber;

                    //if (animationData.REPEATANIMATION) //if loop then reset
                    //{
                    //    SOURCERECTANGLEX = 0;
                    //    currentFrame = 0;
                    //}
                    //else
                    //{
                    //    Sprite.spriteManager.remove(this); //if no loop then remove
                    //}
                }
                totalElapsedSinceLastFrame = 0; //reset count until we show the next frame
            }
            else
            {
                currentFrameRateCount++;
            }
            this.sourceRectangle = new Rectangle(currentFrameNumber * frameInfo.FRAMEWIDTH,
                                                    0, frameInfo.FRAMEWIDTH, frameInfo.FRAMEHEIGHT);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


        protected void setTextureColorData2DFromSourceRectangle()
        {
            //this sets the color data to be equal to the color data of the current frame
            //remember that the per-pixel cd method Collision::IntersectsNonAA(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
            //takes the color data as an parameter
            TEXTURECOLORDATA2D = frameInfo[currentFrameNumber];
        }

    }
}







//        public override void Update(GameTime gameTime)
//        {
//            //0.016, 0.032, 0.048
//           
//            base.Update(gameTime);
//        }
//        public override void Draw(GameTime gameTime)
//        {
//            // base.Draw(gameTime); //do not want default Sprite::draw() behaviour
//            game.SPRITEBATCH.Draw(texture, sourceRectangle, color);
//        }
//    }
//}