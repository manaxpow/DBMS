using System;

public class LogManager : ILogManager
{
    private object _lsnGen;
    private IWALProtocol _walProtocol;
    private ILogBufferManager _logBufferManager;

    public LogManager(IWALProtocol walProtocol, ILogBufferManager logBufferManager)
    {
        _walProtocol = walProtocol;
        _logBufferManager = logBufferManager;
    }

    public LogSequenceNumber AppendLog(LogRecord record)
    {
        return default;
    }

    public void FlushToLSN(LogSequenceNumber lsn)
    {
    }

    public LogRecord GetLogRecord(LogSequenceNumber lsn)
    {
        return default;
    }

    public void CreateAppendRequest()
    {
    }
}
