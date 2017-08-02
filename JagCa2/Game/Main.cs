using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CGPLibrary;
using JagCa2;

namespace JagCa2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        public enum GameState{ MainMenu, InGame, Win, Lose }
        public GameState currentState = GameState.MainMenu;

        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont menuFont;
        private AnimatedPlayerSprite player1;
        private AnimatedPlayerSprite player2;
        private ProgressSprite hud1;
 
        private int score = 0;
        private int lives = 2;

        private SpriteManager spriteManager;
        private KeyboardManager keyboardManager;
        private ScreenManager screenManager;
        private MouseManager mouseManager;
        private MenuManager menuManager;
        private SoundManager soundManager;
        private CameraManager cameraManager;
        private TextureManager textureManager;
        private Camera2DPathManager pathManager;

        public Texture2D winTexture, introTexture, loseTexture;
        public static Texture2D DevPixal;

        private Camera2D cameraFull, cameraP1, cameraP2;
        public SoundEffectInstance bg;


        #region PROPERTIES
        public AnimatedPlayerSprite PLAYER1
        {
            get
            {
                return player1;
            }
        }
        public AnimatedPlayerSprite PLAYER2
        {
            get
            {
                return player2;
            }
        }
        public ProgressSprite HUD1
        {
            get 
            {
                return hud1;
            }
        }
        public int SCORE
        {
            get
            {
                return score;
            }
            set 
            {
                score = value;
            }
        }
        public int LIVES
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
            }
        }
        public GraphicsDeviceManager GRAPHICS
        {
            get
            {
                return graphics;
            }
        }
        public SpriteBatch SPRITEBATCH
        {
            get
            {
                return spriteBatch;
            }
        }
        public MouseManager MOUSEMANAGER
        {
            get
            {
                return mouseManager;
            }
        }
        public KeyboardManager KEYBOARDMANAGER
        {
            get
            {
                return keyboardManager;
            }
        }
        public ScreenManager SCREENMANAGER
        {
            get
            {
                return screenManager;
            }
        }

        public SpriteManager SPRITEMANAGER
        {
            get
            {
                return spriteManager;
            }
        }

        public MenuManager MENUMANAGER
        {
            get
            {
                return menuManager;
            }
        }
        public SoundManager SOUNDMANAGER
        {
            get
            {
                return soundManager;
            }
        }
        public CameraManager CAMERAMANAGER
        {
            get
            {
                return cameraManager;
            }
        }
        public Camera2DPathManager PATHMANAGER
        {
            get
            {
                return pathManager;
            }
        }
        #endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            addManagers();
            loadGameTextures();
            addCamera();
            addCameraPathManager();
            addSprites();
            addSounds();

            bg = soundManager.getEffectInstance("background");
            bg.Play();

            menuFont = Content.Load<SpriteFont>(@"Fonts\\Menu\\menuFont");
        }

        private void addManagers()
        {
            this.spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
 
            this.keyboardManager = new KeyboardManager(this);
            Components.Add(keyboardManager);

            this.screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            this.mouseManager = new MouseManager(this);
            Components.Add(mouseManager);

            this.textureManager = new TextureManager();

            this.soundManager = new SoundManager();

            String[] arrayMenuTextureNames =  {"Images\\Menu\\mainmenu", "Images\\Menu\\volumemenu", "Images\\Menu\\losemenu", "Images\\Menu\\winmenu"};
            this.menuManager = new MenuManager(this, arrayMenuTextureNames, "Fonts\\Menu\\menuFont", new Integer2(20, 20));

            this.cameraManager = new CameraManager(this, CameraManager.SplitType.NoSplit);
            cameraFull = new Camera2D(this, GraphicsDevice.Viewport, /*cameraTarget1,*/ 0, 1, false);
            cameraManager.add(cameraFull);
            Components.Add(cameraManager);
        }

        private void addCamera()
        {
            //first split screen
            Viewport cameraViewPortA = GraphicsDevice.Viewport;
            cameraViewPortA.Height = cameraViewPortA.Height / 2;
            cameraP1 = new Camera2D(this, cameraViewPortA, /*player1.POSITION,*/ 0, 1, false);
            cameraManager.add(cameraP1);

            //second split screen
            Viewport cameraViewPortB = GraphicsDevice.Viewport;
            cameraViewPortB.Y = cameraViewPortA.Height;
            cameraViewPortB.Height = cameraViewPortB.Height / 2;
            cameraP2 = new Camera2D(this, cameraViewPortB, /*player2.POSITION,*/ 0, 1, true);
            cameraManager.add(cameraP2);
        }

        public void addCameraPathManager()
        {
            this.pathManager = new Camera2DPathManager(this, CAMERAMANAGER[0]);
            Components.Add(pathManager);

            Vector2 initialPosition = new Vector2(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);
            pathManager.add(new Camera2DTransform(initialPosition, 0, 1, 0));
            pathManager.add(new Camera2DTransform(initialPosition + new Vector2(50, 0), 0, 1, 1000));
            pathManager.add(new Camera2DTransform(initialPosition + new Vector2(100, 0), 0, 1, 2000));
            pathManager.add(new Camera2DTransform(initialPosition + new Vector2(200, 0), 0, 1, 3000));
            pathManager.add(new Camera2DTransform(initialPosition + new Vector2(250, 0), 0, 0.5f, 4000));
            pathManager.add(new Camera2DTransform(initialPosition + new Vector2(400, 0), 0, 1, 5000));
            pathManager.add(new Camera2DTransform(initialPosition, 0, 1, 6000));
        }

        private void loadGameTextures()
        {
            textureManager.add("heroTexture" , Content.Load<Texture2D>(@"Images\Characters\Hero1"));
            textureManager.add("platformTexture" , Content.Load<Texture2D>(@"Images\Tiles\Platform1"));
            textureManager.add("grueTexture",  Content.Load<Texture2D>(@"Images\Enemies\Grue"));
            textureManager.add("backgroundTexture" , Content.Load<Texture2D>(@"Images\Backgrounds\Background"));
            textureManager.add("lifeTexture" , Content.Load<Texture2D>(@"Images\PickUps\apple"));
            textureManager.add("rubyTexture" , Content.Load<Texture2D>(@"Images\PickUps\ruby"));

            textureManager.add("introTexture", Content.Load<Texture2D>(@"Images\Menu\opening"));
            textureManager.add("loseTexture", Content.Load<Texture2D>(@"Images\Menu\endGame"));
            textureManager.add("winTexture", Content.Load<Texture2D>(@"Images\Menu\win2"));

            introTexture = textureManager["introTexture"];
            loseTexture = textureManager["loseTexture"];
            winTexture = textureManager["winTexture"];

            DevPixal = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            DevPixal.SetData(new[] { Color.White });
        }

        private void addSounds()
        {
            SoundEffectInfo bgM = new SoundEffectInfo(this, "background", "Audio\\background",
                   0, 0, 0, true);

            //Add sounds to the soundmanager
            soundManager.add(new SoundEffectInfo(this, "start", "Audio\\start", 0.5f, 0.5f, 0, false));
            soundManager.add(new SoundEffectInfo(this, "boing_x", "Audio\\boing_x", 0.5f, 0.5f, 0, false));
            soundManager.add(new SoundEffectInfo(this, "boing_spring", "Audio\\boing_spring", 1, 0, 0, false));
            soundManager.add(new SoundEffectInfo(this, "ohh", "Audio\\ohh", 1, 0, 0, false));
            soundManager.add(new SoundEffectInfo(this, "groan", "Audio\\groan", 1, 0, 0, false));

            soundManager.add(bgM);
        }

        private void addSprites()
        {
            
            Texture2D backgroundTexture = textureManager["backgroundTexture"];
            Texture2D heroTexture = textureManager["heroTexture"];
            Texture2D platformTexture = textureManager["platformTexture"];
            Texture2D lifeTexture = textureManager["lifeTexture"];
            Texture2D rubyTexture = textureManager["rubyTexture"];
            Texture2D grueTexture = textureManager["grueTexture"];

            BackgroundSprite background1 = new BackgroundSprite(this, backgroundTexture,
                    new Rectangle(0, 0, 1807, Window.ClientBounds.Height/2), Color.White, false);
            spriteManager.add(background1);

            BackgroundSprite background2 = new BackgroundSprite(this, backgroundTexture,
                               new Rectangle(0, Window.ClientBounds.Height/2, 1807, Window.ClientBounds.Height/2), Color.White, false);
            spriteManager.add(background2);

            AnimatedPlayerInfo playerAInfo = new AnimatedPlayerInfo(Keys.A, Keys.D, Keys.W, Keys.X, Keys.C, 3f);
            AnimatedPlayerInfo playerBInfo = new AnimatedPlayerInfo(Keys.Left, Keys.Right, Keys.Up, Keys.NumPad1, Keys.NumPad2, 3f);

            AnimatedSpriteFrameInfo plyrFrameInfoA = new AnimatedSpriteFrameInfo(4, 4, 60, 74, 0);
            AnimatedSpriteFrameInfo plyrFrameInfoB = new AnimatedSpriteFrameInfo(0, 4, 60, 74, 4);
            AnimatedSpriteFrameInfo plyrFrameInfoC = new AnimatedSpriteFrameInfo(4, 60, 74, 4);

            AnimatedSpriteFrameInfo grueFrame = new AnimatedSpriteFrameInfo(0, 3, 125, 125, 3);

            List<AnimatedSpriteFrameInfo> grueFrameinfo = new List<AnimatedSpriteFrameInfo>();
            grueFrameinfo.Add(grueFrame);

            List<AnimatedSpriteFrameInfo> frameInfoList = new List<AnimatedSpriteFrameInfo>();
            frameInfoList.Add(plyrFrameInfoA);
            frameInfoList.Add(plyrFrameInfoB);
            frameInfoList.Add(plyrFrameInfoC);

            player1= new AnimatedPlayerSprite(this, heroTexture, new Rectangle(Window.ClientBounds.Width/2, (Window.ClientBounds.Height/2)-43, 25, 25),
                    Color.LawnGreen,frameInfoList,0, 0, playerAInfo, true, 1);
            player2 = new AnimatedPlayerSprite(this, heroTexture,new Rectangle(Window.ClientBounds.Width/2, Window.ClientBounds.Height-43, 25, 25),
                    Color.CadetBlue, frameInfoList, 0, 0, playerBInfo, true, 2);
            
            spriteManager.add(player1);
            spriteManager.add(player2);

            Sprite platformV1  = new Sprite(this, platformTexture, new Rectangle(-18, 0, 18, Window.ClientBounds.Height),
                               Color.White, true);
            spriteManager.add(platformV1);

            LifePickUp life1 = new LifePickUp(this, lifeTexture, new Rectangle(600, (Window.ClientBounds.Height/2)-43, 25, 25),
                                    Color.White, true);
            LifePickUp life2 = new LifePickUp(this, lifeTexture, new Rectangle(600, Window.ClientBounds.Height - 43, 25, 25),
                                    Color.White, true);

            spriteManager.add(life1);
            spriteManager.add(life2);

            RubyPickUp rubyWin = new RubyPickUp(this, rubyTexture, new Rectangle(1700, Window.ClientBounds.Height - 108, 50, 100),
                Color.White, true, true);

            spriteManager.add(rubyWin);

            AnimatedEnemySprite enemy1 = new AnimatedEnemySprite(this, grueTexture, new Rectangle(200, 100, 83, 83),
                                            Color.White, grueFrameinfo, 0, 0, true);
            spriteManager.add(enemy1);

            hud1 = new ProgressSprite(this, DevPixal, player1, "Fonts\\Menu\\menuFont");
            
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                //Press Any Key to Begin
                case GameState.MainMenu:
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        currentState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;

                case GameState.InGame:
                    cameraManager.SPLITTYPE = CameraManager.SplitType.HalfSplit;
                    cameraManager.Update(gameTime, player1.POSITION, player2.POSITION);
                    base.Update(gameTime);
                    break;

                case GameState.Lose:
                    GraphicsDevice.Viewport = cameraFull.VIEWPORT;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        currentState = GameState.MainMenu;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                        Exit();
                    break;

                case GameState.Win:
                    GraphicsDevice.Viewport = cameraFull.VIEWPORT;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        currentState = GameState.MainMenu;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                        Exit();
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (currentState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(introTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    spriteBatch.End();
                    break;

                case GameState.InGame:

                    cameraManager.Draw(gameTime, cameraFull, cameraP1, cameraP2);

                    break;

                case GameState.Lose:
                    spriteBatch.Begin();
                    spriteBatch.Draw(loseTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                    string gameover = "You Died!!";
                    spriteBatch.DrawString(menuFont, gameover, new Vector2(10, 10), Color.White);

                    gameover = "(Press ENTER to Play Again)";
                    spriteBatch.DrawString(menuFont, gameover, new Vector2((Window.ClientBounds.Width / 2) - (menuFont.MeasureString(gameover).X / 2),
                            (Window.ClientBounds.Height / 2) - (menuFont.MeasureString(gameover).Y / 2) + 100), Color.White);
                    spriteBatch.End();
                    break;
                case GameState.Win:
                    spriteBatch.Begin();
                    
                    spriteBatch.Draw(winTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    gameover = "Thanks for Playing!!";
                    spriteBatch.DrawString(menuFont, gameover, new Vector2(10, 10), Color.Black);
        
                    spriteBatch.End();
                    break;
            }
        }
    }
}
