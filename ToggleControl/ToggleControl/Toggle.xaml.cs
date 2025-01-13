using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ToggleControl
{
    /// <summary>
    /// Логика взаимодействия для Toggle.xaml
    /// </summary>
    public partial class Toggle
    {
        #region Private Fields

        private const string SWITCH_TRANSFORM_NAME = "SwitchTransform";
        private const double DEFAULT_HEIGHT = 42d;

        private TranslateTransform _switchTransform = null;

        private bool _isLoaded = false;

        #endregion

        #region Ctor

        public Toggle()
        {
            Height = DEFAULT_HEIGHT;
            
            InitializeComponent();

            Loaded += LoadedEventHandler;
            Unloaded += UnloadedEventHandler;
        }

        #endregion

        #region Properties

        private TranslateTransform SwitchTransform => _switchTransform ??= FindControl<TranslateTransform>(SWITCH_TRANSFORM_NAME);

        /// <summary>
        /// Значение флага IsChecked <see cref="Toggle"/>.
        /// Переопределил, так как переключатель не поддерживает нейтральную позицию с IsChecked = null.
        /// </summary>
        public new bool IsChecked
        {
            get
            {
                bool? isCheckedBase = (bool?)GetValue(IsCheckedProperty);
                return isCheckedBase ?? false;
            }
            set => SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Подпись <see cref="Toggle"/>.
        /// </summary>
        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(Toggle), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Цвет иконки, отображаемой, когда <see cref="IsChecked"/> == true.
        /// </summary>
        public Brush CheckIconColor
        {
            get => (Brush)GetValue(CheckIconColorProperty);
            set => SetValue(CheckIconColorProperty, value);
        }

        public static readonly DependencyProperty CheckIconColorProperty = DependencyProperty.Register(nameof(CheckIconColor), typeof(Brush), typeof(Toggle), new UIPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Цвет иконки, отображаемой, когда <see cref="IsChecked"/> == false.
        /// </summary>
        public Brush UncheckIconColor
        {
            get => (Brush)GetValue(UncheckIconColorProperty);
            set => SetValue(UncheckIconColorProperty, value);
        }

        public static readonly DependencyProperty UncheckIconColorProperty = DependencyProperty.Register(nameof(UncheckIconColor), typeof(Brush), typeof(Toggle), new UIPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Цвет переключателя.
        /// </summary>
        public Brush SwitcherColor
        {
            get => (Brush) GetValue(SwitcherColorProperty);
            set => SetValue(SwitcherColorProperty, value);
        }

        public static readonly DependencyProperty SwitcherColorProperty = DependencyProperty.Register(nameof(SwitcherColor), typeof(Brush), typeof(Toggle), new UIPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Значение полей переключателя.
        /// </summary>
        public double SwitcherMargin
        {
            get => (double) GetValue(SwitcherMarginProperty);
            set => SetValue(SwitcherMarginProperty, value);
        }

        public static readonly DependencyProperty SwitcherMarginProperty = DependencyProperty.Register(nameof(SwitcherMargin), typeof(double), typeof(Toggle), new UIPropertyMetadata(2d, SwitcherMarginPropertyChangedCallback));

        /// <summary>
        /// Время переключения.
        /// </summary>
        public int SwitchDuration
        {
            get => (int)GetValue(SwitchDurationProperty);
            set => SetValue(SwitchDurationProperty, value);
        }

        public static readonly DependencyProperty SwitchDurationProperty = DependencyProperty.Register(nameof(SwitchDuration), typeof(int), typeof(Toggle), new UIPropertyMetadata(100));

        /// <summary>
        /// Значение полей зоны переключателя.
        /// </summary>
        public new double BorderThickness
        {
            get => (double) GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        // Переопределил, так как толщина границ должна быть везде одинаковая. У ToggleButton BorderThickness по умолчанию 1, чтобы не было расхождений ставим у себя такое же значение.
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof(BorderThickness), typeof(double), typeof(Toggle), new UIPropertyMetadata(1d, BorderThicknessPropertyChangedCallback));

        /// <summary>
        /// Координата X иконки, отображаемой, когда <see cref="IsChecked"/> == true.
        /// </summary>
        public double CheckIconLeft
        {
            get => (double)GetValue(CheckIconLeftProperty);
            private set => SetValue(CheckIconLeftProperty, value);
        }

        public static readonly DependencyProperty CheckIconLeftProperty = DependencyProperty.Register(nameof(CheckIconLeft), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));

        /// <summary>
        /// Координата X иконки, отображаемой, когда <see cref="IsChecked"/> == false.
        /// </summary>
        public double UncheckIconLeft
        {
            get => (double)GetValue(UncheckIconLeftProperty);
            private set => SetValue(UncheckIconLeftProperty, value);
        }

        public static readonly DependencyProperty UncheckIconLeftProperty = DependencyProperty.Register(nameof(UncheckIconLeft), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));

        /// <summary>
        /// Координата Y иконок.
        /// </summary>
        public double IconTop
        {
            get => (double)GetValue(IconTopProperty);
            private set => SetValue(IconTopProperty, value);
        }

        public static readonly DependencyProperty IconTopProperty = DependencyProperty.Register(nameof(IconTop), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));

        /// <summary>
        /// Размер иконок.
        /// Так как иконки квадратные использовал double, а не Size.
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            private set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));
        
        /// <summary>
        /// Радиус скругления углов зоны переключателя.
        /// </summary>
        public CornerRadius BorderRadius
        {
            get => (CornerRadius)GetValue(BorderRadiusProperty);
            private set => SetValue(BorderRadiusProperty, value);
        }

        public static readonly DependencyProperty BorderRadiusProperty = DependencyProperty.Register(nameof(BorderRadius), typeof(CornerRadius), typeof(Toggle), new UIPropertyMetadata(new CornerRadius()));

        /// <summary>
        /// Радиус переключателя.
        /// </summary>
        public double SwitcherRadius
        {
            get => (double)GetValue(SwitcherRadiusProperty);
            private set => SetValue(SwitcherRadiusProperty, value);
        }

        public static readonly DependencyProperty SwitcherRadiusProperty = DependencyProperty.Register(nameof(SwitcherRadius), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));

        /// <summary>
        /// Ширина зоны переключателя.
        /// </summary>
        public double SwitchBorderWidth
        {
            get => (double)GetValue(SwitchBorderWidthProperty);
            private set => SetValue(SwitchBorderWidthProperty, value);
        }

        public static readonly DependencyProperty SwitchBorderWidthProperty = DependencyProperty.Register(nameof(SwitchBorderWidth), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));
        
        /// <summary>
        /// Высота зоны переключателя.
        /// </summary>
        public double SwitchBorderHeight
        {
            get => (double)GetValue(SwitchBorderHeightProperty);
            private set => SetValue(SwitchBorderHeightProperty, value);
        }

        public static readonly DependencyProperty SwitchBorderHeightProperty = DependencyProperty.Register(nameof(SwitchBorderHeight), typeof(double), typeof(Toggle), new UIPropertyMetadata(0d));
        
        /// <summary>
        /// Поля зоны переключателя.
        /// </summary>
        public Thickness SwitchBorderMargin
        {
            get => (Thickness)GetValue(SwitchBorderMarginProperty);
            private set => SetValue(SwitchBorderMarginProperty, value);
        }

        public static readonly DependencyProperty SwitchBorderMarginProperty = DependencyProperty.Register(nameof(SwitchBorderMargin), typeof(Thickness), typeof(Toggle), new UIPropertyMetadata(new Thickness(5d, 5d, 0d, 5d)));
        
        #endregion

        #region Private Methods

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

            // Если делать переключатель без отступа, то во включенном положении справа виден фон. Сдвигаем его вправо на 1 пиксель.
            double additionalShift = SwitcherMargin == 0d ? 1d : 0d;
            
            double xCoordinate = IsChecked ? UncheckIconLeft + additionalShift : CheckIconLeft;
                                             
            SwitchTransform.BeginAnimation(TranslateTransform.XProperty, MakeAnimation(xCoordinate, duration));
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
            return (T) Template.FindName(controlName, this);
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

            SwitchBorderHeight = Height - SwitchBorderMargin.Top - SwitchBorderMargin.Bottom; // Высота зоны переключателя равна высоте контрола минус маржины сверху и снизу
            SwitchBorderWidth = SwitchBorderHeight * 2d; // Ширина зоны переключателя в два раза больше ее высоты
            BorderRadius = new CornerRadius(SwitchBorderHeight / 2d); // Радиус скругления рассчитываем как половина высоты зоны переключателя

            SwitcherRadius = SwitchBorderHeight - SwitcherMargin * 2d - BorderThickness * 2d; // Радиус переключателя рассчитываем из высоты зоны переключателя минус маржин переключателя минус толщина границ поля переключателя сверху и снизу

            if (SwitcherRadius <= 0)
            {
                throw new ArgumentException("With current parameters of 'Height', 'SwitcherMargin' and 'BorderThickness' radius of switcher less or equal zero.");
            }

            IconSize = SwitcherRadius; // Размер иконок по высоте и ширине одинаковый и равен радиусу переключателя минус маржины с каждой стороны
            CheckIconLeft = SwitcherMargin; // Расположение иконки по оси X, указывающей, что переключатель включен
            UncheckIconLeft = SwitchBorderWidth - SwitcherRadius - SwitcherMargin - BorderThickness * 2d; // расположение иконки по оси X, указывающей, что переключатель выключен
            IconTop = SwitcherMargin; // Расположение иконок по оси Y

            // Ставим переключатель в корректную позицию по оси Y
            SwitchTransform.BeginAnimation(TranslateTransform.YProperty, MakeAnimation(SwitcherMargin, 0));

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

        private static void SwitcherMarginPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Toggle) d).Redraw();
        }

        private static void BorderThicknessPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ToggleButton) d).BorderThickness = new Thickness((double) e.NewValue);
            ((Toggle) d).Redraw();
        }

        #endregion
    }
}
