using Painter.Shapes.Behaviours;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.PoliLine
{
    public class SelectPolyLineBehaviour : SelectShapeBehaviour<PoliLineState>
    {

        public SelectPolyLineBehaviour(PoliLineState lineState) : base(lineState)
        {
            
        }

        protected override bool MoveToOffset(Vector offset)
        {
            var points = shapeState.Points;

            for (int i = 0; i < points.Count; i++)
            {
                points[i] += offset;
            }

            return true;
        }
    }
}
