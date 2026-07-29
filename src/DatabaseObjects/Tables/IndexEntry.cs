using System;
using System.Collections.Generic;

public class IndexEntry
{
    public object Key { get; set; } = null!;

    public IReadOnlyList<object> RecordPointers { get; set; } = null!;
}
