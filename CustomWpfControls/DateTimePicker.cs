using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using CustomWpfControls.Converters;
using Calendar = System.Windows.Controls.Calendar;

namespace CustomWpfControls
{
    /// <summary>
    /// Контрол выбора даты и времени
    /// </summary>
    [TemplatePart(Name = DATE_TIME_TEXT_BOX_PART_NAME, Type = typeof(TextBox))]
    [TemplatePart(Name = SELECT_BUTTON_PART_NAME, Type = typeof(Button))]
    [TemplatePart(Name = SELECTOR_POPUP_PART_NAME, Type = typeof(Popup))]
    [TemplatePart(Name = CALENDAR_PART_NAME, Type = typeof(Calendar))]
    [TemplatePart(Name = TIME_PICKER_PART_NAME, Type = typeof(TimePicker))]
    [TemplatePart(Name = SAVE_BUTTON_PART_NAME, Type = typeof(Button))]
    [TemplatePart(Name = CANCEL_BUTTON_PART_NAME, Type = typeof(Button))]
    public class DateTimePicker : Control, IDataErrorInfo
    {
        #region Constants

        public const string DATE_TIME_TEXT_BOX_PART_NAME = "PART_DateTimeTextBox";
        public const string SELECT_BUTTON_PART_NAME = "PART_SelectButton";
        public const string SELECTOR_POPUP_PART_NAME = "PART_SelectorPopup";
        public const string CALENDAR_PART_NAME = "PART_Calendar";
        public const string TIME_PICKER_PART_NAME = "PART_TimePicker";
        public const string SAVE_BUTTON_PART_NAME = "PART_SaveButton";
        public const string CANCEL_BUTTON_PART_NAME = "PART_CancelButton";

        #endregion

        #region Private Fields

        private Popup _dateTimeSelector = null;
        private TextBox _dateTimeTextBox = null;

        #endregion

        #region Ctor.

        public DateTimePicker()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Дата и время, отображаемое в контроле.
        /// </summary>
        public DateTime? DateTime
        {
            get => (DateTime?)GetValue(DateTimeProperty);
            set => SetValue(DateTimeProperty, value);
        }

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(nameof(DateTime), typeof(DateTime?), typeof(DateTimePicker), new UIPropertyMetadata(null));

        /// <summary>
        /// Дата, отображаемое в панели редактирования.
        /// </summary>
        internal DateTime DateForEdit
        {
            get => (DateTime)GetValue(DateForEditProperty);
            set => SetValue(DateForEditProperty, value);
        }

        internal static readonly DependencyProperty DateForEditProperty = DependencyProperty.Register(nameof(DateForEdit), typeof(DateTime), typeof(DateTimePicker), new UIPropertyMetadata(System.DateTime.Now));

        /// <summary>
        /// Время, отображаемое в панели редактирования.
        /// </summary>
        internal TimeSpan TimeForEdit
        {
            get => (TimeSpan)GetValue(TimeForEditProperty);
            set => SetValue(TimeForEditProperty, value);
        }

        internal static readonly DependencyProperty TimeForEditProperty = DependencyProperty.Register(nameof(TimeForEdit), typeof(TimeSpan), typeof(DateTimePicker), new UIPropertyMetadata(TimeSpan.Zero));

        /// <summary>
        /// Строка форматирования для отображения даты и времени.
        /// </summary>
        public string DateTimeFormatString
        {
            get => (string)GetValue(DateTimeFormatStringProperty);
            set => SetValue(DateTimeFormatStringProperty, value);
        }

        public static readonly DependencyProperty DateTimeFormatStringProperty = DependencyProperty.Register(nameof(DateTimeFormatString), typeof(string), typeof(DateTimePicker), new UIPropertyMetadata());

        /// <summary>
        /// Известные строки форматирования даты разделенные запятой. Предназначены для парсинга введенной в контрол строки.
        /// </summary>
        public string KnownDateTimeFormatStrings
        {
            get => (string)GetValue(KnownDateTimeFormatStringsProperty);
            set => SetValue(KnownDateTimeFormatStringsProperty, value);
        }

        public static readonly DependencyProperty KnownDateTimeFormatStringsProperty = DependencyProperty.Register(nameof(KnownDateTimeFormatStrings), typeof(string), typeof(DateTimePicker), new UIPropertyMetadata(string.Empty));

        #endregion

        #region Public Methods

        /// <summary>
        /// Получаем выполняем байндинг и подписываемся на событие PART элементов.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dateTimeTextBox = GetTemplateChild(DATE_TIME_TEXT_BOX_PART_NAME) as TextBox;
            if (_dateTimeTextBox != null)
            {
                MultiBinding dateTimeBinding = new MultiBinding
                {
                    Bindings =
                    {
                        new Binding
                        {
                            Path = new PropertyPath(nameof(DateTime)),
                            RelativeSource = new RelativeSource()
                            {
                                Mode = RelativeSourceMode.FindAncestor,
                                AncestorType = typeof(DateTimePicker)
                            }
                        },
                        new Binding
                        {
                            Path = new PropertyPath(nameof(DateTimeFormatString)),
                            RelativeSource = new RelativeSource()
                            {
                                Mode = RelativeSourceMode.FindAncestor,
                                AncestorType = typeof(DateTimePicker)
                            }
                        }
                    },
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = new DateTimeToStringConverter()
                };
                _dateTimeTextBox.SetBinding(TextBox.TextProperty, dateTimeBinding);

                _dateTimeTextBox.TextChanged += DateTimeTextBoxTextChangedEventHandler;
            }

