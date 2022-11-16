using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Lab1._3D
{
    public class Face
    {
        public List<int> points = new List<int>();
        public Vector3 normal;
        public Vector3 midpoint;
        

        public Brush GetColor() 
        {
            if(normal == null) return null;

            double clr = 1 - normal.z / normal.Abs(); // 0 to 2
            return new SolidColorBrush(Color.FromRgb((byte)(clr * 120d), (byte)(clr * 120d + 10), (byte)(clr * 120d)));           
        }

        public Brush GetColor(Vector3 color, Vector3 light, double indensity) 
        {
            color = ClampColor(color);

            Vector3 vectorToLight = light - midpoint;



            byte r = 0;
            byte g = 0;
            byte b = 0;
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        private Vector3 ClampColor(Vector3 color) 
        {
            if (color.x < 0) color.x = 0;
            else if(color.x > 255) color.x = 255;
            if (color.y < 0) color.y = 0;
            else if (color.y > 255) color.y = 255;
            if(color.z < 0) color.z = 0;
            else if( color.z > 255) color.z = 255;

            return color;
        }
    }
}
