using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calculator.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ICalculator _calculator;

        public IndexModel(ILogger<IndexModel> logger, ICalculator calculator)
        {
            _logger = logger;
            _calculator = calculator;
        }

        [BindProperty]
        public string? Expression { get; set; }

        public string? Message { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(Expression))
            {
                return Page();
            }

            try
            {
                Message = $"Результат: {_calculator.Calculate(Expression)}";
            }
            catch (ApplicationException e)
            {
                Message = e.Message;
            }

            return Page();
        }
    }
}