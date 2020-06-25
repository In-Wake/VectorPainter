using Painter.Shapes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter
{
    public class ShapeMouseStrategy : IMouseStrategy
    {
        readonly IScene scene;
        bool isCapture;

        public ShapeMouseStrategy(IScene scene) {
            this.scene = scene;
        }

        public IShapeObject Target { get; set; }

        

        public void MouseDown(Point clickPoint, MouseButton button, int clickCount)
        {
            if (button == MouseButton.Left)
            {
                var localClickPoint = scene.TranslateToLocal(clickPoint);

                switch (clickCount)
                {
                    case 1:
                        Target?.LeftClick(localClickPoint);
                        isCapture = true;
                        break;
                    case 2:
                        Target?.DoubleLeftClick(localClickPoint);
                        break;
                    default:
                        break;
                }
            }
        }

        public void MouseMove(Point newPosition)
        {
            var localPosition = scene.TranslateToLocal(newPosition);

            if (isCapture)
            {
                Target?.MoveTo(localPosition);
            }
            else
            {
                Mouse.OverrideCursor = Target?.MouseOver(localPosition);
            }
        }

        public void MouseUp(Point clickPoint, MouseButton button)
        {

            Target?.MouseLeave();
            isCapture = false;
        }
    }
}
