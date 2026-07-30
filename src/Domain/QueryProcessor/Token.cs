public enum TokenType
{
    Keyword,
    Identifier,
    Operator,
    Literal,
    Punctuation,
}

public class Token
{
    public TokenType Type { get; set; }

    public string Value { get; set; } = null!;
}
