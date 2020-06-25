using System.Collections.Generic;
using System.Windows;

namespace Painter.Shapes
{
    public abstract class ShapeCreator : IShapeCreate
    {
        private readonly ShapeSerializer shapeSerialize;

        public ShapeCreator(ShapeSerializer shapeSerialize)
        {
            this.shapeSerialize = shapeSerialize;
        }

        public abstract IShapeObject Example { get; }

        public abstract IShapeObject Create(Point creationPoint);

        public IShapeObject Deserialize(Dictionary<string, object> shapeState)
        {
            var shape = Create(new Point(0,0));

            shape.Freeze();

            shapeSerialize.Deserialize(shape, shapeState);

            shape.Unfreeze();

            return shape;
        }

        public Dictionary<string, object> Serialize(IShapeObject shape)
        {
            return shapeSerialize.Serialize(shape);
        }
    }
}
