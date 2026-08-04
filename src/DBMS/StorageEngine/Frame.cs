public sealed class Frame(FrameId id, Page? page = null)
{
    public FrameId Id { get; init; } = id;

    public Page? Page { get; internal set; } = page;

    public bool IsDirty { get; internal set; } = false;

    public int PinCount { get; internal set; } = 0;

    public void MarkDirty() => IsDirty = true;
}
