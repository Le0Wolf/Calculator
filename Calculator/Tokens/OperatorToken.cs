using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Calculator.Tokens;

public record OperatorToken : BaseToken
{
    public OperatorType Operator { get; set; }

    public override string ToString()
    {
        var name = Operator.GetType().GetCustomAttribute<DisplayAttribute>()?.Name;
        return $"Оператор: {name ?? Operator.ToString()}";
    }

    public static bool IsValidChar(char chr)
    {
        return char.IsDigit(chr) || chr == '.';
    }
}
