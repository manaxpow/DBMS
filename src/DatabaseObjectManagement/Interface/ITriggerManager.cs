using System;

public interface ITriggerManager
{
    TriggerId CreateTrigger(TableId tableId, TriggerDefinition def);
    void DropTrigger(TriggerId triggerId);
}
