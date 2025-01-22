using System.Windows;
using CustomWpfControls.Sample.ViewModels;

namespace CustomWpfControls.Sample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void ThemeToggleButtonCheckedEventHandler(object sender, RoutedEventArgs e)
        {
            App app = (App) Application.Current;
            app.ChangeTheme(app.CurrentTheme == Theme.Dark ? Theme.Light : Theme.Dark);
        }


    }
}