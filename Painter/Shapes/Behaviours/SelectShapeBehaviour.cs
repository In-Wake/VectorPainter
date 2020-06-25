using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.Behaviours
{
    public abstract class SelectShapeBehaviour<T> : ShapeBehaviour<T> where T : ShapeState
    {
        protected Vector shiftOffset;

        public SelectShapeBehaviour(T state) : base(state) { 
        }

        public override bool DoubleLeftClick(Point clickPoint)
        {
            return false;
        }

        public override bool LeftClick(Point clickPoint)
        {
            shiftOffset = clickPoint - shapeState.Position;
            return true;
        }

        public override bool MouseLeave()
        {
            return false;
        }

        public override Cursor MouseOver(Point mousePosition)
        {
            return Cursors.SizeAll;
        }

        public override bool MoveTo(Point newPosition)
        {
            var offset = newPosition - shapeState.Position - shiftOffset;

            return MoveToOffset(offset);
        }

        protected abstract bool MoveToOffset(Vector offset);
    }
}