            if (GetTemplateChild(SELECT_BUTTON_PART_NAME) is Button selectButton)
            {
                selectButton.Click += SelectButtonClickEventHandler;
            }

            if (GetTemplateChild(SELECTOR_POPUP_PART_NAME) is Popup selectorPopup)
            {
                _dateTimeSelector = selectorPopup;
                _dateTimeSelector.Closed += DateTimeSelectorClosedEventHandler;
            }

            if (GetTemplateChild(CALENDAR_PART_NAME) is Calendar calendar)
            {
                calendar.GotMouseCapture += CalendarGotMouseCaptureEventHandler;

                Binding dateBinding = new Binding
                {
                    Path = new PropertyPath(nameof(DateForEdit)),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    RelativeSource = new RelativeSource()
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(DateTimePicker)
                    }
                };
                calendar.SetBinding(Calendar.SelectedDateProperty, dateBinding);

                Binding displayDateBinding = new Binding
                {
                    Path = new PropertyPath(nameof(DateForEdit)),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    RelativeSource = new RelativeSource()
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(DateTimePicker)
                    }
                };
                calendar.SetBinding(Calendar.DisplayDateProperty, displayDateBinding);
            }

            if (GetTemplateChild(TIME_PICKER_PART_NAME) is TimePicker timePicker)
            {
                Binding timeBinding = new Binding
                {
                    Path = new PropertyPath(nameof(TimeForEdit)),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    RelativeSource = new RelativeSource()
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(DateTimePicker)
                    }
                };
                timePicker.SetBinding(TimePicker.TimeProperty, timeBinding);
            }

            if (GetTemplateChild(CANCEL_BUTTON_PART_NAME) is Button cancelButton)
            {
                cancelButton.Click += CancelButtonClickEventHandler;
            }

            if (GetTemplateChild(SAVE_BUTTON_PART_NAME) is Button saveButton)
            {
                saveButton.Click += SaveButtonClickEventHandler;
            }
        }

        /// <summary>
        /// Установить фокус на поле ввода.
        /// </summary>
        public void SetFocus()
        {
            _dateTimeTextBox?.Focus();
            _dateTimeTextBox?.SelectAll();
        }

        #endregion

        #region IDataErrorInfo Implementation

        public string Error => throw new NotImplementedException();

        // use a specific validation or ask for Validation Error 
        public string this[string columnName] => Validation.GetHasError(this) ? "DateTimePicker has Error" : null;

        #endregion

        #region Private Methods

        private void SelectButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (!_dateTimeSelector.IsOpen)
            {
                if (DateTime.HasValue)
                {
                    DateForEdit = DateTime.Value;
                    TimeForEdit = DateTime.Value.TimeOfDay;
                }
                else
                {
                    DateForEdit = System.DateTime.Now;
                    TimeForEdit = System.DateTime.Now.TimeOfDay;
                }

                _dateTimeSelector.IsOpen = true;
            }
        }

        private void CalendarGotMouseCaptureEventHandler(object sender, MouseEventArgs e)
        {
            // чтобы не пришлось дважды кликать мышью после использования календаря 
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }

        private void CancelButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            _dateTimeSelector.IsOpen = false;
        }

        private void SaveButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            _dateTimeSelector.IsOpen = false;
            DateTime = new DateTime(DateForEdit.Year, DateForEdit.Month, DateForEdit.Day, TimeForEdit.Hours, TimeForEdit.Minutes, 0);
        }

        private void DateTimeTextBoxTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                DateTime = null;
                return;
            }

            text = PreprocessDateTimeString(text);

            List<string> formatStrings = new List<string>() { DateTimeFormatString };
            if (!string.IsNullOrEmpty(KnownDateTimeFormatStrings))
            {
                formatStrings.AddRange(KnownDateTimeFormatStrings.Split(','));
            }

            if (DateTimeToStringConverter.ConvertToString(DateTime, DateTimeFormatString) != text)
            {
                if (System.DateTime.TryParseExact(text, formatStrings.ToArray(), CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dt))
                {
                    DateTime = dt;
                }
                else
                {
                    DateTime = null;
                }
            }
        }
        private void DateTimeSelectorClosedEventHandler(object sender, EventArgs e)
        {
            _dateTimeTextBox?.Focus();
        }

        /// <summary>
        /// Если считать дату из баркода в русской раскладке она будет записана в формате 'yyyy-MM-ddЕHHЖmmЖssЯ'. Заменяем русские символы.
        /// </summary>
        private string PreprocessDateTimeString(string dateTime)
        {
            return dateTime.Replace('Е', 'T').Replace('Ж', ':').Replace('Я', 'Z');
        }

        #endregion
    }
}
