namespace CustomWpfControls.Sample.Models
{
    public class TestComboBoxItem
    {
        public TestComboBoxItem()
        {
        }

        public TestComboBoxItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
