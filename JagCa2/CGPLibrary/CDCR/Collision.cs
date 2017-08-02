using Microsoft.Xna.Framework;
using System;
using JagCa2;

namespace CGPLibrary
{
    public class Collision
    {
        protected static float alphaThreshold = 10;
        public static Rectangle topLeft, topRight, bottomLeft, bottomRight;

        public static void Initialise(Main game)
        {
            int halfWidth = (int)Math.Round(game.GRAPHICS.PreferredBackBufferWidth / 2.0f);
            int halfHeight = (int)Math.Round(game.GRAPHICS.PreferredBackBufferHeight / 2.0f);

            topLeft = new Rectangle(0, 0, halfWidth, halfHeight);
            topRight = new Rectangle(halfWidth + 1, 0, halfWidth, halfHeight);
            bottomLeft = new Rectangle(0, halfHeight + 1, halfWidth, halfHeight);
            bottomRight = new Rectangle(halfWidth + 1, halfHeight + 1, halfWidth, halfHeight);
        }

        public static bool Intersects(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }

        public static bool Touches(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect or touch sides
            return (a.Right >= b.Left && a.Left <= b.Right &&
                    a.Bottom >= b.Top && a.Top <= b.Bottom);
        }

        public static bool Contains(Rectangle a, Rectangle b, int inflate)
        {
            // check if two Rectangles intersect or touch sides
            return (a.Right >= b.Left && a.Left <= b.Right &&
                    a.Bottom >= b.Top && a.Top <= b.Bottom);
        }


        //nmcg - 29.10.11
        #region PER PIXEL NON AXIS ALIGNED
        /// <summary>
        /// Calculates an axis aligned rectangle which fully contains an arbitrarily
        /// transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the trasnformed rectangle.</returns>
        public static Rectangle CalculateTransformedBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            //   Matrix inverseMatrix = Matrix.Invert(transform);
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)Math.Round(min.X), (int)Math.Round(min.Y),
                                 (int)Math.Round(max.X - min.X), (int)Math.Round(max.Y - min.Y));
        }
        
        public static Vector2 IntersectsNonAA(Color[,] tex1, Matrix mat1,
            Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);
            Vector2 pos1 = Vector2.Zero, pos2 = Vector2.Zero;
            int x2 = 0, y2 = 0;

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    pos1 = new Vector2(x1, y1);
                    pos2 = Vector2.Transform(pos1, mat1to2);
                    x2 = (int)Math.Round(pos2.X);
                    y2 = (int)Math.Round(pos2.Y);
                    if (((x2 >= 0) && (x2 < width2)) && ((y2 >= 0) && (y2 < height2)))
                    {
                        if ((tex1[x1, y1].A > alphaThreshold) && (tex2[x2, y2].A > alphaThreshold))
                        {
                            return Vector2.Transform(pos1, mat1);
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        #endregion

        #region BSP
        public static bool IntersectsBSP(CollidableSprite a, CollidableSprite b)
        {
            return ((a.SECTOR & b.SECTOR) != 0) ? true : false;
        }
        public static bool IntersectsBSP(Rectangle boundsA, CollidableSprite b)
        {
            return ((getSector(boundsA) & b.SECTOR) != 0) ? true : false;
        }
        public static bool IntersectsBSP(Rectangle boundsA, Rectangle boundsB)
        {
            if ((getSector(boundsA) & getSector(boundsB)) != 0)
            {
                return true;
            }
            return false;
        }
        public static int setSector(Rectangle bounds)
        {
            int bSector = 0;

            if (bounds.Intersects(Collision.topLeft))
            {
                bSector |= 1;
            }
            if (bounds.Intersects(Collision.topRight))
            {
                bSector |= 2;
            }
            if (bounds.Intersects(Collision.bottomLeft))
            {
                bSector |= 4;
            }
            if (bounds.Intersects(Collision.bottomRight))
            {
                bSector |= 8;
            }

            return bSector;
        }
        public static int getSector(Rectangle bounds)
        {
            int sector = 0;

            if (bounds.Intersects(Collision.topLeft))
            {
                sector |= 1;
            }
            if (bounds.Intersects(Collision.topRight))
            {
                sector |= 2;
            }
            if (bounds.Intersects(Collision.bottomLeft))
            {
                sector |= 4;
            }
            if (bounds.Intersects(Collision.bottomRight))
            {
                sector |= 8;
            }
            return sector;
        }
        #endregion

        #region PER PIXEL AXIS ALIGNED
        public static bool IntersectsAA(Color[] textureColorDataA, Color[] textureColorDataB,
                                        Rectangle boundsRectangleA, Rectangle boundsRectangleB)
        {
            // Find the extents of the rectangle intersection
            int top = Math.Max(boundsRectangleA.Top, boundsRectangleB.Top);
            int bottom = Math.Min(boundsRectangleA.Bottom, boundsRectangleB.Bottom);
            int left = Math.Max(boundsRectangleA.Left, boundsRectangleB.Left);
            int right = Math.Min(boundsRectangleA.Right, boundsRectangleB.Right);

            // Check every point within the intersection bounds
            for (int i = top; i < bottom; i++)
            {
                for (int j = left; j < right; j++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = textureColorDataA[(j - boundsRectangleA.Left) +
                                         (i - boundsRectangleA.Top) * boundsRectangleA.Width];
                    Color colorB = textureColorDataB[(j - boundsRectangleB.Left) +
                                         (i - boundsRectangleB.Top) * boundsRectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A >= alphaThreshold && colorB.A >= alphaThreshold)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
        #endregion
    }
}
