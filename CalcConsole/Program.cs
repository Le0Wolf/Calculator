// See https://aka.ms/new-console-template for more information
using Calculator.Impl;

start:
Console.Write("Введите выражение для вычисления: ");
var expr = Console.ReadLine();
if (string.IsNullOrEmpty(expr))
{
    goto start;
}
var calculator = new CalculatorImpl(expr => new TokensEnumerator(new TokensEnumeratorContext(expr), new TokensFactory()));

Console.Write("Результат: ");
Console.WriteLine(calculator.Calculate(expr));

Console.WriteLine("Нажмите любую кладвишу для продолжения...");
Console.ReadKey();
