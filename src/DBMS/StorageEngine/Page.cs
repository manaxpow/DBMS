using System;
using System.Collections.Generic;

public class Page(PageId pageId, byte[] data)
{
    public PageId PageId { get; set; } = pageId;

    public int FreeSpace { get; set; } = data.Length;

    public byte[] Data { get; set; } = data;

    public List<Slot> SlotDirectory { get; set; } = new List<Slot>();

    public Slot InsertRecord(Record record)
    {
        throw new NotImplementedException();
    }

    public void UpdateRecord(Record record)
    {
        throw new NotImplementedException();
    }

    public void DeleteRecord(object slotId)
    {
        throw new NotImplementedException();
    }

    public object Read()
    {
        throw new NotImplementedException();
    }

    private int CalculateRequiredSpace(Record record)
    {
        throw new NotImplementedException();
    }

    private bool HasAvailableSpace(int requiredSpace)
    {
        throw new NotImplementedException();
    }

    private int WriteRecordData(Record record)
    {
        throw new NotImplementedException();
    }

    private object AddSlot(int recordOffset, int recordLength)
    {
        throw new NotImplementedException();
    }

    private void UpdateFreeSpaceMetadata()
    {
        throw new NotImplementedException();
    }

    private object FindSlot(object slotId)
    {
        throw new NotImplementedException();
    }

    private void MarkRecordDeleted(object slot)
    {
        throw new NotImplementedException();
    }

    private void RemoveOrInvalidateSlot(object slotId)
    {
        throw new NotImplementedException();
    }

    public object GetRecord(int v)
    {
        throw new NotImplementedException();
    }

    public void Compact()
    {
        throw new NotImplementedException();
    }
}
