using System;

public class TriggerManager : ITriggerManager
{
    public TriggerId CreateTrigger(TableId tableId, TriggerDefinition def)
    {
        return default;
    }

    public void DropTrigger(TriggerId triggerId)
    {
    }
}
