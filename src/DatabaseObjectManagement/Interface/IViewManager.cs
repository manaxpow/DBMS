using System;

public interface IViewManager
{
    ViewId CreateView(SchemaId schemaId, ViewDefinition def);
    void DropView(ViewId viewId);
}
