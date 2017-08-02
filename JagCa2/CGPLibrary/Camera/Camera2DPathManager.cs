using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JagCa2;

namespace CGPLibrary
{
    /*
     * NMCG
     * 12.11.12
     * Allows the user to define a sequence of transforms for the camera to follow (e.g. a fly-through for your game objectives)
     */
    public class Camera2DPathManager : GameComponent
    {
        //stores a list of transforms to be applied to the camera
        public List<Camera2DTransform> transformList;

        //uses this in update() to change camera position if play is enabled
        protected Camera2D camera2D;

        //this stores the time the camera started moving (based on number of msecs that the game has been running)
        private double startTimeMS;

        //stores elapsed time since the camera started moving
        private double elapsedTimeMS;

        //see updateTransform() for comments
        private Camera2DTransform transformA, transformB;

        //target position of camera (i.e. what position camera is looking at on-screen)
        protected Vector2 nextTranslation;

        //rotation and scale for camera
        protected float nextRotation, nextScale;

        //see updateTransform() for comments
        private float lerpFactor;

        //see updateTransform() for comments
        private int foundIndex = 0;

        //set to true when path has finished InGame
        private bool bFinished = false;

        //true = play, false = pause, see play() and pause(), called in Camera2D from keyboard input
        private bool bPlay = false;

        //used to round the value from the lerp - see updateTransform()
        private const int ROUND_PRECISION = 2;

        #region PROPERTIES
        //these properties get the next transformation to be performed
        public Vector2 NEXTTRANSLATION
        {
            get
            {
                return nextTranslation;
            }
        }
        public float NEXTROTATION
        {
            get
            {
                return nextRotation;
            }
        }

        public float NEXTSCALE
        {
            get
            {
                return nextScale;
            }
        }
        #endregion

        public Camera2DPathManager(Main game, Camera2D camera2D)
            : base(game)
        {
            this.transformList = new List<Camera2DTransform>();

            //uses this in update() to change camera position if play is enabled
            this.camera2D = camera2D;
        }

        #region LIST MODIFICATION METHODS
        public void add(Camera2DTransform transform)
        {
            transformList.Add(transform);
        }

        public void remove(Camera2DTransform transform)
        {
            transformList.Remove(transform);
        }

        public void clear()
        {
            transformList.Clear();
        }
        #endregion

        #region PATH MODIFICATION METHODS
        public void play(GameTime gameTime)
        {
            startTimeMS = gameTime.TotalGameTime.TotalMilliseconds;
            //starts looking at first transform in the list
            foundIndex = 0;
            //true to play
            bPlay = true;
            //sets variable that records if we are finished InGame the path
            bFinished = false;
        }

        public void pause()
        {
            //toggle pause on and off
            bPlay = !bPlay;
        }
        //starts the start time for when the camera started moving on the path
        //clearly we need to know this so that we can say when to apply a transform (e.g. 60ms after start rotation 5 degrees)
        public void reset(GameTime gameTime)
        {
            startTimeMS = gameTime.TotalGameTime.TotalMilliseconds;
            //starts looking at first transform in the list
            foundIndex = 0;
            //resets but doesnt play
            bPlay = false;
            //sets variable that records if we are finished InGame the path
            bFinished = false;

            //reset the camera to reset the viewport
            camera2D.reset();
        }

        //sets new camera translation, rotation, and scale
        private void updateCamera()
        {
            //  System.Diagnostics.Debug.WriteLine("scale: " + NEXTSCALE);
            //   System.Diagnostics.Debug.WriteLine("translation: " + NEXTTRANSLATION);
            //   System.Diagnostics.Debug.WriteLine("rotation: " + NEXTROTATION);
            //camera2D.TRANSLATION = NEXTTRANSLATION;
            camera2D.ROTATION = NEXTROTATION;
            camera2D.SCALE = NEXTSCALE;
        }

        //called each update to find what transform should be performed based on elapsed time since start and list contents
        private void updateTransform(GameTime gameTime)
        {
            elapsedTimeMS = gameTime.TotalGameTime.TotalMilliseconds - startTimeMS;

            for (int i = foundIndex; i < transformList.Count - 1; i++)
            {
                //get next transform details
                transformA = transformList[i];
                //get transform after that
                transformB = transformList[i + 1];
                //see if the amount of elapsed time falls between the two
                if ((elapsedTimeMS >= transformA.TIME) && (elapsedTimeMS <= transformB.TIME))
                {
                    lerpFactor = getLerpFactor(elapsedTimeMS, transformA.TIME, transformB.TIME);
                    nextTranslation = getLerpedTranslation(ref transformA, ref transformB, lerpFactor);
                    nextRotation = (float)Math.Round(MathHelper.Lerp(transformA.ROTATION, transformB.ROTATION, lerpFactor), ROUND_PRECISION);
                    nextScale = (float)Math.Round(MathHelper.Lerp(transformA.SCALE, transformB.SCALE, lerpFactor), ROUND_PRECISION);

                    //records where we last found the transform
                    //this means that we dont restart from i = 0 each time update is called
                    foundIndex = i;

                    //records if we finish
                    if (i == transformList.Count - 1)
                    {
                        bFinished = true;
                    }
                    break;
                }
            }
        }

        private Vector2 getLerpedTranslation(ref Camera2DTransform transformA, ref Camera2DTransform transformB, float lerpFactor)
        {
            //takes two translations x1,y1 and x2,y2 and interpolates linearly between them using a factor    
            return new Vector2(
                (float)Math.Floor(MathHelper.Lerp(transformA.TRANSLATIONX, transformB.TRANSLATIONX, lerpFactor)),
                 (float)Math.Floor(MathHelper.Lerp(transformA.TRANSLATIONY, transformB.TRANSLATIONY, lerpFactor)));
        }

        //this factor is between 0 and 1 and tells us how much of each transform we use
        private float getLerpFactor(double elapsedTimeMS, double transformATime, double transformBTime)
        {
            //need to be sure that elapsedTimeMS >= transformATime AND transformBTime > transformATime
            if ((elapsedTimeMS >= transformATime) && (transformBTime > transformATime))
            {
                return (float)(elapsedTimeMS - transformATime) / (float)(transformBTime - transformATime);
            }
            //Returning 0 means we just use the transformA data
            return 0;
        }
        #endregion

        #region GAME COMPONENT METHODS
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //if we have already played the path then dont replay unless we call set()
            if (bPlay)
            {
                //get new transform to apply to the camera
                updateTransform(gameTime);

                //update camera with new transform
                updateCamera();
            }
            base.Update(gameTime);
        }
        #endregion
    }
}
