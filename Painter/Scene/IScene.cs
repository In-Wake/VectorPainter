using Painter.Controls;
using Painter.Shapes;
using Painter.Shapes.Params;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Painter
{
    public interface IScene
    {
        public PaintCanvas Host { get; }
        public List<IShapeParam> ShapeParams { get; }

        public IShapeObject SelectedShape { get; set; }

        public ShapeTool SelectedShapeTool { get; set; }

        public Point TranslateToLocal(Point point);
    }
}
