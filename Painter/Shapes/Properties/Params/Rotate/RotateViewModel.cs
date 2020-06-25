using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Shapes.Params
{
    public class RotateViewModel : ViewModelBase
    {
        public RotateViewModel(RotateParam command) {
            Command = command;
        }

        public int IntValue
        {
            get => Command.Angle; set
            {
                if (Command.Angle != value)
                {
                    Command.Angle = value;
                    RaisePropertyChanged();
                } 
            } 
        }


        public RotateParam Command { get; }
    }
}
