using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace Painter.Shapes
{
    public interface IShapeCreate
    {
        IShapeObject Example { get; }

        IShapeObject Create(Point creationPoint);

        IShapeObject Deserialize(Dictionary<string, object> shapeState);

        Dictionary<string, object> Serialize(IShapeObject shape);
    }
}
