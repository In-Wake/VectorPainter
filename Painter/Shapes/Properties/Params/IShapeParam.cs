using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Shapes.Params
{
    public interface IShapeParam
    {
        bool CanApply(IShapeObject target);
        void Apply(IShapeObject target);
    }
}
