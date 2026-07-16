using System;

public interface ILogBufferManager
{
    void WriteToBuffer(LogRecord record);
    void FlushBuffer();
}
