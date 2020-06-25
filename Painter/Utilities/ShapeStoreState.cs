using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Utilities
{
    public class ShapeStoreState
    {
        public ShapeStoreState() {
            Props = new Dictionary<string, object>();
        }

        public int Id { get; set; }

        public Dictionary<string, object> Props { get; set; }
    }
}
