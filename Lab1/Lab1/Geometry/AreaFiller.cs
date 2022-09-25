using System;
using System.Collections.Generic;
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
            double mult = a.x * b.x + a.y * b.y;
            return Math.Acos(mult / (a.Abs() * b.Abs()));
        }

    }
    public class AreaFiller
    {
        public List<Vector2> area = new List<Vector2>();
        List<Vector2> areaToFill = new List<Vector2>();

        public void Print(UIElementCollection content)
        {
            Line line = new Line { Stroke = Brushes.Gray, X1 = area[0].x, X2 = area[area.Count -1].x, Y1 = area[0].y, Y2 = area[area.Count - 1].y, StrokeThickness = 2 };
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

        public void CalculateFillArea(double offset)
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

        public static Vector2 GetMidPoint(List<Vector2> points)
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
        /// 
        /// </summary>
        /// <param name="r">Radius of hole</param>
        /// <param name="h">wall width</param>
        /// <param name="patternRotation">in radians</param>
        public void Fill(double r, double h, double patternRotation) 
        {
            Vector2 midpoint = GetMidPoint(areaToFill);
            

        }

        /// <summary>
        /// checking if a point is in an area
        /// </summary>
        /// <returns></returns>
        public bool CheckPointInArea(Vector2 point) 
        {
            double b = point.y - point.x; //y = x + b

            int j;
            int count = 0;
            for (int i = 0; i < areaToFill.Count; i++)
            {
                j = i < areaToFill.Count - 1 ? i + 1 : 0;

                
                double tg = (areaToFill[j].y - areaToFill[i].y) / (areaToFill[j].x - areaToFill[i].x);
                double b_2 = areaToFill[i].y - areaToFill[i].x * tg;

                
            }
            return false;
        }
    }
}
