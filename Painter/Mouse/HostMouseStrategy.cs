using Painter.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Painter
{
    public class HostMouseStrategy : IMouseStrategy
    {
        enum State {
            Free,
            HostCapture,
            ShapeCapture,
        }

        readonly IScene scene;
        readonly EllipseGeometry hitArea;
        readonly ShapeMouseStrategy shapeStrategy;
        Vector mouseOffset;

        State currentState;

        public HostMouseStrategy(IScene scene, EllipseGeometry hitArea, ShapeMouseStrategy shapeStrategy)
        {
            this.scene = scene;
            this.hitArea = hitArea;
            this.shapeStrategy = shapeStrategy;
        }

        public void MouseDown(Point clickPoint, MouseButton button, int clickCount)
        {
            if (button == MouseButton.Left)
            {
                switch (currentState)
                {
                    case State.Free:
                        {
                            var hit = GetHit(clickPoint);

                            if (hit != null)
                            {
                                shapeStrategy.Target = hit;
                                shapeStrategy.MouseDown(clickPoint, button, clickCount);
                                currentState = State.ShapeCapture;
                            }
                            else 
                            {
                                if (button == MouseButton.Left && clickCount == 1)
                                {
                                    mouseOffset = clickPoint - scene.Host.Position;
                                    currentState = State.HostCapture;
                                }
                            }
                        }
                        break;
                    case State.HostCapture:
                    case State.ShapeCapture:
                    default:
                        break;
                }
            }
        }

        public void MouseMove(Point newPosition)
        {
            switch (currentState)
            {
                case State.Free:
                    {
                        IShapeObject hit = GetHit(newPosition);

                        if (shapeStrategy.Target != hit)
                        {
                            shapeStrategy.Target?.MouseLeave();
                            shapeStrategy.Target = hit;
                        }

                        shapeStrategy.MouseMove(newPosition);
                    }
                    break;
                case State.HostCapture:
                    {
                        var movePoint = newPosition - mouseOffset;
                        scene.Host.Position = movePoint;
                    }
                    break;
                case State.ShapeCapture:
                    {
                        shapeStrategy.MouseMove(newPosition);
                    }
                    break;
                default:
                    break;
            }
        }

        public void MouseUp(Point clickPoint, MouseButton button)
        {
            switch (currentState)
            {
                case State.HostCapture:
                    if (button == MouseButton.Left)
                    {
                        currentState = State.Free;
                    }
                    break;
                case State.ShapeCapture:
                    if (button == MouseButton.Left)
                    {
                        shapeStrategy.MouseUp(clickPoint, button);
                        currentState = State.Free;
                        scene.SelectedShape = GetHit(clickPoint);
                    }
                    break;
                case State.Free:
                default:
                    break;
            }
        }

        IShapeObject GetHit(Point mousePoint)
        {
            hitArea.Center = mousePoint;

            var visuals = scene.Host.GetVisuals(hitArea);

            var hit = visuals.Select(v => ShapeTool.GetShapeObject(v)).FirstOrDefault();
            return hit;
        }
    }
}
