using System;
using System.Collections.Generic;
using DBMS.Exceptions;

public class CatalogManager
{
    private Dictionary<string, object> _store;

    public void Register(object obj)
    {
        throw new ObjectAlreadyExistsException();
    }

    public object Find(string name)
    {
        throw new ObjectNotFoundException();
    }

    public void Remove(object obj)
    {
        throw new ObjectNotFoundException();
    }
}
