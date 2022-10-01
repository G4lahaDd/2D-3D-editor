using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1._3D
{
    public class Vector2
    {
        public double x;
        public double y;

        public Vector2()
        {
            x = 0;
            y = 0;
        }

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x - vector2.x, vector1.y - vector2.y);
        }

        public static double operator *(Vector2 vector1, Vector2 vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y;
        }

        public static Vector2 operator *(Vector2 vector, double a)
        {
            return new Vector2(vector.x * a, vector.y * a);
        }

        /// <summary>
        /// finding the length of a vector
        /// </summary>
        /// <returns>absolute value of vector</returns>
        public double Abs()
        {
            return Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// finding the angle between vector a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Angle in radians</returns>
        public static double GetAngleBetween(Vector2 a, Vector2 b)
        {
            return Math.Acos(a * b / (a.Abs() * b.Abs()));
        }

        public override string ToString()
        {

            return $"({x}, {y})";
        }
    }
}
