using System;

public interface IPhysicalFileSystem
{
    void ReadBlock(DiskAddress address, byte[] buffer);
    void WriteBlock(DiskAddress address, byte[] buffer);
}
