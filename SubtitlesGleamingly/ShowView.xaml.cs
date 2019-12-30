using SubtitlesGleamingly.Base;
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
        }

        public ShowView(ObservableCollection<BookLine> subTitleItems, BookLine selected) : this()
        {
            SubTitleItems = subTitleItems;
            SelectedSubTitleItem = selected==null ? subTitleItems?.Count == 0 ? null : subTitleItems[0] : selected;
            LineIndex = SubTitleItems.IndexOf(SelectedSubTitleItem);
        }

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


        double _LineOpacity = 0.5;
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

        double _LineFontSize = 18;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.MouseDown += delegate { DragMove(); };
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
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
            //double stepX = 20;
            //double stepY = 10;
            //if (e.Delta < 0)
            //{
            //    this.Width = this.Width - stepX < 100 ? 100 : this.Width - stepX;
            //    this.Height = this.Height - stepY < 100 ? 100 : this.Height - stepY;
            //}
            //else
            //{
            //    this.Width = this.Width + stepX;
            //    this.Height = this.Height + stepY;
            //}

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

        }
    }
}
