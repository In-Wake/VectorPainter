using Painter.Shapes.Behaviours;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Painter.Shapes.PoliLine
{
    public class PoliLineState : ShapeState
    {
        public List<Point> Points { get; set; }

        public double StrokeThickness { get; set; }

        public override Point Position { get => Points[0]; }

        public int PointMoveIndex { get; set; }

        public Point PointMove { get; set; }
    }
}
