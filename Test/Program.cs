// See https://aka.ms/new-console-template for more information
using Calculator.Impl;

//var expr = "5 45+1 -45* 7";
var expr = "2 + 2 - 1";
var enumerator = new TokensEnumerator(new TokensEnumeratorContext(expr), new TokensFactory());
while (enumerator.MoveNext())
{
    Console.Write("Value: ");
    Console.WriteLine(enumerator.Current);
}

Console.WriteLine("-----");

var calculator = new CalculatorImpl(expr => new TokensEnumerator(new TokensEnumeratorContext(expr), new TokensFactory()));
foreach (var token in calculator.ToRPN(expr))
{
    Console.Write("Value: ");
    Console.WriteLine(token);
}

Console.WriteLine("-----------");
var result = calculator.Calculate(expr);
Console.WriteLine(result);

Console.ReadKey();
