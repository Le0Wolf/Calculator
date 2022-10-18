namespace Calculator.Extensions;

public static class TokensEnumeratorContextExtensions
{
    public static void MoveToNextCharClass(this ITokensEnumeratorContext context, bool skipWhitespace = false)
    {
        var charClass = context.CurrentCharClass;
        while (context.MoveNext())
        {
            if (charClass != context.CurrentCharClass)
            {
                if (!skipWhitespace || context.CurrentCharClass != CharClass.Whitespace)
                {
                    break;
                }
            }
        }
    }

    public static void SkipWhitespace(this ITokensEnumeratorContext context)
    {
        if (context.CurrentCharClass == CharClass.Whitespace || context.Current == '\0')
        {
            while (context.MoveNext())
            {
                if (context.CurrentCharClass != CharClass.Whitespace)
                {
                    break;
                }
            }
        }
    }
}
