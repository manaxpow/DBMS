using System;

public interface ILogWriter
{
    void WriteBlock(LogBlock block);
    void Sync();
}
