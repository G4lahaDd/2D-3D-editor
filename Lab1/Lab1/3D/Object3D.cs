using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lab1._3D
{
    public class Object3D
    {
        ObservableCollection<Line3D> lines = new ObservableCollection<Line3D>();
        public ObservableCollection<Line3D> Lines 
        { 
            get { return lines; }
            set { lines = value; }
        }

    }
}
