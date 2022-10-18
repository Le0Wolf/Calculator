namespace Calculator.Impl;

public class TokensEnumeratorSubContext
{
    private string _expression;

    public TokensEnumeratorSubContext(string expression, int start, int end, CharClass charClass)
    {
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        Start = start;
        Length = end - start;
        CharClass = charClass;
    }

    public CharClass CharClass { get; }

    public int Start { get; }

    public int Length { get; }

    public override string ToString()
    {
        return _expression.Substring(Start, Length);
    }
}
