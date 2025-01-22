using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomWpfControls.Tools
{
    public static class ControlsHelper
    {
        public static DependencyObject GetParent(DependencyObject obj, Func<DependencyObject, bool> matchFunction)
        {
            DependencyObject parent = obj;

            do
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            while (parent != null && !matchFunction.Invoke(parent));

            return parent;
        }

        public static CustomWpfControls.DragAnimatedPanel GetDragAnimatedPanel(DependencyObject itemsControl)
        {
            ItemsPresenter itemsPresenter = GetVisualChild<ItemsPresenter>(itemsControl);
            CustomWpfControls.DragAnimatedPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as CustomWpfControls.DragAnimatedPanel;

            return itemsPanel;
        }

        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
