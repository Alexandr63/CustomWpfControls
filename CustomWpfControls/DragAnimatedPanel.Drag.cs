using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CustomWpfControls.Tools;

namespace CustomWpfControls
{
    public partial class DragAnimatedPanel
    {
        #region Private Fields

        private DateTime _mouseDownTime;
        private int _draggedIndex;
        private bool _firstScrollRequest = true;
        private ScrollViewer _scrollContainer;
        private double _lastMousePosX;
        private double _lastMousePosY;
        private UIElement _mouseSelectedElement = null;
        private int _lastMouseMoveTime;
        private double _x;
        private double _y;

        #endregion

        #region Properties

        public UIElement DraggedElement { get; set; }

        #endregion

        #region Private Methods

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DraggedElement == null && !IsMouseCaptured)
            {
                // Защита от случайного перехода в режим перетаскивания при быстром 'прокликивании' элементов.
                // Смотрит, как долго была нажата кнопка мыши и насколько она переместилась.
                if (CheckClick())
                {
                    StartDrag(e);
                }
            }
            else if (DraggedElement != null)
            {
                OnDragOver(e);
            }
        }

        /// <summary>
        /// Возвращает true, если мышь была нажата достаточно долго и была сдвинута на достаточное расстояние по оси X или по оси Y.
        /// </summary>
        private bool CheckClick()
        {
            const double MIN_CLICK_TIME_MILLISECONDS = 40d;
            const int MIN_SHIFT_VALUE = 10;

            TimeSpan clickDuration = DateTime.Now - _mouseDownTime;
            Point mousePos = Mouse.GetPosition(this);
            double difX = Math.Abs(mousePos.X - _lastMousePosX);
            double difY = Math.Abs(mousePos.Y - _lastMousePosY);

            return clickDuration.Milliseconds > MIN_CLICK_TIME_MILLISECONDS && (difX > MIN_SHIFT_VALUE || difY > MIN_SHIFT_VALUE);
        }
        
        private void StartDrag(MouseEventArgs e)
        {
            DraggedElement = _mouseSelectedElement;
            _mouseSelectedElement = null;
            if (DraggedElement == null)
            {
                return;
            }

            _draggedIndex = Children.IndexOf(DraggedElement);
            Point p = GetItemVisualPoint(DraggedElement);
            _x = p.X;
            _y = p.Y;

            Panel.SetZIndex(DraggedElement, 1000);

            _lastMouseMoveTime = e.Timestamp;

            InvalidateArrange();

            e.Handled = true;

            CaptureMouse();
        }

        private void OnDragOver(MouseEventArgs e)
        {
            const int MOUSE_TIME_DIF = 25;
            const double MOUSE_DIF = 10d;

            Point mousePos = Mouse.GetPosition(this);
            double difX = mousePos.X - _lastMousePosX;
            double difY = mousePos.Y - _lastMousePosY;
            int timeDif = e.Timestamp - _lastMouseMoveTime;
            if ((Math.Abs(difX) > MOUSE_DIF || Math.Abs(difY) > MOUSE_DIF) && timeDif > MOUSE_TIME_DIF)
            {
                DoScroll();
                
                int index = _layoutStrategy.GetIndex(mousePos);
                _x += difX;
                _y += difY;

                _lastMousePosX = mousePos.X;
                _lastMousePosY = mousePos.Y;
                _lastMouseMoveTime = e.Timestamp;
                SwapElement(index);
                AnimateTo(DraggedElement, _x, _y, 0);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Point mousePos = Mouse.GetPosition(this);
            _lastMousePosX = mousePos.X;
            _lastMousePosY = mousePos.Y;
            _mouseDownTime = DateTime.Now;
            _mouseSelectedElement = GetChildThatHasMouseOver();
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }
        }

        private void SwapElement(int targetIndex)
        {
            if (targetIndex == _draggedIndex || targetIndex < 0)
            {
                return;
            }
            
            if (targetIndex >= Children.Count)
            {
                targetIndex = Children.Count - 1;
            }

            ItemsControl parentItemsControl = ControlsHelper.GetParent(this, (x) => x is ItemsControl) as ItemsControl;

            // NOTE: логика заточена под IList, если в ItemsSource будет что-то другое - надо будет переписать метод
            IList list = (IList)parentItemsControl.ItemsSource; 

            if (_draggedIndex < 0 || targetIndex < 0 || _draggedIndex >= list.Count || targetIndex >= list.Count)
            {
                return;
            }

            object dragged = list[_draggedIndex];
            list.Remove(dragged);
            list.Insert(targetIndex, dragged);

            // Получаем новый элемент UI после изменения коллекции
            DraggedElement = Children[targetIndex]; 
            FillNewDraggedChild(DraggedElement);
            _draggedIndex = targetIndex;

            InvalidateArrange();
        }

        private void FillNewDraggedChild(UIElement child)
        {
            if (!(child.RenderTransform is TransformGroup))
            {
                child.RenderTransformOrigin = new Point(0.5, 0.5);
                TransformGroup group = new TransformGroup();
                child.RenderTransform = group;
                group.Children.Add(new TranslateTransform());
            }
            
            AnimateTo(child, _x, _y, 0);
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            FinishDrag();
        }

        private void FinishDrag()
        {
            if (DraggedElement != null)
            {
                DraggedElement = null;

                InvalidateMeasure();
            }
        }

        private void DoScroll()
        {
            ScrollViewer scrollViewer = GetScrollViewer();

            if (scrollViewer != null)
            {
                Point position = Mouse.GetPosition(scrollViewer);
                double scrollMargin = Math.Min(scrollViewer.FontSize * 2, scrollViewer.ActualHeight / 2);

                if (position.X >= scrollViewer.ActualWidth - scrollMargin && scrollViewer.HorizontalOffset < scrollViewer.ExtentWidth - scrollViewer.ViewportWidth)
                {
                    scrollViewer.LineRight();
                }
                else if (position.X < scrollMargin && scrollViewer.HorizontalOffset > 0)
                {
                    scrollViewer.LineLeft();
                }
                else if (position.Y >= scrollViewer.ActualHeight - scrollMargin && scrollViewer.VerticalOffset < scrollViewer.ExtentHeight - scrollViewer.ViewportHeight)
                {
                    scrollViewer.LineDown();
                }
                else if (position.Y < scrollMargin && scrollViewer.VerticalOffset > 0)
                {
                    scrollViewer.LineUp();
                }
            }
        }

        private ScrollViewer GetScrollViewer()
        {
            if (_firstScrollRequest && _scrollContainer == null)
            {
                _firstScrollRequest = false;
                _scrollContainer = (ScrollViewer)ControlsHelper.GetParent(this, (ve) => ve is ScrollViewer);
            }

            return _scrollContainer;
        }

        private UIElement GetChildThatHasMouseOver()
        {
            return ControlsHelper.GetParent(Mouse.DirectlyOver as DependencyObject, (x) => Children.Contains(x as UIElement)) as UIElement;
        }

        private Point GetItemVisualPoint(UIElement element)
        {
            TransformGroup group = (TransformGroup)element.RenderTransform;
            TranslateTransform trans = (TranslateTransform)group.Children[0];

            return new Point(trans.X, trans.Y);
        }

        #endregion
    }
}
