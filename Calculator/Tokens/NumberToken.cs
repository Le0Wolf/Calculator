namespace Calculator.Tokens;

public record NumberToken : BaseToken
{
    public decimal Value { get; init; }

    public override string ToString()
    {
        return $"Число: {Value}";
    }

    public static bool IsValidChar(char chr)
    {
        return char.IsDigit(chr) || chr == '.';
    }
}
