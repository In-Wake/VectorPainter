using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Painter
{
    class HostProxyBinding : Freezable
    {
            #region Overrides of Freezable

            protected override Freezable CreateInstanceCore()
            {
                return new HostProxyBinding();
            }

            #endregion

            public object DataSource
            {
                get { return (object)GetValue(DataProperty); }
                set { SetValue(DataProperty, value); }
            }

            public static readonly DependencyProperty DataProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(HostProxyBinding), new UIPropertyMetadata(null));
        }
}
