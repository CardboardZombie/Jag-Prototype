using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JagCa2;

namespace CGPLibrary
{
    public class CameraManager : DrawableGameComponent
    {
        public enum SplitType: byte {NoSplit=0, HalfSplit=1}; //an enumeration of split types
        protected SplitType splitType; //a variable to store the actual split type

        protected Main game;
        protected List<Camera2D> cameraList;
        protected Camera2D activeCamera;
        protected int activeCameraIndex;

        #region PROPERTIES
        public SplitType SPLITTYPE
        {
            set
            {
                splitType = value;
            }
            get
            {
                return splitType;
            }
        }
        public int ACTIVECAMERAINDEX
        {
            get
            {
                return activeCameraIndex;
            }
            set
            {
                activeCameraIndex = ((value >= 0) && (value <= cameraList.Count)) ? value : 0;
                activeCamera = cameraList[activeCameraIndex];
            }
        }
        public Camera2D ACTIVECAMERA
        {
            get
            {
                return activeCamera;
            }
            set
            {
                activeCamera = value;
            }
        }
        public Camera2D this[int index]
        {
            get
            {
                return cameraList[index];
            }
        }
        #endregion

        public CameraManager(Main game, SplitType splitType)
            : base(game)
        {
            this.game = game;

            this.splitType = splitType;
            this.cameraList = new List<Camera2D>();
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public void add(Camera2D theCamera)
        {
            cameraList.Add(theCamera);
        }
        public void remove(Camera2D theCamera)
        {
            cameraList.Remove(theCamera);
            //tag for garbage collection
            theCamera = null;
        }
        public int size()
        {
            return cameraList.Count;
        }
        public void clear()
        {
            cameraList.Clear();
        }

        public /*override*/ void Update(GameTime gameTime, Vector2 position1, Vector2 position2)
        {
            if (splitType == SplitType.NoSplit)
            {
                cameraList[0].Update(position1);
            }
            else if(splitType == SplitType.HalfSplit)
            {
                cameraList[1].Update(position1);
                cameraList[2].Update(position2);
            }

            game.SPRITEMANAGER.Update(gameTime);
            base.Update(gameTime);
        }

        public /*override*/ void Draw(GameTime gameTime, Camera2D camera1, Camera2D camera2, Camera2D camera3)
                                       // Viewport view1, Viewport view2, Viewport view3)
        {
            if (splitType == SplitType.NoSplit)
            {
                //bug fix - 28.11.12
                ACTIVECAMERAINDEX = 0;
                //assumes camerList[0] is always fullscreen camera
                game.GraphicsDevice.Viewport = cameraList[0].VIEWPORT;
                game.SPRITEMANAGER.Draw(gameTime, camera1, game.currentState);
            }
            else if(splitType == SplitType.HalfSplit)
            {
                    //bug fix - 28.11.12
                    ACTIVECAMERAINDEX = 1;
                    game.GraphicsDevice.Viewport = cameraList[1].VIEWPORT;
                    game.SPRITEMANAGER.Draw(gameTime, camera2, game.currentState);

                    ACTIVECAMERAINDEX = 2;
                    game.GraphicsDevice.Viewport = cameraList[2].VIEWPORT;
                    game.SPRITEMANAGER.Draw(gameTime, camera3, game.currentState);

            }
            //base.Draw(gameTime);
        }

    }
}