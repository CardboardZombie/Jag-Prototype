using System;
using Microsoft.Xna.Framework;


namespace JagCa2
{
    public class GameData
    {
        #region MENU_STRINGS;
        //all the strings shown to the user through the menu
        public static String GAME_TITLE = "JAG";
        public static String MENU_RESUME = "Resume";
        public static String MENU_SAVE = "Save";
        public static String MENU_AUDIO = "Audio";
        public static String MENU_EXIT = "Exit";

        public static String MENU_VOLUMEUP = "Volume Up";
        public static String MENU_VOLUMEDOWN = "Volume Down";
        public static String MENU_BACK = "Back";

        public static Color MENU_INACTIVE_COLOR = Color.Blue;
        public static Color MENU_ACTIVE_COLOR = Color.Red;

        public static float PLAYERUP = MathHelper.PiOver2; //90
        public static float PLAYERLEFT = MathHelper.Pi; //180
        public static float PLAYERDOWN = 3 * MathHelper.PiOver2; //270
        public static float PLAYERRIGHT = 0;  //0
        #endregion;

        //tests proposed player rotation and sets to one of four fixed values
        public static float setValidRotation(float rotation)
        {
            if ((rotation != PLAYERUP) || (rotation != PLAYERLEFT)
                    || (rotation != PLAYERDOWN) || (rotation != PLAYERRIGHT))
            {
                return PLAYERRIGHT; //default rotation is facing right
            }
            return rotation;
        }
    }
}
