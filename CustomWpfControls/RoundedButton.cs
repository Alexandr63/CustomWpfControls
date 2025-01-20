using System.Windows;

namespace CustomWpfControls
{
    public class RoundedButton
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(RoundedButton),
            new FrameworkPropertyMetadata(new CornerRadius(0d), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetCornerRadius(UIElement element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(UIElement element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }
    }
}
