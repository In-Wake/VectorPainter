using Painter.Shapes.Behaviours;
using Painter.Shapes.Params;
using Painter.Shapes.Rectangle.Behaviours;
using Painter.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Painter.Shapes
{
    public class RectangleObject : IShapeObject, IFillObject
    {
        readonly FreeRectangleBehaviour freeBehaviour;
        readonly List<ShapeBehaviour<RectangleState>> behaviours;
        readonly RectangleState state;

        private Brush stroke;
        private Pen pen;
        private Brush fill;

        ShapeBehaviour<RectangleState> currentBehaviour;
        private bool isFreeze;

        public RectangleObject(int id, Point leftTop, Point rightBottom, Brush stroke, double strokeThickness, Brush fill, LineHit lineHit)
        {
            Freeze();

            ShapeId = id;

            var rightTop = new Point(rightBottom.X, leftTop.Y);
            var leftBottom = new Point(leftTop.X, rightBottom.Y);

            state = new RectangleState { TopLeft = leftTop, TopRight = rightTop, BottomRight = rightBottom, BottomLeft = leftBottom, StrokeThickness = strokeThickness };

            freeBehaviour = new FreeRectangleBehaviour(state, lineHit);

            behaviours = new List<ShapeBehaviour<RectangleState>> {
                new ResizeRectangleBehaviour(state, lineHit),
                new SelectRectangleBehaviour(state),
                freeBehaviour,
            };

            currentBehaviour = freeBehaviour;

            Shape = new DrawingVisual();

            Stroke = stroke;
            Fill = fill;

            Unfreeze();
        }

        public int ShapeId { get; }

        public DrawingVisual Shape { get; }

        public Point Position => state.Position;

        public double StrokeThickness
        {
            get => state.StrokeThickness; set
            {
                if (state.StrokeThickness != value)
                {
                    state.StrokeThickness = value;
                    CreatePen();
                    Repaint();
                }
            }
        }

        public Brush Stroke
        {
            get => stroke;
            set
            {
                if (Stroke != value)
                {
                    stroke = value;
                    CreatePen();
                    Repaint();
                }
            }
        }

        public Brush Fill
        {
            get => fill;
            set
            {
                if (fill != value)
                {
                    fill = value;
                    Repaint();
                }
            }
        }

        public Point BottomLeft { get => state.BottomLeft; set => state.BottomLeft = value; }
        public Point BottomRight { get => state.BottomRight; set => state.BottomRight = value; }
        public Point TopLeft { get => state.TopLeft; set => state.TopLeft = value; }
        public Point TopRight { get => state.TopRight; set => state.TopRight = value; }

        public double RotateValue { get => state.RotateValue; set => state.RotateValue = value; }

        public void MoveTo(Point newPosition)
        {
            if (currentBehaviour.MoveTo(newPosition))
            {
                Repaint();
            }
        }

        public void LeftClick(Point clickPoint)
        {
            currentBehaviour = behaviours.First(behaviour => behaviour.LeftClick(clickPoint));
        }

        public Cursor MouseOver(Point mousePosition)
        {
            return currentBehaviour.MouseOver(mousePosition);
        }

        public void MouseLeave()
        {
            if (currentBehaviour.MouseLeave())
            {
                Repaint();
            }

            currentBehaviour = freeBehaviour;
        }

        public void DoubleLeftClick(Point clickPoint)
        {
            if (currentBehaviour.DoubleLeftClick(clickPoint))
            {
                Repaint();
            }
        }

        public void Rotate(RotateTransform rotateTransform)
        {

            state.TopLeft = rotateTransform.Transform(state.TopLeft);
            state.TopRight = rotateTransform.Transform(state.TopRight);
            state.BottomLeft = rotateTransform.Transform(state.BottomLeft);
            state.BottomRight = rotateTransform.Transform(state.BottomRight);

            state.RotateValue += rotateTransform.Angle;

            Repaint();
        }

        public void Freeze()
        {
            isFreeze = true;
        }

        public void Unfreeze()
        {
            isFreeze = false;
            Repaint();

        }

        private void CreatePen()
        {
            pen = new Pen(Stroke, StrokeThickness);
        }

        private void Repaint()
        {
            if (isFreeze) return;

            using (var context = Shape.RenderOpen())
            {
                StreamGeometry streamGeometry = new StreamGeometry();
                using (StreamGeometryContext geometryContext = streamGeometry.Open())
                {
                    geometryContext.BeginFigure(state.TopLeft, true, true);
                    PointCollection points = new PointCollection
                                             {
                                                 state.TopRight,
                                                 state.BottomRight,
                                                 state.BottomLeft,
                                             };
                    geometryContext.PolyLineTo(points, true, true);
                }

                context.DrawGeometry(Fill, pen, streamGeometry);
            }
        }
    }
}
