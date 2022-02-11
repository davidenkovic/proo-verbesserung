#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Persistence;
using Core.Contracts;
using System.ComponentModel.DataAnnotations;
using Core.Dtos;

namespace WebApp.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public Customer CurrentCustomer { get; set; }

        [BindProperty]
        public List<BookingForCustomerDTO> CustomerBookings { get; set; }

        public EditModel(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            CurrentCustomer = await _uow.Customers.GetByIdAsync((int)id);
            CustomerBookings = await _uow.Bookings.GetBookingsForCustomer((int)id);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                //await _uow.Customers.AddAsync(CurrentCustomer); 
                await _uow.SaveChangesAsync();
                return Page();
            }
            catch (ValidationException e)
            {

                var validationResult = e.ValidationResult;
                foreach (var field in validationResult.MemberNames)
                {
                    ModelState.AddModelError(field, validationResult.ToString());
                }

                ModelState.AddModelError("", e.Message);

                CurrentCustomer = await _uow.Customers.GetByIdAsync(CurrentCustomer.Id);
                CustomerBookings = await _uow.Bookings.GetBookingsForCustomer(CurrentCustomer.Id);

                return Page();
            }
        }

    }
}
