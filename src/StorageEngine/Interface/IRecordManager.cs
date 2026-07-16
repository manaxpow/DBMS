using System;

public interface IRecordManager
{
    RecordId InsertRecord(Record record);
    Record GetRecord(RecordId recordId);
    void UpdateRecord(RecordId recordId, Record record);
    void DeleteRecord(RecordId recordId);
}
