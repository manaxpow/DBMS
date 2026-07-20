public class RestrictAction : IReferentialAction
{
    public void Execute(Row parentRow, Table childTable)
    {
        throw new ReferentialIntegrityException();
    }
}
