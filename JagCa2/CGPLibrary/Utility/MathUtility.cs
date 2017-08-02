using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace CGPLibrary
{
    public class MathUtility
    {

        /// <summary>
        /// Returns a normalized vector with an angle (relative to UnitX) of between loAngle and hiAngle
        /// </summary>
        /// <param name="randomValue">Max value for the random number generator</param>
        /// <param name="loAngle">Minimum angle from UnitX</param>
        /// <param name="hiAngle">Maximum angle from UnitX</param>
        /// <returns>Normalized vector</returns>
        public static Vector2 getRandomNormalizedVectorWithinRange(int randomValue, int loAngle, int hiAngle)
        {
            int randomX = 0, randomY = 0;
            float degrees = 0;
            Vector2 temp;

            do
            {
                do
                {
                    randomX = new Random().Next(randomValue) - (int)(randomValue / 2);
                    randomY = new Random().Next(randomValue) - (int)(randomValue / 2);
                } while ((randomX == 0) || (randomY == 0) || (randomX == randomY));

                temp = Vector2.Normalize(new Vector2((float)Math.Abs(randomX), (float)Math.Abs(randomY)));
                degrees = MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(temp, Vector2.UnitX)));
            } while (degrees <= loAngle || degrees >= hiAngle);


            // System.Diagnostics.Debug.WriteLine(degrees);

            return Vector2.Normalize(new Vector2(randomX, randomY));
        }


    }
}
