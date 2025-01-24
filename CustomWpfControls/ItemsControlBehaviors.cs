using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CustomWpfControls
{
    /// <summary>
    /// Регистрация Attached Property для инициализации класса трансформации.
    /// Пример использования:
    /// 
    /// <ListBox>
    ///     <customWpfControls:ItemsControlBehaviors.ContentTransform>
    ///         <ScaleTransform ScaleX = "1" ScaleY="1" />
    ///     </customWpfControls:ItemsControlBehaviors.ContentTransform>
    /// </ListBox>
    ///
    /// 
    /// Класс делает примерно тоже самое, что и следующая разметка:
    /// 
    /// <ListBox.ItemsPanel>
    ///    <ItemsPanelTemplate>
    ///        <WrapPanel>
    ///            <WrapPanel.RenderTransform>
    ///                <TransformGroup>
    ///                    <ScaleTransform ScaleX = "1" ScaleY="1" />
    ///                </TransformGroup>
    ///            </WrapPanel.RenderTransform>
    ///        </WrapPanel>
    ///    </ItemsPanelTemplate>
    /// </ListBox.ItemsPanel>
    ///
    /// Источник: https://stackoverflow.com/questions/30888478/zoom-into-listview-contents-without-also-scaling-scroll-bars
    /// </summary>
    public static class ItemsControlBehaviors
    {
        public static readonly DependencyProperty ContentTransformProperty = DependencyProperty.RegisterAttached("ContentTransform",
            typeof(Transform),
            typeof(ItemsControlBehaviors),
            new PropertyMetadata((Transform)null, OnContentTransformChanged));

        public static Transform GetContentTransform(ItemsControl obj)
        {
            return (Transform)obj.GetValue(ContentTransformProperty);
        }

        public static void SetContentTransform(ItemsControl obj, Transform value)
        {
            obj.SetValue(ContentTransformProperty, value);
        }

        private static void OnContentTransformChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ItemsControl view)
            {
                if (view.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                {
                    EventHandler handler = null;
                    handler = (_, _) => ItemContainerGenerator_StatusChanged(view, handler);
                    view.ItemContainerGenerator.StatusChanged += handler;
                }
                else
                {
                    UpdateTransform(view);
                }
            }
        }

        private static void ItemContainerGenerator_StatusChanged(ItemsControl view, EventHandler handler)
        {
            if (view.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                view.ItemContainerGenerator.StatusChanged -= handler;
                UpdateTransform(view);
            }
        }

        private static void UpdateTransform(ItemsControl view)
        {
            if (view.IsArrangeValid)
            {
                DoUpdateTransform(view);
            }
            else
            {
                EventHandler handler = null;
                handler = (_, _) => LayoutUpdated(view, handler);
                view.LayoutUpdated += handler;
            }
        }

        private static void LayoutUpdated(ItemsControl view, EventHandler handler)
        {
            view.LayoutUpdated -= handler;
            DoUpdateTransform(view);
        }

        private static void DoUpdateTransform(ItemsControl view)
        {
            ScrollViewer scrollViewer = FindDescendant<ScrollViewer>(view);
            if (scrollViewer != null)
            {
                Transform transform = GetContentTransform(view);

                FrameworkElement header = FindDescendant<ScrollViewer>(scrollViewer);
                if (header != null)
                {
                    header.LayoutTransform = transform;
                }

                if (scrollViewer.Template.FindName("PART_ScrollContentPresenter", scrollViewer) is FrameworkElement content)
                {
                    content.LayoutTransform = transform;
                }
            }
        }

        private static T FindDescendant<T>(DependencyObject ancestor) where T : DependencyObject
        {
            return FindDescendant<T>(ancestor, item => true);
        }

        private static T FindDescendant<T>(DependencyObject ancestor, Predicate<T> predicate) where T : DependencyObject
        {
            return FindDescendant(typeof(T), ancestor, item => predicate((T)item)) as T;
        }

        private static DependencyObject FindDescendant(Type itemType, DependencyObject ancestor, Predicate<DependencyObject> predicate)
        {
            if (itemType == null) throw new ArgumentNullException("itemType");
            if (ancestor == null) throw new ArgumentNullException("ancestor");
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (!typeof(DependencyObject).IsAssignableFrom(itemType)) throw new ArgumentException("itemType", "The passed in type must be or extend DependencyObject");

            Queue<DependencyObject> queue = new Queue<DependencyObject>();
            queue.Enqueue(ancestor);

            while (queue.Count > 0)
            {
                DependencyObject currentChild = queue.Dequeue();
                if (currentChild != ancestor && itemType.IsInstanceOfType(currentChild))
                {
                    if (predicate.Invoke(currentChild))
                    {
                        return currentChild;
                    }
                }

                int count = VisualTreeHelper.GetChildrenCount(currentChild);
                for (int i = 0; i < count; ++i)
                {
                    queue.Enqueue(VisualTreeHelper.GetChild(currentChild, i));
                }
            }

            return null;
        }
    }
}
