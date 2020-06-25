using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Shapes.Params
{
    public class ThiknessViewModel : ViewModelBase
    {
        public ThiknessViewModel(ThiknessParam command)
        {
            Command = command;
        }

        public int Minimum { get => 1; }

        public int IntValue
        {
            get => Command.Thikness; set
            {
                if (Command.Thikness != value)
                {
                    Command.Thikness = value;
                    RaisePropertyChanged();
                }
            }
        }


        public ThiknessParam Command { get; }
    }
}
