using System;

public interface IPageReplacementPolicy
{
    void Pin(FrameId frameId);
    void Unpin(FrameId frameId);
    FrameId Victim();
}
