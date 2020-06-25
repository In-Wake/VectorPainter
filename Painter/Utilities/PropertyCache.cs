using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter.Utilities
{
    public class PropertyCache
    {
        readonly Dictionary<Type, Dictionary<string, Property>> shapeProperty = new Dictionary<Type, Dictionary<string, Property>>();


        public Dictionary<string, Property> GetProperties(Type shapeType)
        {
            if (!shapeProperty.ContainsKey(shapeType))
            {
                var result = new Dictionary<string, Property>();

                var properties = shapeType.GetProperties().Where(p => p.CanRead && p.CanWrite);

                foreach (var prop in properties)
                {
                    result[prop.Name] = new Property
                    {
                        PropName = prop.Name,
                        PropType = prop.PropertyType,
                        Getter = prop.GetGetMethod().CreateDelegate(typeof(Func<,>).MakeGenericType(shapeType, prop.PropertyType)),
                        Setter = prop.GetSetMethod().CreateDelegate(typeof(Action<,>).MakeGenericType(shapeType, prop.PropertyType)),
                    };
                }

                shapeProperty[shapeType] = result;
            }

            return shapeProperty[shapeType];
        }
    }
}
