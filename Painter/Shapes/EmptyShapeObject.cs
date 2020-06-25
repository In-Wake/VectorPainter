using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Painter.Shapes
{
    public class EmptyShapeObject : IShapeObject
    {
        public int ShapeId => int.MinValue;

        public DrawingVisual Shape { get; }

        public Point Position => new Point(int.MinValue, int.MinValue);

        public double StrokeThickness { get; set; }
        public Brush Stroke { get; set; }

        public void DoubleLeftClick(Point clickPoint)
        {

        }

        public void Freeze()
        {
            
        }

        public void LeftClick(Point clickPoint)
        {

        }

        public void MouseLeave()
        {

        }

        public Cursor MouseOver(Point mousePosition)
        {
            return null;
        }

        public void MoveTo(Point newPosition)
        {
            
        }

        public void Rotate(RotateTransform rotateTransform)
        {
            
        }

        public void Unfreeze()
        {

        }
    }
}
