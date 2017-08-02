using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

using JagCa2;

namespace CGPLibrary
{
    public class TextureManager
    {
        protected Dictionary<string, Texture2D> texture2DDictonary;

        #region PROPERTIES
        public Texture2D this[string name]
        {
            get
            {
                return texture2DDictonary[name];
            }
        }

        #endregion

        public TextureManager()
        {
            texture2DDictonary = new Dictionary<string, Texture2D>();
        }

        public bool add(string name, Texture2D texture2D)
        {
            if (!texture2DDictonary.ContainsKey(name))
            {
                texture2DDictonary.Add(name, texture2D);
                return true;
            }

            return false;
        }

        public Texture2D get(string name)
        {
            return texture2DDictonary[name];
        }


        public bool remove(string name)
        {
            //find so we can nullify for garbage collection
            Texture2D texture2D = texture2DDictonary[name];
            //remove from dictionary and store return value
            bool wasRemoved = texture2DDictonary.Remove(name);
            //nullify for garbage collection
            texture2DDictonary = null;
            return wasRemoved;
        }

        public void clear()
        {
            texture2DDictonary.Clear();
        }

        public int size()
        {
            return texture2DDictonary.Count;
        }
    }


}
