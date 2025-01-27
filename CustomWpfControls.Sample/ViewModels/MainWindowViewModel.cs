using System;
using System.Collections.Generic;
using System.Linq;
using CustomWpfControls.Sample.Helpers;
using CustomWpfControls.Sample.Models;

namespace CustomWpfControls.Sample.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedObject
    {
        #region Private Fields

        public const double RESIZE_FACTOR = 20d;

        private List<TestComboBoxItem> _testItems;

        private ExtendedObservableCollection<ImageModel> _images1;
        private ExtendedObservableCollection<ImageModel> _images2;
        private ExtendedObservableCollection<ImageModel> _images3;
        private ExtendedObservableCollection<ImageModel> _images4;
        private ExtendedObservableCollection<ImageModel> _images5;

        private IEnumerable<ListFillType> _fillTypes;
        private ListFillType _selectedFillType = ListFillType.Table;
        private bool _resizeEnable = true;

        #endregion

        #region Ctor

        public MainWindowViewModel()
        {
            TestItems = new List<TestComboBoxItem>()
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

            Images1 = new ExtendedObservableCollection<ImageModel>()
            {
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/0.jpg"), Tag = "0" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/1.jpg"), Tag = "1" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/2.jpg"), Tag = "2" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/3-4.jpg"), Tag = "3-4" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/5-6.jpg"), Tag = "5-6" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/7a.jpg"), Tag = "7a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/8a.jpg"), Tag = "8a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/9.jpg"), Tag = "9" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/10-11a.jpg"), Tag = "10-11a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/12-13a.jpg"), Tag = "12-13a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/14.jpg"), Tag = "14" }
            };

            Images2 = new ExtendedObservableCollection<ImageModel>()
            {
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/0.jpg"), Tag = "0" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/1.jpg"), Tag = "1" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/2.jpg"), Tag = "2" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/3-4.jpg"), Tag = "3-4" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/5-6.jpg"), Tag = "5-6" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/7a.jpg"), Tag = "7a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/8a.jpg"), Tag = "8a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/9.jpg"), Tag = "9" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/10-11a.jpg"), Tag = "10-11a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/12-13a.jpg"), Tag = "12-13a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/14.jpg"), Tag = "14" }
            };

            Images3 = new ExtendedObservableCollection<ImageModel>()
            {
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/0.jpg"), Tag = "0" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/1.jpg"), Tag = "1" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/2.jpg"), Tag = "2" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/3-4.jpg"), Tag = "3-4" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/5-6.jpg"), Tag = "5-6" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/7a.jpg"), Tag = "7a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/8a.jpg"), Tag = "8a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/9.jpg"), Tag = "9" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/10-11a.jpg"), Tag = "10-11a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/12-13a.jpg"), Tag = "12-13a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/14.jpg"), Tag = "14" }
            };

            Images4 = new ExtendedObservableCollection<ImageModel>()
            {
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/0.jpg"), Tag = "0" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/1.jpg"), Tag = "1" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/2.jpg"), Tag = "2" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/3-4.jpg"), Tag = "3-4" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/5-6.jpg"), Tag = "5-6" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/7a.jpg"), Tag = "7a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/8a.jpg"), Tag = "8a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/9.jpg"), Tag = "9" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/10-11a.jpg"), Tag = "10-11a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/12-13a.jpg"), Tag = "12-13a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/14.jpg"), Tag = "14" }
            };

            Images5 = new ExtendedObservableCollection<ImageModel>()
            {
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/0.jpg"), Tag = "0" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/1.jpg"), Tag = "1" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/2.jpg"), Tag = "2" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/3-4.jpg"), Tag = "3-4" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/5-6.jpg"), Tag = "5-6" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/7a.jpg"), Tag = "7a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/8a.jpg"), Tag = "8a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/9.jpg"), Tag = "9" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/10-11a.jpg"), Tag = "10-11a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/12-13a.jpg"), Tag = "12-13a" },
                new ImageModel { ImageSource = BitmapImageHelper.FileToBitmapImage("Images/14.jpg"), Tag = "14" }
            };

            foreach (ImageModel image in Images1)
            {
                image.Height /= RESIZE_FACTOR;
                image.Width /= RESIZE_FACTOR;
            }
            foreach (ImageModel image in Images2)
            {
                image.Height /= RESIZE_FACTOR;
                image.Width /= RESIZE_FACTOR;
            }
            foreach (ImageModel image in Images3)
            {
                image.Height /= RESIZE_FACTOR;
                image.Width /= RESIZE_FACTOR;
            }
            foreach (ImageModel image in Images4)
            {
                image.Height /= RESIZE_FACTOR;
                image.Width /= RESIZE_FACTOR;
            }
            foreach (ImageModel image in Images5)
            {
                image.Height /= RESIZE_FACTOR;
                image.Width /= RESIZE_FACTOR;
            }

            _fillTypes = Enum.GetValues(typeof(ListFillType)).Cast<ListFillType>();
        }

        #endregion

        #region Public Properties

        public List<TestComboBoxItem> TestItems
        {
            get => _testItems;
            set => SetField(ref _testItems, value);
        }

        public ExtendedObservableCollection<ImageModel> Images1
        {
            get => _images1;
            set => SetField(ref _images1, value);
        }

        public ExtendedObservableCollection<ImageModel> Images2
        {
            get => _images2;
            set => SetField(ref _images2, value);
        }

        public ExtendedObservableCollection<ImageModel> Images3
        {
            get => _images3;
            set => SetField(ref _images3, value);
        }

        public ExtendedObservableCollection<ImageModel> Images4
        {
            get => _images4;
            set => SetField(ref _images4, value);
        }

        public ExtendedObservableCollection<ImageModel> Images5
        {
            get => _images5;
            set => SetField(ref _images5, value);
        }

        public IEnumerable<ListFillType> FillTypes
        {
            get => _fillTypes;
            set => SetField(ref _fillTypes, value);
        }

        public ListFillType SelectedFillType
        {
            get => _selectedFillType;
            set => SetField(ref _selectedFillType, value);
        }

        public bool ResizeEnable
        {
            get => _resizeEnable;
            set => SetField(ref _resizeEnable, value);
        }

        #endregion
    }
}
