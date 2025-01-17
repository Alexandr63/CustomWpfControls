using System;
using System.Windows;

namespace CustomWpfControls.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ResourceDictionary ThemeDictionary => Resources.MergedDictionaries[0];

        public Theme CurrentTheme { get; private set; } = Theme.Dark;
        
        public void ChangeTheme(Theme theme)
        {
            if (CurrentTheme == theme)
            {
                return;
            }

            CurrentTheme = theme;

            Uri themeSource;
            switch (CurrentTheme)
            {
                case Theme.Dark:
                    themeSource = new Uri("pack://application:,,,/CustomWpfControls;component/Style/DarkTheme/DarkTheme.xaml", UriKind.Absolute);
                    break;
                case Theme.Light:
                    themeSource = new Uri("pack://application:,,,/CustomWpfControls;component/Style/LightTheme/LightTheme.xaml", UriKind.Absolute);
                    break;
                default:
                    return;
            }

            ThemeDictionary.Clear();
            ThemeDictionary.Source = themeSource;

        }
    }

}
