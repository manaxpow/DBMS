public class LogRecord
{
}

public class WALManager
{
    private List<LogRecord> _logBuffer = new List<LogRecord>();
    private int _flushedLSN;
    private int _currentLSN;

    public void WriteLog()
    {
        throw new NotImplementedException();
    }

    public int Append(object record)
    {
        throw new NotImplementedException();
    }

    public void Flush(int targetLSN)
    {
        throw new NotImplementedException();
    }

    public List<LogRecord> ReadAllRecords()
    {
        throw new NotImplementedException();
    }

    private int GenerateNextLSN()
    {
        throw new NotImplementedException();
    }

    private void RevertNextLSN(int lsn)
    {
        throw new NotImplementedException();
    }
}
