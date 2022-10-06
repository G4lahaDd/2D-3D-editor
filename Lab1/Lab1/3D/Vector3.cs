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

        public static Vector3 operator *(Vector3 vector, Matrix3x3 m)
        {
            double _x = m[0] * vector.x + m[1] * vector.y + m[2] * vector.z;
            double _y = m[3] * vector.x + m[4] * vector.y + m[5] * vector.z;
            double _z = m[6] * vector.x + m[7] * vector.y + m[8] * vector.z;
            return new Vector3(_x,_y,_z);
        }

        public Vector3 Rotate(double rx, double rz) 
        {
            Vector3 result = this * new Matrix3x3(RotationAxis.X, rx) * new Matrix3x3(RotationAxis.Z, rz);
            return result;
        }
        public Vector3 Rotate(Vector2 rot)
        {
            return Rotate(rot.x, rot.y);
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
