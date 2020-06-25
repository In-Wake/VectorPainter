using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Utilities
{
    public class Property
    {
        public string PropName { get; set; }

        public Type PropType { get; set; }

        public Delegate Getter { get; set; }

        public Delegate Setter { get; set; }
    }
}
