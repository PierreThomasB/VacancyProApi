﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Route("[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IMailService _mailService;
        
        /// <summary>
        /// Constructeur du controller  qui permet de contacter l'équipe d'administration du site
        /// </summary>
        /// <param name="context">Objet permettant d'intéragir avec la base de données</param>
        /// <param name="userManager">Service permettant de gérer l'utilisateur connecté</param>
        /// <param name="mailService">Service qui permet d'envoyer des mails</param>
        
        public ContactController( IMailService mailService , ILogger<ContactController> logger)
        {
            _logger = logger;
            _mailService = mailService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Permet d'envoyer un mail à l'équipe d'administration du site")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Les informations du formulaires ne sont pas valides", typeof(ErrorViewModel))]
        public async Task<ActionResult<SuccessViewModel>> Contact(ContactDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.Log(LogLevel.Warning , "Informations invalide");
                return BadRequest(new ErrorViewModel("Informations invalide"));
            }
            _mailService.SendMail(new ContactToAdminMail(request.Subject, null, request.Email,
                new[] { request.Message }));
            _mailService.SendMail(new ContactToUser("Contact avec l'équipe d'adninistration", request.Email, null,
                Array.Empty<string>()));
            _logger.Log(LogLevel.Information , "Mail envoyé");
            return Ok(new SuccessViewModel("Mail envoyé"));
        }
    }
}