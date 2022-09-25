using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab1.Geometry
{
    public class Vertex : INotifyPropertyChanged
    {
        double x = 0;
        double y = 0;

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

        public Vertex()
        { }
        public Vertex(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public double GetX()
        {
            return x;
        }
        public double GetY()
        {
            return y;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
