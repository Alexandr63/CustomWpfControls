using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FilteredComboBoxControl
{
    public class FilteredComboBox : ComboBox
    {
        #region Private Fields

        private TextBox _filterTextBox;
        private ContentPresenter _contentSite;

        #endregion

        #region Ctor

        public FilteredComboBox() : base()
        {
            Initialized += InitializedEventHandler;
        }

        #endregion

        #region Private Methods

        private void InitializedEventHandler(object sender, EventArgs e)
        {
            // Отключаем режим редактирования, если его по ошибке включат
            IsEditable = false;

            DropDownOpened += DropDownOpenedEventHandler;
            DropDownClosed += DropDownClosedEventHandler;

            _filterTextBox = (TextBox)GetDescendantByName(this, "PART_EditableTextBox");
            _contentSite = (ContentPresenter)GetDescendantByName(this, "ContentSite");

            _filterTextBox.TextChanged += FilterTextBoxKeyUpEventHandler;
        }

        private void FilterTextBoxKeyUpEventHandler(object sender, TextChangedEventArgs e)
        {
            string searchString = ((TextBox)e.Source).Text.Trim();

            ApplyFilter(searchString);
        }

        private void DropDownClosedEventHandler(object sender, EventArgs e)
        {
            _filterTextBox.Visibility = Visibility.Hidden;
            _contentSite.Visibility = Visibility.Visible;
        }

        private void DropDownOpenedEventHandler(object sender, EventArgs e)
        {
            _filterTextBox.Visibility = Visibility.Visible;
            _contentSite.Visibility = Visibility.Hidden;

            if (SelectedValue != null)
            {
                _filterTextBox.Text = this.Text;
            }
            else
            {
                _filterTextBox.Text = string.Empty;
            }

            ApplyFilter();

            _filterTextBox.Focus();
            _filterTextBox.SelectAll();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // Если в фильтре был введен текст и нажат enter - выбираем первое подходящее значение
            if (e.Key == Key.Enter && _filterTextBox.Text != string.Empty)
            {
                string searchString = _filterTextBox.Text.Trim().ToUpper();
                foreach (object item in Items)
                {
                    ComboBoxItem comboBoxItem = GetComboBoxItem(item);
                    if (comboBoxItem == null)
                    {
                        continue;
                    }

                    string displayValue = GetDisplayValue(comboBoxItem);
                    if (displayValue.ToUpper().Contains(searchString))
                    {
                        comboBoxItem.Visibility = Visibility.Visible;
                        SelectedItem = item;
                        return;
                    }
                }

                _filterTextBox.Text = string.Empty;
            }
            else if(IsDropDownOpen && !_filterTextBox.IsFocused)
            {
                _filterTextBox.Focus();
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Применяет значение фильтра.
        /// </summary>
        private void ApplyFilter(string searchString = "")
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                foreach (object item in Items)
                {
                    ComboBoxItem comboBoxItem = GetComboBoxItem(item);
                    if (comboBoxItem == null)
                    {
                        continue;
                    }

                    comboBoxItem.Visibility = Visibility.Visible;
                }
            }
            else
            {
                searchString = searchString.ToUpper();
                foreach (object item in Items)
                {
                    ComboBoxItem comboBoxItem = GetComboBoxItem(item);
                    if (comboBoxItem == null)
                    {
                        continue;
                    }

                    string displayValue = GetDisplayValue(comboBoxItem);
                    if (displayValue.ToUpper().Contains(searchString))
                    {
                        comboBoxItem.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        comboBoxItem.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает значение отображаемого поля.
        /// </summary>
        private string GetDisplayValue(ComboBoxItem comboBoxItem)
        {
            string displayValue;
            if (comboBoxItem.Content is string strContent)
            {
                displayValue = strContent;
            }
            else
            {
                Type t = comboBoxItem.Content.GetType();
                PropertyInfo pi = t.GetProperty(DisplayMemberPath);
                displayValue = pi?.GetValue(comboBoxItem.Content)?.ToString();
            }

            return displayValue ?? string.Empty;
        }

        /// <summary>
        /// Возвращает визуальный контейнер для заданного объекта.
        /// </summary>
        private ComboBoxItem GetComboBoxItem(object item)
        {
            ComboBoxItem comboBoxItem;
            if (item is ComboBoxItem cbi)
            {
                comboBoxItem = cbi;
            }
            else
            {
                comboBoxItem = (ComboBoxItem)ItemContainerGenerator.ContainerFromItem(item);
            }

            return comboBoxItem;
        }

        /// <summary>
        /// Возвращает первый найденный дочерний элемент заданного типа.
        /// </summary>
        private Visual GetDescendantByName(Visual element, string name)
        {
            if (element == null)
            {
                return null;
            }

            if ((string)element.GetValue(FrameworkElement.NameProperty) == name)
            {
                return element;
            }

            Visual foundElement = null;
            if (element is FrameworkElement frameworkElement)
            {
                frameworkElement.ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByName(visual, name);
                if (foundElement != null)
                {
                    break;
                }
            }

            return foundElement;
        }

        #endregion
    }
}
