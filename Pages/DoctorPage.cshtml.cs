using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConfigurationsEntities.Entites;
using MyHospialoo.Data;
using Microsoft.AspNetCore.Identity;

namespace MyHospialoo.Pages
{
	public class WalterPageModel : PageModel
	{
		private readonly MyHospialoo.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public WalterPageModel(MyHospialoo.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [BindProperty]
		public Doctor Doctor { get; set; } = default!;

		[BindProperty]
		public Massage Massages { get; set; } = new Massage();
        public string doctorname { get; set; }
        public Dictionary<int, List<string>> AllMessagesDictionary { get; set; } = new Dictionary<int, List<string>>();
	

		public async Task<IActionResult> OnGetAsync(int? id = 1)
		{
            var userid = _userManager.GetUserId(User);


            doctorname = await _context.Doctors.Where(y => y.SNumber == userid)
                .Select(y => y.Name).FirstOrDefaultAsync();

            var massages = await _context.Massages
				.Include(m => m.Sender)
				.Include(m => m.Receiver)
				.Where(m => m.Receiver.IdUser == id && !m.IsReplyed)
				.ToListAsync();

		
			AllMessagesDictionary = new Dictionary<int, List<string>>();

			foreach (var m in massages)
			{
				if (m.Receiver != null && m.Sender != null)
				{
					if (AllMessagesDictionary.ContainsKey(m.Sender.IdUser))
					{
						AllMessagesDictionary[m.Sender.IdUser].Add(
                          $"{m.Sender.GetType().Name} {m.SenderName} : {m.Contant}"
                        );
					}
					else
					{
                        AllMessagesDictionary[m.Sender.IdUser] =
                            new List<string> { $"{m.Sender.GetType().Name} {m.SenderName} : {m.Contant}" };
                    }
				}
			}

			return Page();
		}



		public async Task<IActionResult> OnPostAsync(int? id = 1)
		{
			if (id == null)
			{
				return NotFound();
			}

			var doctor = await _context.Doctors.FindAsync(id);
			if (doctor != null)
			{
				Doctor = doctor;
				_context.Doctors.Remove(Doctor);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
