using Lab1.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Lab1.Controller
{
    public delegate void DeleteObjectDelegate(GraphicObject graphicObject);
    public abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            MessageBox.Show("Default");
        }
    }
}
