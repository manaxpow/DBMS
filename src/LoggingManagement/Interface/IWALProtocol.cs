using System;

public interface IWALProtocol
{
    void EnsureWAL(LogSequenceNumber pageLsn);
}
