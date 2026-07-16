using System;

public class ViewManager : IViewManager
{
    public ViewId CreateView(SchemaId schemaId, ViewDefinition def)
    {
        return default;
    }

    public void DropView(ViewId viewId)
    {
    }

    public void CompileView()
    {
    }
}
