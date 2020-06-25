namespace Painter.Shapes.Params
{
    public class ThiknessParam : ShapeParam<IShapeObject>
    {
        public int Thikness { get; set; }

        protected override void ApplyParam(IShapeObject target)
        {
            target.StrokeThickness = Thikness;
        }
    }
}
