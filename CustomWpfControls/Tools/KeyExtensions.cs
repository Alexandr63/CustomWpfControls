using System.Windows.Input;

namespace CustomWpfControls.Tools
{
    /// <summary>
    /// Расширения для класса <see cref="Key"/>
    /// </summary>
    public static class KeyExtensions
    {
        /// <summary>
        /// Если нажата числовая клавиша - возвращает true и ее значение в поле <see cref="value"/>. В противном случаи возвращает false и -1 в поле <see cref="value"/>.
        /// </summary>
        public static bool IsDigit(this Key key, out int value)
        {
            bool isNumber = false;
            value = -1;
            
            if (key is >= Key.D0 and <= Key.D9)
            {
                isNumber = true;
                value = (int)key - (int)Key.D0;
            }
            else if (key is >= Key.NumPad0 and <= Key.NumPad9)
            {
                isNumber = true;
                value = (int)key - (int)Key.NumPad0;
            }

            return isNumber;
        }
    }
}
