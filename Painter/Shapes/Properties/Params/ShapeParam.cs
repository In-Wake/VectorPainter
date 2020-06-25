using Painter.Shapes.Params;
using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Shapes.Params
{
    public abstract class ShapeParam<T> : IShapeParam
        where T : class
    {

        public void Apply(IShapeObject target)
        {
            if (target is T selection)
            {
                ApplyParam(selection);
            }
        }

        public bool CanApply(IShapeObject target)
        {
            return target is T;
        }

        protected abstract void ApplyParam(T selection);
    }
}
