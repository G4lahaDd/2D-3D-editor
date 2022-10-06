using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab1._3D
{
    public class Line2D
    {
        public Vector2 p1, p2;

        public Brush brush;

        public double thickness = 2;

        public Line2D(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            brush = Brushes.White;           
        }
        public Line2D(Vector2 p1, Vector2 p2, Brush brush, double thickness) : this(p1, p2)
        {
            this.brush = brush;
            this.thickness = thickness;
        }
        public Line2D(Vector2 p1, Vector2 p2, Line3D line) : this(p1, p2)
        {
            thickness = line.thickness;
            brush = line.brush;
        }

        public Line GetLine() 
        {
            return new Line() { Stroke = brush, X1 = p1.x, X2 = p2.x, Y1 = p1.y, Y2 = p2.y, StrokeThickness = thickness };
        }
    }
}
