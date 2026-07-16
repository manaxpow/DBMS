using System;
using System.Collections.Generic;

public class ClockReplacementPolicy : IPageReplacementPolicy
{
    private List<FrameId> _clockHand = new List<FrameId>();

    public void AdvanceClock()
    {
    }

    public void Pin(FrameId frameId)
    {
    }

    public void Unpin(FrameId frameId)
    {
    }

    public FrameId Victim()
    {
        return default;
    }
}
