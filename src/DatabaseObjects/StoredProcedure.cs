using System;

public class StoredProcedure : ISchemaObject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public SchemaObjectType ObjectType => SchemaObjectType.StoredProcedure;
    public bool IsEnabled { get; set; }
    public bool IsDropped { get; set; }
    public ProcedureBody Body { get; set; }

    private object _transactionManager;

    public StoredProcedure()
    {
    }

    public object Execute(object parameters) => throw new NotImplementedException();
    public void AlterProcedure(ProcedureBody newBody) => throw new NotImplementedException();
    public void Drop() => throw new NotImplementedException();

    private bool ValidateParameters(object parameters) => throw new NotImplementedException();
    private bool ValidateBody(ProcedureBody newBody) => throw new NotImplementedException();
}
