using Painter.Shapes.Behaviours;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Painter.Shapes.Rectangle.Behaviours
{
    public class ResizeRectangleBehaviour : ShapeBehaviour<RectangleState>
    {
        readonly LineHit lineHit;

        RectangleResizeMode mode;
        Point capturePoint;

        Matrix transformToOrigin;
        Matrix transform;

        public ResizeRectangleBehaviour(RectangleState shapeState, LineHit lineHit) : base(shapeState)
        {
            this.lineHit = lineHit;
        }

        public override bool DoubleLeftClick(Point clickPoint)
        {
            return false;
        }

        public override bool LeftClick(Point clickPoint)
        {
            transformToOrigin = new Matrix();
            transformToOrigin.RotateAt(shapeState.RotateValue, shapeState.Position.X, shapeState.Position.Y);
            transformToOrigin.Invert();

            transform = new Matrix();
            transform.RotateAt(-shapeState.RotateValue, shapeState.Position.X, shapeState.Position.Y);
            transform.Invert();

            shapeState.OriginTopLeft = transformToOrigin.Transform(shapeState.TopLeft);
            shapeState.OriginTopRight = transformToOrigin.Transform(shapeState.TopRight);
            shapeState.OriginBottomLeft = transformToOrigin.Transform(shapeState.BottomLeft);
            shapeState.OriginBottomRight = transformToOrigin.Transform(shapeState.BottomRight);

            mode = shapeState.GetResizeMode(clickPoint, lineHit);

            switch (mode)
            {
                case RectangleResizeMode.TopLeft:
                    {
                        capturePoint = shapeState.OriginTopLeft;

                        return true;
                    }
                case RectangleResizeMode.TopRight:
                    {
                        capturePoint = shapeState.OriginTopRight;

                        return true;
                    }
                case RectangleResizeMode.BottomLeft:
                    {
                        capturePoint = shapeState.OriginBottomLeft;

                        return true;
                    }
                case RectangleResizeMode.BottomRight:
                    {
                        capturePoint = shapeState.OriginBottomRight;

                        return true;
                    }
                case RectangleResizeMode.Top:
                case RectangleResizeMode.Left:
                case RectangleResizeMode.Right:
                case RectangleResizeMode.Bottom:
                    {

                        capturePoint = transformToOrigin.Transform(clickPoint);
                        return true;
                    }
                case RectangleResizeMode.None:
                default:
                    return false;
            }
        }

        public override bool MouseLeave()
        {
            return false;
        }

        public override Cursor MouseOver(Point mousePosition)
        {
            return shapeState.GetCursor(shapeState.GetResizeMode(mousePosition, lineHit));
        }

        public override bool MoveTo(Point newPosition)
        {
            var transformPosition = transformToOrigin.Transform(newPosition);

            var offset = transformPosition - capturePoint;

            switch (mode)
            {
                case RectangleResizeMode.TopLeft:
                    {
                        var topLeft = shapeState.OriginTopLeft + offset;
                        var bottomLeft = new Point(topLeft.X, shapeState.OriginBottomLeft.Y);
                        var topRight = new Point(shapeState.OriginTopRight.X, topLeft.Y);

                        shapeState.TopLeft = transform.Transform(topLeft);
                        shapeState.BottomLeft = transform.Transform(bottomLeft);
                        shapeState.TopRight = transform.Transform(topRight);

                        return true;
                    }
                case RectangleResizeMode.TopRight:
                    {
                        var topRight = shapeState.OriginTopRight + offset;
                        var topLeft = new Point(shapeState.OriginBottomLeft.X, topRight.Y);
                        var bottomRight = new Point(topRight.X, shapeState.OriginBottomRight.Y);

                        shapeState.TopRight = transform.Transform(topRight);
                        shapeState.TopLeft = transform.Transform(topLeft);
                        shapeState.BottomRight = transform.Transform(bottomRight);

                        return true;
                    }
                case RectangleResizeMode.BottomLeft:
                    {
                        var bottomLeft = shapeState.OriginBottomLeft + offset;
                        var topLeft = new Point(bottomLeft.X, shapeState.OriginTopLeft.Y);
                        var bottomRight = new Point(shapeState.OriginBottomRight.X, bottomLeft.Y);

                        shapeState.BottomLeft = transform.Transform(bottomLeft);
                        shapeState.TopLeft = transform.Transform(topLeft);
                        shapeState.BottomRight = transform.Transform(bottomRight);

                        return true;
                    }
                case RectangleResizeMode.BottomRight:
                    {
                        var bottomRight = shapeState.OriginBottomRight + offset;
                        var bottomLeft = new Point(shapeState.OriginBottomLeft.X, bottomRight.Y);
                        var topRight = new Point(bottomRight.X, shapeState.OriginTopRight.Y);

                        shapeState.BottomRight = transform.Transform(bottomRight);
                        shapeState.BottomLeft = transform.Transform(bottomLeft);
                        shapeState.TopRight = transform.Transform(topRight);


                        return true;
                    }
                case RectangleResizeMode.Top:
                    {
                        var topLeft = new Point(shapeState.OriginTopLeft.X, shapeState.OriginTopLeft.Y + offset.Y);
                        var topRight = new Point(shapeState.OriginTopRight.X, shapeState.OriginTopRight.Y + offset.Y);

                        shapeState.TopLeft = transform.Transform(topLeft);
                        shapeState.TopRight = transform.Transform(topRight);

                        return true;
                    }
                case RectangleResizeMode.Left:
                    {
                        var topLeft = new Point(shapeState.OriginTopLeft.X + offset.X, shapeState.OriginTopLeft.Y);
                        var bottomLeft = new Point(shapeState.OriginBottomLeft.X + offset.X, shapeState.OriginBottomLeft.Y);

                        shapeState.TopLeft = transform.Transform(topLeft);
                        shapeState.BottomLeft = transform.Transform(bottomLeft);

                        return true;
                    }
                case RectangleResizeMode.Right:
                    {
                        var topRight = new Point(shapeState.OriginTopRight.X + offset.X, shapeState.OriginTopRight.Y);
                        var bottomRight = new Point(shapeState.OriginBottomRight.X + offset.X, shapeState.OriginBottomRight.Y);

                        shapeState.TopRight = transform.Transform(topRight);
                        shapeState.BottomRight = transform.Transform(bottomRight);

                        return true;
                    }
                case RectangleResizeMode.Bottom:
                    {
                        var bottomLeft = new Point(shapeState.OriginBottomLeft.X, shapeState.OriginBottomLeft.Y + offset.Y);
                        var bottomRight = new Point(shapeState.OriginBottomRight.X, shapeState.OriginBottomRight.Y + offset.Y);

                        shapeState.BottomLeft = transform.Transform(bottomLeft);
                        shapeState.BottomRight = transform.Transform(bottomRight);

                        return true;
                    }
                case RectangleResizeMode.None:
                default:
                    return false;
            }
        }
    }
}
