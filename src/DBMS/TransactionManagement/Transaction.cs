using System;

public class Transaction
{
    public int Id { get; set; }

    public TransactionState State { get; set; }

    public Transaction(int id)
    {
        this.Id = id;
        this.State = TransactionState.New;
    }

    public void Begin()
    {
        throw new NotImplementedException();
    }

    public void Commit()
    {
        throw new NotImplementedException();
    }

    public void Rollback()
    {
        throw new NotImplementedException();
    }

    public void MarkFailed()
    {
        throw new NotImplementedException();
    }
}
