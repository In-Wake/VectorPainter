using System.Windows.Media;

namespace Painter.Shapes.Params
{
    public abstract class ColorParam<T> : ShapeParam<T>
        where T : class
    {
        public Color Color { get; set; }
    }

    public class ColorParam : ColorParam<IShapeObject>
    {
        protected override void ApplyParam(IShapeObject selection)
        {
            selection.Stroke = new SolidColorBrush(Color);
        }
    }

    public class FillParam : ColorParam<IFillObject> 
    {
        protected override void ApplyParam(IFillObject selection)
        {
            selection.Fill = new SolidColorBrush(Color);
        }
    }
}
