using System;
using System.IO;

public class PhysicalFileSystem : IPhysicalFileSystem
{
    private FileStream _diskStream;

    public void SeekToAddress(DiskAddress addr)
    {
    }

    public void ReadBlock(DiskAddress address, byte[] buffer)
    {
    }

    public void WriteBlock(DiskAddress address, byte[] buffer)
    {
    }
}
