public class DDLCommandExecutor
{
    public DDLResult Execute(IDDLCommand command)
    {
        return command.Execute();
    }
}
