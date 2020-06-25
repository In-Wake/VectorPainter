using Painter.Shapes;
using Painter.Shapes.Params;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Painter
{
    public class Bootstrapper
    {
        public void Start() {



            var host = new Host();

            var lineHit = new LineHit();

            var propCache = new PropertyCache();

            var shapeSerializer = new ShapeSerializer(propCache);

            var sceneStore = new JsonSceneStore();

            var rotateParam = new RotateParam { Angle = 0 };
            var rotateView = new IntInputView { DataContext = new RotateViewModel(rotateParam) };

            var thiknessParam = new ThiknessParam { Thikness = 5 };
            var thiknessView = new IntInputView { DataContext = new ThiknessViewModel(thiknessParam) };

            var colorParam = new ColorParam { Color = Colors.Black };
            var colorView = new ColorView { DataContext = new ColorViewModel<IShapeObject>(colorParam) };

            var fillParam = new FillParam { Color = Colors.Transparent };
            var fillView = new ColorView { DataContext = new ColorViewModel<IFillObject>(fillParam) };


            var vm = new HostViewModel(
                host.Canvas, 
                
                new List<ShapeToolParam> { 
                    new ShapeToolParam("Поворот", rotateView, rotateParam), 
                    new ShapeToolParam("Толщина", thiknessView, thiknessParam),
                    new ShapeToolParam("Цвет", colorView, colorParam),
                    new ShapeToolParam("Заливка", fillView, fillParam),
                },

                sceneStore,

                new ShapeTool(0, LoadImage("pack://application:,,,/Painter;component/Resources/Images/line.png"),
                    new PolyLineCreator(0, 50, Brushes.Black, 10, lineHit, shapeSerializer)),
                new ShapeTool(1, LoadImage("pack://application:,,,/Painter;component/Resources/Images/rectangle.png"),
                    new RectangleCreator(1, 50, 50, Brushes.Black, 10, Brushes.Transparent, lineHit, shapeSerializer))
            );

            host.DataContext = vm;


            App.Current.MainWindow = host;
            host.Show();
        }

        static ImageSource LoadImage(string path) {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path);
            image.EndInit();

            return image;
        }
    }
}
