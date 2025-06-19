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
    public class NursePageModel : PageModel
    {
        private readonly MyHospialoo.Data.ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        public NursePageModel(MyHospialoo.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public Nurse Nurse { get; set; } = default!;
        public string NurseName { get; set; }
        public Massage Massages { get; set; } = new Massage();
        public Dictionary<int, List<string>> AllMessagesDictionary { get; set; } = new Dictionary<int, List<string>>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userid = _userManager.GetUserId(User);

            NurseName = await _context.Nurses.Where(y => y.SNumber == userid)
                .Select(y => y.Name).FirstOrDefaultAsync();


            var nurseid = await _context.Nurses
                .Where(x => x.SNumber == userid)
                .Select(x => x.IdUser).FirstOrDefaultAsync();
           

     

            var massages = await _context.Massages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.Receiver.IdUser == nurseid && !m.IsReplyed)
                .ToListAsync();

         
            AllMessagesDictionary = new Dictionary<int, List<string>>();

            foreach (var m in massages)
            {
                if (m.Receiver != null && m.Sender != null)
                {
                    if (AllMessagesDictionary.ContainsKey(m.Sender.IdUser))
                    {
                        AllMessagesDictionary[m.Sender.IdUser].Add($"{m.Sender.GetType().Name} {m.SenderName} : {m.Contant}");
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
    }
}
