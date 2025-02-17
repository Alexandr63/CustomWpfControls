using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CustomWpfControls.Converters;
using CustomWpfControls.Tools;

namespace CustomWpfControls
{
    /// <summary>
    /// Контрол выбора времени
    /// </summary>
    [TemplatePart(Name = HOURS_TEXT_BOX_PART_NAME, Type = typeof(TextBox))]
    [TemplatePart(Name = MINUTES_TEXT_BOX_PART_NAME, Type = typeof(TextBox))]
    [TemplatePart(Name = UP_BUTTON_PART_NAME, Type = typeof(Button))]
    [TemplatePart(Name = DOWN_BUTTON_PART_NAME, Type = typeof(Button))]
    public class TimePicker : Control
    {
        #region Constants

        public const string HOURS_TEXT_BOX_PART_NAME = "PART_HoursTextBox";
        public const string MINUTES_TEXT_BOX_PART_NAME = "PART_MinutesTextBox";
        public const string UP_BUTTON_PART_NAME = "PART_UpButton";
        public const string DOWN_BUTTON_PART_NAME = "PART_DownButton";

        #endregion

        #region Private Fields

        private static readonly TimeSpan _maxTimeSpan = new TimeSpan(23, 59, 00);
        private static readonly TimeSpan _minTimeSpan = TimeSpan.Zero;

        private int _previewDigit = 0;

        #endregion

        #region Ctor

        public TimePicker()
        {
            Time = _minTimeSpan;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Время, отображаемое в контроле.
        /// </summary>
        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new UIPropertyMetadata(TimePropertyChangedCallback));

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Получаем выполняем байндинг и подписываемся на событие PART элементов.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(HOURS_TEXT_BOX_PART_NAME) is TextBox hoursTextBox)
            {
                hoursTextBox.PreviewKeyUp += HoursTextBoxKeyUpEventHandler;
                hoursTextBox.LostFocus += TextBoxLostFocuseventHandler;
                hoursTextBox.SelectionChanged += TextBoxSelectionChangedEventHandler;

                Binding hoursBinding = new Binding
                {
                    Path = new PropertyPath(nameof(Time)),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = new TimespanToHoursStringConverter(),
                    RelativeSource = new RelativeSource()
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(TimePicker)
                    }
                };
                hoursTextBox.SetBinding(TextBox.TextProperty, hoursBinding);
            }

            if (GetTemplateChild(MINUTES_TEXT_BOX_PART_NAME) is TextBox minutesTextBox)
            {
                minutesTextBox.PreviewKeyUp += MinutesTextBoxKeyUpEventHandler;
                minutesTextBox.LostFocus += TextBoxLostFocuseventHandler;
                minutesTextBox.SelectionChanged += TextBoxSelectionChangedEventHandler;

                Binding minutesBinding = new Binding
                {
                    Path = new PropertyPath(nameof(Time)),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = new TimespanToMinutesStringConverter(),
                    RelativeSource = new RelativeSource()
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(TimePicker)
                    }
                };
                minutesTextBox.SetBinding(TextBox.TextProperty, minutesBinding);
            }

            if (GetTemplateChild(UP_BUTTON_PART_NAME) is Button upButton)
            {
                upButton.Click += UpButtonClickEventHandler;
            }

            if (GetTemplateChild(DOWN_BUTTON_PART_NAME) is Button downButton)
            {
                downButton.Click += DownButtonClickEventHandler;
            }
        }

        #endregion

        #region Private Methods

        private static void TimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((TimeSpan)e.NewValue < _minTimeSpan)
            {
                ((TimePicker)d).Time = _minTimeSpan;
            }
            else if ((TimeSpan)e.NewValue > _maxTimeSpan)
            {
                ((TimePicker)d).Time = _maxTimeSpan;
            }
        }

        private void UpButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            switch (GetStepValue())
            {
                case StepValue.FiveMinutes:
                    Time = Time.Add(new TimeSpan(0, 5, 0));
                    break;
                case StepValue.TenMinutes:
                    Time = Time.Add(new TimeSpan(0, 10, 0));
                    break;
                case StepValue.OneHour:
                    Time = Time.Add(new TimeSpan(1, 0, 0));
                    break;
                default:
                    Time = Time.Add(new TimeSpan(0, 1, 0));
                    break;
            }
        }

        private void DownButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            switch (GetStepValue())
            {
                case StepValue.FiveMinutes:
                    Time = Time.Add(new TimeSpan(0, -5, 0));
                    break;
                case StepValue.TenMinutes:
                    Time = Time.Add(new TimeSpan(0, -10, 0));
                    break;
                case StepValue.OneHour:
                    Time = Time.Add(new TimeSpan(-1, 0, 0));
                    break;
                default:
                    Time = Time.Add(new TimeSpan(0, -1, 0));
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

                Time = new TimeSpan(_previewDigit * 10 + keyValue, Time.Minutes, 0);

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

                Time = new TimeSpan(Time.Hours, _previewDigit * 10 + keyValue, 0);

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
