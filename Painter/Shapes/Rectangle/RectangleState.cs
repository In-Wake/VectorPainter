using Painter.Shapes.Behaviours;
using Painter.Shapes.Rectangle;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes
{
    public class RectangleState : ShapeState
    {
        public Point BottomLeft { get; set; }
        public Point BottomRight { get; set; }
        public Point TopLeft { get; set; }
        public Point TopRight { get; set; }

        public double StrokeThickness { get; set; }

        public override Point Position { get => TopLeft; }

        public double RotateValue { get; set; }

        public Point OriginTopLeft { get; set; }
        public Point OriginTopRight { get; set; }
        public Point OriginBottomLeft { get; set; }
        public Point OriginBottomRight { get; set; }

        public RectangleResizeMode GetResizeMode(Point target, LineHit lineHit) {

            var pointHitArea = lineHit.GetHitArea(target, StrokeThickness);

            var result = target switch {
                _ when pointHitArea.Contains(TopLeft) => RectangleResizeMode.TopLeft,
                _ when pointHitArea.Contains(BottomRight) => RectangleResizeMode.BottomRight,
                _ when pointHitArea.Contains(TopRight) => RectangleResizeMode.TopRight,
                _ when pointHitArea.Contains(BottomLeft) => RectangleResizeMode.BottomLeft,

                Point t when lineHit.PointInLine(TopLeft, TopRight, t, StrokeThickness) => RectangleResizeMode.Top,
                Point t when lineHit.PointInLine(TopRight, BottomRight, t, StrokeThickness) => RectangleResizeMode.Right,
                Point t when lineHit.PointInLine(BottomLeft, BottomRight, t, StrokeThickness) => RectangleResizeMode.Bottom,
                Point t when lineHit.PointInLine(TopLeft, BottomLeft, t, StrokeThickness) => RectangleResizeMode.Left,
                _ => RectangleResizeMode.None,
            };

            return result;
        }

        public Cursor GetCursor(RectangleResizeMode resizeMode) 
        {
            switch (resizeMode)
            {
                case RectangleResizeMode.TopLeft:
                    return Cursors.ScrollAll;
                case RectangleResizeMode.TopRight:
                    return Cursors.ScrollAll;
                case RectangleResizeMode.BottomLeft:
                    return Cursors.ScrollAll;
                case RectangleResizeMode.BottomRight:
                    return Cursors.ScrollAll;
                case RectangleResizeMode.Top:
                    return Cursors.SizeNS;
                case RectangleResizeMode.Left:
                    return Cursors.SizeWE;
                case RectangleResizeMode.Right:
                    return Cursors.SizeWE;
                case RectangleResizeMode.Bottom:
                    return Cursors.SizeNS;
                case RectangleResizeMode.None:
                default:
                    return Cursors.SizeAll;
            }
        }
    }
}
