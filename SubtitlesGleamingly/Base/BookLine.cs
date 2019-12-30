using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SubtitlesGleamingly.Base
{
    public class BookLine : NotifyPropertyChangedBase
    {
        string _LineValue="";
        public string LineValue
        {
            get
            {
                return _LineValue;
            }
            set
            {
                _LineValue = value;
                OnPropertyChanged(nameof(LineValue));
            }
        }

        Label _Label;
        public Label Label
        {
            get
            {
                return _Label;
            }
            set
            {
                _Label = value;
                OnPropertyChanged(nameof(Label));
                IsShowLabel= Label!=null? Visibility.Visible: Visibility.Hidden;
            }
        }

        Visibility _IsShowLabel =  Visibility.Hidden;
        public Visibility IsShowLabel
        {
            get
            {
                return _IsShowLabel;
            }
            set
            {
                _IsShowLabel = value;
                OnPropertyChanged(nameof(IsShowLabel));
            }
        }
    }
}
