using System;
using Lab1.Controller;
using System.Windows;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab1.Geometry
{
    
    public abstract class GraphicObject : INotifyPropertyChanged
    {

        private string name = string.Empty;
        protected double dx = 0;
        protected double dy = 0;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Dx
        {
            get
            {
                return dx.ToString();
            }
            set
            {
                dx = double.Parse(value);
                OnPropertyChanged("Dx");
            }
        }
        public string Dy
        {
            get
            {
                return dy.ToString();
            }
            set
            {
                dy = double.Parse(value);
                OnPropertyChanged("Dy");
            }
        }

        public ObjectType TypeOfObject { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand MoveCommand { get; }
        public Brush Color { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event DeleteObjectDelegate DeleteObjectEvent;

        public GraphicObject()
        {
            Color = Brushes.White;
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted);
            MoveCommand = new LambdaCommand(Move);
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnDeleteCommandExecuted(object p)
        {
            DeleteObjectEvent(this);
            MessageBox.Show($"{name} deleted");
        }
        public virtual void Move(object p) { }

        public virtual void Draw(UIElementCollection content) { }

    }
    
    
}
