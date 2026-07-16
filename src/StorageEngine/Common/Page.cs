using System;

public class Page
{
    public PageId Id { get; set; }
    public byte[] Data { get; set; }
    public bool IsDirty { get; set; }
    public int PinCount { get; set; }
}
