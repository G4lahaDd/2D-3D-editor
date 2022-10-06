using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Lab1._3D
{
    public class Line3D
    {
        public Vector3 p1;
        public Vector3 p2;

        public Brush brush = Brushes.White;
        public double thickness = 2;

        public Line3D()
        {
            p1 = new Vector3();
            p2 = new Vector3();
        }

        public Line3D(Vector3 p1, Vector3 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public double GetLength() 
        {
            return (p2 - p1).Abs();
        }
    }
}
