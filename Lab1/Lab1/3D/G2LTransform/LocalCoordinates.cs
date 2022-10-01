using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab1._3D.G2LTransform
{
    /// <summary>
    /// Сreate Cartesian coordinates in space
    /// </summary>
    public class LocalCoordinates
    {
        //new cartesian coordinates in global space
        Vector3 origin; // Origin of new coordinates, point (0,0)
        double xAxisAngle;// Angles of new axis in Global system. 
        double yAxisAngle;

        /// <summary>
        ///  Initialize new local Cartesian coordinates and return local coordinates of vertices.
        /// </summary>
        /// <param name="vertices">Points in global space, minimum 3 points</param>
        public List<Vector2> TransformToLocalCoordinate(List<Vector3> vertices)
        {
            //init 
            Vector3 normal = GetNormal(vertices[0], vertices[1], vertices[2]);
            xAxisAngle = Math.Atan2(-normal.x, normal.z);
            yAxisAngle = Math.Atan2(-normal.y, normal.x);
            origin = vertices[0];
            return Transform(vertices);
        }

        /// <summary>
        ///  Return local coordinates of vertices.
        /// </summary>
        public List<Vector2> Transform(List<Vector3> vertices)
        {
            if (origin == null) return null;
            List<Vector2> localVertices = new List<Vector2>();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] -= origin;
                double x = vertices[i].x / Math.Cos(xAxisAngle);
                double y = vertices[i].y / Math.Cos(yAxisAngle);
                localVertices.Add(new Vector2(x, y));
            }
            return localVertices;
        }

        public List<Vector3> TransformToGlobalCoordinate(List<Vector2> vertices)
        {
            if (origin == null) return null;
            List<Vector3> globalVertices = new List<Vector3>();
            foreach (Vector2 v in vertices)
            {
                double x = v.x * Math.Cos(xAxisAngle);
                double y = v.y * Math.Cos(yAxisAngle);
                double z = v.x * Math.Sin(xAxisAngle) + v.y * Math.Sin(yAxisAngle);
                //Debug.WriteLine($"x = {x}, y = {y}, z = {z}");
                globalVertices.Add(new Vector3(x, y, z) + origin);
            }
            return globalVertices;
        }

        /// <summary>
        /// Return a normal vector of a plane that based on points p0, p1, p2;
        /// </summary>
        private Vector3 GetNormal(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            double a = (p1.y - p0.y) * (p2.z - p0.z) - (p2.y - p0.y) * (p1.z - p0.z);
            double b = (p1.z - p0.z) * (p2.x - p0.x) - (p2.z - p0.z) * (p1.x - p0.x);
            double c = (p1.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (p1.y - p0.y);
            return new Vector3(a, b, c);
        }
    }
}
