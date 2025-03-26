using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CustomWpfControls.LayoutStrategies;

namespace CustomWpfControls
{
    /// <summary>
    /// Панель с поддержкой переноса элементов.
    /// </summary>
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
        /// Включение / отключение возможности перемещения объектов мышью.
        /// </summary>
        public bool IsDragAndDropEnable
        {
            get => (bool)GetValue(IsDragAndDropEnableProperty);
            set => SetValue(IsDragAndDropEnableProperty, value);
        }

        public static readonly DependencyProperty IsDragAndDropEnableProperty = DependencyProperty.Register(nameof(IsDragAndDropEnable),
            typeof(bool),
            typeof(DragAnimatedPanel),
            new FrameworkPropertyMetadata(true)
        );

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

        #endregion

        #region Override Methods

        /// <summary>
        /// Определяем, сколько пространства желает занять каждый дочерний элемент.
        /// </summary>
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

        /// <summary>
        /// Размещаем дочерние элементы в пределах доступного пространства.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
            {
                return _calculatedSize;
            }

            double horizontalPosition = 0;
            double verticalPosition = 0;
            int rowIndex = 0;
            ItemLayoutInfo currentLayoutInfo = _layoutStrategy.GetLayoutInfo(0);
            
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement child = Children[i];

                // Размещаем элемент на панели в точке (0;0)
                child.Arrange(new Rect(child.DesiredSize));
                
                // Инициализируем и применяем трансформацию для переноса его в нужное нам место.
                // Трансформация с информацией о расположении объекта понадобится позже для работы Drag`n`Drop.
                if (child != DraggedElement)
                {
                    MoveTo(child, horizontalPosition, verticalPosition);
                }

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

                    currentLayoutInfo = nextLayoutInfo;
                }
            }

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
        
        private void MoveTo(UIElement element, double x, double y)
        {
            TranslateTransform trans = InitTransform(element);
            trans.X = x;
            trans.Y = y;
        }

        private TranslateTransform InitTransform(UIElement element)
        {
            TranslateTransform trans;
            if (!(element.RenderTransform is TransformGroup))
            {
                TransformGroup group = new TransformGroup();
                element.RenderTransform = group;
                trans = new TranslateTransform();
                group.Children.Add(trans);
            }
            else
            {
                TransformGroup group = (TransformGroup)element.RenderTransform;
                trans = (TranslateTransform)group.Children[0];
            }

            return trans;
        }
        
        private static void FillTypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DragAnimatedPanel)d).UpdateLayoutStrategy();
        }

        #endregion
    }
}
