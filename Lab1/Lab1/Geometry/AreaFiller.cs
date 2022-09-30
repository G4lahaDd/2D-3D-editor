using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1.Geometry
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
            return new Vector3(a.y*b.z - a.z*b.y, a.z*b.x - a.x*b.z, a.x*b.y - a.y*b.x);
        }

        public override string ToString()
        {

            return $"({x}, {y}, {z})";
        }
    }

    public class AreaFiller
    {
        public List<Vector2> area = new List<Vector2>();
        List<Vector2> areaToFill = new List<Vector2>();
        List<LineOfPoints> lines = new List<LineOfPoints>();

        public void Print(UIElementCollection content)
        {
            Line line = new Line { Stroke = Brushes.Gray, X1 = area[0].x, X2 = area[area.Count - 1].x, Y1 = area[0].y, Y2 = area[area.Count - 1].y, StrokeThickness = 2 };
            content.Add(line);
            for (int i = 0; i < area.Count - 1; i++)
            {
                line = new Line { Stroke = Brushes.Gray, X1 = area[i].x, X2 = area[i + 1].x, Y1 = area[i].y, Y2 = area[i + 1].y, StrokeThickness = 2 };
                content.Add(line);
            }

            line = new Line { Stroke = Brushes.OrangeRed, X1 = areaToFill[0].x, X2 = areaToFill[area.Count - 1].x, Y1 = areaToFill[0].y, Y2 = areaToFill[area.Count - 1].y, StrokeThickness = 2 };
            content.Add(line);
            for (int i = 0; i < area.Count - 1; i++)
            {
                line = new Line { Stroke = Brushes.OrangeRed, X1 = areaToFill[i].x, X2 = areaToFill[i + 1].x, Y1 = areaToFill[i].y, Y2 = areaToFill[i + 1].y, StrokeThickness = 2 };
                content.Add(line);
            }

            for (int i = 0; i < area.Count; i++)
            {
                Thickness pos = new Thickness { Left = area[i].x - 2, Top = area[i].y - 2, Right = 0, Bottom = 0 };
                Ellipse ellipse = new Ellipse { Fill = Brushes.White, Height = 4, Width = 4, Margin = pos };
                content.Add(ellipse);
                pos = new Thickness { Left = areaToFill[i].x - 2, Top = areaToFill[i].y - 2, Right = 0, Bottom = 0 };
                ellipse = new Ellipse { Fill = Brushes.Orange, Height = 4, Width = 4, Margin = pos };
                content.Add(ellipse);
            }
        }

        private void CalculateFillArea(double offset)
        {
            areaToFill.Clear();
            for (int i = 0; i < area.Count; i++)
            {
                int next = i < area.Count - 1 ? i + 1 : 0;
                int prev = i > 0 ? i - 1 : area.Count - 1;

                double alpha1 = Math.Atan2(area[i].y - area[prev].y, area[i].x - area[prev].x); //y = tg(A)*x + b, where A = alpha 
                double alpha2 = Math.Atan2(area[next].y - area[i].y, area[next].x - area[i].x);

                double y_1 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha1);
                double x_1 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha1);

                double y_2 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha2);
                double x_2 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha2);

                double b_1 = y_1 - Math.Tan(alpha1) * x_1; //y = tg(A)*x + b
                double b_2 = y_2 - Math.Tan(alpha2) * x_2;

                double X = (b_2 - b_1) / (Math.Tan(alpha1) - Math.Tan(alpha2));
                double Y = Math.Tan(alpha1) * X + b_1;

                areaToFill.Add(new Vector2(X, Y));
            }
        }

        private static Vector2 GetMidPoint(List<Vector2> points)
        {
            double X = 0;
            double Y = 0;
            for (int i = 0; i < points.Count; i++)
            {
                X += points[i].x;
                Y += points[i].y;
            }
            X /= points.Count;
            Y /= points.Count;
            return new Vector2(X, Y);
        }


        /// <summary>
        /// </summary>
        /// <param name="r">Radius of hole</param>
        /// <param name="h">wall width</param>
        /// <param name="patternRotation">in radians</param>
        /// <returns>Points in area</returns>
        public List<Vector2> Fill(double r, double h, double patternRotation)
        {
            List<Vector2> fill = new List<Vector2>();
            //Offset betweenpoints
            double radiusOffset = r * Math.Sqrt(3) + h;
            CalculateFillArea(radiusOffset);
            //Midpoint
            Vector2 midpoint = GetMidPoint(areaToFill);
            //the two extreme points of the rectangle in which the circle is inscribed            
            Vector2 rectMin = new Vector2(areaToFill[0].x, areaToFill[0].y);
            Vector2 rectMax = new Vector2(areaToFill[0].x, areaToFill[0].y);
            //Clamp rotation
            if (patternRotation > (Math.PI / 3)) { patternRotation %= (Math.PI / 3); }
            else if (patternRotation < 0)
            {
                patternRotation = Math.PI / 3 - (-patternRotation) % (Math.PI / 3);
            }
            //Calculate Rect
            for (int i = 0; i < areaToFill.Count; i++)
            {
                if (areaToFill[i].y < rectMin.y)
                { rectMin.y = areaToFill[i].y; }
                if (areaToFill[i].x < rectMin.x)
                { rectMin.x = areaToFill[i].x; }
                if (areaToFill[i].y > rectMax.y)
                { rectMax.y = areaToFill[i].y; }
                if (areaToFill[i].x > rectMax.x)
                { rectMax.x = areaToFill[i].x; }
            }
            //Get Lines in Rect
            double betta = 2 * Math.PI / 3 - patternRotation;
            double dx = radiusOffset * Math.Cos(betta);
            double dy = radiusOffset * Math.Sin(betta);
            Vector2 offset = new Vector2(-dx, dy);
            Vector2 temp = midpoint;
            LineOfPoints result;
            while (true)//left  
            {
                temp += offset;
                result = LineOfPoints.CreateLine(rectMin, rectMax, temp, Math.Tan(patternRotation), radiusOffset);
                if (result == null) break;
                lines.Add(result);
            }
            temp = midpoint;
            while (true)//right  
            {
                result = LineOfPoints.CreateLine(rectMin, rectMax, temp, Math.Tan(patternRotation), radiusOffset);
                if (result == null) break;
                lines.Add(result);
                temp -= offset;
            }
            //Get Points in Area
            foreach (LineOfPoints line in lines)
            {
                foreach (Vector2 point in line.points)
                {
                    if (CheckPointInArea(point))
                    {
                        fill.Add(point);
                    }
                }
            }
            return fill;
        }

        /// <summary>
        /// checking if a point is in an area
        /// </summary>
        /// <returns></returns>
        private bool CheckPointInArea(Vector2 point)
        {
            double b = point.y - point.x; //y = x + b

            int j;
            int count = 0;
            for (int i = 0; i < areaToFill.Count; i++)
            {
                j = i < areaToFill.Count - 1 ? i + 1 : 0;

                Vector2 p1 = areaToFill[i];
                Vector2 p2 = areaToFill[j];

                double tg = (p2.y - p1.y) / (p2.x - p1.x);
                double b_2 = p1.y - p1.x * tg;

                if (Math.Abs(1 - tg) < 0.01f)
                    continue;

                double X = (b_2 - b) / (1 - tg);
                double Y = X + b;

                if (X < point.x)
                    continue;

                if (X > Math.Min(p1.x, p2.x) && X < Math.Max(p1.x, p2.x) || Y > Math.Min(p1.y, p2.y) && Y < Math.Max(p1.y, p2.y))
                {
                    count++;
                }
            }
            if (count % 2 == 1)
            {
                return true;
            }
            return false;
        }
    }

    public class LineOfPoints
    {
        Vector2 p1, p2;
        Vector2 origin;
        double tg;
        double offset;

        public List<Vector2> points = new List<Vector2>();

        private LineOfPoints(Vector2 p1, Vector2 p2, Vector2 origin, double tg, double offset)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.origin = origin;
            this.tg = tg;
            this.offset = offset;
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            double dx = offset * Math.Cos(Math.Atan(tg));
            double dy = offset * Math.Sin(Math.Atan(tg));
            int N1 = (int)((p1.x - origin.x) / dx) - 1;
            int N2 = (int)((p2.x - origin.x) / dx) + 1;
            for (int i = Math.Min(N1, N2); i <= Math.Max(N1, N2); i++)
            {
                points.Add(new Vector2(origin.x + dx * i, origin.y + dy * i));
            }
        }

        /// <summary>
        /// Create new Line
        /// 
        /// </summary>
        /// <param name="p1"> Left-bottom point of rect</param>
        /// <param name="p2">Right-top point of rect</param>
        /// <param name="origin">point that belongs line,</param>
        /// <param name="tg">slope tangent</param>
        /// <param name="offset">distance between points</param>
        /// <returns>null, if line does not cross rect</returns>
        public static LineOfPoints CreateLine(Vector2 p1, Vector2 p2, Vector2 origin, double tg, double offset)
        {
            double b = origin.y - tg * origin.x;
            if (tg < 0.015f && (origin.y > p2.y || origin.y < p1.y))
            {
                return null;
            }
            if (tg * p1.x + b > p2.y || tg * p2.x + b < p1.y)
            {
                return null;
            }
            return new LineOfPoints(p1, p2, origin, tg, offset);
        }
    }


}

