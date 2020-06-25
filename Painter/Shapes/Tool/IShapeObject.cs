using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Painter.Shapes
{
    public interface IShapeObject
    {
        int ShapeId { get; }

        DrawingVisual Shape { get; }

        Point Position { get; }

        double StrokeThickness { get; set; }

        Brush Stroke { get; set; }

        void MoveTo(Point newPosition);

        void LeftClick(Point clickPoint);

        void DoubleLeftClick(Point clickPoint);

        Cursor MouseOver(Point mousePosition);

        void MouseLeave();

        void Rotate(RotateTransform rotateTransform);

        void Freeze();
        void Unfreeze();
    }
}
