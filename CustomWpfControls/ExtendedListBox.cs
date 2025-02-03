using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomWpfControls
{
    /// <summary>
    /// ListBox с поддержкой масштабирования содержимого с помощью колесика мыши.
    /// При этом необходимо в разметке установить ScaleTransform в необходимом месте. Например:
    /// 
    /// <customWpfControls:ExtendedListBox.ItemTemplate>
    ///   <DataTemplate>
    ///     <Border>
    ///       <Image>
    ///         <Image.LayoutTransform>
    ///           <TransformGroup>
    ///             <ScaleTransform ScaleX="{Binding Scale, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type customWpfControls:ExtendedListBox}}}"
    ///                             ScaleY="{Binding Scale, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type customWpfControls:ExtendedListBox}}}"/>
    ///           </TransformGroup>
    ///         </Image.LayoutTransform>
    ///       </Image>
    ///     </Border>
    ///   </DataTemplate>
    /// </customWpfControls:ExtendedListBox.ItemTemplate>
    ///
    /// или
    ///
    /// <customWpfControls:ExtendedListBox.ItemsPanel>
    ///   <ItemsPanelTemplate>
    ///     <WrapPanel>
    ///       <WrapPanel>
    ///         <TransformGroup>
    ///           <ScaleTransform ScaleX="{Binding Scale, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type customWpfControls:ExtendedListBox}}}"
    ///                           ScaleY="{Binding Scale, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type customWpfControls:ExtendedListBox}}}"/>
    ///         </TransformGroup>
    ///       </WrapPanel>
    ///     </WrapPanel>
    ///   </ItemsPanelTemplate>
    /// </customWpfControls:ExtendedListBox.ItemsPanel>
    /// 
    /// </summary>
    public class ExtendedListBox : ListBox
    {
        #region Ctor.

        public ExtendedListBox()
        {
            PreviewMouseWheel += OnMouseWheelEventHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Множитель размера элементов, используемый в данный момент для отображения 
        /// </summary>
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), 
            typeof(double),
            typeof(ExtendedListBox), 
            new FrameworkPropertyMetadata(1d,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure)
            {
                CoerceValueCallback = ScalePropertyCoerceValueCallback
            });

        /// <summary>
        /// Минимально возможный масштаб.
        /// </summary>
        public double MinScale
        {
            get => (double)GetValue(MinScaleProperty);
            set => SetValue(MinScaleProperty, value);
        }

        public static readonly DependencyProperty MinScaleProperty = DependencyProperty.Register(nameof(MinScale), 
            typeof(double),
            typeof(ExtendedListBox),
            new UIPropertyMetadata(0.1d, MinScalePropertyChangedCallback, MinScalePropertyCoerceValueCallback));

        /// <summary>
        /// Максимально возможный масштаб.
        /// </summary>
        public double MaxScale
        {
            get => (double)GetValue(MaxScaleProperty);
            set => SetValue(MaxScaleProperty, value);
        }

        public static readonly DependencyProperty MaxScaleProperty = DependencyProperty.Register(nameof(MaxScale),
            typeof(double), 
            typeof(ExtendedListBox),
            new UIPropertyMetadata(10d, MaxScalePropertyChangedCallback, MaxScalePropertyCoerceValueCallback));

        /// <summary>
        /// Признак, что включено изменение масштаба.
        /// </summary>
        public bool ResizeEnable
        {
            get => (bool)GetValue(ResizeEnableProperty);
            set => SetValue(ResizeEnableProperty, value);
        }

        public static readonly DependencyProperty ResizeEnableProperty = DependencyProperty.Register(nameof(ResizeEnable),
            typeof(bool),
            typeof(ExtendedListBox),
            new UIPropertyMetadata(true, ResizeEnablePropertyChangedCallback));

        #endregion

        #region Private Methods

        private static object ScalePropertyCoerceValueCallback(DependencyObject d, object value)
        {
            if ((double) value < (double) d.GetValue(MinScaleProperty) ||
                (double) value > (double) d.GetValue(MaxScaleProperty))
            {
                return DependencyProperty.UnsetValue;
            }

            return value;
        }

        private static void MaxScalePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((double)e.NewValue < (double)d.GetValue(ScaleProperty))
            {
                d.SetValue(ScaleProperty, e.NewValue);
            }
        }

        private static object MaxScalePropertyCoerceValueCallback(DependencyObject d, object value)
        {
            if ((double)value <= (double)d.GetValue(MinScaleProperty))
            {
                return (double)d.GetValue(MaxScaleProperty);
            }

            return value;
        }

        private static void MinScalePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((double)e.NewValue > (double)d.GetValue(ScaleProperty))
            {
                d.SetValue(ScaleProperty, e.NewValue);
            }
        }

        private static object MinScalePropertyCoerceValueCallback(DependencyObject d, object value)
        {
            double newValue = (double)value;
            if (newValue >= (double) d.GetValue(MaxScaleProperty) || newValue <= 0)
            {
                return (double) d.GetValue(MinScaleProperty);
            }

            return value;
        }
        
        private static void ResizeEnablePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool) e.NewValue)
            {
                if ((double) d.GetValue(MinScaleProperty) <= 1 && (double) d.GetValue(MaxScaleProperty) >= 1)
                {
                    ((ExtendedListBox) d).Scale = 1;
                }
                else
                {
                    ((ExtendedListBox)d).Scale = (double)d.GetValue(MaxScaleProperty);
                }
            }
        }

        private void OnMouseWheelEventHandler(object sender, MouseWheelEventArgs e)
        {
            if (!ResizeEnable || Keyboard.Modifiers != ModifierKeys.Control)
            {
                return;
            }

            const double DELTA_DIVISOR = 500d;

            double zoomScale = e.Delta / DELTA_DIVISOR;
            double newScaleFactor = Scale + zoomScale;

            Scale = newScaleFactor;
            
            e.Handled = true;
        }

        #endregion
    }
}
