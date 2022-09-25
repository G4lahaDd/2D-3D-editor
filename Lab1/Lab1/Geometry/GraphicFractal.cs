using System;
using Lab1.Controller;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace Lab1.Geometry
{
    public class GraphicFractal : GraphicObject
    {
        private double scaleX = 1;
        private double scaleY = 1;
        private double rotation = 0;
        private int iterationCount = 0;

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
        public string IterationCount 
        {
            get
            {
                return iterationCount.ToString();
            }
            set
            {
                iterationCount = int.Parse(value);
                OnPropertyChanged("IterationCount");
            }
        }

        public ICommand AddPointCommand { get; }
        public ICommand CreateFractalCommand { get; }

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

        public List<Vertex> convertedPoints = new List<Vertex>();


        public GraphicFractal() : base()
        {
            TypeOfObject = ObjectType.Fractal;
            _points = new ObservableCollection<Vertex>();
            AddPointCommand = new LambdaCommand(AddPoint);
            CreateFractalCommand = new LambdaCommand(CreateFractal);
        }


        public override void Draw(UIElementCollection content)
        {
            CalculatePivot();

            var points = convertedPoints.Count == 0 ? _points.ToList() : convertedPoints;

            int count = points.Count - 1;
            if (count < 1) return;
            double[] p1 = TransformVertex(points[0]);
            double[] p2 = TransformVertex(points[count]);
            Line line;           
            for (int i = 0; i < count; i++)
            {
                p1 = TransformVertex(points[i]);
                p2 = TransformVertex(points[i + 1]);
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
        private void CreateFractal(object P) 
        {
            convertedPoints.Clear();
            if (iterationCount < 1) return;
            for (int i = 0; i < _points.Count - 1; i++) 
            {
                convertedPoints.Add(_points[i]);
                convertedPoints.AddRange(LineToFractal(_points[i], _points[i + 1], 1));
            }
            convertedPoints.Add(_points[_points.Count - 1]);
        }
        private List<Vertex> LineToFractal(Vertex p1, Vertex p2, int iteration) 
        { 
            List<Vertex> result = new List<Vertex>();
            double vectorX = (p2.GetX() - p1.GetX())/3;
            double vectorY = (p2.GetY() - p1.GetY())/3;

            double angle = Math.Atan2(vectorX, vectorY);
            double r = Math.Sqrt(3*(vectorX*vectorX + vectorY*vectorY)) / 2;

            double x = (p2.GetX() + p1.GetX()) / 2 - r * Math.Cos(angle);
            double y = (p2.GetY() + p1.GetY()) / 2 + r * Math.Sin(angle);

            Vertex point1 = new Vertex(p1.GetX() + vectorX, p1.GetY() + vectorY);
            Vertex point2 = new Vertex(x,y);
            Vertex point3 = new Vertex(p1.GetX() + 2 * vectorX, p1.GetY() + 2 * vectorY);

            if(iteration < iterationCount) 
            {
                result.AddRange(LineToFractal(p1, point1, iteration + 1));
                result.Add(point1);
                result.AddRange(LineToFractal(point1, point2, iteration + 1));
                result.Add(point2);
                result.AddRange(LineToFractal(point2, point3, iteration + 1));
                result.Add(point3);
                result.AddRange(LineToFractal(point3, p2, iteration + 1));
            }
            else             
            {
                result.Add(point1);
                result.Add(point2);
                result.Add(point3);
            }

            return result;
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
