using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1.Geometry
{
    public class SierpinskiTriangle
    {
        public static UIElementCollection content;
        private static void DrawTriangle(Vector2 point, double radius, double angle)
        {
            Vector2[] points = new Vector2[3];

            for (int i = 0; i < 3; i++)
            {
                points[i] = new Vector2();
                points[i].x = point.x + radius * Math.Cos(angle + Math.PI * 2 * i / 3);
                points[i].y = point.y + radius * Math.Sin(angle + Math.PI * 2 * i / 3);
            }
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                j = i < 2 ? i + 1 : 0;
                var line = new Line { Stroke = Brushes.White, X1 = points[i].x, X2 = points[j].x, Y1 = points[i].y, Y2 = points[j].y, StrokeThickness = 2 };
                content.Add(line);
            }
        }
        public static void Draw(Vector2 point, double radius, double angle, int maxIteration)
        {
            DrawTriangle(point, radius, angle);
            if(maxIteration > 0)
            Calculate(point, radius, angle + Math.PI, 1, maxIteration);
        }
        public static void Calculate(Vector2 point, double radius, double angle, int iteration, int maxIteration)
        {
            DrawTriangle(point,radius/2, angle);

            Vector2[] points = new Vector2[3];

            if (iteration > maxIteration) return;
            for (int i = 0; i < 3; i++)
            {
                points[i] = new Vector2();
                points[i].x = point.x + radius/2 * Math.Cos(angle + Math.PI + Math.PI * 2 * i / 3);
                points[i].y = point.y + radius/2 * Math.Sin(angle + Math.PI + Math.PI * 2 * i / 3);
                Calculate(points[i], radius / 2, angle, iteration+1,maxIteration);
            }


        }
    }
}
