using System;

public class Partition
{
    public string Range { get; set; }

    public Partition RouteRow(Row row, string partitionKey)
    {
        throw new NotImplementedException();
    }

    public void AddRange(string range)
    {
        throw new NotImplementedException();
    }

    public bool Contains(object key)
    {
        throw new NotImplementedException();
    }
}
