using Lab1.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Lab1._3D
{
    public delegate void UpdateDelegate();
    public class Object3D : INotifyPropertyChanged
    {
        private string name;
        private Vector3 position;
        private Vector2 rotation;

        public event PropertyChangedEventHandler PropertyChanged;
        public event UpdateDelegate UpdateChanged;
        public event DeleteObject3DDelegate DeleteObject3DEvent;

        public ICommand DeleteCommand { get; }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public List<Line3D> Lines = new List<Line3D>();

        public Object3D() 
        {
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            UpdateChanged?.Invoke();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnDeleteCommandExecuted(object p)
        {
            DeleteObject3DEvent(this);
            MessageBox.Show($"{name} deleted");
        }
    }
}
