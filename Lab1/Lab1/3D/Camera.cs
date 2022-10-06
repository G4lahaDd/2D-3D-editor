using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Shapes;

namespace Lab1._3D
{
    public class Camera
    {
        public double width;
        public double height;
        public Vector2 center { get { return new Vector2(width / 2, height / 2); } }

        public double FOV;

        public double clipDistanse = 0.5;
        public double viewDistanse = 50;

        public Vector3 position;
        public Vector3 rotation;

        public Vector3 ViewRay;
        //{
        //    get 
        //    {
        //        double x = Math.Sin(rotation.x) * Math.Sin(rotation.z); // right
        //        double y = Math.Sin(rotation.x) * Math.Cos(rotation.z); // forward
        //        double z = Math.Cos(rotation.x); // up
        //        Debug.WriteLine($"{x} {y} {z}");
        //        return new Vector3(x, y, z);
        //    } 
        //}
        public Vector3 rightAxis;
        public Vector3 upAxis;

        public Vector2 GetPointOnScreen(Vector3 point)
        {
            point -= position;
            Debug.WriteLine($"{position}");
            Vector2 cam = new Vector2(ViewRay.x, ViewRay.y);
            Vector2 p = new Vector2(point.x, point.y);
            double zAngle = -Math.Atan2(point.y, point.x) + rotation.z;
            double xAngle = Math.PI / 2 - rotation.x - Math.Atan2(point.z, p.Abs());
            Debug.WriteLine($"angle z: {zAngle}, angle x: {xAngle}");
            double VFOV = height * FOV / width;

            double h = height / 2 / Math.Tan(VFOV / 2);
            double X = h * Math.Tan(zAngle);
            double maxVAngle = Math.Atan2(height / 2, Math.Sqrt(h * h + X * X));
            if (Math.Abs(xAngle) > Math.Abs(maxVAngle) || Math.Abs(X) > width / 2) { Debug.WriteLine($"\nbombom"); return null; }
            double y = Math.Sqrt(h * h + X * X) * Math.Tan(xAngle);
            Debug.WriteLine($"\nPoint: {X}, {y}");
            return new Vector2(X, y);
        }
        public Vector2 GetPointOnScreen2(Vector3 p) 
        {
            Vector3 n = ViewRay;
            Vector3 o = position + ViewRay * viewDistanse;
            Vector3 l = p - position;

            if (Math.Abs(Vector3.GetAngleBetween(n, l) - Math.PI / 2) < 0.15f) { Debug.WriteLine("parallel"); return null; }
            else
            {
                Debug.WriteLine("point on plane");

                double D = -(n.x * o.x + n.y * o.y + n.z * o.z);//coeff of plane 
                double ycoeff = n.y * l.y / l.x;
                double zycoeff = n.z * l.z / l.x;

                double x = (p.x * (ycoeff + zycoeff) - D - n.y * p.y - n.z * p.z) / (n.x + ycoeff + zycoeff);

                double y = (l.y / l.x) * (x - p.x) + p.y;
                double z = (l.z / l.x) * (x - p.x) + p.z;

                Vector3 PointOnPlane = new Vector3(x, y, z);
                Vector3 temp = PointOnPlane - o;

                double h = (width / 2) / Math.Tan(FOV / 2);//  viewDistanse in pixels

                double x_pixel = (temp * rightAxis) * h / viewDistanse;
                double y_pixel = (temp * (upAxis * -1)) * h / viewDistanse;

                return new Vector2(x_pixel, y_pixel) + center;
            }           
        }

        public Line2D DrawLine(Line3D line)
        {
            Debug.WriteLine($"\nviewray: {ViewRay}");
            Debug.WriteLine($"horizontal: {rightAxis}");
            Debug.WriteLine($"up: {upAxis}");

            Vector2 p1 = GetPointOnScreen(line.p1);
            Vector2 p2 = GetPointOnScreen(line.p2);
            if (p1 == null || p2 == null)
            {
                Debug.WriteLine("points not exist");
                Vector3 normal = ViewRay;
                Vector3 planePos = position + ViewRay * clipDistanse;
                Vector3 lineV = line.p2 - line.p1;

                if (Math.Abs(Vector3.GetAngleBetween(normal, lineV) - Math.PI / 2) < 0.15f) { Debug.WriteLine("parallel"); return null; }
                else
                {
                    Debug.WriteLine("point on plane");
                    double A = normal.y * lineV.y / lineV.x;
                    double B = normal.z * lineV.z / lineV.y;

                    double D = -(normal.x * planePos.x + normal.y * planePos.y + normal.z * planePos.z);

                    double x = (line.p1.x * (A + B) - D - normal.y * line.p1.y - normal.z * line.p1.z) / (normal.x + A + B);
                    double y = lineV.y / lineV.x * (x - line.p1.x) + line.p1.y;
                    double z = lineV.z / lineV.x * (x - line.p1.x) + line.p1.z;

                    Vector3 PointOnPlane = new Vector3(x, y, z);
                    Vector3 temp = PointOnPlane - planePos;

                    double h = width / 2 / Math.Tan(FOV / 2);//  clipDistanse in pixels

                    double x_pixel = temp * rightAxis * h / clipDistanse;
                    double y_pixel = temp * upAxis * h / clipDistanse;

                    Vector2 point = new Vector2(x_pixel, y_pixel);
                    p1 = p1 == null ? point : p1;
                    p2 = p2 == null ? point : p2;
                }             
            }           
            Debug.WriteLine($"line: p1:{p1.ToString()} p2:{p2.ToString()}\n");
            return new Line2D(p1 + center, p2 + center, line);
        }

        public Line2D DrawLine2(Line3D line)
        {
            Vector2 p1 = GetPointOnScreen2(line.p1);
            Vector2 p2 = GetPointOnScreen2(line.p2);
            if (p1 == null || p2 == null) return null;
            return new Line2D(p1, p2, line);

        }
            public static Vector2[] Test()
        {
            Camera camera = new Camera();
            camera.width = 400;
            camera.height = 400;
            camera.FOV = Math.PI * 2 / 3;
            camera.position = new Vector3(1, 1, 1);
            camera.rotation = new Vector3(Math.PI * 3 / 4, 0, Math.PI / 4);

            Vector3 test = new Vector3();
            Vector3 testz = new Vector3(0, 0, 0.5);
            Vector3 testx = new Vector3(0.5, 0, 0);
            Vector3 testy = new Vector3(0, 0.5, 0);

            Vector2 test2d = camera.GetPointOnScreen(test);
            Vector2 test2dx = camera.GetPointOnScreen(testx);
            Vector2 test2dy = camera.GetPointOnScreen(testy);
            Vector2 test2dz = camera.GetPointOnScreen(testz);

            Debug.WriteLine(test2d.ToString());
            Debug.WriteLine(test2dx.ToString());
            Debug.WriteLine(test2dy.ToString());
            Debug.WriteLine(test2dz.ToString());

            Vector2 origin = new Vector2(camera.width / 2, camera.height / 2);
            return new[] { test2d + origin, test2dx + origin, test2dy + origin, test2dz + origin };
        }
    }
}
