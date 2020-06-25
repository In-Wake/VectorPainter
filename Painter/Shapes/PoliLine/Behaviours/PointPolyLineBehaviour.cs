using Painter.Shapes.Behaviours;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.PoliLine
{
    public class PointPolyLineBehaviour : ShapeBehaviour<PoliLineState>
    {
        private readonly LineHit lineHit;

        public PointPolyLineBehaviour(PoliLineState lineState, LineHit lineHit) : base(lineState)
        {
            this.lineHit = lineHit;
        }

        public override bool DoubleLeftClick(Point clickPoint)
        {
            return false;
        }

        public override bool LeftClick(Point clickPoint)
        {
            var points = shapeState.Points;

            var pointIndex = lineHit.PointIndex(points, clickPoint, shapeState.StrokeThickness);

            if (pointIndex != LineHit.MISS_INDEX)
            {
                shapeState.PointMoveIndex = pointIndex;
                shapeState.PointMove = points[pointIndex];

                return true;
            }

            return false;
        }

        public override bool MouseLeave()
        {
            var points = shapeState.Points;

            if (points.Count > 2)
            {
                var point = points[shapeState.PointMoveIndex];

                var leftPointIndex = shapeState.PointMoveIndex - 1;
                var rightPointIndex = shapeState.PointMoveIndex + 1;

                var pointTolerance = lineHit.GetPointHitTolerance(shapeState.StrokeThickness);

                var hitArea = lineHit.GetHitArea(point, pointTolerance);

                if (leftPointIndex >= 0 && hitArea.Contains(points[leftPointIndex]) || rightPointIndex < points.Count && hitArea.Contains(points[rightPointIndex]))
                {
                    points.RemoveAt(shapeState.PointMoveIndex);
                    return true;
                }
            }

            return false;
        }

        public override Cursor MouseOver(Point mousePosition)
        {
            return Cursors.SizeNS;
        }

        public override bool MoveTo(Point newPosition)
        {
            var points = shapeState.Points;

            points[shapeState.PointMoveIndex] = newPosition;

            return true;
        }
    }
}
