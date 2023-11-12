using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.DTOs;
using VacancyProAPI.Models.Mails;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.MailService;

namespace VacancyProAPI.Controllers
{
    /// <summary>
    /// Controller qui permet de contacter l'équipe d'administration du site
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;
        
        /// <summary>
        /// Constructeur du controller  qui permet de contacter l'équipe d'administration du site
        /// </summary>
        /// <param name="context">Objet permettant d'intéragir avec la base de données</param>
        /// <param name="userManager">Service permettant de gérer l'utilisateur connecté</param>
        /// <param name="mailService">Service qui permet d'envoyer des mails</param>
        
        public ContactController(DatabaseContext context, UserManager<User> userManager, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _mailService = mailService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Permet d'envoyer un mail à l'équipe d'administration du site")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Les informations du formulaires ne sont pas valides", typeof(ErrorViewModel))]
        public async Task<ActionResult<SuccessViewModel>> Contact(ContactDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorViewModel("Informations invalide"));
            var user = await _userManager.FindByEmailAsync(request.Email);
            _mailService.SendMail(new ContactToAdminMail(request.Subject, null, request.Email,
                new[] { request.Message }));
            _mailService.SendMail(new ContactToUser("Contact avec l'équipe d'adninistration", request.Email, null,
                Array.Empty<string>()));
            return Ok(new SuccessViewModel("Mail envoyé"));
        }
    }
}