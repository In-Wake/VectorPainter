using Painter.Shapes.Behaviours;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.PoliLine
{
    public class FreePoliLineBehaviour : ShapeBehaviour<PoliLineState>
    {
        readonly LineHit lineHit;

        public FreePoliLineBehaviour(PoliLineState lineState, LineHit lineHit) : base(lineState)
        {
            this.lineHit = lineHit;
        }

        public override bool DoubleLeftClick(Point clickPoint)
        {
            var points = shapeState.Points;

            for (int i = 1; i < points.Count; i++)
            {
                if (lineHit.PointInLine(points[i - 1], points[i], clickPoint, shapeState.StrokeThickness))
                {
                    points.Insert(i, clickPoint);
                    return true;
                }
            }

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
            var pointIndex = lineHit.PointIndex(shapeState.Points, mousePosition, shapeState.StrokeThickness);

            if (pointIndex != LineHit.MISS_INDEX)
            {
                return Cursors.SizeNS;
            }

            return Cursors.SizeAll;
        }

        public override bool MoveTo(Point newPosition)
        {
            return false;
        }
    }
}
