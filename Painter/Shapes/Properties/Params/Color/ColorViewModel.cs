using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Painter.Shapes.Params
{
    public class ColorViewModel<T> : ViewModelBase
        where T : class
    {
        public ColorViewModel(ColorParam<T> command)
        {
            Command = command;
        }

        public Color Color
        {
            get => Command.Color; set
            {
                if (Command.Color != value)
                {
                    Command.Color = value;
                    RaisePropertyChanged();
                }
            }
        }


        public ColorParam<T> Command { get; }
    }
}
