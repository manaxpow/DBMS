using System;

public interface ILogManager
{
    LogSequenceNumber AppendLog(LogRecord record);
    void FlushToLSN(LogSequenceNumber lsn);
    LogRecord GetLogRecord(LogSequenceNumber lsn);
}
