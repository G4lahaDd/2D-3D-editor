using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1._3D
{
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z);
        }

        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z);
        }

        public static double operator *(Vector3 vector1, Vector3 vector2)
        {
            return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }

        public static Vector3 operator *(Vector3 vector, double a)
        {
            return new Vector3(vector.x * a, vector.y * a, vector.z * a);
        }

        /// <summary>
        /// finding the length of a vector
        /// </summary>
        /// <returns>absolute value of vector</returns>
        public double Abs()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// finding the angle between vector a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Angle in radians</returns>
        public static double GetAngleBetween(Vector3 a, Vector3 b)
        {
            return Math.Acos(a * b / (a.Abs() * b.Abs()));
        }

        public static Vector3 Product(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        public override string ToString()
        {

            return $"({x}, {y}, {z})";
        }
    }

}
