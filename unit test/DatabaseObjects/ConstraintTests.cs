public class ConstraintTests
{
    private readonly Constraint _constraint;

    public ConstraintTests()
    {
        var table = new Table("TestTable");
        var column = new Column("Id", typeof(int), isNullable: false);
        table.AddColumn(column);

        _constraint = new Constraint(column);
    }
    [Fact]
    public void Validate_WhenValueSatisfiesConstraint_ShouldSucceed()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Validate_WhenValueViolatesConstraint_ShouldFail()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Apply_WhenConstraintIsDisabled_ShouldSkipValidation()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void Enable_WhenConstraintIsDisabled_ShouldEnable()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Disable_WhenConstraintIsEnabled_ShouldDisable()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Apply_WhenValidationFails_ShouldNotMutateState()
    {
        throw new NotImplementedException();
    }
}
