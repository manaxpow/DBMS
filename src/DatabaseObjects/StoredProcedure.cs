using System;

public class StoredProcedure
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDropped { get; set; }
    public ProcedureBody Body { get; set; }
    
    private object _transactionManager;

    public StoredProcedure()
    {
    }

    public object Execute(object parameters) => throw new NotImplementedException();
    public void AlterProcedure(ProcedureBody newBody) => throw new NotImplementedException();
    public void DropProcedure() => throw new NotImplementedException();
    
    private bool ValidateParameters(object parameters) => throw new NotImplementedException();
    private bool ValidateBody(ProcedureBody newBody) => throw new NotImplementedException();
}
