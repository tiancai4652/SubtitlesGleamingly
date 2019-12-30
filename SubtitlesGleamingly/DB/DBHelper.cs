using SubtitlesGleamingly.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitlesGleamingly.DB
{
    public class DBHelper
    {
        public static string ConnectionString { get; set; } = @"Data Source=Database\DB.db3";

        public static IFreeSql MyContext { get; set; } = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.Sqlite, ConnectionString)
    .UseAutoSyncStructure(true) //自动同步实体结构到数据库
    .Build();

        public static void xx()
        {
            var book = new BookLabel()
            {
                ID = Guid.NewGuid(),
                FileName = "1111",
                Labels = new ObservableCollection<Label> { new Label() { ID = Guid.NewGuid(), Location = 1 } }
            };
            MyContext.GetRepository<BookLabel>().Insert(book);
        }

        public static void xx1()
        {
            var list = MyContext.GetRepository<BookLabel>().Where(t => t.FileName == "1111").IncludeMany(t => t.Labels).ToList();
        }

        public static List<BookLabel> GetBookLabel(string fileName)
        {
            var sql1 = MyContext.GetRepository<BookLabel>().Where(t => t.FileName == fileName).ToSql();
            var query1 = MyContext.GetRepository<BookLabel>().Where(t => t.FileName == fileName);
            var sql2 = query1.IncludeMany(t => t.Labels.Where(x => x.BookLabel_ID == t.ID)).ToSql();
            return MyContext.GetRepository<BookLabel>().Where(t => t.FileName == fileName).IncludeMany(t => t.Labels.Where(l => l.BookLabel_ID == t.ID)).ToList();
        }

        public static void SetBookLabel(string filename, Label label)
        {
            var list = GetBookLabel(filename);
            if (list.Count > 0)
            {
                var bl = list[0];
                bl.Labels.Add(label);
                MyContext.GetRepository<BookLabel>().Update(bl);
            }
            else
            {
                var book = new BookLabel()
                {
                    ID = Guid.NewGuid(),
                    FileName = filename,
                    Labels = new ObservableCollection<Label> { label }
                };
                MyContext.GetRepository<BookLabel>().Insert(book);
            }
        }

        //public static void insertTest(string fileName, int lineIndex)
        //{
        //    var id = Guid.NewGuid();
        //    var book = new BookLabel()
        //    {
        //        ID = id,
        //        FileName = fileName,
        //        Labels = new List<Label> { new Label() { ID = Guid.NewGuid(), Location = 1 } }
        //    };
        //    MyContext.Insert<BookLabel>(book).ExecuteAffrows();

        //    foreach (var item in book.Labels)
        //    {
        //        MyContext.Insert<Label>(item).ExecuteAffrows();
        //    }
        //}

        //public static void InsertBookLabel(string fileName, int lineIndex, string note = "")
        //{
        //    var bl = MyContext.Queryable<BookLabel>().Where(a => a.FileName.Equals(fileName)).First();
        //    if (bl == null)
        //    {
        //        var label = new Label() { BookLabelID = bl.ID, ID = Guid.NewGuid(), Location = lineIndex, Note = note };
        //        MyContext.Insert<Label>(label).ExecuteAffrows();
        //    }
        //    else
        //    {
        //        var id = Guid.NewGuid();
        //        var book = new BookLabel()
        //        {
        //            ID = id,
        //            FileName = fileName,
        //            Labels = new List<Label> { new Label() { ID = Guid.NewGuid(), BookLabelID = id, Location = lineIndex, Note = note } }
        //        };
        //        MyContext.Insert<BookLabel>(book).ExecuteAffrows();

        //        foreach (var item in book.Labels)
        //        {
        //            MyContext.Insert<Label>(item).ExecuteAffrows();
        //        }
        //    }
        //}

        //public static BookLabel QueryBookLable(string fileName)
        //{
        //    return MyContext.Queryable<BookLabel>().Where(t => t.FileName.Equals(fileName)).LeftJoin<Label>((a, b) => b.BookLabelID.Equals(a.ID)).First();
        //}

    }
}
