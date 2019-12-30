using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        BookLabel _BookLabel;
        public BookLabel BookLabel
        {
            get
            {
                return _BookLabel;
            }
            set
            {
                _BookLabel = value;
                OnPropertyChanged(nameof(BookLabel));
            }
        }
    }
}
