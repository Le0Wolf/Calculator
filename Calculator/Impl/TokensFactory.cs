using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Calculator.Tokens;

namespace Calculator.Impl;

public class TokensFactory : ITokensFactory
{
    private readonly Dictionary<string, OperatorType> _operatorTokenTypes;

    public TokensFactory()
    {
        _operatorTokenTypes = Enum.GetValues<OperatorType>()
            .ToDictionary(
                x =>
                {
                    var name = x.ToString();
                    var field = x.GetType().GetField(name);
                    return field!.GetCustomAttribute<DisplayAttribute>()?.Name ?? name;
                },
                x => x);
    }

    public NumberToken CreateNumberToken(string value, int position)
    {
        return decimal.TryParse(value, out var decimalValue)
            ? new NumberToken { Value = decimalValue, Position = position }
            : throw new ArgumentException(nameof(value));
    }

    public OperatorToken CreateOperatorToken(string value, int position)
    {
        return _operatorTokenTypes.TryGetValue(value, out var operatorType)
            ? new OperatorToken { Operator = operatorType, Position = position }
            : throw new ArgumentOutOfRangeException(nameof(value));
    }
}
