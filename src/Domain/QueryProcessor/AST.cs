using System;

public class AST
{
    public Node Root { get; set; } = null!;

    public void GetRoot()
    {
        throw new NotImplementedException();
    }

    public void Accept(object visitor)
    {
        throw new NotImplementedException();
    }

    public void Build()
    {
        throw new NotImplementedException();
    }
}
