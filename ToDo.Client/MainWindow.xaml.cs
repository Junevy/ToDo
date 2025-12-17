using Wpf.Ui;

namespace ToDo.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(ISnackbarService snackbarService)
        {
            InitializeComponent();
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            //SnackbarPresenter.AllowDrop = true;
        }
    }
}
