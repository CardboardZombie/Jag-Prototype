using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JagCa2;

namespace CGPLibrary
{
    /*
     * NMCG
     * 12.11.12
     * Performs camera transformations by transforming the viewport.
     * This will not affect the position, scale, or rotation of anything drawn on screen.
     */
    
    public class Camera2D //: GameComponent
    {
        //minimum amount by which we can scale the Viewport (i.e. zoom out)
        private const float SCALE_MINIMUM = 0.05f;
        private const float MOVE_SPEED_INCREMENT = 0.1f;
        //roughly 1 degree of rotation every update if the correct key is pressed - assumes 16ms update
        private float ROTATION_SPEED_INCREMENT = MathHelper.ToRadians(1 / 16.0f);
        //how much to zoom in or out
        private const float SCALE_SPEED_INCREMENT = 0.001f;

        //used to calculate how much the camera should move, rotate, and scale - see Camera2D::Draw()
        private float moveIncrement, rotationIncrement, scaleIncrement;

        //gets access to keyboard manager. could also use a service
        protected Main game;
        //See split screen example
        protected Viewport viewPort;

        //see SpriteManager::Draw()
        //protected Matrix worldTransform;

        //target position of camera (i.e. what position camera is looking at on-screen)
        //protected Vector2 translation;

        //rotation and scale for camera
        protected float rotation, scale;

        //used by reset() - see handleInput()
        private Vector2 originalTranslation;
        private float originaRotation, originalScale;

        private Vector2 centre;
        private Matrix transform;
        private bool isSecond;

        #region PROPERTIES
        public Viewport VIEWPORT
        {
            get
            {
                return viewPort;
            }
            set
            {
                viewPort = value;
            }
        }
        /*public Vector2 TRANSLATION
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;
            }
        }*/
        public Vector2 CENTRE
        {
            get
            {
                return centre;
            }
        }
        public Matrix TRANSFORM
        {
            get
            {
                return transform;
            }
        }
        public bool ISSECOND
        {
            get
            {
                return isSecond;
            }
        }
        public float ROTATION
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public float SCALE
        {
            get
            {
                return scale;
            }
            set
            {
                //if less than, set to minimum, otherwise set value
                scale = (value < SCALE_MINIMUM) ? SCALE_MINIMUM : value;
            }
        }

        //called by SpriteManager::Draw() in spriteBatch.Begin()
        public Matrix MATRIX
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(1,1,0))*//-translation.X, -translation.Y, 0)) *
                             Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(new Vector3(scale, scale, 1));// *
                                    //Matrix.CreateTranslation(new Vector3(viewPort.Width / 2, viewPort.Height / 2, 0));

            }
        }
        //use these methods to move the camera either in XY or in X and Y independently
        public Vector2 MOVE
        {
            set
            {
                //translation += value;
            }
        }
        public float MOVEX
        {
            set
            {
                //translation.X += value;
            }
        }
        public float MOVEY
        {
            set
            {
                //translation.Y += value;
            }
        }

        #endregion

        public Camera2D(Main game, Viewport viewPort, /*Vector2 translation,*/ float rotation, float scale, bool isSecond)
        // : base(game)
        {
            this.game = game;

            //we need access to viewport to get dimensions of screen
            //we cant use windowDimensions because we may want to create more than one viewport (i.e. for splitscreen)
            this.viewPort = viewPort;

            //sets the position of the camera
            //this.translation = translation;

            //sets any rotation around Z (i.e. coming out of the screen)
            this.rotation = rotation;

            //sets the zoom level (i.e. if > 1 zoom in, if < 1 zoom out, bounded at minimum value)
            //call property and not this.scale to ensure scale not set to less than minimum
            SCALE = scale;

            //stored for reset
            //this.originalTranslation = translation;
            this.originaRotation = rotation;
            this.originalScale = scale;

            this.isSecond = isSecond;
        }

        public void reset()
        {
            //this.translation = originalTranslation;
            this.rotation = originaRotation;
            //call property and not this.scale to ensure scale not set to less than minimum
            SCALE = originalScale;
        }

        private void handleInput(GameTime gameTime)
        {
            //these variables are declared as instance variables above to prevent re-declaration on each Update()
            moveIncrement = gameTime.ElapsedGameTime.Milliseconds * MOVE_SPEED_INCREMENT;
            rotationIncrement = gameTime.ElapsedGameTime.Milliseconds * ROTATION_SPEED_INCREMENT;
            scaleIncrement = gameTime.ElapsedGameTime.Milliseconds * SCALE_SPEED_INCREMENT;

            //Up
            if (game.KEYBOARDMANAGER.isKeyDown(Keys.I))
            {
                MOVEY = moveIncrement;
            }
            //Down
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.K))
            {
                MOVEY = -moveIncrement;
            }
            //Right
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.L))
            {
                MOVEX = moveIncrement;
            }
            //Left
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.J))
            {
                MOVEX = -moveIncrement;
            }
            //Anti-clock wise
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.O))
            {
                ROTATION += rotationIncrement;
            }
            //Clock wise
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.U))
            {
                ROTATION -= rotationIncrement;
            }
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.V))
            {
                SCALE += scaleIncrement;
            }
            //down
            else if (game.KEYBOARDMANAGER.isKeyDown(Keys.B))
            {
                SCALE -= scaleIncrement;
            }

            /*if (game.KEYBOARDMANAGER.isFirstKeyPress(Keys.F5))
            {
                game.PATHMANAGER.play(gameTime);
            }
            else if (game.KEYBOARDMANAGER.isFirstKeyPress(Keys.F6))
            {
                game.PATHMANAGER.pause();
            }
            else if (game.KEYBOARDMANAGER.isFirstKeyPress(Keys.F7))
            {
                game.PATHMANAGER.reset(gameTime);
            }*/
        }

        public /*override*/ void Initialize()
        {
            //   base.Initialize();
        }

        public /*override*/ void Update(Vector2 position)
        {
            int addY;

            //might want to add keyboard control to the camera
            //disable or redefine movement based on your requirements
            if (isSecond)
            {
                addY = game.Window.ClientBounds.Height / 2;
            }
            else 
            {
                addY = 0;
            }
            centre = new Vector2(position.X - 400, addY); //gm.Window.ClientBounds.Width / 2

            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                          Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
            
            //handleInput(gameTime);

            //base.Update(gameTime);
        }
    }

}
