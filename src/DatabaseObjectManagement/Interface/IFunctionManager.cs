using System;

public interface IFunctionManager
{
    FunctionId CreateFunction(SchemaId schemaId, FunctionDefinition def);
    void DropFunction(FunctionId functionId);
}
