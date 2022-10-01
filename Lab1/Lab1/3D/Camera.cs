using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1._3D
{
    public class Camera
    {
        public double width;
        public double height;

        public double FOV;

        public Vector3 position;
        public Vector3 rotation;

        public Vector3 ViewRay 
        {
            get 
            {
                double x = Math.Sin(rotation.y) * Math.Sin(rotation.z);
                double y = Math.Sin(rotation.y) * Math.Cos(rotation.z);
                double z = Math.Cos(rotation.x);
                return new Vector3(x, y, z);
            } 
        }

        public Vector2 GetPointOnScreen(Vector3 point) 
        {
            point -= position;
            double angle = Vector3.GetAngleBetween(ViewRay, point);

            return new Vector2(0,0);
        }
    }
}
