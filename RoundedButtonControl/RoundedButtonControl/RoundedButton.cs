using System.Windows;
using System.Windows.Controls;

namespace RoundedButtonControl
{
    public class RoundedButton : Button
    {
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundedButton),
            new FrameworkPropertyMetadata(new CornerRadius(0d), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
