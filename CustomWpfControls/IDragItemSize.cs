using System.Windows;

namespace CustomWpfControls
{
    public interface IDragItemSize 
    {
        double Width { get; set; }

        double Height { get; set; }

        Size GetSize();
    }
}
