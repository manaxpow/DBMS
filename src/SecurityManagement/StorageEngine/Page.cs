using System;
using System.Collections.Generic;



public class Page
{
    public PageId PageId { get; set; }
    public int FreeSpace { get; set; }
    public byte[] Data { get; set; }
    public List<Slot> SlotDirectory { get; set; }

    public Page(PageId pageId, byte[] data)
    {
        PageId = pageId;
        Data = data;
        FreeSpace = data.Length;
        SlotDirectory = new List<Slot>();
    }
    public object InsertRecord(object record)
    {
        throw new NotImplementedException();
    }

    public void UpdateRecord(object record)
    {
        throw new NotImplementedException();
    }

    public void DeleteRecord(object slotId)
    {
        throw new NotImplementedException();
    }

    private int CalculateRequiredSpace(object record)
    {
        throw new NotImplementedException();
    }

    private bool HasAvailableSpace(int requiredSpace)
    {
        throw new NotImplementedException();
    }

    private int WriteRecordData(object record)
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

    public object Read()
    {
        throw new NotImplementedException();
    }
}
