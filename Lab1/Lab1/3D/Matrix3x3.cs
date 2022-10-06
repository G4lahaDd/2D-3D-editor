using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation.Provider;

namespace Lab1._3D
{
    public enum RotationAxis
    {
        X,
        Z
    }

    public class Matrix3x3
    {
        public double[] elements;

        public Matrix3x3()
        {
            elements = new double[9]
            {
                1,0,0,
                0,1,0,
                0,0,1
            };
        }

        public Matrix3x3(RotationAxis rotationAxis, double angle) : this()
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    elements[4] = elements[8] = Math.Cos(angle);
                    elements[5] = -(elements[7] = Math.Sin(angle));
                    break;
                case RotationAxis.Z:
                    elements[0] = elements[4] = Math.Cos(angle);
                    elements[1] = -(elements[3] = Math.Sin(angle));
                    break;
            }
        }

        //indexer
        public double this[int index]
        {
            get
            {
                return elements[index];
            }

            set
            {
                elements[index] = value;
            }
        }
    }
}
