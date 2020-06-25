using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Painter.Shapes.Behaviours
{
    public abstract class ShapeState
    {
        public abstract Point Position { get; }
    }
}
