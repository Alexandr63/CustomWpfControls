using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CustomWpfControls.LayoutStrategies;

namespace CustomWpfControls
{
    public sealed partial class DragAnimatedPanel : Panel
    {
        #region Private Fields

        private Size _calculatedSize;

        private ILayoutStrategy _layoutStrategy;

        #endregion

        #region Ctor

        public DragAnimatedPanel()
        {
            UpdateLayoutStrategy();

            MouseLeftButtonUp += OnMouseUp;
            LostMouseCapture += OnLostMouseCapture;
            MouseMove += OnMouseMove;
            
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(OnMouseDown), true);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Тип заполнения панели: колонка, строка, построчное заполнение и т.п.
        /// </summary>
        public FillType FillType
        {
            get => (FillType)GetValue(FillTypeProperty);
            set => SetValue(FillTypeProperty, value);
        }

        public static readonly DependencyProperty FillTypeProperty = DependencyProperty.Register(nameof(FillType), 
            typeof(FillType),
            typeof(DragAnimatedPanel),
            new FrameworkPropertyMetadata(FillType.Wrap, 
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                FillTypePropertyChangedCallback)
        );

        /// <summary>
        /// Время анимации перестроения документов при изменении размера панели и т.п.
        /// </summary>
        public int AnimationMilliseconds
        {
            get => (int)GetValue(AnimationMillisecondsProperty);
            set => SetValue(AnimationMillisecondsProperty, value);
        }

        public static readonly DependencyProperty AnimationMillisecondsProperty = DependencyProperty.Register(nameof(AnimationMilliseconds),
            typeof(int),
            typeof(DragAnimatedPanel),
            new UIPropertyMetadata(75)
        );

        #endregion

        #region Override Methods

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            if (Children.Count == 0)
            {
                _calculatedSize = new Size();
            }
            else
            {
                List<Size> childSizes = new List<Size>(Children.Count);
                foreach (UIElement child in Children)
                {
                    childSizes.Add(child.DesiredSize);
                }

                _layoutStrategy.MeasureLayout(availableSize, childSizes, DraggedElement != null);

                _calculatedSize = _layoutStrategy.ResultSize;
            }

            return _calculatedSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // Сначала помещаем все элементы в точку (0;0), после чего анимация поместит их в нужное место
            foreach (UIElement child in InternalChildren)
            {
                // Ставим TransformGroup в RenderTransform для использования нашей анимации
                if (!(child.RenderTransform is TransformGroup))
                {
                    child.RenderTransformOrigin = new Point(0.5, 0.5);
                    TransformGroup group = new TransformGroup();
                    child.RenderTransform = group;
                    group.Children.Add(new TranslateTransform());
                }

                // Помещаем все элементы в точку (0;0)
                child.Arrange(new Rect(new Point(0, 0), child.DesiredSize));
            }

            AnimateAll();

            return _calculatedSize;
        }

        #endregion

        #region Private Methods

        private void UpdateLayoutStrategy()
        {
            switch (FillType)
            {
                case FillType.Column:
                    _layoutStrategy = new ColumnLayoutStrategy();
                    break;
                case FillType.Row:
                    _layoutStrategy = new RowLayoutStrategy();
                    break;
                case FillType.Table:
                    _layoutStrategy = new TableLayoutStrategy();
                    break;
                case FillType.Wrap:
                    _layoutStrategy = new WrapLayoutStrategy();
                    break;
                default:
                    throw new ArgumentException($"Unknown FillType {FillType}");
            }

            InvalidateMeasure();
        }
        
        private void AnimateAll()
        {
            if (Children.Count == 0)
            {
                return;
            }

            double horizontalPosition = 0;
            double verticalPosition = 0;

            int rowIndex = 0;
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];

                // Отображаем перемещение всех элементов кроме того, которые перемещает пользователь
                if (child != DraggedElement)
                {
                    AnimateTo(child, horizontalPosition, verticalPosition, IsLoaded ? AnimationMilliseconds : 0);
                }

                ItemLayoutInfo currentLayoutInfo = _layoutStrategy.GetLayoutInfo(i);
                horizontalPosition += currentLayoutInfo.ColumnWidth;

                if (i + 1 < Children.Count)
                {
                    ItemLayoutInfo nextLayoutInfo = _layoutStrategy.GetLayoutInfo(i + 1);
                    if (nextLayoutInfo.RowIndex > rowIndex)
                    {
                        rowIndex++;
                        horizontalPosition = 0;
                        verticalPosition += currentLayoutInfo.RowHeight;
                    }
                }
            }
        }

        private void AnimateTo(UIElement child, double x, double y, int duration)
        {
            TransformGroup group = (TransformGroup)child.RenderTransform;
            TranslateTransform trans = (TranslateTransform)group.Children.First((groupElement) => groupElement is TranslateTransform);

            trans.BeginAnimation(TranslateTransform.XProperty, MakeAnimation(x, duration));
            trans.BeginAnimation(TranslateTransform.YProperty, MakeAnimation(y, duration));
        }

        private DoubleAnimation MakeAnimation(double to, int duration)
        {
            return new DoubleAnimation(to, TimeSpan.FromMilliseconds(duration))
            {
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.7
            };
        }

        private static void FillTypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DragAnimatedPanel)d).UpdateLayoutStrategy();
        }

        #endregion
    }
}
