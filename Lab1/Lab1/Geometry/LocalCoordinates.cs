using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab1.Geometry
{
    /// <summary>
    /// Transform List of 3d vertices to list of 2d vertices and back
    /// </summary>
    public class LocalCoordinates
    {
        //new cartesian coordinates in global space
        Vector3 origin = new Vector3();
        Vector3 horizontalAxis = new Vector3();
        Vector3 verticalAxis;

        double xAxisAngle;
        double yAxisAngle;

        public List<Vector2> TransformToLocalCoordinate(List<Vector3> vertices)
        {
            if(vertices == null || vertices.Count < 3) return null;
            List<Vector2> localVertices = new List<Vector2>();

            origin = vertices[0];
            Vector3[] axis = GetAxisOnPlane(vertices[0], vertices[1], vertices[2]);
            horizontalAxis = axis[0];
            verticalAxis = axis[1];
            //Debug.WriteLine($"hor: {horizontalAxis.ToString()},   vert: {verticalAxis.ToString()}");
            //Debug.WriteLine( "axis mult: " + horizontalAxis * verticalAxis);
            horizontalAxis = vertices[1] - origin;
            verticalAxis = vertices[2] - origin;
            horizontalAxis *= 1 / horizontalAxis.Abs();
            verticalAxis *= 1 / verticalAxis.Abs();
           // Debug.WriteLine($"hor: {horizontalAxis.ToString()},   vert: {verticalAxis.ToString()}");
            for (int i = 0; i < vertices.Count; i++) 
            {
                vertices[i] -= origin;

                double len = vertices[i].Abs();
                if (Math.Abs(len) > 0.01d)
                {
                    //double x = Vector3.Product(vertices[i],verticalAxis).Abs();
                    //double y = Vector3.Product(vertices[i],horizontalAxis).Abs();
                    double x = len * Math.Sin(Math.Acos((vertices[i] * verticalAxis) / len));
                    double y = len * Math.Sin(Math.Acos((vertices[i] * horizontalAxis) / len));

                    //if(horizontalAxis.x > 0) 
                    //{
                    //    if (vertices[i].x > 0) x = Math.Abs(x);
                    //    else x = -Math.Abs(x);
                    //}
                    //else 
                    //{
                    //    if (vertices[i].x < 0) x = Math.Abs(x);
                    //    else x = -Math.Abs(x);
                    //}
                    //if (verticalAxis.y > 0)
                    //{
                    //    if (vertices[i].y > 0) y = Math.Abs(y);
                    //    else y = -Math.Abs(y);
                    //}
                    //else
                    //{
                    //    if (vertices[i].y < 0) y = Math.Abs(y);
                    //    else y = -Math.Abs(y);
                    //}
                    localVertices.Add(new Vector2(x, y));
                }
                else
                {
                    localVertices.Add(new Vector2());
                }
            }

            return localVertices;
        }

        public List<Vector2> TransformToLocalCoordinate2(List<Vector3> vertices)
        {
            List<Vector2> localVertices = new List<Vector2>();
            Vector3 normal = GetNormal(vertices[0], vertices[1], vertices[2]);
            xAxisAngle = Math.Atan2(normal.x, normal.z);
            yAxisAngle = Math.Atan2(normal.y, normal.x);
            origin = vertices[0];
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] -= origin;
                double x = vertices[i].x / Math.Cos(xAxisAngle);
                double y = vertices[i].y / Math.Cos(yAxisAngle);
                //Debug.WriteLine($"x = {x}, y = {y}");
                localVertices.Add(new Vector2(x, y));
            }
            return localVertices;
        }

        /// <summary>
        /// </summary>
        /// <param name="p1">horizontal axis</param>
        /// <param name="p2">point on plane</param>
        /// <returns>vertical axis</returns>
        private Vector3[] GetAxisOnPlane(Vector3 p0, Vector3 p1, Vector3 p2) 
        {
            Vector3 normal = GetNormal(p0, p1, p2);

            double hAngle = Math.Atan2(normal.x,normal.z);
            double vAngle = Math.Atan2(normal.y,normal.z);

            Vector3[] result = new Vector3[2];
            result[0] = new Vector3(Math.Cos(hAngle), 0, Math.Sin(hAngle));//horizontal axis
            result[1] = new Vector3(0,Math.Cos(vAngle), Math.Sin(vAngle));//verticalAxis

            //double e = (p0.x*a + p0.y*b + p0.z*c);
           // double z = (e - a + b*(p1.x - p0.x) / (p1.y - p0.y)) / (c - b*(p1.z - p0.z) / (p1.y - p0.y));
           // double y = (z*(p1.z - p0.z) - p0.x + p1.x) / (p0.y - p1.y);
            //return new Vector3(1,y,z);
            return result;
        }

        private Vector3 GetNormal(Vector3 p0, Vector3 p1, Vector3 p2) 
        {
            double a = (p1.y - p0.y) * (p2.z - p0.z) - (p2.y - p0.y) * (p1.z - p0.z);
            double b = (p1.z - p0.z) * (p2.x - p0.x) - (p2.z - p0.z) * (p1.x - p0.x);
            double c = (p1.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (p1.y - p0.y);
            return new Vector3(a, b, c);
        }
        public List<Vector3> TransformToGlobalCoordinate(List<Vector2> vertices) 
        {
            List<Vector3> globalVertices = new List<Vector3>();

            if (verticalAxis == null) return null;
            foreach(Vector2 v in vertices) 
            {
                globalVertices.Add(origin + horizontalAxis * v.x + verticalAxis * v.y);
            }

            return globalVertices;
        }
        public List<Vector3> TransformToGlobalCoordinate2(List<Vector2> vertices)
        {
            List<Vector3> globalVertices = new List<Vector3>();
            foreach(Vector2 v in vertices) 
            {
                double x = v.x * Math.Cos(xAxisAngle);
                double y = v.y * Math.Cos(yAxisAngle);
                double z = v.x * Math.Sin(xAxisAngle) + v.y * Math.Sin(yAxisAngle);
                //Debug.WriteLine($"x = {x}, y = {y}, z = {z}");
                globalVertices.Add(new Vector3(x, y, z) + origin);
            }
            return globalVertices;
        }
    }
}
