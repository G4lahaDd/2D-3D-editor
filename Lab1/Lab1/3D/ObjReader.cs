using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Lab1._3D
{
    public class ObjReader
    {
        public static string path = @"C:\Ilya\BNTU\3D\SimplePaint\Lab1\Lab1\3D\Resource\";
        public static Object3D ReadObj(string name)
        {

            if(!File.Exists(path+name)) return null;
            List<string> file = File.ReadLines(path+name).ToList();
            Debug.WriteLine("do: " + file.Count);
            int faces = file.FindIndex(x => x[0] == 'f');
            int vectices = file.FindLastIndex(x => x[0] == 'v' && x[1] == ' ');
            file.RemoveRange(vectices + 1, faces - vectices - 1);
            Debug.WriteLine("after: " + file.Count);

            List<Vector3> points = new List<Vector3>();
            double x;
            double y;
            double z;
            foreach(string vert in file.FindAll(x => x[0] == 'v' && x[1] == ' '))
            {
                var temp = vert.Split(' '); 
                x = double.Parse(temp[1].Replace('.', ','));
                y = double.Parse(temp[2].Replace('.', ','));
                z = double.Parse(temp[3].Replace('.', ','));
                points.Add(new Vector3(x, y, z));
            }
            List<line> lines = new List<line>();
            foreach(string face in file.FindAll(x => x[0] == 'f'))
            {
                var temp = face.Split(' ');
                int[] keys = new int[temp.Length-1];
                for(int i = 1; i < temp.Length; i++) 
                {
                    keys[i-1] = int.Parse(temp[i].Split('/')[0]);
                }
                for(int i = 0; i < keys.Length; i++) 
                {
                    int j = i < keys.Length - 1 ? i + 1 : 0;
                    if(!lines.Exists(x => (x.i == keys[i] && x.j == keys[j]) || (x.i == keys[j] && x.j == keys[i]))) 
                    {
                        lines.Add(new line(keys[i],keys[j]));
                    }
                }
            }

            Object3D object3D = new Object3D();
            object3D.Name = "name";
            object3D.Position = new Vector3();
            object3D.Rotation = new Vector2();
            foreach(var line in lines) 
            {
                object3D.Lines.Add(new Line3D(points[line.i-1], points[line.j-1]));
            }
                
            return object3D;
        }

        public static GraphicObject3D ReadGraphicObj(string name)
        {
            if (!File.Exists(path + name)) return null;
            List<string> file = File.ReadLines(path + name).ToList();
            Debug.WriteLine("do: " + file.Count);
            int faces = file.FindIndex(x => x[0] == 'f');
            int vectices = file.FindLastIndex(x => x[0] == 'v' && x[1] == ' ');
            file.RemoveRange(vectices + 1, faces - vectices - 1);
            Debug.WriteLine("after: " + file.Count);

            GraphicObject3D object3D = new GraphicObject3D();

            List<Vector3> points = new List<Vector3>();
            double x;
            double y;
            double z;
            foreach (string vert in file.FindAll(x => x[0] == 'v' && x[1] == ' '))
            {
                var temp = vert.Split(' ');
                x = double.Parse(temp[1].Replace('.', ','));
                y = double.Parse(temp[2].Replace('.', ','));
                z = double.Parse(temp[3].Replace('.', ','));
                points.Add(new Vector3(x, y, z));
            }

            object3D.points = points;

            foreach (string face in file.FindAll(x => x[0] == 'f'))
            {
                Face tmpFace = new Face();

                var temp = face.Split(' ');
                for (int i = 1; i < temp.Length; i++)
                {
                    tmpFace.points.Add(int.Parse(temp[i].Split('/')[0]) - 1);
                }

                object3D.faces.Add(tmpFace);
            }
            object3D.Update();
            return object3D;
        }
        private struct line 
        {
           public int i;
           public int j;
           
            public line(int i, int j) 
            {
                this.i = i;
                this.j = j;
            }
        }
    }
}
