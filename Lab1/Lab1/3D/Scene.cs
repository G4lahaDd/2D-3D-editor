using Lab1.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Lab1._3D
{
    internal enum State
    {
        Idle,
        Move,
        Rotate,
        Zoom
    }

    internal class UpdateData
    {
        public State state;
        public Vector2 delta;
        public UpdateData(State state, Vector2 delta)
        {
            this.state = state;
            this.delta = delta;
        }

        public UpdateData(State state, double delta)
        {
            this.state = state;
            this.delta = new Vector2(delta, 0);
        }
    }

    public class Scene
    {
        public List<Object3D> objects;
        private List<Line3D> defaultScene = new List<Line3D>()
        {
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,3,0), p2 = new Vector3(3,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,2,0), p2 = new Vector3(3,2,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,1,0), p2 = new Vector3(3,1,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,0,0), p2 = new Vector3(3,0,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,-1,0), p2 = new Vector3(3,-1,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,-2,0), p2 = new Vector3(3,-2,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,-3,0), p2 = new Vector3(3,-3,0), thickness = 1},

            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(3,-3,0), p2 = new Vector3(3,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(2,-3,0), p2 = new Vector3(2,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(1,-3,0), p2 = new Vector3(1,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(0,-3,0), p2 = new Vector3(0,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-1,-3,0), p2 = new Vector3(-1,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-2,-3,0), p2 = new Vector3(-2,3,0), thickness = 1},
            new Line3D(){ brush = Brushes.Gray, p1 = new Vector3(-3,-3,0), p2 = new Vector3(-3,3,0), thickness = 1},

            new Line3D(){ brush = Brushes.Blue, p1 = new Vector3(), p2 = new Vector3(0,0,1), thickness = 2},
            new Line3D(){ brush = Brushes.Green, p1 = new Vector3(), p2 = new Vector3(0,1,0), thickness = 2},
            new Line3D(){ brush = Brushes.Red, p1 = new Vector3(), p2 = new Vector3(1,0,0), thickness = 2}
        };


        public Vector3 origin = new Vector3();
        public Vector2 rotation = new Vector2(Math.PI / 4, Math.PI / 4); //x,z

        public double radius = 10;
        public double minR = 1;
        public double maxR = 50;
        public double scrollSens = 0.01d;
        public double moveSens = 0.01;
        public double rotateSens = 0.01;

        //Local Coordinates
        private Vector3 xDefaultDirection = new Vector3(1, 0, 0);
        private Vector3 xDirection;
        private Vector3 yDefaultDirection = new Vector3(0, 1, 0);
        private Vector3 yDirection;
        private Vector3 zDefaultDirection = new Vector3(0, 0, 1);
        private Vector3 zDirection;

        private State state = State.Idle;
        private Point prevMousePos;
        private bool isMouseDown = false;
        private bool isShiftDown = false;
        private Camera camera = new Camera();
        private UIElementCollection content;

        private Queue<UpdateData> updateQueue = new Queue<UpdateData>();
        Thread thread;

        public Scene(UIElementCollection content)
        {
            this.content = content;
            camera.width = 1920;
            camera.height = 1080;
            camera.FOV = Math.PI * 10 / 18;
            UpdateLocalCoolrdinates();
            TransformCamera();
            Redraw();
            thread = new Thread(Update);
            thread.Start();
        }

        public void Update()
        {
            UpdateData data;
            while (true)
            {
                if (updateQueue.Count > 0)
                {
                    data = updateQueue.Dequeue();
                    switch(data.state)
                    {
                        case State.Move:
                            Move(data.delta);
                            break;
                        case State.Rotate:
                            Rotation(data.delta);
                            break;
                        case State.Zoom:
                            Zoom(data.delta.x);
                            break;
                    }
                    Redraw();
                }
            }
        }

        public void Redraw() 
        {
            World.world.Dispatcher.Invoke(() =>
            {
                content.Clear();
                //camera.GetPointOnScreen(new Vector3());
                foreach (Line3D line in defaultScene)
                {
                    content.Add(camera.DrawLine2(line).GetLine());
                }
            });
        }

        #region Transform

        private void UpdateLocalCoolrdinates()
        {
            xDirection = xDefaultDirection.Rotate(rotation);
            yDirection = yDefaultDirection.Rotate(rotation);
            zDirection = zDefaultDirection.Rotate(rotation);
        }

        private void Rotation(Vector2 delta)
        {
            rotation += delta * rotateSens;
            UpdateLocalCoolrdinates();
            TransformCamera();
        }
        private void Move(Vector2 delta)
        {
            origin += xDirection * delta.x;
            origin += zDirection * delta.y;
            TransformCamera();
        }
        private void Zoom(double delta)
        {

            radius += delta * scrollSens;
            if (radius < minR) radius = minR;
            else if (radius > maxR) radius = maxR;
            TransformCamera();
        }
        private void TransformCamera()
        {
            camera.position = origin + yDirection * radius;
            camera.rotation = new Vector3(-rotation.x + Math.PI, 0, rotation.y - Math.PI/2);
            camera.ViewRay = yDirection * (-1);
            camera.rightAxis = xDirection * (-1);
            camera.upAxis = zDirection;
        }

        #endregion

        #region Events
        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("mouse down");
            isMouseDown = true;
            if (isShiftDown) state = State.Move;
            else state = State.Rotate;
            prevMousePos = e.GetPosition(sender as IInputElement);
            Debug.WriteLine($"mouse down pos {prevMousePos.ToString()}");
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("mouse up");
            state = State.Idle;
            isMouseDown = false;
        }

        public void OnShiftDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("shift down");
            if (e.Key != Key.LeftShift) return;
            if (isMouseDown) state = State.Move;
            else state = State.Idle;
            isShiftDown = true;
        }

        public void OnShiftUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("shift up");
            if (e.Key != Key.LeftShift) return;
            isShiftDown = false;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("mouse move");
            if (state != State.Move && state != State.Rotate) return;
            Vector2 delta = new Vector2();
            Point pos = e.GetPosition(sender as IInputElement);
            delta.y = -(pos.X - prevMousePos.X);
            delta.x = pos.Y - prevMousePos.Y;
            Debug.WriteLine($"mouse move delta {delta.ToString()}");
            prevMousePos = pos;
            updateQueue.Enqueue(new UpdateData(state, delta));
        }

        public void OnScroll(object sender, MouseWheelEventArgs e)
        {
            Debug.WriteLine("scroll");
            updateQueue.Enqueue(new UpdateData(State.Zoom, e.Delta));
        }
        #endregion
    }
}
