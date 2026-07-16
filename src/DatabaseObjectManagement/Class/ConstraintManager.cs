using System;

public class ConstraintManager : IConstraintManager
{
    public ConstraintId AddConstraint(TableId tableId, ConstraintDefinition def)
    {
        return default;
    }

    public void DropConstraint(ConstraintId constraintId)
    {
    }
}
