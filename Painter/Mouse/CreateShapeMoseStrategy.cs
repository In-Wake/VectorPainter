using Painter.Shapes;
using Painter.Shapes.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter
{
    public class CreateShapeMoseStrategy : IMouseStrategy
    {
        readonly IScene scene;

        public CreateShapeMoseStrategy(IScene scene) {
            this.scene = scene;
        }

        public void MouseDown(Point clickPoint, MouseButton button, int clickCount)
        {
            
        }

        public void MouseMove(Point newPosition)
        {
            
        }

        public void MouseUp(Point clickPoint, MouseButton button)
        {
            if (button == MouseButton.Left && scene.SelectedShapeTool != null)
            {
                var localPoint = scene.TranslateToLocal(clickPoint);

                var shapeObject = scene.SelectedShapeTool.Create(localPoint);

                shapeObject.Freeze();

                foreach (var param in scene.ShapeParams)
                {
                    param.Apply(shapeObject);
                }

                shapeObject.Unfreeze();

                AddShape(shapeObject, scene.SelectedShapeTool);

                scene.SelectedShape = shapeObject;

                scene.SelectedShapeTool = null;
            }
        }

        public void AddShape(IShapeObject shapeObject, ShapeTool tool) {
            var visual = shapeObject.Shape;

            ShapeTool.SetShapeObject(visual, shapeObject);
            ShapeTool.SetShapeCreate(visual, tool);

            scene.Host.Paint(visual);
        }
    }
}
