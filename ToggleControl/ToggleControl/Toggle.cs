using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace ToggleControl
{
    public class Toggle : ToggleButton
    {
        /*
        #region Private Fields

        private const string SWITCH_TRANSFORM_NAME = "SwitchTransform";

        private TranslateTransform _switchTransform = null;
        private bool _isLoaded = false;

        #endregion

        #region Ctor

        public Toggle()
        {
            Loaded += LoadedEventHandler;
            Unloaded += UnloadedEventHandler;
        }

        #endregion

        #region Properties
        
        private TranslateTransform SwitchTransform => _switchTransform ??= FindControl<TranslateTransform>(SWITCH_TRANSFORM_NAME);

        /// <summary>
        /// Подпись <see cref="Toggle"/>.
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(Toggle), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Время переключения.
        /// </summary>
        public int SwitchDuration
        {
            get => (int)GetValue(SwitchDurationProperty);
            set => SetValue(SwitchDurationProperty, value);
        }

        public static readonly DependencyProperty SwitchDurationProperty = DependencyProperty.Register(nameof(SwitchDuration), typeof(int), typeof(Toggle), new UIPropertyMetadata(100));

        #endregion

        #region Private Methods

        private double? _checkedSwitchY = null;
        private double? _uncheckedSwitchY = null;

        private double GetSwitcherCoordinateY(bool isChecked)
        {
            if (_checkedSwitchY == null || _uncheckedSwitchY == null)
            {
                const string CHECK_ICON_IMAGE_NAME = "CheckIcon";
                const string UNCHECK_ICON_IMAGE_NAME = "UncheckIcon";
                // const string SWITCHER_NAME = "Switcher";

                Image checkIcon = FindControl<Image>(CHECK_ICON_IMAGE_NAME);
                Image uncheckIcon = FindControl<Image>(UNCHECK_ICON_IMAGE_NAME);
                // Ellipse switcher = FindControl<Ellipse>(SWITCHER_NAME);

                _uncheckedSwitchY = Canvas.GetLeft(checkIcon);
                _checkedSwitchY = Canvas.GetLeft(uncheckIcon);
            }

            return isChecked ? _checkedSwitchY.Value : _uncheckedSwitchY.Value;
        }

        private bool NeedDrawControl()
        {
            return _isLoaded && IsVisible;
        }

        /// <summary>
        /// Запуск анимации перемещения переключателя.
        /// </summary>
        /// <param name="duration">Время на перемещение переключателя.</param>
        private void ApplyAnimate(int duration = 0)
        {
            if (!NeedDrawControl())
            {
                return;
            }

            double yCoordinate = GetSwitcherCoordinateY(IsChecked == true);

            SwitchTransform.BeginAnimation(TranslateTransform.XProperty, MakeAnimation(yCoordinate, duration));
        }

        /// <summary>
        /// Возвращает класс анимации перемещения с ускорением в начале перемещения и замедлением в конце.
        /// </summary>
        private DoubleAnimation MakeAnimation(double to, int duration)
        {
            return new DoubleAnimation(to, TimeSpan.FromMilliseconds(duration))
            {
                AccelerationRatio = 0.2d,
                DecelerationRatio = 0.7d
            };
        }

        /// <summary>
        /// Возвращает контрол по имени.
        /// </summary>
        private T FindControl<T>(string controlName)
        {
            return (T)Template.FindName(controlName, this);
        }

        /// <summary>
        /// Перерисовка содержимого контрола.
        /// </summary>
        private void Redraw()
        {
            if (!NeedDrawControl())
            {
                return;
            }

            // Ставим переключатель в корректную позицию по оси Y
            double yCoordinate = GetSwitcherCoordinateY(IsChecked == true);
            SwitchTransform.BeginAnimation(TranslateTransform.YProperty, MakeAnimation(yCoordinate, 0));

            // Ставим переключатель в корректную позицию по оси X, в зависимости от значения параметра IsChecked
            ApplyAnimate();
        }

        private void LoadedEventHandler(object sender, RoutedEventArgs e)
        {
            SizeChanged += SizeChangedEventHandler;
            Checked += CheckedChangedEventHandler;
            Unchecked += CheckedChangedEventHandler;
            IsVisibleChanged += IsVisibleChangedEventHandler;

            _isLoaded = true;

            Redraw();
        }

        private void UnloadedEventHandler(object sender, RoutedEventArgs e)
        {
            SizeChanged -= SizeChangedEventHandler;
            Checked -= CheckedChangedEventHandler;
            Unchecked -= CheckedChangedEventHandler;
            IsVisibleChanged -= IsVisibleChangedEventHandler;
        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            Redraw();
        }

        private void CheckedChangedEventHandler(object sender, RoutedEventArgs e)
        {
            ApplyAnimate(SwitchDuration);
        }

        private void SizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                Redraw();
            }
        }

        #endregion
        */
    }
}
