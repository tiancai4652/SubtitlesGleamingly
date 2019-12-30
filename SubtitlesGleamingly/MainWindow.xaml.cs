using Microsoft.Win32;
using SubtitlesGleamingly.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SubtitlesGleamingly
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.DataContext = this;

            SubTitleFileName = $@"{System.Windows.Forms.Application.StartupPath}\Subtitles\OldFriend1Season\Friends.S01E01.eng.ass";
            SubTitleItems.Clear();
            foreach (var item in ParseSubTitle())
            {
                SubTitleItems.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.insertTest("1111",22);

            var result = DBHelper.QueryBookLable("1111");


            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = $@"{System.Windows.Forms.Application.StartupPath}\Subtitles\OldFriend1Season",
                //Filter = "字幕|*.ass"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SubTitleFileName = openFileDialog.FileName;
                if (SubTitleFileName.EndsWith(".ass"))
                {
                    SubTitleItems.Clear();
                    foreach (var item in ParseSubTitle())
                    {
                        SubTitleItems.Add(item);
                    }
                }
                else if (SubTitleFileName.EndsWith(".txt"))
                {
                    SubTitleItems.Clear();
                    foreach (var item in ParseText())
                    {
                        SubTitleItems.Add(item);
                    }
                }
            }
        }

        List<string> ParseSubTitle()
        {
            List<string> result = new List<string>();
            var parser = new SubtitlesParser.Classes.Parsers.SsaParser();
            using (var fileStream = File.OpenRead(SubTitleFileName))
            {
                var items = parser.ParseStream(fileStream, Encoding.UTF8);
                string pattern = @"{.*?}";
                var list = items.Select(t => t.Lines.Select(x => Regex.Replace(x, pattern, " ").Replace("\\N", "\n")).ToList()).ToList();
                foreach (var item in list)
                {
                    foreach (var line in item)
                    {
                        result.Add(line);
                    }
                }
            }
            return result;
        }

        List<string> ParseText()
        {
            List<string> result = new List<string>();

            var longStr = File.ReadAllText(SubTitleFileName, Encoding.UTF8);

            var list = longStr.Split('，', '。','？');

            foreach (var line in list)
            {
                result.Add(line);
            }
            return result;
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

        string _SubTitleFileName = "";
        public string SubTitleFileName
        {
            get
            {
                return _SubTitleFileName;
            }
            set
            {
                _SubTitleFileName = value;
                OnPropertyChanged(nameof(SubTitleFileName));
            }
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Visibility= Visibility.Hidden;
            ShowView showView = new ShowView(SubTitleItems,SelectedSubTitleItem);
            showView.ShowDialog();
            this.Visibility = Visibility.Visible;
        }
    }
}
