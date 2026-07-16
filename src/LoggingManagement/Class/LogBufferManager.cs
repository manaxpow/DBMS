using System;

public class LogBufferManager : ILogBufferManager
{
    private LogBuffer _buffer;
    private ILogWriter _logWriter;

    public LogBufferManager(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    public void WriteToBuffer(LogRecord record)
    {
    }

    public void FlushBuffer()
    {
    }

    public void RotateBuffer()
    {
    }
}
