using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyHospialoo.Data;
using ConfigurationsEntities.Entites;
using ConfigurationsEntities.Enum;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace MyHospialoo.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            ILogger<CreateModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

       
        [BindProperty(SupportsGet = true)] public int? ReplyId { get; set; } 
		[BindProperty] public Massage Massage { get; set; } = new Massage();

        public IdentityUser CurrentUser { get; set; } = new IdentityUser();

        public async Task<IActionResult> OnGetAsync( int? replyid)
        {
           

       
           

           

			ReplyId = replyid;
			var userId = _userManager.GetUserId(User);

            Claim idcliem=User.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.NameIdentifier);
            var id = idcliem.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID is null or empty.");
                return NotFound();
            }

            CurrentUser = await _userManager.FindByIdAsync(userId);

            if (CurrentUser == null)
            {
                _logger.LogWarning("Current user not found.");
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(CurrentUser);
            var isDoctor = claims.Any(c => c.Type == "Doctor" && c.Value == "Cardiology");
            var isNurse = claims.Any(c => c.Type == "Nurse" && c.Value == "WalterWhite");
            var isSecretary = claims.Any(c => c.Type == "Secreetarie" && c.Value == "MedicialEquations");


   //         var claims2 =  User.Claims;
			//var isDoctor2 = claims.Any(c => c.Type == "Doctor" && c.Value == "Cardiology");
			//var isNurse2 = claims.Any(c => c.Type == "Nurse" && c.Value == "WalterWhite");
			//var isSecretary2 = claims.Any(c => c.Type == "Secreetarie" && c.Value == "MedicialEquations");

			if (!(isDoctor || isNurse || isSecretary))
            {
                var userName = await _context.Users
                    .Where(x => x.Id == userId)
                    .Select(x => x.UserName)
                    .FirstOrDefaultAsync();





                var patient = new Patient()
                {
                    SecurityNumber = userId,
                    PersonalTypes = PersonalTypes.Patient,
                    SNumber = userId,
                    Name = userName,
                    UserName = userName
                };


                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();

            }
     

            return Page();
        }


		
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (Massage == null)
			{
				_logger.LogError("Massage object is null.");
				return Page();
			}

			if (string.IsNullOrEmpty(Massage.Contant))
			{
				_logger.LogError("Message content is null or empty.");
				ModelState.AddModelError(string.Empty, "Message content cannot be null or empty");
				return Page();
			}

			var userId = _userManager.GetUserId(User);

			if (string.IsNullOrEmpty(userId))
			{
				_logger.LogWarning("User ID is null or empty.");
				return Page();
			}
			CurrentUser = await _userManager.FindByIdAsync(userId);
			var claims = await _userManager.GetClaimsAsync(CurrentUser);
			var isDoctor = claims.Any(c => c.Type == "Doctor" && c.Value == "Cardiology");
			var isNurse = claims.Any(c => c.Type == "Nurse" && c.Value == "WalterWhite");
			var isSecretary = claims.Any(c => c.Type == "Secreetarie" && c.Value == "MedicialEquations");

			if ( !(isDoctor || isSecretary || isNurse) && ReplyId.HasValue)
			{
				
				var sender1 = await _context.Patients
				.Where(x => x.SNumber == userId)
			    	.FirstOrDefaultAsync();
			    var recieved1 = await _context.Doctors
			    	.Where(x => x.IdUser == ReplyId)
			    	.FirstOrDefaultAsync();
			    
			    if (sender1 == null || recieved1 == null)
			    {
			    	_logger.LogError("Sender or Receiver is null.");
			    	return Page();
			    }
			    
               
                try
                {
                    var message = new Massage()
                    {
                        Date = DateTime.Now,
                        Sender = sender1,
                        Receiver = recieved1,
                        Contant = Massage.Contant,
                        SenderName = sender1.Name
                    };
			      
                    await _context.Massages.AddAsync(message);



                   
                    var messageToReplyTo = await _context.Massages
                        .Where(x => x.SenderId == ReplyId && !x.IsReplyed)
                        .FirstOrDefaultAsync();  

                    if (messageToReplyTo != null)
                    {
                      
                        var currentSenderType = messageToReplyTo.Sender.GetType();
                        var currentSenderId = messageToReplyTo.SenderId;

                      
                        if (messageToReplyTo.SenderId == currentSenderId && messageToReplyTo.Sender.GetType() == currentSenderType)
                        {
                            messageToReplyTo.IsReplyed = true; 
                        }

                        await _context.SaveChangesAsync(); 
                    }


                    await _context.SaveChangesAsync();
			      
			      		
                    return RedirectToPage("/PatientsPage"); //CHANGE
                }
                catch (Exception ex)
                {
                    _logger.LogError(" failed: " + ex.Message);
			      		
                    return Page();
                }
                 
			     

			
			}
            else if (isDoctor && ReplyId.HasValue)
            {
                try
                {
                    var doctor = await _context.Doctors
                        .Where(x => x.SNumber == userId)
                        .FirstOrDefaultAsync();

                    var nurse = await _context.Nurses
                        .Where(x => x.IdUser == ReplyId)
                        .FirstOrDefaultAsync();

                    var patient = await _context.Patients
                        .Where(x => x.IdUser == ReplyId)
                        .FirstOrDefaultAsync();

                    if (doctor == null)
                    {
                        throw new Exception("Doctor not found");
                    }

                    var message = new Massage()
                    {
                        Date = DateTime.Now,
                        Sender = doctor,
                        Contant = Massage.Contant,
                        SenderName = doctor.Name
                    };

                   
                    if (nurse != null)
                    {
                        message.Receiver = nurse;
                    }
                    else if (patient != null)
                    {
                        message.Receiver = patient;
                    }
                    else
                    {
                        _logger.LogError("Receiver not found.");
                        return Page();
                    }

                    await _context.Massages.AddAsync(message);
                   
                    var messageToReplyTo = await _context.Massages
                        .Where(x => x.SenderId == ReplyId && !x.IsReplyed)
                        .FirstOrDefaultAsync(); 

                    if (messageToReplyTo != null)
                    {
                       
                        var currentSenderType = messageToReplyTo.Sender.GetType();
                        var currentSenderId = messageToReplyTo.SenderId;

                        
                        if (messageToReplyTo.SenderId == currentSenderId && messageToReplyTo.Sender.GetType() == currentSenderType)
                        {
                            messageToReplyTo.IsReplyed = true; 
                        }

                        await _context.SaveChangesAsync();
                    }


                    await _context.SaveChangesAsync();
                    return RedirectToPage("/DoctorPage");

                }
                catch (Exception ex)
                {
                    _logger.LogError("Message sending failed: " + ex.Message);
                    return Page();
                }
            }

            else if (isNurse && ReplyId.HasValue)
			{
                var sender1 = await _context.Nurses
                    .Where(x => x.SNumber == userId)
                    .FirstOrDefaultAsync();
                var recieved1 = await _context.Doctors
                    .Where(x => x.IdUser == ReplyId)
                    .FirstOrDefaultAsync();

                if (sender1 == null || recieved1 == null)
                {
                    _logger.LogError("Sender or Receiver is null.");
                    return Page();
                }

               
                try
                {
                    var message = new Massage()
                    {
                        Date = DateTime.Now,
                        Sender = sender1,
                        Receiver = recieved1,
                        Contant = Massage.Contant,
                        SenderName = sender1.Name
                    };

                    await _context.Massages.AddAsync(message);



                  
                    var messageToReplyTo = await _context.Massages
                        .Where(x => x.SenderId == ReplyId && !x.IsReplyed)
                        .FirstOrDefaultAsync();

                    if (messageToReplyTo != null)
                    {
                       
                        var currentSenderType = messageToReplyTo.Sender.GetType();
                        var currentSenderId = messageToReplyTo.SenderId;

                       
                        if (messageToReplyTo.SenderId == currentSenderId && messageToReplyTo.Sender.GetType() == currentSenderType)
                        {
                            messageToReplyTo.IsReplyed = true; 
                        }

                        await _context.SaveChangesAsync(); 
                    }


                    await _context.SaveChangesAsync();

                    
                    return RedirectToPage("/NursePage"); //CHANGE
                }
                catch (Exception ex)
                {
                    _logger.LogError(" failed: " + ex.Message);
                 
                    return Page();
                }
            }

			return Page();
		}

	}
}

