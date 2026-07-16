using System;

public class LoggingManagement
{
    private ILogManager _logManager;
    private IWALProtocol _walProtocol;
    private ILogBufferManager _logBufferManager;
    private ILogWriter _logWriter;

    public LoggingManagement(
        ILogManager logManager,
        IWALProtocol walProtocol,
        ILogBufferManager logBufferManager,
        ILogWriter logWriter)
    {
        _logManager = logManager;
        _walProtocol = walProtocol;
        _logBufferManager = logBufferManager;
        _logWriter = logWriter;
    }

    public void Initialize()
    {
    }

    public void Shutdown()
    {
    }
}
