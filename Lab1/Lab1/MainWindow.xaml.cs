using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab1._3D;
using Lab1._3D.G2LTransform;
using Lab1.Geometry;


namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {

        public ObservableCollection<GraphicObject> graphicObjects = new ObservableCollection<GraphicObject>
        {
            new GraphicPoint
            {
                X = "100",
                Y = "100",
                Dx = "0",
                Dy = "0",
                Name = "Point 0",
            },
            new GraphicShape
            {   
                ScaleX = "1",
                ScaleY = "1",
                Rotation = "0",
                Points = new ObservableCollection<Vertex>()
                {
                    new Vertex(200, 300),
                    new Vertex(500, 300),
                    new Vertex(350, 50)
                },
                Dx = "0",
                Dy = "0",
                Name = "Triangle"
            },
            //new GraphicShape
            //{   ScaleX = "1",
            //    ScaleY = "1",
            //    Rotation = "0",
            //    Points = new ObservableCollection<Vertex>()
            //    {
            //        new Vertex(600, 100),
            //        new Vertex(600, 300),
            //        new Vertex(800, 300),
            //        new Vertex(800, 100)
            //    },
            //    Dx = "0",
            //    Dy = "0",
            //    Name = "Square"
            //},
            new GraphicFractal
            {  
                ScaleX = "1",
                ScaleY = "1",
                Rotation = "0",
                Points = new ObservableCollection<Vertex>()
                {
                    new Vertex(600, 100),
                    new Vertex(600, 300),
                    new Vertex(800, 300),
                    new Vertex(800, 100),
                    new Vertex(600, 100)
                },
                Dx = "0",
                Dy = "0",
                Name = "Fractal square"
            }
        };

        private Dictionary<ObjectType, string> objectTemplates = new Dictionary<ObjectType, string>();
        private Dictionary<ObjectType, int> heightOfTemplates = new Dictionary<ObjectType, int>();

        public ObservableCollection<GraphicObject> GraphicObjects
        {
            get => graphicObjects;
            set
            {
                graphicObjects = value;
                OnPropertyChanged("GraphicObjects");
            }
        }

        public MainWindow()
        {
            foreach(var graphicObject in graphicObjects) 
            {
                graphicObject.DeleteObjectEvent += DeleteObject;
            }
            InitializeComponent(); 
            InitTemplates();
            DataContext = this;

            //Test
            //AreaFiller af = new AreaFiller();
            //af.area = new List<Vector2>()
            //{
            //    new Vector2(200,200),
            //    new Vector2(600,200),
            //    new Vector2(600,500),
            //    new Vector2(200,500)

            //};
            //List<Vector2> vector2s = af.Fill(10, 5, Math.PI / 6);
            //af.PrintFill(DrawCanvas.Children);
            //af.Print(DrawCanvas.Children);
            //foreach (var vector2 in vector2s)
            //{
            //    Thickness pos = new Thickness { Left = vector2.x - 2, Top = vector2.y - 2, Right = 0, Bottom = 0 };
            //    Ellipse ellipse = new Ellipse { Fill = Brushes.White, Height = 4, Width = 4, Margin = pos };
            //    DrawCanvas.Children.Add(ellipse);
            //}

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitTemplates()
        {
            objectTemplates.Add(ObjectType.Point, "Point");
            heightOfTemplates.Add(ObjectType.Point, 250);
            objectTemplates.Add(ObjectType.Shape, "Shape");
            heightOfTemplates.Add(ObjectType.Shape, 520);
            objectTemplates.Add(ObjectType.Fractal, "Fractal");
            heightOfTemplates.Add(ObjectType.Fractal, 600);
        }
        private void OpenObjectSettings(object sender, RoutedEventArgs e)
        {
            if (ObjectInteractionPanel.Children.Count > 1)
            {
                ObjectInteractionPanel.Children.RemoveAt(1);
            }

            GraphicObject temp = (GraphicObject)(sender as RadioButton).DataContext;

            ContentControl contentControl = new ContentControl
            {
                Template = Application.Current.FindResource(objectTemplates[temp.TypeOfObject]) as ControlTemplate,
                DataContext = temp,
                Height = heightOfTemplates[temp.TypeOfObject],
                Margin = new Thickness(10, 5, 10, 5)
            };
            ObjectInteractionPanel.Children.Add(contentControl);

        }
        private void AddPoint(object sender, RoutedEventArgs e)
        {
            var temp = new GraphicPoint() { Name = $"New point {GraphicObjects.Count + 1}" };
            temp.DeleteObjectEvent += DeleteObject;
            GraphicObjects.Add(temp);
        }
        private void AddShape(object sender, RoutedEventArgs e)
        {
            var temp = new GraphicShape() { Name = $"New shape {GraphicObjects.Count + 1}" };
            temp.DeleteObjectEvent += DeleteObject;
            GraphicObjects.Add(temp);
        }
        private void AddFractal(object sender, RoutedEventArgs e)
        {
            var temp = new GraphicFractal() { Name = $"New fractal {GraphicObjects.Count + 1}" };
            temp.DeleteObjectEvent += DeleteObject;
            GraphicObjects.Add(temp);
        }
        private void Redraw(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            for (int i = 0; i < graphicObjects.Count; i++)
            {
                graphicObjects[i].Draw(DrawCanvas.Children);
            }
        }
        private void DeleteObject(GraphicObject gObject)
        {
            if (ObjectInteractionPanel.Children.Count > 1)
            {
                ObjectInteractionPanel.Children.RemoveAt(1);
            }
            graphicObjects.Remove(gObject);
        }
    }

}
