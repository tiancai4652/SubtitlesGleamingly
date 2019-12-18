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

        public ShowView(ObservableCollection<string> subTitleItems, string selected) : this()
        {
            SubTitleItems = subTitleItems;
            SelectedSubTitleItem = selected;
            LineIndex = SubTitleItems.IndexOf(SelectedSubTitleItem);
        }

        ObservableCollection<string> _SubTitleItems = new ObservableCollection<string>();
        public ObservableCollection<string> SubTitleItems
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

        string _SelectedSubTitleItem = "";
        public string SelectedSubTitleItem
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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
    }
}
