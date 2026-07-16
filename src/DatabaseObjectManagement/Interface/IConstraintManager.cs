using System;

public interface IConstraintManager
{
    ConstraintId AddConstraint(TableId tableId, ConstraintDefinition def);
    void DropConstraint(ConstraintId constraintId);
}
