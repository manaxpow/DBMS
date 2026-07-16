using System;

public interface IIndex
{
    void Insert(IndexKey key, RecordPointer ptr);
    void Delete(IndexKey key);
    RecordPointer Search(IndexKey key);
}
