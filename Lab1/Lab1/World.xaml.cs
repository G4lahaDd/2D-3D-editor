using Lab1._3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для World.xaml
    /// </summary>
    public partial class World : Window
    {
        Scene scene;
        public static World world;
        public World()
        {
            InitializeComponent();
            world = this;
            scene = new Scene(Screen.Children);
        }


        private void s_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => scene.OnMouseDown(sender, e);        
        private void s_MouseLeftButtoUp(object sender, MouseButtonEventArgs e) => scene.OnMouseUp(sender, e);
        private void s_MouseMove(object sender, MouseEventArgs e) => scene.OnMouseMove(sender, e);
        private void s_MouseWheel(object sender, MouseWheelEventArgs e) => scene.OnScroll(sender, e);
        private void s_KeyDown(object sender, KeyEventArgs e) => scene.OnShiftDown(sender, e);
        private void s_KeyUp(object sender, KeyEventArgs e) => scene.OnShiftUp(sender, e);
    }
}
