using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.Behaviours
{
    public abstract class ShapeBehaviour<T> where T : ShapeState
    {
        protected readonly T shapeState;

        public ShapeBehaviour(T shapeState)
        {
            this.shapeState = shapeState;
        }

        public abstract bool MoveTo(Point newPosition);

        public abstract Cursor MouseOver(Point mousePosition);

        public abstract bool DoubleLeftClick(Point clickPoint);

        public abstract bool MouseLeave();

        public abstract bool LeftClick(Point clickPoint);
    }
}
