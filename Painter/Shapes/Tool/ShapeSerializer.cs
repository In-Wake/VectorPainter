using Painter.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Painter.Utilities;

namespace Painter.Shapes
{
    public class ShapeSerializer
    {
        private readonly PropertyCache propertyCache;

        public ShapeSerializer(PropertyCache propertyCache)
        {
            this.propertyCache = propertyCache;
        }

        public void Deserialize(IShapeObject shape, Dictionary<string, object> shapeState)
        {
            shape = shape ?? throw new ArgumentNullException(nameof(shape));

            var shapeType = shape.GetType();

            var props = propertyCache.GetProperties(shapeType);

            foreach ((string propName, object value) in shapeState)
            {
                props[propName].Setter.DynamicInvoke(shape, value);
            } 
        }

        public Dictionary<string, object> Serialize(IShapeObject shape)
        {
            shape = shape ?? throw new ArgumentNullException(nameof(shape));

            var shapeType = shape.GetType();
            var shapeProperty = propertyCache.GetProperties(shapeType);

            var result = shapeProperty.Values.ToDictionary(k => k.PropName, v => v.Getter.DynamicInvoke(shape));

            return result;

        }
    }
}
