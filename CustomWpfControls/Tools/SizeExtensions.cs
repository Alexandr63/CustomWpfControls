using System.Windows;

namespace CustomWpfControls.Tools
{
    /// <summary>
    /// Расширения для класса <see cref="Size"/>
    /// </summary>
    public static class SizeHelper
    {
        public static Size ZeroSize => SizeHelper._zeroSize;

        private static readonly Size _zeroSize = SizeHelper.CreateZeroSize();

        private static Size CreateZeroSize() => new Size()
        {
            Width = 0,
            Height = 0
        };
    }
}
