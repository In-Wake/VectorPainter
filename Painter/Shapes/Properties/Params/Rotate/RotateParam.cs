using System.Windows.Media;

namespace Painter.Shapes.Params
{
    public class RotateParam : ShapeParam<IShapeObject>
    {
        public int Angle { get; set; }

        protected override void ApplyParam(IShapeObject target)
        {
            var transform = new RotateTransform(Angle, target.Position.X, target.Position.Y);
            target.Rotate(transform);
        }
    }
}
