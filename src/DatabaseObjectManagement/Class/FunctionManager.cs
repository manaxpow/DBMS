using System;

public class FunctionManager : IFunctionManager
{
    public FunctionId CreateFunction(SchemaId schemaId, FunctionDefinition def)
    {
        return default;
    }

    public void DropFunction(FunctionId functionId)
    {
    }
}
