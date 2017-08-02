using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JagCa2;

namespace CGPLibrary
{
    public class Sprite
    {
        protected Main game; //need this to access Main::spriteBatch
        protected Texture2D texture;
        //postion, depth, and color variables
        protected Vector2 translation, origin, scale;
        protected int rotationInDegrees;
        protected Color color;
        protected float zDepth;

        //draw-related variables
        protected Rectangle sourceRectangle;
        protected bool isOutOfBounds = false;
        protected bool bCollidable = false;

        #region PROPERTIES
        public bool COLLIDABLE
        {
            get
            {
                return bCollidable;
            }
            set
            {
                bCollidable = value;
            }
        }
        public Matrix MATRIX
        {
            get
            {
                return getMatrix();
            }
        }
        public virtual int ROTATION
        {
            get
            {
                return rotationInDegrees;
            }
            set
            {
                //never let rotation run outside range -360 -> 0 -> 360 (i.e. never let it be 362, or -725) in radians
                rotationInDegrees = value % 360;
            }
        }
        public virtual Vector2 SCALE
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }
        public virtual Vector2 TRANSLATION
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;
            }
        }
        public virtual float TRANSLATIONX
        {
            get
            {
                return translation.X;
            }

            set
            {
                translation.X = value;
            }
        }
        public virtual float TRANSLATIONY
        {
            get
            {
                return translation.Y;
            }

            set
            {
                translation.Y = value;
            }
        }
        public virtual Vector2 POSITION
        {
            get
            {
                return Vector2.Transform(origin, MATRIX);
            }
        }
        public virtual Texture2D TEXTURE
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;

            }
        }
        public virtual int TEXTUREWIDTH
        {
            get
            {
                return texture.Width;
            }
        }
        public virtual int TEXTUREHEIGHT
        {
            get
            {
                return texture.Height;
            }
        }
        public virtual Rectangle SOURCERECTANGLE
        {
            set
            {
                sourceRectangle = value;
            }
            get
            {
                return sourceRectangle;
            }
        }
        public virtual int SOURCERECTANGLEX
        {
            set
            {
                sourceRectangle.X = value;
            }
            get
            {
                return sourceRectangle.X;
            }
        }
        #endregion

        #region CONSTRUCTORS
        public Sprite(Main theGame, Texture2D texture, Rectangle sourceRectangle,
            Vector2 translation, int rotationInDegrees, Vector2 scale, Vector2 origin, Color color, float zDepth)
        {
            this.game = theGame;
            this.texture = texture;
            this.translation = translation;
            this.rotationInDegrees = rotationInDegrees;
            this.origin = origin;
            this.scale = scale;
            this.color = color;
            this.zDepth = zDepth;
            this.sourceRectangle = sourceRectangle;
        }
        #endregion

        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, POSITION, sourceRectangle, color, MathHelper.ToRadians(rotationInDegrees),
                             origin, scale, SpriteEffects.None, zDepth);

        }
        protected Matrix getMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-origin, 0))
                * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationInDegrees))
                * Matrix.CreateScale(scale.X, scale.Y, 1)
                * Matrix.CreateTranslation(new Vector3(translation, 0));
        }

        public static Matrix getMatrix(Vector2 translation, Vector2 origin, Vector2 scale, int rotationInDegrees)
        {
            return Matrix.CreateTranslation(new Vector3(-origin, 0))
                 * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationInDegrees))
                * Matrix.CreateScale(scale.X, scale.Y, 1)
                * Matrix.CreateTranslation(new Vector3(translation, 0));
        }
        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (TRANSLATIONX < -TEXTUREWIDTH || TRANSLATIONX > clientRect.Width ||
                TRANSLATIONY < -TEXTUREHEIGHT || TRANSLATIONY > clientRect.Height)
                return true;

            return false;
        }
        #region TEXTURE ARRAY MANIPULATION
        //converts color data from texture from 1d to 2d array
        public static Color[,] get2DColorDataArray(Texture2D texture)
        {
            int width = texture.Width;
            int height = texture.Height;

            //read data into 1d array
            Color[] colors1D = new Color[width * height];
            texture.GetData(colors1D);

            //create 2d array to store data
            Color[,] colors2D = new Color[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * width];
                }
            }
            return colors2D;
        }
        #endregion
    }
}



