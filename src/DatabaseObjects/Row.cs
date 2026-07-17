using System;

namespace DBMS.DatabaseObjects
{
    public class Row
    {
        public object[] Values { get; set; }

        public object GetValue(string columnName)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string columnName, object value)
        {
            throw new NotImplementedException();
        }
    }
}
