using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CustomWpfControls.Sample.Helpers;
using CustomWpfControls.Sample.Models;
using CustomWpfControls.Sample.ViewModels;
using CustomWpfControls.Tools;

namespace CustomWpfControls.Sample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _model = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _model;
        }

        private void ThemeToggleButtonCheckedEventHandler(object sender, RoutedEventArgs e)
        {
            App app = (App) Application.Current;
            app.ChangeTheme(app.CurrentTheme == Theme.Dark ? Theme.Light : Theme.Dark);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            List<ImageModel> images = new List<ImageModel>()
            {
                new ImageModel {ImageSource = BitmapImageHelper.FileToBitmapImage("Images/2.jpg"), Tag = "new-2"},
                new ImageModel {ImageSource = BitmapImageHelper.FileToBitmapImage("Images/3-4.jpg"), Tag = "new-3-4"},
                new ImageModel {ImageSource = BitmapImageHelper.FileToBitmapImage("Images/10-11a.jpg"), Tag = "10-11a"},
            };
            foreach (ImageModel image in images)
            {
                image.Height /= MainWindowViewModel.RESIZE_FACTOR;
                image.Width /= MainWindowViewModel.RESIZE_FACTOR;
            }

            _model.Images5.AddRange(images);

        }
        
        //private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    ScrollContentPresenter presenter = ControlsHelper.GetVisualChild<ScrollContentPresenter>((DependencyObject)sender);
        //    MouseWheelZoom mouseWheelZoom = new MouseWheelZoom(presenter);
        //    PreviewMouseWheel += mouseWheelZoom.Zoom;
        //}
    }
}