//    public class Sprite
//    {
//        
//        protected Color[] textureData;
//        protected Vector2 position;
//        protected Color color;
//        protected Rectangle destinationRectangle;
//        protected double totalElapsedSinceLastScroll;


//        protected static SpriteManager spriteManager;

//        //Defines if sprite participates in CD/CR
//        protected bool bCollidable; //step 1 - BSP

//        //Stores the sector(s) that the sprite currently occupies
//        protected int bCollisionSector; //step 3 - BSP

//        #region PROPERTIES
//        public int COLLISIONSECTOR
//        {
//            get
//            {
//                return bCollisionSector;
//            }
//        }
//        public bool IS_COLLIDABLE
//        {
//            get
//            {
//                return bCollidable;
//            }
//        }
//        public Texture2D TEXTURE
//        {
//            get
//            {
//                return texture;
//            }
//        }
//        public Color[] TEXTURE_DATA
//        {
//            get
//            {
//                return textureData;
//            }
//        }
//        public static SpriteManager SPRITEMANAGER
//        {
//            get
//            {
//                return spriteManager;
//            }
//            set
//            {
//                spriteManager = value;
//            }
//        }

//        public int TEXTUREWIDTH
//        {
//            get
//            {
//                return texture.Width;
//            }
//        }
//        public int TEXTUREHEIGHT
//        {
//            get
//            {
//                return texture.Height;
//            }
//        }
//        public Color COLOR
//        {
//            set
//            {
//                color = value;
//            }
//        }
//        public Rectangle BOUNDINGRECTANGLE
//        {
//            get
//            {
//                return destinationRectangle;
//            }
//            /*  
//              set
//              {
//                  destinationRectangle = value;
//              }
//             */
//        }
//        public Vector2 POSITION
//        {
//            get
//            {
//                return position;
//            }
//        }
//        #endregion

//        public Sprite()
//        {
//        }

//        public Sprite(Main game, Texture2D texture,
//           Rectangle destinationRectangle, Color color,
//            bool bCollidable)
//        {
//            this.game = game;
//            this.texture = texture;
//            this.textureData = new Color[texture.Width * texture.Height];
//            this.texture.GetData(this.textureData);
//            this.destinationRectangle = destinationRectangle;
//            this.color = color;
//            this.position = new Vector2(destinationRectangle.X,
//                                    destinationRectangle.Y);
//            this.bCollidable = bCollidable; //step 2 - BSP
//        }

//        public virtual void Initialize()
//        {

//        }

//        //Change state information
//        public virtual void Update(GameTime gameTime)
//        {
//            /*totalElapsedSinceLastScroll += gameTime.ElapsedGameTime.TotalSeconds;

//            if (totalElapsedSinceLastScroll > 0.05)
//            {
//                //Change the current frame number

//                destinationRectangle.Y += 2;
//                totalElapsedSinceLastScroll = 0; //reset the count until next increment
//            }*/

//        }

//        public void setPosition(float x, float y)
//        {
//            destinationRectangle.X = (int)x;
//            destinationRectangle.Y = (int)y;

//            position.X = destinationRectangle.X;
//            position.Y = destinationRectangle.Y;
//            //Any time we move we must update sector
//            bCollisionSector = Collision.getSector(this);

//        }

//        protected void updatePosition(float x, float y)
//        {
//            destinationRectangle.X += (int)x;  //typical 5, 10, 15
//            destinationRectangle.Y += (int)y;

//            position.X = destinationRectangle.X;
//            position.Y = destinationRectangle.Y;
//            //Any time we move we must update sector
//            bCollisionSector = Collision.getSector(this);
//        }

//        protected void updatePosition(Vector2 updatePosition, float speed)
//        {
//            destinationRectangle.X += (int)(updatePosition.X * speed);  //typical 5, 10, 15
//            destinationRectangle.Y += (int)(updatePosition.Y * speed);

//            position.X = destinationRectangle.X;
//            position.Y = destinationRectangle.Y;
//            //Any time we move we must update sector
//            bCollisionSector = Collision.getSector(this);
//        }

//        public virtual void Draw(GameTime gameTime)
//        {
//            //draw our sprite on screen
//            game.SPRITEBATCH.Draw(texture, destinationRectangle, color);

//            //Main.DrawRectangle(game.SPRITEMANAGER.SPRITEBATCH, destinationRectangle, 1, Color.Gray);
//        }


//    }
//}
