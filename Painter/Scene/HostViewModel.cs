using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Painter.Controls;
using Painter.Shapes;
using Painter.Shapes.Params;
using Painter.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Painter
{
    public class HostViewModel : ViewModelBase, IScene
    {
        enum ToolMode { 
            Host,
            CreateTool,
            DeleteShape,
        }

        readonly IScene scene;
        readonly HostMouseStrategy hostMouseStrategy;
        readonly CreateShapeMoseStrategy createMouseStrategy;
        readonly EmptyShapeObject emptyShape;
        readonly JsonSceneStore sceneStore;

        readonly Dictionary<int, Dictionary<string, Type>> shapeStateInfo;

        ToolMode mode;
        IMouseStrategy mouseStrategy;


        public HostViewModel(PaintCanvas host, List<ShapeToolParam> shapeParams, JsonSceneStore sceneStore , params ShapeTool[] tools) {

            shapeStateInfo = tools.ToDictionary(k => k.Id, v => v.Serialize(v.Example).ToDictionary(k => k.Key, v => v.Value.GetType()));

            ShapeTools = new ObservableCollection<ShapeTool>(tools);
            ShapeToolParams = new ObservableCollection<ShapeToolParam>(shapeParams);

            ShapeParams = new List<IShapeParam>(ShapeToolParams.Select(p => p.Param));

            emptyShape = new EmptyShapeObject();

            DeleteCommand = new RelayCommand(DeleteSelectedShape,()=>(selectedShape??emptyShape) != emptyShape && (selectedShape ?? emptyShape) != SelectedShapeTool?.Example);
            SaveCommand = new RelayCommand(SaveScene);
            LoadCommand = new RelayCommand(LoadScene);

            Host = host;
            this.sceneStore = sceneStore;
            Host.MouseUp += Host_MouseUp;
            Host.MouseMove += Host_MouseMove;
            Host.MouseDown += Host_MouseDown;

            scene = this;

            hostMouseStrategy = new HostMouseStrategy(
                scene, 
                new EllipseGeometry { RadiusX = 3, RadiusY = 3 }, 
                new ShapeMouseStrategy(scene));

            createMouseStrategy = new CreateShapeMoseStrategy(scene);

            SelectedShape = emptyShape;

            SetMode(ToolMode.Host);
        }

        public Point TranslateToLocal(Point point)
        {
            return Host.TranslateToViewPort(point);
        }

        public ObservableCollection<ShapeTool> ShapeTools { get; }

        public ObservableCollection<ShapeToolParam> ShapeToolParams { get; }

        ShapeTool selectedShapeTool;
        public ShapeTool SelectedShapeTool
        {
            get { return selectedShapeTool; }
            set
            {
                if (Set(ref selectedShapeTool, value))
                {
                    if (value == null)
                    {
                        SetMode(ToolMode.Host);
                    }
                    else 
                    {
                        SelectedShape = SelectedShapeTool.Example;
                        SetMode(ToolMode.CreateTool); 
                    }
                } 
            }
        }

        bool isFreeTool;
        private IShapeObject selectedShape;

        public bool IsFreeTool
        {
            get { return isFreeTool; }
            set
            {
                if (mode == ToolMode.Host)
                {
                    value = true;
                }

                if (Set(ref isFreeTool, value))
                {
                    if (value)
                    {
                        SetMode(ToolMode.Host);
                    }
                } 
            }
        }

        private double? operationProgress;

        public double? OperationProgress
        {
            get { return operationProgress; }
            set { Set(ref operationProgress, value); }
        }


        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand DeleteCommand { get; }

        public PaintCanvas Host { get; private set; }

        public List<IShapeParam> ShapeParams { get; private set; }

        public IShapeObject SelectedShape
        {
            get => selectedShape; set
            {
                if (Set(ref selectedShape, value??emptyShape))
                {
                    foreach (var toolParam in ShapeToolParams)
                    {
                        toolParam.SelectShape(SelectedShape, SelectedShape != emptyShape && SelectedShape != SelectedShapeTool?.Example);
                    }
                } 
            } 
        }

        private void DeleteSelectedShape()
        {
            Host.Erase(SelectedShape.Shape);

            SelectedShape = emptyShape;
        }

        private void SetMode(ToolMode changeMode)
        {
            mode = changeMode;

            switch (mode)
            {
                case ToolMode.Host:
                    mouseStrategy = hostMouseStrategy;
                    SelectedShapeTool = null;
                    break;
                case ToolMode.CreateTool:
                    mouseStrategy = createMouseStrategy;
                    IsFreeTool = false;
                    break;
                case ToolMode.DeleteShape:
                    break;
                default:
                    break;
            }
        }

        private void Host_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var clickPoint = e.GetPosition(Host);

                mouseStrategy.MouseDown(clickPoint, e.ChangedButton, e.ClickCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Host_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                var newPosition = e.GetPosition(Host);

                mouseStrategy.MouseMove(newPosition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Host_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var clickPoint = e.GetPosition(Host);

                mouseStrategy.MouseUp(clickPoint, e.ChangedButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveScene()
        {
            Task delay = Task.FromResult(true);

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = $"{sceneStore.Extension} files (*.{sceneStore.Extension})|*.{sceneStore.Extension}|All files (*.*)|*.*",
                    RestoreDirectory = true
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    delay = Task.Delay(1000);

                    OperationProgress = 0;

                    using (var writer = new StreamWriter(new FileStream(saveFileDialog.FileName, FileMode.Create)))
                    {
                        await sceneStore.WriteHeaderAsync(Host.Position, writer);

                        OperationProgress = 0.02;

                        var delta = (1 - OperationProgress.Value) / Host.VisualCount;

                        foreach (var visualShape in Host.GetVisuals())
                        {
                            var shapeObject = ShapeTool.GetShapeObject(visualShape);
                            var creator = ShapeTool.GetShapeCreate(visualShape);

                            var shapestate = new ShapeStoreState { Id = shapeObject.ShapeId, Props = creator.Serialize(shapeObject) };

                            await sceneStore.WriteAsync(shapestate, writer);
                            writer.WriteLine();

                            OperationProgress += delta;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { await delay; OperationProgress = null; }
        }

        private async void LoadScene()
        {
            Task delay = Task.FromResult(true);

            try
            {

                var openFileDialog = new OpenFileDialog
                {
                    Filter = $"{sceneStore.Extension} files (*.{sceneStore.Extension})|*.{sceneStore.Extension}|All files (*.*)|*.*",
                    RestoreDirectory = true
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    OperationProgress = 0;

                    delay = Task.Delay(1000);

                    using (var reader = new StreamReader(File.OpenRead(openFileDialog.FileName)))
                    {
                        const int CHUNCK_COUNT = 100;

                        while (Host.VisualCount > 0)
                        {
                            foreach (var visual in Host.GetVisuals().Take(CHUNCK_COUNT).ToArray())
                            {
                                Host.Erase(visual);
                            }

                            //что бы интерфейс не вис
                            await System.Threading.Tasks.Task.Delay(10);
                        }

                        var delta = 1 / (double)reader.BaseStream.Length;

                        var position = await sceneStore.ReadHeaderAsync<Point>(reader);

                        Host.Position = position;

                        OperationProgress = reader.BaseStream.Position * delta;

                        while (!reader.EndOfStream)
                        {
                            var shapeState = await sceneStore.ReadAsync(reader, shapeStateInfo);

                            
                            if (shapeState == null && reader.EndOfStream)
                            {
                                break;
                            }

                            var shapeTool = ShapeTools.FirstOrDefault(tool => tool.Id == shapeState.Id);

                            if (shapeTool == null)
                            {
                                throw new ArgumentOutOfRangeException(nameof(shapeState.Id), $"Unknown tool id = {shapeState.Id}");
                            }

                            var shape = shapeTool.Deserialize(shapeState.Props);

                            createMouseStrategy.AddShape(shape, shapeTool);

                            OperationProgress = reader.BaseStream.Position * delta;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { await delay; OperationProgress = null; }
        }
    }
}
