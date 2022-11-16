using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Media.Imaging;

namespace Lab1._3D
{
    public enum ProjectionPlane
    {
        XY,
        YZ,
        XZ,
        ISO,
        DIM,
        TRI
    }
    public static class Projection
    {
        public static List<Line2D> ProjectionOnPlane(List<Line3D> lines, ProjectionPlane plane)
        {
            List<Line2D> result = new List<Line2D>();
            foreach (Line3D line in lines)
            {
                switch (plane)
                {
                    case ProjectionPlane.XY:
                        result.Add(new Line2D(new Vector2(line.p1.x, -line.p1.y), new Vector2(line.p2.x, -line.p2.y), line));
                        break;
                    case ProjectionPlane.YZ:
                        result.Add(new Line2D(new Vector2(line.p1.y, -line.p1.z), new Vector2(line.p2.y, -line.p2.z), line));
                        break;
                    case ProjectionPlane.XZ:
                        result.Add(new Line2D(new Vector2(line.p1.x, -line.p1.z), new Vector2(line.p2.x, -line.p2.z), line));
                        break;
                    case ProjectionPlane.ISO:
                        {
                            double y1 = line.p1.z - Math.Sin(Math.PI / 6) * 0.82 * (line.p1.x + line.p1.y);
                            double x1 = Math.Cos(Math.PI / 6) * 0.82 * (line.p1.x - line.p1.y);

                            double y2 = line.p2.z - Math.Sin(Math.PI / 6) * 0.82 * (line.p2.x + line.p2.y);
                            double x2 = Math.Cos(Math.PI / 6) * 0.82 * (line.p2.x - line.p2.y);
                            result.Add(new Line2D(new Vector2(x1, -y1), new Vector2(x2, -y2), line));
                            break;
                        }
                    case ProjectionPlane.DIM:
                        {
                            double y1 = line.p1.z - Math.Sin(Math.PI * 41.4 / 180) * 0.47 * line.p1.x - Math.Sin(Math.PI * 7.16 / 180) * line.p1.y;
                            double x1 = Math.Cos(Math.PI * 41.4 / 180) * 0.47 * line.p1.x - Math.Cos(Math.PI * 7.16 / 180) * line.p1.y;

                            double y2 = line.p2.z - Math.Sin(Math.PI * 41.4 / 180) * 0.47 * line.p2.x - Math.Sin(Math.PI * 7.16 / 180) * line.p2.y;
                            double x2 = Math.Cos(Math.PI * 41.4 / 180) * 0.47 * line.p2.x - Math.Cos(Math.PI * 7.16 / 180) * line.p2.y;
                            result.Add(new Line2D(new Vector2(x1, -y1), new Vector2(x2, -y2), line));
                            break;
                        }
                    case ProjectionPlane.TRI:
                        {
                            double y1 = line.p1.z - Math.Sin(Math.PI / 12) * line.p1.x - Math.Sin(Math.PI / 3) * 0.75 * line.p1.y;
                            double x1 = Math.Cos(Math.PI / 12) * line.p1.x - Math.Cos(Math.PI / 3) * 0.75 * line.p1.y;

                            double y2 = line.p2.z - Math.Sin(Math.PI / 12) * line.p2.x - Math.Sin(Math.PI / 3) * 0.75 * line.p2.y;
                            double x2 = Math.Cos(Math.PI / 12) * line.p2.x - Math.Cos(Math.PI / 3) * 0.75 * line.p2.y;
                            result.Add(new Line2D(new Vector2(x1, -y1), new Vector2(x2, -y2), line));
                            break;
                        }
                }
            }
            return result;
        }
    }
}
