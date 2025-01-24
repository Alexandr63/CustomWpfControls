using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace CustomWpfControls.Sample.Models
{
    public class StringModel : NotifyPropertyChangedObject
    {
        private string _str = string.Empty;

        public StringModel()
        {
        }

        public StringModel(string s)
        {
            Str = s;
        }

        public string Str
        {
            get => _str;
            set => SetField(ref _str, value);
        }

        public override string ToString()
        {
            return Str;
        }
    }
}
