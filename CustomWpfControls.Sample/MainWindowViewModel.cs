using System.Collections.Generic;

namespace CustomWpfControls.Sample
{
    public class MainWindowViewModel
    {
        public List<TestComboBoxItem> TestItems { get; } = new List<TestComboBoxItem>()
        {
            new TestComboBoxItem(0, "11111 item"),
            new TestComboBoxItem(1, "22222 item"),
            new TestComboBoxItem(2, "33333 item"),
            new TestComboBoxItem(3, "112233 item"),
            new TestComboBoxItem(4, "223344 item"),
            new TestComboBoxItem(5, "1234567 item"),
            new TestComboBoxItem(6, "890 item"),
            new TestComboBoxItem(7, "990088 item"),
            new TestComboBoxItem(8, "4567 item"),
            new TestComboBoxItem(9, "345678 item")
        };
}
}
