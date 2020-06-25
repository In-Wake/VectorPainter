using Painter.Shapes.Behaviours;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.Rectangle.Behaviours
{
    public class FreeRectangleBehaviour : ShapeBehaviour<RectangleState>
    {
        readonly LineHit lineHit;

        public FreeRectangleBehaviour(RectangleState state, LineHit lineHit) : base(state)
        {
            this.lineHit = lineHit;
        }

        public override bool DoubleLeftClick(Point clickPoint)
        {
            return false;
        }

        public override bool LeftClick(Point clickPoint)
        {
            return true;
        }

        public override bool MouseLeave()
        {
            return false;
        }

        public override Cursor MouseOver(Point mousePosition)
        {
            return shapeState.GetCursor( shapeState.GetResizeMode(mousePosition, lineHit));
        }

        public override bool MoveTo(Point newPosition)
        {
            return false;
        }
    }
}
