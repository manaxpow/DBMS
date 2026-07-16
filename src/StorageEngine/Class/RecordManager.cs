using System;

public class RecordManager : IRecordManager
{
    private RecordLayoutCalculator _layout;
    private IBufferPoolManager _bufferPool;

    public RecordManager(IBufferPoolManager bufferPool)
    {
        _bufferPool = bufferPool;
    }

    public void CompactPage(Page page)
    {
    }

    public RecordId InsertRecord(Record record)
    {
        return default;
    }

    public Record GetRecord(RecordId recordId)
    {
        return default;
    }

    public void UpdateRecord(RecordId recordId, Record record)
    {
    }

    public void DeleteRecord(RecordId recordId)
    {
    }
}
