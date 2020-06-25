using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Painter.Shapes.Params
{
    public enum ValueMode { 
        All = 0,
        PositiveOnly = 0b01,
    }

    /// <summary>
    /// Interaction logic for RotateView.xaml
    /// </summary>
    public partial class IntInputView : UserControl
    {
        public IntInputView()
        {
            InitializeComponent();
        }
    }
}
