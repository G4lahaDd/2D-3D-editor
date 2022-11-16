using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1._3D.G2LTransform
{
    enum LineType 
    {
        vertical,
        horizontal,
        basic
    }


    public class AreaFiller
    {
        public List<Vector2> area = new List<Vector2>();
        List<Vector2> areaToFill = new List<Vector2>();
        List<LineOfPoints> lines = new List<LineOfPoints>();
#if DEBUG
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

        public void PrintFill(UIElementCollection content) 
        {
            foreach(LineOfPoints line in lines) 
            {
                foreach(Vector2 p in line.points) 
                {
                    Thickness pos = new Thickness { Left = p.x - 3, Top = p.y - 3, Right = 0, Bottom = 0 };
                    Ellipse ellipse = new Ellipse { Fill = Brushes.Black, Height = 6, Width = 6, Margin = pos };
                    content.Add(ellipse);
                }
            }
        }
#endif

        private void CalculateFillArea(double offset)
        {
            areaToFill.Clear();
            for (int i = 0; i < area.Count; i++)
            {
                int next = i < area.Count - 1 ? i + 1 : 0;
                int prev = i > 0 ? i - 1 : area.Count - 1;

                double alpha1 = Math.Atan2(area[i].y - area[prev].y, area[i].x - area[prev].x); //y = tg(A)*x + b, where A = alpha 
                double alpha2 = Math.Atan2(area[next].y - area[i].y, area[next].x - area[i].x);

                LineType l1 = LineType.basic;
                LineType l2 = LineType.basic;


                if (Math.Abs(alpha1) < 0.001f || Math.Abs(Math.PI - Math.Abs(alpha1)) < 0.001f)
                {
                    l1 = LineType.horizontal;
                }
                else if(Math.Abs(Math.Abs(alpha1) - Math.PI / 2) < 0.001f) 
                {
                    l1 = LineType.vertical;
                }

                if (Math.Abs(alpha2) < 0.001f || Math.Abs(Math.PI - Math.Abs(alpha2)) < 0.001f)
                {
                    l2 = LineType.horizontal;
                }
                else if (Math.Abs(Math.Abs(alpha2) - Math.PI / 2) < 0.001f)
                {
                    l2 = LineType.vertical;
                }

                Debug.WriteLine($"l1 = {l1}, l2 = {l2}");

                double X = 0;
                double Y = 0;

                double y_1 = 0;
                double x_1 = 0;

                double y_2 = 0;
                double x_2 = 0;

                double b_1 = 0; //y = tg(A)*x + b
                double b_2 = 0;


                Vector2 line1 = area[i] - area[prev];
                Vector2 line2 = area[next] - area[i];

                if ( l1 == LineType.horizontal) 
                {
                    double offsetY = (line1.x > 0 ? 1 : -1) * offset; 
                    switch (l2) 
                    {
                        case LineType.vertical:
                            double offsetX = (line2.y > 0 ? -1 : 1) * offset;
                            X = area[i].x + offsetX; 
                            Y = area[i].y + offsetY; 
                            break;
                        case LineType.basic:
                            Y = area[i].y + offsetY; 
                            y_2 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha2);
                            x_2 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha2);
                            b_2 = y_2 - Math.Tan(alpha2) * x_2;
                            X = (Y - b_2) / Math.Tan(alpha2);
                            break;
                    }
                }
                else if(l1 == LineType.vertical) 
                {
                    double offsetX = (line1.y > 0 ? -1 : 1) * offset;
                    switch (l2)
                    {
                        case LineType.horizontal:
                            double offsetY = (line2.x > 0 ? 1 : -1) * offset;
                            X = area[i].x + offsetX;
                            Y = area[i].y + offsetY;
                            break;
                        case LineType.basic:
                            X = area[i].x + offsetX; 
                            y_2 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha2);
                            x_2 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha2);
                            b_2 = y_2 - Math.Tan(alpha2) * x_2;
                            Y = X * Math.Tan(alpha2) + b_2;
                            break;
                    }
                }
                else 
                {
                    switch (l2)
                    {
                        case LineType.vertical:
                            y_1 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha1);
                            x_1 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha1);
                            b_1 = y_1 - Math.Tan(alpha1) * x_1; //y = tg(A)*x + b
                            X = area[i].x; //+ offset
                            Y = X * Math.Tan(alpha1) + b_1; //+ offset
                            break;
                        case LineType.horizontal:
                            y_1 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha1);
                            x_1 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha1);
                            b_1 = y_1 - Math.Tan(alpha1) * x_1; //y = tg(A)*x + b
                            Y = area[i].y; //+ offset
                            X = (Y - b_1) / Math.Tan(alpha1);
                            break;
                        case LineType.basic:
                            y_1 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha1);
                            x_1 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha1);

                            y_2 = area[i].y + offset * Math.Sin(Math.PI / 2 - alpha2);
                            x_2 = area[i].x - offset * Math.Cos(Math.PI / 2 - alpha2);

                            b_1 = y_1 - Math.Tan(alpha1) * x_1; //y = tg(A)*x + b
                            b_2 = y_2 - Math.Tan(alpha2) * x_2;

                            X = (b_2 - b_1) / (Math.Tan(alpha1) - Math.Tan(alpha2));
                            Y = Math.Tan(alpha1) * X + b_1;
                            break;
                    }
                }

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

        /// <param name="r">Radius of hole (radius of hexagon)</param>
        /// <param name="h">wall width</param>
        /// <param name="patternRotation">in radians</param>
        /// <returns> List of Points in area</returns>
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
            if (patternRotation > Math.PI / 3) { patternRotation %= Math.PI / 3; }
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

                double X = 0;
                double Y = 0;

                LineType lType = LineType.basic;
                if (Math.Abs(p1.x - p2.x) < 0.001f) { lType = LineType.vertical; }
                else if (Math.Abs(p1.y - p2.y) < 0.001f) { lType = LineType.horizontal; }

                switch (lType)
                {
                    case LineType.horizontal:
                        Y = p1.y;
                        X = Y - b;
                        break;
                    case LineType.vertical:
                        X = p1.x;
                        Y = X + b;
                        break;
                    case LineType.basic:
                        double tg = (p2.y - p1.y) / (p2.x - p1.x);
                        double b_2 = p1.y - p1.x * tg;

                        if (Math.Abs(1 - tg) < 0.001f)
                            continue;

                        X =(b_2 - b) / (1 - tg);
                        Y = X + b;
                        break;
                }

                if (X < point.x || Y < point.y)
                    continue;

                if(lType == LineType.horizontal && X > Math.Min(p1.x, p2.x) && X < Math.Max(p1.x, p2.x)) { count++; }
                else if (lType == LineType.vertical && Y > Math.Min(p1.y, p2.y) && Y < Math.Max(p1.y, p2.y)) { count++; }
                else if (X > Math.Min(p1.x, p2.x) && X < Math.Max(p1.x, p2.x) || Y > Math.Min(p1.y, p2.y) && Y < Math.Max(p1.y, p2.y))
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

   class LineOfPoints
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
            Debug.WriteLine($"Create line: p1: {p1}  p2: {p2}  o: {origin}");
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

