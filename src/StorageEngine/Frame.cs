public sealed class Frame
{
    public FrameId Id { get; init; }

    public Page? Page { get; internal set; }

    public bool IsDirty { get; internal set; }

    public int PinCount { get; internal set; }

    public Frame(FrameId id)
    {
        Id = id;
        Page = null;
        IsDirty = false;
        PinCount = 0;
    }

    public Frame(FrameId id, Page page)
    {
        Id = id;
        Page = page;
        IsDirty = false;
        PinCount = 0;
    }
}
