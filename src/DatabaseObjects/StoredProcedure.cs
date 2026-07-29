public class StoredProcedure : ISchemaObject
{
    public StoredProcedure()
    {
    }

    public StoredProcedure(string name, string body)
    {
        this.Name = name;
        this.Body = new ProcedureBody();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public SchemaObjectType ObjectType => SchemaObjectType.StoredProcedure;

    public bool IsEnabled { get; set; }

    public bool IsDropped { get; set; }

    public ProcedureBody Body { get; set; } = null!;

    public object Execute(object parameters) => throw new NotImplementedException();

    public void AlterProcedure(ProcedureBody newBody) => throw new NotImplementedException();

    public void Drop() => throw new NotImplementedException();

    public void Accept(ISchemaVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public ISchemaObject Clone() => throw new NotImplementedException();

    private bool ValidateParameters(object parameters) => throw new NotImplementedException();

    private bool ValidateBody(ProcedureBody newBody) => throw new NotImplementedException();
}
