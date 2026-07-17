using System;

namespace DBMS.DatabaseObjects
{
    public class Table
    {
        public string Name { get; set; }

        public void InsertRow(Row row)
        {
            throw new NotImplementedException();
        }

        public void AddColumn(Column column)
        {
            throw new NotImplementedException();
        }
    }
}
