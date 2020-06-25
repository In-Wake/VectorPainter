using Painter.Utilities;
using System;
using System.Windows;
using System.Windows.Media;

namespace Painter.Shapes
{
    public class RectangleCreator : ShapeCreator
    {
        private readonly int id;
        private readonly int defaultLength;
        private readonly int defaultHeight;
        private readonly Brush stroke;
        private readonly double strokeThickness;
        private readonly Brush fill;
        private readonly LineHit lineHit;

        readonly Lazy<RectangleObject> exampleFactory;

        public RectangleCreator(int id, int defaultLength, int defaultHeight, Brush stroke, double strokeThickness, Brush fill, LineHit lineHit, ShapeSerializer shapeSerializer) : base(shapeSerializer) {
            this.id = id;
            this.defaultLength = defaultLength;
            this.defaultHeight = defaultHeight;
            this.stroke = stroke;
            this.strokeThickness = strokeThickness;
            this.fill = fill;
            this.lineHit = lineHit;

            exampleFactory = new Lazy<RectangleObject>(()=>new RectangleObject(id, new Point(0,0), new Point(0, 0), Brushes.Transparent, 0, Brushes.Transparent, lineHit));
        }

        public override IShapeObject Example { get => exampleFactory.Value; }

        public override IShapeObject Create(Point creationPoint)
        {
            var topLeft = creationPoint;

            var bottomRight = new Point(topLeft.X + defaultLength, topLeft.Y + defaultHeight);

            return new RectangleObject(id, topLeft, bottomRight, stroke, strokeThickness, fill, lineHit);
        }
    }
}
