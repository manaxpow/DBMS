using System;

public class BPlusTreeIndex : IIndex
{
    private BPlusTreeNode _root;
    private IBufferPoolManager _bufferPool;

    public BPlusTreeIndex(IBufferPoolManager bufferPool)
    {
        _bufferPool = bufferPool;
    }

    public void SplitNode(BPlusTreeNode node)
    {
    }

    public void MergeNode(BPlusTreeNode node)
    {
    }

    public void Insert(IndexKey key, RecordPointer ptr)
    {
    }

    public void Delete(IndexKey key)
    {
    }

    public RecordPointer Search(IndexKey key)
    {
        return default;
    }
}
