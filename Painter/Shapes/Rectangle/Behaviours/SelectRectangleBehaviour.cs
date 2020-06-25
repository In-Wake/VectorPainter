using Painter.Shapes.Behaviours;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Painter.Shapes.Rectangle.Behaviours
{
    public class SelectRectangleBehaviour : SelectShapeBehaviour<RectangleState>
    {
        public SelectRectangleBehaviour(RectangleState state) : base(state)
        {
        }

        protected override bool MoveToOffset(Vector offset)
        {
            //shapeState.Rect.Offset(offset);

            shapeState.TopLeft += offset;
            shapeState.TopRight += offset;
            shapeState.BottomRight += offset;
            shapeState.BottomLeft += offset;

            return true;
        }
    }
}
