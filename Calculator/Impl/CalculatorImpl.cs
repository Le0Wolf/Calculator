using Calculator.Tokens;

namespace Calculator.Impl;

public class CalculatorImpl : ICalculator
{
    private readonly Func<string, ITokensEnumerator> _tokensEnumeratorFactory;

    public CalculatorImpl(Func<string, ITokensEnumerator> tokensEnumeratorFactory)
    {
        _tokensEnumeratorFactory = tokensEnumeratorFactory ?? throw new ArgumentNullException(nameof(tokensEnumeratorFactory));
    }

    /// <summary>
    /// Реализация алгоритма вычисления на стеке с использованием обратной польской нотации
    /// </summary>
    /// <param name="expression">Выражение для вычисления</param>
    /// <returns>Результат вычисления</returns>
    /// <exception cref="ApplicationException">Исключение в случае, если в после вычисления в стеке остались лишние элементы </exception>
    public decimal Calculate(string expression)
    {
        var stack = new Stack<NumberToken>();
        foreach (var token in ToRPN(expression))
        {
            switch (token)
            {
                case OperatorToken operatorToken:
                    var result = Calculate(stack, operatorToken.Operator);
                    stack.Push(result);
                    break;
                case NumberToken numberToken:
                    stack.Push(numberToken);
                    break;
                default:
                    break;
            }
        }

        if (stack.Count != 1)
        {
            throw new ApplicationException("Invalid state");
        }

        return stack.Pop().Value;
    }

    /// <summary>
    /// Преобразует выражение в перечисление токенов в обратной польской нотации
    /// </summary>
    /// <param name="expression">Выражение для преобразования</param>
    /// <returns></returns>
    public IEnumerable<BaseToken> ToRPN(string expression)
    {
        var stack = new Stack<OperatorToken>();

        using var tokensEnumerator = _tokensEnumeratorFactory(expression);
        while (tokensEnumerator.MoveNext())
        {
            switch (tokensEnumerator.Current)
            {
                case OperatorToken operatorToken:
                    var operatorPriority = GetOperatorPriority(operatorToken);

                    while (stack.Count > 0)
                    {
                        var stackOperatorToken = stack.Peek();

                        var stackOperatorPriority = GetOperatorPriority(stackOperatorToken);
                        if (stackOperatorPriority < operatorPriority)
                        {
                            break;
                        }

                        yield return stack.Pop();
                    }

                    stack.Push(operatorToken);

                    break;
                case NumberToken numberToken:
                    yield return numberToken;
                    break;
                default:
                    break;
            }
        }

        while (stack.Count > 0)
        {
            yield return stack.Pop();
        }
    }

    /// <summary>
    /// Выполняет вычисления
    /// </summary>
    /// <param name="numberTokens"></param>
    /// <param name="operatorType"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    private NumberToken Calculate(Stack<NumberToken> numberTokens, OperatorType operatorType)
    {
        if (numberTokens.Count < 2)
        {
            throw new ApplicationException("Expression error");
        }

        var op1 = numberTokens.Pop().Value;
        var op2 = numberTokens.Pop().Value;
        decimal value;
        switch (operatorType)
        {
            case OperatorType.Addition:
                value = op2 + op1;
                break;
            case OperatorType.Subtraction:
                value = op2 - op1;
                break;
            case OperatorType.Multiplication:
                value = op2 * op1;
                break;
            case OperatorType.Division:
                value = op2 / op1;
                break;
            default:
                throw new ApplicationException("Invalid operation");
        }

        return new NumberToken { Value = value };
    }

    /// <summary>
    /// Возвращает приоритет операции
    /// </summary>
    /// <param name="operatorToken">Токен, для которого нужно вычислить приоритет</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Исключение в случае, если предеан токен с неизвестной операцией</exception>
    private int GetOperatorPriority(OperatorToken operatorToken)
    {
        switch (operatorToken.Operator)
        {
            case OperatorType.Addition:
            case OperatorType.Subtraction:
                return 0;
            case OperatorType.Multiplication:
            case OperatorType.Division:
                return 1;
            default:
                var exMessage = "An unexpected action for the operator: " +
                    $"{operatorToken.Operator}.";
                throw new ApplicationException(exMessage);
        }
    }
}
