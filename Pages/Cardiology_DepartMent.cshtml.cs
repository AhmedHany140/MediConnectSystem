using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConfigurationsEntities.Entites;
using MyHospialoo.Data;

namespace MyHospialoo.Pages
{
    public class Cardiology_DepartMentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Cardiology_DepartMentModel(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            else
            {
                Department = department;
            }
            return Page();
        }
    }
}
