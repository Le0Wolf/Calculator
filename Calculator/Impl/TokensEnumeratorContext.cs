using System.Collections;
using System.Diagnostics;

namespace Calculator.Impl;

[DebuggerDisplay("Current  = {Current}, Index = {CurrentIndex}")]
public class TokensEnumeratorContext : ITokensEnumeratorContext
{
    private readonly string _expression;

    private Stack<(int start, CharClass charClass)> _subcontextStartPositions = new();

    public TokensEnumeratorContext(string expression)
    {
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public int CurrentIndex { get; private set; } = -1;

    public CharClass CurrentCharClass { get; private set; }

    public char Current { get; private set; }

    public bool MoveNext()
    {
        if (CurrentIndex >= _expression.Length - 1)
        {
            CurrentIndex = _expression.Length;
            return false;
        }

        CurrentIndex++;
        Current = _expression[CurrentIndex];
        CurrentCharClass = GetCharClass(Current);

        return true;
    }

    public void BeginSubcontextFromThis()
    {
        _subcontextStartPositions.Push((CurrentIndex, CurrentCharClass));
    }

    public TokensEnumeratorSubContext EndSubcontext()
    {
        return _subcontextStartPositions.TryPop(out var tuple)
            ? new TokensEnumeratorSubContext(_expression, tuple.start, CurrentIndex, tuple.charClass)
            : throw new ApplicationException("Subcontexts list is empty");
    }

    object IEnumerator.Current => Current;

    public void Reset()
    {
        _subcontextStartPositions.Clear();
        CurrentIndex = -1;
    }

    public void Dispose()
    {
        Reset();
    }

    private CharClass GetCharClass(char chr)
    {
        if (char.IsWhiteSpace(chr))
        {
            return CharClass.Whitespace;
        }

        if (char.IsDigit(chr) || chr == '.')
        {
            return CharClass.Number;
        }

        if (chr is '/' or '*' or '-' or '+')
        {
            return CharClass.Operator;
        }

        return CharClass.Unknown;
    }
}
