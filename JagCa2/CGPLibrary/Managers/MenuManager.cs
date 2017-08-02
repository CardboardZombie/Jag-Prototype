using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using JagCa2;

namespace CGPLibrary
{
    public class MenuManager : DrawableGameComponent
    {
        protected List<MenuItem> menuItemList;

        public Main game;
        public SpriteFont menuFont;

        private Texture2D[] menuTextures;
        private Rectangle textureRectangle;

        private MenuItem menuResume, menuExit, menuAudio;
        private MenuItem menuVolumeUp, menuVolumeDown, menuBack;

        protected bool bVisible = false;

        #region PROPERTIES
        public bool VISIBLE
        {
            get
            {
                return bVisible;
            }
            set
            {
                bVisible = value;
            }
        }
        #endregion




        protected int currentMenuTextureIndex = 0; //0 = main, 1 = volume

        public MenuManager(Main game, String[] strMenuTextures, String strMenuFont,
                                            Integer2 textureBorderPadding)
            : base(game)
        {
            this.game = game;

            //nmcg - create an array of textures
            this.menuTextures = new Texture2D[strMenuTextures.Length];

            //nmcg - load the textures
            for (int i = 0; i < strMenuTextures.Length; i++)
            {
                this.menuTextures[i] = game.Content.Load<Texture2D>(@"" + strMenuTextures[i]);
            }

            //nmcg - load menu font
            this.menuFont = game.Content.Load<SpriteFont>(@"" + strMenuFont);

            //nmcg - stores all menu item (e.g. Save, Resume, Exit) objects
            this.menuItemList = new List<MenuItem>();


            this.textureRectangle = new Rectangle(textureBorderPadding.X, textureBorderPadding.Y,
                game.SCREENMANAGER.WINDOWWIDTH - 2 * textureBorderPadding.X,
                game.SCREENMANAGER.WINDOWHEIGHT - 2 * textureBorderPadding.Y);

        }
        public override void Initialize()
        {
            //add the basic items - "Resume", "Save", "Exit"
            initialiseMenuOptions();
            showMainMenuScreen();


            base.Initialize();
        }

        private void initialiseMenuOptions()
        {
            //nmcg - add the menu items to the list
            this.menuResume = new MenuItem(GameData.MENU_RESUME, GameData.MENU_RESUME,
                new Rectangle(50, 50, 110, 30), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuAudio = new MenuItem(GameData.MENU_AUDIO, GameData.MENU_AUDIO,
               new Rectangle(50, 100, 65, 30), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuExit = new MenuItem(GameData.MENU_EXIT, GameData.MENU_EXIT,
                new Rectangle(50, 150, 50, 30), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);

            //nmcg - second menu - audio settings
            this.menuVolumeUp = new MenuItem(GameData.MENU_VOLUMEUP, GameData.MENU_VOLUMEUP,
               new Rectangle(550, 50, 65, 30), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuVolumeDown = new MenuItem(GameData.MENU_VOLUMEDOWN, GameData.MENU_VOLUMEDOWN,
               new Rectangle(550, 100, 65, 30), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);
            this.menuBack = new MenuItem(GameData.MENU_BACK, GameData.MENU_BACK,
                new Rectangle(550, 150, 50, 30), GameData.MENU_INACTIVE_COLOR, GameData.MENU_ACTIVE_COLOR);

            //nmcg - static variable used by the MenuItem class
            MenuItem.menuManager = this;
        }

        private void showMainMenuScreen()
        {
            add(menuResume);
            add(menuAudio);
            add(menuExit);
            currentMenuTextureIndex = 0;
        }

        private void showVolumeMenuScreen()
        {
            add(menuVolumeUp);
            add(menuVolumeDown);
            add(menuBack);
            currentMenuTextureIndex = 1;
        }
        private void showLoseScreen()
        {
            currentMenuTextureIndex = 2;
        }
        private void showWinScreen()
        {
            currentMenuTextureIndex = 3;
        }


        public void add(MenuItem theMenuItem)
        {
            menuItemList.Add(theMenuItem);
        }

        public void remove(MenuItem theMenuItem)
        {
            menuItemList.Remove(theMenuItem);
        }

        public void removeAll()
        {
            menuItemList.Clear();
        }

        public override void Update(GameTime gameTime)
        {

            for (int i = 0; i < menuItemList.Count; i++)
            {
                //nmcg - call the update() to test for collisions with the mouse
                menuItemList[i].Update(gameTime);
                if (menuItemList[i].isItemClicked())
                {
                    menuAction(menuItemList[i].name);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SPRITEBATCH.Begin();
            //nmcg - draw whatever background we expect to see based on what menu or sub-menu we are viewing
            game.SPRITEBATCH.Draw(menuTextures[currentMenuTextureIndex], textureRectangle, Color.White);

            //nmcg - draw the text on top of the background
            for (int i = 0; i < menuItemList.Count; i++)
            {
                menuItemList[i].Draw(game.SPRITEBATCH);
            }

            game.SPRITEBATCH.End();

            base.Draw(gameTime);
        }

        //nmcg - perform whatever actions are listed on the menu
        private void menuAction(String name)
        {
            if (name.Equals(GameData.MENU_RESUME))
            {
                removeAll();  //clear the menu of all menu items

                this.game.Components.Remove(this.game.MENUMANAGER);
                //bug - shouldnt remove the mouse manager
                // this.game.Components.Remove(this.game.MOUSEMANAGER);
                Sprite.SPRITEMANAGER.PAUSE = false;
                this.game.MENUMANAGER.VISIBLE = false;

            }
            else if (name.Equals(GameData.MENU_AUDIO))
            {
                remove(menuResume);
                remove(menuAudio);
                remove(menuExit);
                showVolumeMenuScreen();
            }
            else if (name.Equals(GameData.MENU_EXIT))
            {
                this.game.Exit();
            }
            else if (name.Equals(GameData.MENU_BACK))
            {
                remove(menuVolumeUp);
                remove(menuVolumeDown);
                remove(menuBack);

                showMainMenuScreen();
            }
            else if (name.Equals(GameData.MENU_VOLUMEUP))
            { 
                game.bg.Volume = 1f;
            }
            else if (name.Equals(GameData.MENU_VOLUMEDOWN))
            {
                game.bg.Volume = 0.5f;
            }
        }
    }
}
