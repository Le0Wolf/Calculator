using System.Collections;

using Calculator.Extensions;
using Calculator.Tokens;

namespace Calculator.Impl;

public class TokensEnumerator : ITokensEnumerator
{
    private readonly ITokensEnumeratorContext _context;

    private readonly ITokensFactory _tokensFactory;

    public TokensEnumerator(ITokensEnumeratorContext context, ITokensFactory tokensFactory)
    {
        _context = context;
        _tokensFactory = tokensFactory ?? throw new ArgumentNullException(nameof(tokensFactory));
    }

    public BaseToken? Current { get; private set; }
    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        _context.Dispose();
    }

    public bool MoveNext()
    {
        _context.SkipWhitespace();
        _context.BeginSubcontextFromThis();
        _context.MoveToNextCharClass(skipWhitespace: true);

        var subContext = _context.EndSubcontext();
        if (subContext.Length > 0)
        {
            var value = subContext.ToString().Replace(" ", string.Empty);
            var start = subContext.Start;
            switch (subContext.CharClass)
            {
                case CharClass.Number:
                    Current = _tokensFactory.CreateNumberToken(value, start);
                    return true;
                case CharClass.Operator:
                    Current = _tokensFactory.CreateOperatorToken(value, start);
                    return true;
                default:
                    throw new ApplicationException("Unknown char class");
            }
        }

        Current = null;
        return false;
    }

    public void Reset()
    {
        _context.Reset();
    }
}
