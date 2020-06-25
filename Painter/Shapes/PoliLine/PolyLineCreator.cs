using Painter.Utilities;
using System;
using System.Windows;
using System.Windows.Media;

namespace Painter.Shapes
{
    public class PolyLineCreator : ShapeCreator
    {
        private readonly int id;
        private readonly int defaultLength;
        private readonly Brush stroke;
        private readonly double strokeThickness;
        private readonly LineHit lineHit;
        readonly Lazy<PolyLineObject> exampleFactory;

        public PolyLineCreator(int id, int defaultLength, Brush stroke, double strokeThickness, LineHit lineHit, ShapeSerializer shapeSerializer) : base(shapeSerializer)
        {
            this.id = id;
            this.defaultLength = defaultLength;
            this.stroke = stroke;
            this.strokeThickness = strokeThickness;
            this.lineHit = lineHit;
            exampleFactory = new Lazy<PolyLineObject>(() => new PolyLineObject(id, new Point(0, 0), new Point(0, 0), Brushes.Transparent, 0, lineHit));
        }

        public override IShapeObject Example { get => exampleFactory.Value; }

        public override IShapeObject Create(Point creationPoint)
        {
            var endPoint = new Point(creationPoint.X + defaultLength, creationPoint.Y);

            return new PolyLineObject(id, creationPoint, endPoint, stroke, strokeThickness, lineHit);
        }
    }
}
