using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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

            #region test
            //AreaFiller tester = new AreaFiller()
            //{
            //    area = new List<Vector2>()
            //    {
            //        new Vector2(100,300),
            //        new Vector2(200,200),
            //        new Vector2(400,200),
            //        new Vector2(550,340),
            //        new Vector2(270,410)
            //    }
            //};
            //tester.CalculateFillArea(20);
            //tester.Print(DrawCanvas.Children);
            AreaFiller areaFiller = new AreaFiller()
            {
                area = new List<Vector2>()
                {
                    new Vector2(400,40),
                    new Vector2(517,277),
                    new Vector2(777, 315),
                    new Vector2(589, 500),
                    new Vector2(633, 759),
                    new Vector2(400,636),
                    new Vector2(167,759),
                    new Vector2(211, 500),
                    new Vector2(22, 315),
                    new Vector2(283,277)
                }
            };
            areaFiller.CalculateFillArea(15);
            areaFiller.Print(DrawCanvas.Children);
            #endregion
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
