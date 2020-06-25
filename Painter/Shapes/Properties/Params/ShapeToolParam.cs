using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Input;

namespace Painter.Shapes.Params
{
    public class ShapeToolParam : ViewModelBase
    {
        bool canProcessSelection;
        private IShapeObject selection;

        public ShapeToolParam(string description, FrameworkElement view, IShapeParam param) {
            Description = description;
            View = view;
            Param = param;

            ExecuteShapeCommand = new RelayCommand(() => Param.Apply(selection), ()=> canProcessSelection);
        }

        public string Description { get; }
        public FrameworkElement View { get; }
        public IShapeParam Param { get; }

        public bool IsShow { get; private set; }

        public ICommand ExecuteShapeCommand { get; }

        public void SelectShape(IShapeObject selection, bool canProcess) {
            canProcessSelection = canProcess;
            this.selection = selection;

            IsShow = Param.CanApply(selection);
            RaisePropertyChanged(nameof(IsShow));
        }
    }
}
