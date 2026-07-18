using System;
using System.Collections.Generic;

public class IndexEntry
{
    public object Key { get; set; }
    public IReadOnlyList<object> RecordPointers { get; set; }
}
