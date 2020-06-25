using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter
{
    public interface IMouseStrategy
    {
        void MouseDown(Point clickPoint, MouseButton button, int clickCount);

        void MouseMove(Point newPosition);

        void MouseUp(Point clickPoint, MouseButton button);
    }
}
