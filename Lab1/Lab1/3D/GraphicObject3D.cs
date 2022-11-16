using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Lab1._3D
{
    public class GraphicObject3D
    {
        public List<Vector3> points = new List<Vector3>();
        public List<Face> faces = new List<Face>();

        
        public void Update()
        {
            foreach (Face face in faces)
            {
                int i = face.points[0];
                int j = face.points[1];
                int k = face.points[2];

                Vector3 v1 = points[i] - points[j];
                Vector3 v2 = points[k] - points[j];

                double A = v1.y * v2.z - v1.z * v2.z;
                double B =  - v1.x * v2.z + v1.z * v2.x;
                double C = v1.x * v2.y - v1.y * v2.x;

                face.normal = new Vector3(A, B, C);

                Vector3 midpoint = new Vector3();
                for(int o = 0; o < face.points.Count; o++) 
                {
                    midpoint += points[o];
                }

                face.midpoint = midpoint * (1 / face.points.Count);
            }
        }
            
    }
}
