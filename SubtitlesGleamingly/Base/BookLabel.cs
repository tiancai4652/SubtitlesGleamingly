using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitlesGleamingly.Base
{
    public class BookLabel: NotifyPropertyChangedBase
    {
        [Column(IsPrimary = true)]
        public Guid ID { get; set; }

        string _FileName = "";
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        ObservableCollection<Label> _Labels = new ObservableCollection<Label>();
        public virtual ObservableCollection<Label> Labels
        {
            get
            {
                return _Labels;
            }
            set
            {
                _Labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }
    }

    public class Label: NotifyPropertyChangedBase
    {
        [Column(IsPrimary = true)]
        public Guid ID { get; set; }
        public Guid BookLabel_ID { get; set; }

        int _Location;
        public int Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        string _Note;
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

    }
}
