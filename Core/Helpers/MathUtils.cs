using Microsoft.Xna.Framework;
using System;

namespace SupernovaMod.Core.Helpers
{
    /// <summary>
    /// A collection of extra Math helper methods
    /// </summary>
    public static class MathUtils
    {
        // See: https://mathworld.wolfram.com/PolarVector.html
        /// <summary>
        /// Initializes a new <see cref="Vector2"/> with the specified <paramref name="radius"/> and <paramref name="theta"/>
        /// known as a 'Polar Vector'.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static Vector2 NewPolarVector(float radius, float theta)
        {
            return new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
        }
    }
}
