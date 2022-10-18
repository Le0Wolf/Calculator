using System.ComponentModel.DataAnnotations;

namespace Calculator.Tokens;

public enum OperatorType
{
    [Display(Name = "+")]
    Addition = 1,

    [Display(Name = "-")]
    Subtraction = 2,

    [Display(Name = "*")]
    Multiplication = 3,

    [Display(Name = "/")]
    Division = 4
}
