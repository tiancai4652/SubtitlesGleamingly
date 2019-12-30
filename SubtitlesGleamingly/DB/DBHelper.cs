﻿using System;
using System.Collections.Generic;
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

       

        public static void insertTest(string fileName, int lineIndex)
        {
            var id = Guid.NewGuid();
            var book = new BookLabel()
            {
                ID = id,
                FileName = fileName,
                Labels = new List<Label> { new Label() { ID = Guid.NewGuid(), BookLabelID = id, Location = 1 } }
            };
            MyContext.Insert<BookLabel>(book).ExecuteAffrows();

            foreach (var item in book.Labels)
            {
                MyContext.Insert<Label>(item).ExecuteAffrows();
            }
        }

        public static void InsertBookLabel(string fileName, int lineIndex, string note = "")
        {
            var bl = MyContext.Queryable<BookLabel>().Where(a => a.FileName.Equals(fileName)).First();
            if (bl == null)
            {
                var label = new Label() { BookLabelID = bl.ID, ID = Guid.NewGuid(), Location = lineIndex, Note = note };
                MyContext.Insert<Label>(label).ExecuteAffrows();
            }
            else
            {
                var id = Guid.NewGuid();
                var book = new BookLabel()
                {
                    ID = id,
                    FileName = fileName,
                    Labels = new List<Label> { new Label() { ID = Guid.NewGuid(), BookLabelID = id, Location = lineIndex, Note = note } }
                };
                MyContext.Insert<BookLabel>(book).ExecuteAffrows();

                foreach (var item in book.Labels)
                {
                    MyContext.Insert<Label>(item).ExecuteAffrows();
                }
            }
        }

        public static BookLabel QueryBookLable(string fileName)
        {
            return MyContext.Queryable<BookLabel>().Where(t => t.FileName.Equals(fileName)).LeftJoin<Label>((a, b) => b.BookLabelID.Equals(a.ID)).First();
        }

    }
}
