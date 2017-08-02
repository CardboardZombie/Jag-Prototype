using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JagCa2;

namespace CGPLibrary
{
    public class CollidableSprite : Sprite
    {
        private static Texture2D debugTexture;

        //collision detection variables
        protected Matrix worldTransform;
        protected Rectangle bounds;
        private Rectangle originalBounds;
        protected static float alphaThreshold = 20;

        //1, 2, 4, 8 for each sector 
        //(15 if you're in all sectors i.e. in centre)
        protected int bSector = 0;
        public Color[,] textureColorData2D;


        //note we have to redefine some properties in this subclass (e.g. scale, rotation, translation)
        //because these properties need to set bounding rectangles and bSector values
        #region PROPERTIES
        public override int ROTATION
        {
            get
            {
                return rotationInDegrees;
            }
            set
            {
                //never let rotation run outside range -360 -> 0 -> 360 (i.e. never let it be 362, or -725) in radians
                rotationInDegrees = value % 360;

                //set new bounds
                updateBounds();

                //if we rotate then we need to find a new rectangle that bounds the shape
                this.bSector = Collision.getSector(this.bounds);
            }

        }
        public override Vector2 SCALE
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;

                //set new bounds
                updateBounds();

                this.bSector = Collision.getSector(this.bounds);
            }
        }

        public override Vector2 TRANSLATION
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;

                //set new bounds
                updateBounds();

                this.bSector = Collision.getSector(this.bounds);
            }
        }
        public override float TRANSLATIONX
        {
            get
            {
                return translation.X;
            }

            set
            {
                translation.X = value;

                //set new bounds
                updateBounds();

                this.bSector = Collision.getSector(this.bounds);
            }
        }
        public override float TRANSLATIONY
        {
            get
            {
                return translation.Y;
            }

            set
            {
                translation.Y = value;

                //set new bounds
                updateBounds();

                this.bSector = Collision.getSector(this.bounds);
            }
        }
        public int SECTOR
        {
            get
            {
                return bSector;
            }
            //no setter. cannot set bSector directly!
        }

        public Rectangle BOUNDS
        {
            //can only get not set bounds
            get
            {
                return bounds;
            }
        }

        public override Texture2D TEXTURE
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                //nmcg - store the texture bits for each sprite
                this.textureColorData2D = get2DColorDataArray(texture);
            }
        }
        public Color[,] TEXTURECOLORDATA2D
        {

            get
            {
                if (textureColorData2D == null)
                    return get2DColorDataArray(texture);
                else
                    return textureColorData2D;
            }
            set
            {
                textureColorData2D = value;
            }
        }
        #endregion

        #region CONSTRUCTORS
        public CollidableSprite(Main game, Texture2D texture, Rectangle sourceRectangle,
            Vector2 translation, int rotationInDegrees, Vector2 scale, Vector2 origin, Color color, float zDepth)
            : base(game, texture, sourceRectangle, translation, rotationInDegrees, scale, origin, color, zDepth)
        {
            //this represents the original bounding rectangle 
            this.originalBounds = new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height);

            //set new bounds first time in so rectangles are drawn in correct initial position
            updateBounds();

            //set the sector value
            this.bSector = Collision.getSector(this.bounds);

           // debugTexture = spriteManager.game.Content.Load<Texture2D>(@"Images\Debug\debugrect");

            //important to set this in CollidableSprite for all sub-classes
            this.bCollidable = true;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        #region PER PIXEL NON AXIS ALIGNED

        //called only when we translate, rotate, or scale and also initially in constructor (i.e. we dont just call it every update)
        private void updateBounds()
        {
            this.bounds
                = Collision.CalculateTransformedBoundingRectangle(originalBounds, MATRIX);
        }

        //get matrix for position the object WILL move to if no collision is detected
        public Matrix getProjectedMatrix(Vector2 projectedTranslation, int projectedRotationInDegrees)
        {
            return getMatrix(projectedTranslation, origin, scale, projectedRotationInDegrees);
        }
        #endregion
    }
}