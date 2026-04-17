using CustomWpfControls.Tools;
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
        /// Признак, что включено изменение масштаба с помощью мыши.
        /// </summary>
        public bool MouseResizeEnable
        {
            get => (bool)GetValue(MouseResizeEnableProperty);
            set => SetValue(MouseResizeEnableProperty, value);
        }

        public static readonly DependencyProperty MouseResizeEnableProperty = DependencyProperty.Register(nameof(MouseResizeEnable),
            typeof(bool),
            typeof(ExtendedListBox),
            new UIPropertyMetadata(true, MouseResizeEnablePropertyChangedCallback));

        /// <summary>
        /// Признак, что скрол колесиком мыши по умолчанию - по вертикали.
        /// </summary>
        public bool IsVerticalMouseWheelScrollDefault
        {
            get => (bool)GetValue(IsVerticalMouseWheelScrollDefaultProperty);
            set => SetValue(IsVerticalMouseWheelScrollDefaultProperty, value);
        }

        public static readonly DependencyProperty IsVerticalMouseWheelScrollDefaultProperty = DependencyProperty.Register(nameof(IsVerticalMouseWheelScrollDefault),
            typeof(bool),
            typeof(ExtendedListBox),
            new UIPropertyMetadata(true));
        
        #endregion

        #region Private Methods

        private static object ScalePropertyCoerceValueCallback(DependencyObject d, object value)
        {
            ExtendedListBox control = (ExtendedListBox)d;

            if (control.MouseResizeEnable)
            {
                if ((double) value < control.MinScale ||
                    (double) value > control.MaxScale)
                {
                    return DependencyProperty.UnsetValue;
                }
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
        
        private static void MouseResizeEnablePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                ExtendedListBox control = (ExtendedListBox) d;

                if (control.Scale < control.MinScale)
                {
                    control.Scale = control.MinScale;
                }
                else if (control.Scale > control.MaxScale)
                {
                    control.Scale = control.MaxScale;
                }
            }
        }

        private void OnMouseWheelEventHandler(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (MouseResizeEnable)
                {
                    ProcessScale(e);
                    e.Handled = true;
                }
            }
            else
            {
                ProcessScroll(sender, e);
                e.Handled = true;
            }
        }

        private void ProcessScroll(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = ControlsHelper.GetVisualChild<ScrollViewer>(sender as ExtendedListBox);

            if (scrollViewer == null)
            {
                return;
            }

            if (IsVerticalMouseWheelScrollDefault)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                }
                else 
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                }
            }
            else
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                }
                else
                {
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
                }
            }

            e.Handled = true;
        }

        private void ProcessScale(MouseWheelEventArgs e)
        {
            const double DELTA_DIVISOR = 1000d;

            double zoomScale = e.Delta / DELTA_DIVISOR;
            double newScaleFactor = Scale + zoomScale;

            if (newScaleFactor >= MinScale && newScaleFactor <= MaxScale)
            {
                Scale = newScaleFactor;
            }
            else if (newScaleFactor < MinScale)
            {
                Scale = MinScale;
            }
            else if (newScaleFactor > MaxScale)
            {
                Scale = MaxScale;
            }
        }

        #endregion
    }
}
