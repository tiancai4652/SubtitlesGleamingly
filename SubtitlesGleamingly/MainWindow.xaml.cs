﻿using Microsoft.Win32;
using SubtitlesGleamingly.Base;
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
            ParseFileAndShow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = $@"{System.Windows.Forms.Application.StartupPath}\Subtitles\OldFriend1Season",
                Filter = "字幕|*.ass|文本|*.txt"
        };
            if (openFileDialog.ShowDialog() == true)
            {
                SubTitleFileName = openFileDialog.FileName;
                ParseFileAndShow();
            }
        }

        #region Private Method

        void ParseFileAndShow()
        {
            if (SubTitleFileName.EndsWith(".ass"))
            {
                SubTitleItems.Clear();
                foreach (var item in ParseSubTitle())
                {
                    SubTitleItems.Add(new BookLine() { LineValue = item });
                }
            }
            else if (SubTitleFileName.EndsWith(".txt"))
            {
                SubTitleItems.Clear();
                foreach (var item in ParseText())
                {
                    SubTitleItems.Add(new BookLine() { LineValue = item });
                }
            }

            if (SubTitleItems.Count > 0)
            {
                var listBookLabel = DBHelper.GetBookLabel(SubTitleFileName);
                if (listBookLabel != null && listBookLabel.Count > 0)
                {
                    var bl = listBookLabel.First();
                    foreach (var label in bl.Labels)
                    {
                        if (SubTitleItems.Count >= label.Location)
                        {
                            SubTitleItems[label.Location].Label = label;
                        }
                    }
                    SelectedSubTitleItem = SubTitleItems[bl.Labels.Last().Location];
                }
                ParseBookLabel();
            }
        }

        void ParseBookLabel()
        {
            if (SubTitleItems.Count > 0)
            {
                var listBookLabel = DBHelper.GetBookLabel(SubTitleFileName);
                if (listBookLabel != null && listBookLabel.Count > 0)
                {
                    var bl = listBookLabel.First();
                    foreach (var label in bl.Labels)
                    {
                        if (SubTitleItems.Count >= label.Location)
                        {
                            SubTitleItems[label.Location].Label = label;
                        }
                    }
                    SelectedSubTitleItem = SubTitleItems[bl.Labels.Last().Location];
                    ListBox.ScrollIntoView(SelectedSubTitleItem);
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

            var list = longStr.Split('，', '。', '？', '\r', '\n','；','：');

            foreach (string line in list)
            {
                var lineTrip = line.Trim();
                if (!string.IsNullOrEmpty(lineTrip))
                {
                    result.Add(lineTrip);
                }
            }
            return result;
        }

        #endregion

        #region Property

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

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            ShowView showView = new ShowView(SubTitleFileName, SubTitleItems, SelectedSubTitleItem);
            showView.ShowDialog();
            this.Visibility = Visibility.Visible;
            ParseBookLabel();
        }


    }
}
