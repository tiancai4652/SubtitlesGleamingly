using SubtitlesGleamingly.Base;
using SubtitlesGleamingly.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Label = SubtitlesGleamingly.Base.Label;
using NHotkey.Wpf;
using NHotkey;
using System.Threading;

namespace SubtitlesGleamingly
{
    /// <summary>
    /// ShowView.xaml 的交互逻辑
    /// </summary>
    public partial class ShowView : Window, INotifyPropertyChanged
    {
        public ShowView()
        {
            InitializeComponent();
            this.DataContext = this;

            HotkeyManager.Current.AddOrReplace("ShutDown", Key.Escape, ModifierKeys.None, ShutDown);
            HotkeyManager.Current.AddOrReplace("CopyCurrentLine", Key.C, ModifierKeys.Control, CopyCurrentLine);
            HotkeyManager.Current.AddOrReplace("AutoPager", Key.Enter, ModifierKeys.Control, AutoPager);
            HotkeyManager.Current.AddOrReplace("IncreaseAutoPagerInternal", Key.Add, ModifierKeys.Control, IncreaseAutoPagerInternal);
            HotkeyManager.Current.AddOrReplace("DecreaseAutoPagerInternal", Key.Subtract, ModifierKeys.Control, DecreaseAutoPagerInternal);

            Task.Factory.StartNew(new Action(() =>
            {
                while (IsAppRunning)
                {
                    if (IsEnabledAutoPager)
                    {
                        Thread.Sleep(AutoPagerIntervalS * 1000);
                        if (LineIndex + 1 <= SubTitleItems.Count)
                        {
                            LineIndex++;
                            SelectedSubTitleItem = SubTitleItems[LineIndex];
                        }
                    }
                    Thread.Sleep(100);
                }
            }));
        }

        private void ShutDown(object sender, HotkeyEventArgs e)
        {
            IsAppRunning = false;
            SaveLableForCurrentLocation();
            Environment.Exit(-1);
        }

        private void IncreaseAutoPagerInternal(object sender, HotkeyEventArgs e)
        {
            AutoPagerIntervalS++;
        }

        private void DecreaseAutoPagerInternal(object sender, HotkeyEventArgs e)
        {
            if (AutoPagerIntervalS - 1 >= 1)
            {
                AutoPagerIntervalS--;
            }
        }



        bool IsAppRunning = true;
        bool IsEnabledAutoPager = false;
        static object locker = new object();
        int _AutoPagerIntervalS = 10;
        int AutoPagerIntervalS
        {
            get
            {
                lock (locker)
                {
                    return _AutoPagerIntervalS;
                }
            }
            set
            {
                lock (locker)
                {
                    _AutoPagerIntervalS = value;
                }
            }
        }
        private void AutoPager(object sender, HotkeyEventArgs e)
        {
            IsEnabledAutoPager = !IsEnabledAutoPager;
        }

        private void CopyCurrentLine(object sender, HotkeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedSubTitleItem?.LineValue))
            {
                Clipboard.SetDataObject(SelectedSubTitleItem.LineValue);
                if (LineIndex + 1 <= SubTitleItems.Count)
                {
                    LineIndex++;
                    SelectedSubTitleItem = SubTitleItems[LineIndex];
                }
            }
        }

        public ShowView(string fileName, ObservableCollection<BookLine> subTitleItems, BookLine selected) : this()
        {
            FileName = fileName;
            SubTitleItems = subTitleItems;
            SelectedSubTitleItem = selected == null ? subTitleItems?.Count == 0 ? null : subTitleItems[0] : selected;
            LineIndex = SubTitleItems.IndexOf(SelectedSubTitleItem);
        }

        public string FileName { get; set; }

        ObservableCollection<BookLine> _SubTitleItems = new ObservableCollection<BookLine>();
        public ObservableCollection<BookLine> SubTitleItems
        {
            get
            {
                return _SubTitleItems;
            }
            set
            {
                _SubTitleItems = value;
                OnPropertyChanged(nameof(SubTitleItems));
            }
        }


        double _LineOpacity = 0.8;
        public double LineOpacity
        {
            get
            {
                return _LineOpacity;
            }
            set
            {
                _LineOpacity = value;
                OnPropertyChanged(nameof(LineOpacity));
            }
        }

        double _LineFontSize = 20;
        public double LineFontSize
        {
            get
            {
                return _LineFontSize;
            }
            set
            {
                _LineFontSize = value;
                OnPropertyChanged(nameof(LineFontSize));
            }
        }

        int _LineIndex = 0;
        public int LineIndex
        {
            get
            {
                return _LineIndex;
            }
            set
            {
                _LineIndex = value;
                OnPropertyChanged(nameof(LineIndex));
            }
        }

        BookLine _SelectedSubTitleItem;
        public BookLine SelectedSubTitleItem
        {
            get
            {
                return _SelectedSubTitleItem;
            }
            set
            {
                _SelectedSubTitleItem = value;
                OnPropertyChanged(nameof(SelectedSubTitleItem));
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion



        private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LineIndex + 1 <= SubTitleItems.Count)
            {
                LineIndex++;
                SelectedSubTitleItem = SubTitleItems[LineIndex];
            }
        }

        private void Window_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LineIndex - 1 >= 0)
            {
                LineIndex--;
                SelectedSubTitleItem = SubTitleItems[LineIndex];
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double step = 0.1;
            if (e.Delta < 0)
            {
                LineOpacity = LineOpacity - step < 0 ? 0 : LineOpacity - step;
            }
            else
            {
                LineOpacity = LineOpacity + step > 1 ? 1 : LineOpacity + step;
            }
        }

        private void Grid_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            double step = 1;
            if (e.Delta < 0)
            {
                LineFontSize = LineFontSize - step < 10 ? 10 : LineFontSize - step;
            }
            else
            {
                LineFontSize += step;
            }
        }

        private void TextBlock_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (LineIndex + 1 <= SubTitleItems.Count)
                {
                    LineIndex++;
                    SelectedSubTitleItem = SubTitleItems[LineIndex];
                }
            }
            else
            {
                if (LineIndex - 1 >= 0)
                {
                    LineIndex--;
                    SelectedSubTitleItem = SubTitleItems[LineIndex];
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveLableForCurrentLocation();
        }

        void SaveLableForCurrentLocation()
        {
            var Label = new Label() { ID = Guid.NewGuid(), Location = LineIndex };
            DBHelper.SetBookLabel(FileName, Label);
        }
    }
}
