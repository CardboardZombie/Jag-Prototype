using Microsoft.Xna.Framework.Input;
namespace CGPLibrary
{
    public class AnimatedPlayerInfo
    {
        //add name!!!!

        protected Keys keyLeft, keyRight, keyUp, keyAttack, keyAction;
        protected float moveSpeed;

        #region PROPERTIES
        public Keys UP
        {
            get
            {
                return keyUp;
            }
        }
        public Keys LEFT
        {
            get
            {
                return keyLeft;
            }
        }
        public Keys RIGHT
        {
            get
            {
                return keyRight;
            }
        }
        public Keys ATT
        {
            get
            {
                return keyAttack;
            }
        }
        public Keys ACT 
        {
            get
            {
                return keyAction;
            }
        }
        public float SPEED
        {
            get 
            {
                return moveSpeed;
            }
        }
        #endregion

        //turn speed???
        public AnimatedPlayerInfo(Keys keyLeft, Keys keyRight, 
            Keys keyUp, Keys KeyAttack, Keys KeyAction, float moveSpeed)
        { 
            this.keyLeft = keyLeft; 
            this.keyRight = keyRight;
            this.keyUp = keyUp;
            this.keyAttack = KeyAttack;
            this.keyAction = KeyAction;
            this.moveSpeed = moveSpeed;
        }
    }
}
