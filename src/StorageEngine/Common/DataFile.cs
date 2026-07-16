using System;
using System.Collections.Generic;

public class DataFile
{
    public FileId Id { get; set; }
    public string FilePath { get; set; }
    public long FileSize { get; set; }
    
    // Additional metadata or structures can go here
    // public FileHeader Header { get; set; }
    // public List<Extent> Extents { get; set; }
}
