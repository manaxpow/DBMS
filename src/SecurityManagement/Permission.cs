using System;

namespace DBMS.SecurityManagement
{
    public class Permission
    {
        public string Action { get; set; }

        public bool Allows(string action, object resource)
        {
            throw new NotImplementedException();
        }
    }
}
