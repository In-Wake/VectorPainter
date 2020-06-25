using Painter.Shapes.Behaviours;
using Painter.Shapes.PoliLine;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Painter.Shapes
{
    /// <summary>
    /// Ломанная
    /// </summary>
    public class PolyLineObject : IShapeObject
    {

        readonly PoliLineState state;

        readonly FreePoliLineBehaviour freeBehaviour;

        readonly List<ShapeBehaviour<PoliLineState>> behaviours;

        ShapeBehaviour<PoliLineState> currentBehaviour;

        Pen pen;
        Pen pointPen;
        Brush stroke;
        private bool isFreeze;

        public PolyLineObject(int id, System.Windows.Point start, System.Windows.Point end, Brush stroke, double strokeThickness, LineHit lineHit)
        {
            Freeze();

            ShapeId = id;

            state = new PoliLineState { StrokeThickness = strokeThickness, Points = new List<System.Windows.Point> { start, end } };

            freeBehaviour = new FreePoliLineBehaviour(state, lineHit);
            currentBehaviour = freeBehaviour;

            behaviours = new List<ShapeBehaviour<PoliLineState>> { 
                new PointPolyLineBehaviour(state, lineHit),
                new SelectPolyLineBehaviour(state),
                freeBehaviour };

            Shape = new DrawingVisual();
            Stroke = stroke;

            Unfreeze();
            
        }

        public int ShapeId { get; }

        public DrawingVisual Shape { get; }

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
            var points = state.Points;

            for (int i = 0; i < points.Count; i++)
            {
                points[i] = rotateTransform.Transform(points[i]);
            }
            Repaint();
        }

        public double StrokeThickness { get => state.StrokeThickness; set {
                if (state.StrokeThickness != value)
                {
                    state.StrokeThickness = value;
                    CreatePen();
                    Repaint();
                }
            } }

        public List<Point> Points
        {
            get => state.Points; set
            {
                if (state.Points != value)
                {
                    state.Points = value;
                    Repaint();
                }  } }

        public Point Position { get => state.Position; }

        public void Freeze()
        {
            isFreeze = true;
        }

        public void Unfreeze()
        {
            isFreeze = false;
            Repaint();

        }

        void CreatePen()
        {
            pen = new Pen(Stroke, StrokeThickness);
            pointPen = new Pen(Stroke, StrokeThickness);
        }

        void Repaint()
        {
            if (isFreeze) return;

            var points = state.Points;

            using (var context = Shape.RenderOpen())
            {
                for (int i = 1; i < points.Count; i++)
                {
                    context.DrawLine(pen, points[i - 1], points[i]);
                }

                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        context.DrawEllipse(pointPen.Brush, pointPen, points[i], 0, 0);
                    }
                }
            }
        }
    }
}
