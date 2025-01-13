using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DateTimePickerControl.Tools;

namespace DateTimePickerControl
{
    /// <summary>
    /// Логика взаимодействия для TimePicker.xaml
    /// </summary>
    public partial class TimePicker
    {
        #region Private Fields

        private static readonly TimeSpan _maxTimeSpan = new TimeSpan(23, 59, 00);
        private static readonly TimeSpan _minTimeSpan = TimeSpan.Zero;

        private int _previewDigit = 0;

        #endregion

        #region Ctor

        public TimePicker()
        {
            InitializeComponent();

            SelectedTime = _minTimeSpan;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Время, отображаемое в контроле.
        /// </summary>
        public TimeSpan SelectedTime
        {
            get => (TimeSpan)GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(nameof(SelectedTime), typeof(TimeSpan), typeof(TimePicker), new UIPropertyMetadata(TimePropertyChangedCallback));

        #endregion

        #region Private Methods

        private static void TimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((TimeSpan)e.NewValue < _minTimeSpan)
            {
                ((TimePicker)d).SelectedTime = _minTimeSpan;
            }
            else if ((TimeSpan)e.NewValue > _maxTimeSpan)
            {
                ((TimePicker)d).SelectedTime = _maxTimeSpan;
            }
        }

        private void UpButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            switch (GetStepValue())
            {
                case StepValue.FiveMinutes:
                    SelectedTime = SelectedTime.Add(new TimeSpan(0, 5, 0));
                    break;
                case StepValue.TenMinutes:
                    SelectedTime = SelectedTime.Add(new TimeSpan(0, 10, 0));
                    break;
                case StepValue.OneHour:
                    SelectedTime = SelectedTime.Add(new TimeSpan(1, 0, 0));
                    break;
                default:
                    SelectedTime = SelectedTime.Add(new TimeSpan(0, 1, 0));
                    break;
            }
        }

        private void DownButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            switch (GetStepValue())
            {
                case StepValue.FiveMinutes:
                    SelectedTime = SelectedTime.Add(new TimeSpan(0, -5, 0));
                    break;
                case StepValue.TenMinutes:
                    SelectedTime = SelectedTime.Add(new TimeSpan(0, -10, 0));
                    break;
                case StepValue.OneHour:
                    SelectedTime = SelectedTime.Add(new TimeSpan(-1, 0, 0));
                    break;
                default:
                    SelectedTime = SelectedTime.Add(new TimeSpan(0, -1, 0));
                    break;
            }
        }

        private void HoursTextBoxKeyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key.IsDigit(out int keyValue))
            {
                if (_previewDigit > 2)
                {
                    _previewDigit = 0;
                }

                SelectedTime = new TimeSpan(_previewDigit * 10 + keyValue, SelectedTime.Minutes, 0);

                _previewDigit = keyValue;
            }
        }
        
        private void MinutesTextBoxKeyUpEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key.IsDigit(out int keyValue))
            {
                if (_previewDigit > 5)
                {
                    _previewDigit = 0;
                }

                SelectedTime = new TimeSpan(SelectedTime.Hours, _previewDigit * 10 + keyValue, 0);

                _previewDigit = keyValue;
            }
        }

        private void TextBoxLostFocuseventHandler(object sender, RoutedEventArgs e)
        {
            _previewDigit = 0;
        }

        private void TextBoxSelectionChangedEventHandler(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.SelectionLength != 0)
                {
                    textBox.SelectionLength = 0;
                }
            }
        }

        private StepValue GetStepValue()
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                return StepValue.FiveMinutes;
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                return StepValue.TenMinutes;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                return StepValue.OneHour;
            }

            return StepValue.OneMinute;
        }
        
        #endregion

        #region Nested Types

        private enum StepValue
        {
            OneMinute,
            FiveMinutes,
            TenMinutes,
            OneHour
        }

        #endregion
    }
}
