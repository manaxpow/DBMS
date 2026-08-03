using System;

public class Record
{
    public int Id { get; set; }
    public byte[] Data { get; set; }

    public Record(int id, byte[] data)
    {
        Id = id;
        Data = data;
    }
}
