using Calculator.Impl;

namespace Calculator
{
    public interface ITokensEnumeratorContext : IEnumerator<char>
    {
        int CurrentIndex { get; }

        CharClass CurrentCharClass { get; }

        void BeginSubcontextFromThis();
        TokensEnumeratorSubContext EndSubcontext();
    }
}