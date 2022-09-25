using System;
using Lab1.Controller;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Lab1.Geometry
{
    public class GraphicShape : GraphicObject
    {
        private double scaleX = 1;
        private double scaleY = 1;
        private double rotation = 0;

        private double pivotX = 0;
        private double pivotY = 0;

        public string ScaleX
        {
            get
            {
                return scaleX.ToString();
            }
            set
            {
                try { scaleX = double.Parse(value); }
                catch { scaleX = 1; }
                OnPropertyChanged("ScaleX");
            }
        }
        public string ScaleY
        {
            get
            {
                return scaleY.ToString();
            }
            set
            {
                try { scaleY = double.Parse(value); }
                catch { scaleY = 1; }
                OnPropertyChanged("ScaleY");
            }
        }
        public string Rotation
        {
            get
            {
                return rotation.ToString();
            }
            set
            {
                rotation = double.Parse(value);
                OnPropertyChanged("Rotation");

            }
        }


        public ICommand AddPointCommand { get; }
        ObservableCollection<Vertex> _points;
        public ObservableCollection<Vertex> Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged("Points");
            }
        }


        public GraphicShape() : base()
        {
            TypeOfObject = ObjectType.Shape;
            _points = new ObservableCollection<Vertex>();
            AddPointCommand = new LambdaCommand(AddPoint);
        }


        public override void Draw(UIElementCollection content)
        {
            CalculatePivot();

            int count = Points.Count - 1;
            if (count < 1) return;
            double[] p1 = TransformVertex(Points[0]);
            double[] p2 = TransformVertex(Points[count]);
            Line line = new Line { Stroke = this.Color, X1 = p1[0], X2 = p2[0], Y1 = p1[1], Y2 = p2[1], StrokeThickness = 4 };
            content.Add(line);
            for (int i = 0; i < count; i++)
            {
                p1 = TransformVertex(Points[i]);
                p2 = TransformVertex(Points[i + 1]);
                line = new Line { Stroke = this.Color, X1 = p1[0], X2 = p2[0], Y1 = p1[1], Y2 = p2[1], StrokeThickness = 4 };
                content.Add(line);
            }

        }
        public override void Move(object p)
        {
            foreach (Vertex vertex in Points)
            {
                vertex.X = (double.Parse(vertex.X) + dx).ToString();
                vertex.Y = (double.Parse(vertex.Y) + dy).ToString();
            }
        }
        private void AddPoint(object p)
        {
            Points.Add(new Vertex(0, 0));
        }

        private void CalculatePivot()
        {
            double sx = 0, sy = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                sx += double.Parse(Points[i].X);
                sy += double.Parse(Points[i].Y);
            }
            pivotX = sx / Points.Count;
            pivotY = sy / Points.Count;
        }
        private double[] TransformVertex(Vertex point)
        {
            double deltaX = (double.Parse(point.X) - pivotX) * scaleX;
            double deltaY = (double.Parse(point.Y) - pivotY) * scaleY;
            double TotalAngle = rotation * Math.PI / 180d + Math.Atan2(deltaY, deltaX);
            double radius = Math.Sqrt(deltaY * deltaY + deltaX * deltaX);

            double _x = radius * Math.Cos(TotalAngle) + pivotX;
            double _y = radius * Math.Sin(TotalAngle) + pivotY;

            return new double[2] { _x, _y };
        }
    }
}
