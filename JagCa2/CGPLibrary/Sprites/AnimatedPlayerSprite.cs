using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JagCa2;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace CGPLibrary
{
    public class AnimatedPlayerSprite : AnimatedSprite
    {
        protected List<AnimatedSpriteFrameInfo> frameInfoList = new List<AnimatedSpriteFrameInfo>();
        protected AnimatedPlayerInfo playerInfo;
        protected bool hasJumped;
        //protected int numLives;
        //protected int score;
        protected int playerNo, jumpSpeed;
        protected int gravitY;
        protected bool win = false;

        public AnimatedPlayerSprite(Main game, Texture2D texture,
            Rectangle destinationRectangle, Color color,
            List<AnimatedSpriteFrameInfo> frameInfoList, float rotation, float layerDepth,
            AnimatedPlayerInfo playerInfo, bool bCollidable,
            int playerNo)
            : base(game, texture, destinationRectangle, color,
                        frameInfoList[0], rotation, layerDepth, bCollidable)
        {
            for (int i = 0; i < frameInfoList.Count; i++)
            {
                this.frameInfoList.Add(frameInfoList[i]);
            }
            this.playerInfo = playerInfo;
            //lock into one of 4 possible rotations
            this.rotation = GameData.setValidRotation(rotation);
            this.hasJumped = false;
            //this.numLives = 2;
            this.playerNo = playerNo;
            this.gravitY = destinationRectangle.Y;
            this.jumpSpeed = 0;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public void jump()
        {
            if (hasJumped)
            {

                destinationRectangle.Y += jumpSpeed;//Making it go up
                jumpSpeed += 1;//Some math (explained later)
                if (destinationRectangle.Y >= gravitY) // if hes above ground 
                //If it's farther than ground
                {
                    destinationRectangle.Y = gravitY;//Then set it on ground
                    hasJumped = false;
                }
            }
            else
            {
                if (game.KEYBOARDMANAGER.isKeyDown(playerInfo.UP))
                {
                    hasJumped = true;
                    jumpSpeed = -13;//Give it upward thrust

                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            jump();
            
            if (game.KEYBOARDMANAGER.isKeyDown(playerInfo.LEFT))
            {
                if (checkCollide(-1, 0, 0, 1))
                {
                    destinationRectangle.X += 1;
                }
                else
                {
                    updatePosition(-15, 0);
                }
                if (frameInfo != frameInfoList[1])
                {
                    base.SetFrameInfo(frameInfoList[1]);
                }
                this.flip = SpriteEffects.FlipHorizontally;
            }
            else if (game.KEYBOARDMANAGER.isKeyDown(playerInfo.RIGHT))
            {

                if (frameInfo != frameInfoList[1])
                {
                    base.SetFrameInfo(frameInfoList[1]);
                }
                this.flip = SpriteEffects.None;

                if (checkCollide(1, 0, 0, 1))
                {
                    destinationRectangle.X -= 1;
                }
                else
                {
                    //this.color = Color.Blue;
                    updatePosition(15, 0);
                }

            }
            else if (game.KEYBOARDMANAGER.isKeyDown(playerInfo.ATT))
            {
                Attack();
            }
            else if (game.KEYBOARDMANAGER.isKeyDown(playerInfo.ACT))
            {
                Action();
            }
            else
            {
                base.SetFrameInfo(frameInfoList[0]);
            }


            /*if(!checkCollide(0, 1, 5, 0))
            {
                hasJumped = true;
                //updatePosition(0, 5);
                base.SetFrameInfo(frameInfoList[2]);
            }*/

            if (hasJumped == true)
            {

                if (checkCollide(0, 1, 5, 0))
                {
                    hasJumped = false;
                }
                else
                {
                    updatePosition(0, 5);
                }
            }

            //updateState();

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public bool checkCollide(int directionX, int directionY, int vSpeed, int hSpeed)
        {
            for (int i = 0; i < spriteManager.spriteList.Count; i++)
            {
                Sprite tempSprite = spriteManager.spriteList[i];

                if (tempSprite != this)
                {

                    if (tempSprite.IS_COLLIDABLE)
                    {
                        // if (Collision.BSPIntersects(this, tempSprite))
                        //{


                        /*if (tempSprite.GetType().Equals(typeof(ShieldPickUp)))
                        {
                            ShieldPickUp pickUp = (ShieldPickUp)tempSprite;

                            if (Collision.RectangularIntersects(this, pickUp))
                            {
                                spriteManager.remove(tempSprite);
                                this.color = pickUp.EMBUE;

                                SoundEffectInstance effectInstance
                                   = Sprite.spriteManager.game.SOUNDMANAGER.getEffectInstance("start");

                                effectInstance.Play();
                                return true;
                            }
                        }*/
                        if (tempSprite.GetType().Equals(typeof(RubyPickUp)))
                        {
                            RubyPickUp pickUp = (RubyPickUp)tempSprite;

                            Rectangle testRect = new Rectangle((this.destinationRectangle.X + hSpeed * directionX),
                                                                    (this.destinationRectangle.Y + vSpeed * directionY),
                                                                    this.destinationRectangle.Width, this.destinationRectangle.Height);

                            if (Collision.PerPixelIntersects(testRect, this.textureData,
                                                                tempSprite.BOUNDINGRECTANGLE, pickUp.TEXTURE_DATA))
                            {
                                //Remove the Sprite
                                spriteManager.remove(tempSprite);
                                //Check for Win
                                if (pickUp.WIN)
                                {
                                    win = true;
                                    updateState();
                                }

                                SoundEffectInstance effectInstance
                                   = Sprite.spriteManager.game.SOUNDMANAGER.getEffectInstance("start");

                                effectInstance.Play();

                                return true;
                            }
                        }
               
                         if (tempSprite.GetType().Equals(typeof(LifePickUp)))
                        {
                            LifePickUp pickUp = (LifePickUp)tempSprite;

                            Rectangle testRect = new Rectangle((this.destinationRectangle.X + hSpeed * directionX),
                                                                    (this.destinationRectangle.Y + vSpeed * directionY),
                                                                    this.destinationRectangle.Width, this.destinationRectangle.Height);

                            if (Collision.PerPixelIntersects(testRect, this.textureData,
                                                                tempSprite.BOUNDINGRECTANGLE, pickUp.TEXTURE_DATA))
                            {
                                spriteManager.remove(tempSprite);
                                game.LIVES++;
                                game.SCORE += 100;
                                updateState();

                                SoundEffectInstance effectInstance
                                   = game.SOUNDMANAGER.getEffectInstance("start");

                                effectInstance.Play();
                                return true;
                            }
                        }
                        /*else if (tempSprite.GetType().Equals(typeof(AnimatedBarrierSprite)))
                        {
                            AnimatedBarrierSprite barrier = (AnimatedBarrierSprite)tempSprite;

                            Rectangle testRect = new Rectangle((this.destinationRectangle.X + hSpeed * directionX),
                                                                    (this.destinationRectangle.Y + vSpeed * directionY),
                                                                    this.destinationRectangle.Width, this.destinationRectangle.Height);
                            if (Collision.PerPixelIntersects(testRect, this.textureData,
                                                                barrier.BOUNDINGRECTANGLE, barrier.TEXTURE_DATA))
                            {
                                if (this.color != barrier.PASSCOLOR)
                                {
                                    return true;
                                }
                            }
                        }*/
                        else if ((tempSprite.GetType().Equals(typeof(AnimatedEnemySprite))))
                        {
                            AnimatedEnemySprite enemy = (AnimatedEnemySprite)tempSprite;

                            Rectangle testRect = new Rectangle((this.destinationRectangle.X + hSpeed * directionX),
                                                                    (this.destinationRectangle.Y + vSpeed * directionY),
                                                                    this.destinationRectangle.Width, this.destinationRectangle.Height);
                            //enemy.SetFrameInfo(enemy.LIST[1]);
                            if (Collision.PerPixelIntersects(testRect, this.textureData,
                                                               enemy.BOUNDINGRECTANGLE, enemy.TEXTURE_DATA))
                            {
                                game.LIVES--;
                                updateState();
                            }
                        }
                        else
                        {
                            Rectangle testRect = new Rectangle((this.destinationRectangle.X + hSpeed * directionX),
                                                                   (this.destinationRectangle.Y + vSpeed * directionY),
                                                                   this.destinationRectangle.Width, this.destinationRectangle.Height);


                            //if (Collision.PerPixelIntersects(this.BOUNDINGRECTANGLE, this.TEXTURE_DATA,
                                                                //tempSprite.BOUNDINGRECTANGLE, tempSprite.TEXTURE_DATA))
                            if(Collision.RectangularIntersects(this, tempSprite))
                            {

                                if (directionY >= 1)
                                {
                                    destinationRectangle.Y = tempSprite.BOUNDINGRECTANGLE.Top - (texture.Height);

                                }
                                else if (directionY <= -1)
                                {
                                    destinationRectangle.Y += 15;
                                    return true;
                                }


                                return true;
                            }
                        }
                       
                    }
                }


            }

            return false;
        }

        private void Attack()
        {
            if (hasJumped == false)
            {
                if (checkCollide(0, -1, 135, 0))
                {
                    //destinationRectangle.Y += texture.Height / 2;
                }
                else
                {
                    hasJumped = true;
                    updatePosition(0, -135);

                    base.SetFrameInfo(frameInfoList[2]);
                }

            }
        }
        private void Action()
        {
            if (hasJumped == false)
            {
                if (checkCollide(0, -1, 135, 0))
                {
                    //destinationRectangle.Y += texture.Height / 2;
                }
                else
                {
                    hasJumped = true;
                    updatePosition(0, -135);

                    base.SetFrameInfo(frameInfoList[2]);
                }

            }
        }

        private void updateState()
        {
            if (game.LIVES <= 0)
            {
                game.currentState = Main.GameState.Lose;
            }
            else if (win)
            {
                game.currentState = Main.GameState.Win;
            }
        }

    }
}