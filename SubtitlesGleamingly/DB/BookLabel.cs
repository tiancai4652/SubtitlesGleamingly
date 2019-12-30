using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitlesGleamingly.DB
{
    public class BookLabel
    {
        [Column(IsPrimary = true)]
        public Guid ID { get; set; }

        public string FileName { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
    }

    public class Label
    {
        [Column(IsPrimary = true)]
        public Guid ID { get; set; }
        public int Location { get; set; }
        public string Note { get; set; }
        public Guid BookLabel_ID { get; set; }
    }
}
