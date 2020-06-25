using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Painter.Shapes
{
    public class ShapeTool : ViewModelBase, IShapeCreate
    {
        public static IShapeObject GetShapeObject(DependencyObject obj)
        {
            return (IShapeObject)obj.GetValue(ShapeObjectProperty);
        }

        public static void SetShapeObject(DependencyObject obj, IShapeObject value)
        {
            obj.SetValue(ShapeObjectProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShapeObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapeObjectProperty =
            DependencyProperty.RegisterAttached("ShapeObject", typeof(IShapeObject), typeof(ShapeTool), new PropertyMetadata(null));


        public static IShapeCreate GetShapeCreate(DependencyObject obj)
        {
            return (IShapeCreate)obj.GetValue(ShapeCreateProperty);
        }

        public static void SetShapeCreate(DependencyObject obj, IShapeCreate value)
        {
            obj.SetValue(ShapeCreateProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShapeCreate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapeCreateProperty =
            DependencyProperty.RegisterAttached("ShapeCreate", typeof(IShapeCreate), typeof(ShapeTool), new PropertyMetadata(null));



        readonly IShapeCreate creator;

        public ShapeTool(int id, ImageSource source, IShapeCreate creator) {
            Id = id;
            Source = source;
            this.creator = creator;
        }

        public int Id { get; }
        public ImageSource Source { get; }

        public IShapeObject Create(Point creationPoint) {
            return creator.Create(creationPoint);
        }

        public IShapeObject Deserialize(Dictionary<string, object> shapeState)
        {
            return creator.Deserialize(shapeState);
        }

        public Dictionary<string, object> Serialize(IShapeObject shape)
        {
            return creator.Serialize(shape);
        }

        public IShapeObject Example { get => creator.Example; }
    }
}
