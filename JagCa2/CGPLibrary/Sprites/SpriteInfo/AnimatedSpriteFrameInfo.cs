using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using JagCa2;
namespace CGPLibrary
{
    public class AnimatedSpriteFrameInfo
    {
        protected Texture2D texture;
        //this is a list containing all the source rectangle color data
        protected List<Color[,]> sourceColorDataList;
        //keep showing the animation
        protected bool bRepeatAnimation;

        protected int frameStartNumber, frameWidth, frameHeight, frameRate, frameEndNumber, numberOfFrames;
        protected double frameDurationInSeconds;
        #region PROPERTIES
        public Color[,] this[int index]
        {
            get
            {
                return sourceColorDataList[index];
            }
        }
        public Texture2D TEXTURE
        {
            get
            {
                return texture;
            }

        }
        public int NUMBEROFFRAMES
        {
            get
            {
                return numberOfFrames;
            }

        }
        public double FRAMEDURATIONINSECSONDS
        {
            get
            {
                return frameDurationInSeconds;
            }

        }
        public bool REPEATANIMATION
        {
            get
            {
                return bRepeatAnimation;
            }

        }
        //Able to set Frame Width & Height, rate
        public int FRAMESTARTNUMBER
        {
            get
            {
                return frameStartNumber;
            }
            set
            {
                frameStartNumber = value;
            }
        }
        public int FRAMEWIDTH
        {
            get
            {
                return frameWidth;
            }
            set
            {
                frameWidth = value;
            }
        }
        public int FRAMEHEIGHT
        {
            get
            {
                return frameHeight;
            }
            set
            {
                frameHeight = value;
            }
        }
        public int ENDFRAME
        {
            get
            {
                return frameEndNumber;
            }
            set
            {
                frameEndNumber = value;
            }
        }
        public int FRAMERATE
        {
            get
            {
                return frameRate;
            }
            set
            {
                frameRate = value;
                //set now frame duration in seconds
                this.frameDurationInSeconds = 1.0f / frameRate;
            }
        }
        #endregion
        public AnimatedSpriteFrameInfo(Main game, int frameStartNumber, int frameEndNumber, Texture2D texture,
            int numberOfFrames, int frameRate, int frameWidth, int frameHeight, bool bRepeatAnimation)
        {
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
            this.frameStartNumber = frameStartNumber;
            this.frameRate = frameRate;
            //the time between frame updates
            this.frameDurationInSeconds = 1.0f / frameRate;

            this.frameEndNumber = -1;

            this.frameEndNumber = frameEndNumber;
            this.sourceColorDataList = new List<Color[,]>(numberOfFrames);


            this.numberOfFrames = numberOfFrames;
            this.bRepeatAnimation = bRepeatAnimation;

            this.texture = texture;
            setSourceColorDataList(texture);

        }

        #region TEXTURE ARRAY MANIPULATION FOR ANIMATION
        //converts color data from texture from 1d to 2d array
        public void setSourceColorDataList(Texture2D texture)
        {
            int width = texture.Width;
            int height = texture.Height;

            //read data into 1d array
            Color[] colors1D = new Color[width * height];
            texture.GetData(colors1D);

            //create 2d array to store data
            Color[,] colors2D = new Color[frameWidth, frameHeight];

            //read each frame into a seperate colors2D array and add it to the list
            //then when we want to now the color data for a particular frame we just query the list
            for (int i = 0; i < numberOfFrames; i++)
            {
                for (int x = 0; x < frameWidth; x++)
                {
                    for (int y = 0; y < frameHeight; y++)
                    {
                        colors2D[x, y]
                            = colors1D[x + (y * frameWidth) + frameWidth * frameHeight * i];
                    }
                }
                sourceColorDataList.Add(colors2D);
            }
        }
        #endregion


    }

}

