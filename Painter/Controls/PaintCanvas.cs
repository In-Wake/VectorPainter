using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Painter.Controls
{
    public class PaintCanvas : Canvas
    {
        readonly TranslateTransform offset = new TranslateTransform();
        readonly TranslateTransform toViewPort = new TranslateTransform();
        readonly List<DrawingVisual> visuals = new List<DrawingVisual>();
        private Point position;

        protected override int VisualChildrenCount => visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        public int VisualCount { get => VisualChildrenCount; }

        public Point Position { get => position; set { if (position != value) { position = value; offset.X = position.X; offset.Y = position.Y; } } }

        public void Paint(DrawingVisual visual) {
            visual.Transform = offset;
            visuals.Add(visual);
            AddVisualChild(visual);
            //AddLogicalChild(visual);
        }

        public void Erase(DrawingVisual visual)
        {
            visuals.Remove(visual);
            RemoveVisualChild(visual);
            //RemoveLogicalChild(visual);
        }

        public List<DrawingVisual> GetVisuals(Geometry area) {
            var parameters = new GeometryHitTestParameters(area);

            var hits = new List<DrawingVisual>();

            VisualTreeHelper.HitTest(this, null, result => HitTestCallback(result, hits), parameters);
            return hits;
        }

        public void Scrool(Vector scroolTo) {
            offset.X += scroolTo.X;
            offset.Y += scroolTo.Y;
        }

        public Point TranslateToViewPort(Point point)
        {
            toViewPort.X = -offset.X;
            toViewPort.Y = -offset.Y;

            return toViewPort.Transform(point);
        }

        public IEnumerable<DrawingVisual> GetVisuals() {
            foreach (var visual in visuals)
            {
                yield return visual;
            }
        }

        static HitTestResultBehavior HitTestCallback(HitTestResult result, List<DrawingVisual> hits)
        {
            if (result.VisualHit is DrawingVisual hit)
            {
                hits.Add(hit);
            }

            return HitTestResultBehavior.Continue;

        }
    }
}
