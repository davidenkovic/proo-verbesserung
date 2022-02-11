using Core.Contracts;
using Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IUnitOfWork _unitOfWork;

        [BindProperty]
        public List<CustomerDTO> Customers { get; set; }

        [BindProperty]
        public string NameFiltered { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork; 
            Customers = new List<CustomerDTO>();
            NameFiltered = "alle";
        }

        public async Task OnGet()
        {
            var list = await _unitOfWork.Customers.GetCustomersForView();
            foreach (var item in list)
            {
                if (NameFiltered == "alle")
                {
                    Customers.Add(item);

                }
                if (item.FirstName.Contains(NameFiltered) || item.LastName.Contains(NameFiltered))
                {
                    Customers.Add(item);
                }
            }

        }
    }
}