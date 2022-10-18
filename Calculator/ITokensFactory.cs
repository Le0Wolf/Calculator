using Calculator.Tokens;

namespace Calculator;

public interface ITokensFactory
{
    public OperatorToken CreateOperatorToken(string value, int position);

    public NumberToken CreateNumberToken(string value, int position);
}
