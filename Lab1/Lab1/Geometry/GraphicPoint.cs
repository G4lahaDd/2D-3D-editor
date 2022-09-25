using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Lab1.Geometry
{
    public class GraphicPoint : GraphicObject
    {

        private double x;
        private double y;

        public string X
        {
            get => x.ToString();
            set
            {
                try
                {
                    x = double.Parse(value);
                }
                catch
                {
                    x = 0;
                }

                OnPropertyChanged("X");
            }
        }
        public string Y
        {
            get => y.ToString();
            set
            {
                try
                {
                    y = double.Parse(value);
                }
                catch
                {
                    y = 0;
                }

                OnPropertyChanged("Y");
            }
        }

        public GraphicPoint()
        {
            TypeOfObject = ObjectType.Point;
        }
        public override void Draw(UIElementCollection content)
        {
            Thickness pos = new Thickness { Left = x, Top = y, Right = 0, Bottom = 0 };
            Ellipse ellipse = new Ellipse { Fill = this.Color, Height = 4, Width = 4, Margin = pos };
            content.Add(ellipse);
        }

        public override void Move(object p)
        {
            x += dx;
            y += dy;
        }
    }
}
