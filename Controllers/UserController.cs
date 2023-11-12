﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.DTOs;
using VacancyProAPI.Models.Mails;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.MailService;
using VacancyProAPI.Services.UserService;

namespace VacancyProAPI.Controllers
{
    /// <summary>
    /// Controller qui permet de gérer les utilisateurs
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;

        /// <summary>
        /// Constructeur du controller qui permet de gérer les utilisateurs
        /// </summary>
        /// <param name="context">Objet permettant d'intéragir avec la base de données</param>
        /// <param name="userManager">Service permettant de gérer l'utilisateur connecté</param>
        /// <param name="config">Objet contenant la configuration du système</param>
        /// <param name="mailService">Service qui permet d'envoyer des mails</param>
        /// <param name="userService">Service permettant d'intéragir avec l'utilisateur connecté</param>
        public UserController(DatabaseContext context, UserManager<User> userManager, IConfiguration config,IMailService mailService, IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _mailService = mailService;
            _userService = userService;
        }

        /// <summary>
        /// Route (GET) qui permet de récupérer les utilisateurs de l'application
        /// </summary>
        /// <returns>Une liste d'utilisateur transformées en ViewModel</returns>
        [HttpGet("ListUser")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Récupère la liste des utilisateurs de l'application")]
        [SwaggerResponse(StatusCodes.Status200OK, "La liste des utilisateurs a bien été récupérée", typeof(List<UserViewModel>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
        public async Task<ActionResult<List<UserViewModel>>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Periods).ToListAsync();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                userViewModels.Add(new UserViewModel(user,isAdmin,"nothing to see here"));
            }

            return Ok(userViewModels);
        }
        /// <summary>
        /// Route (POST) qui permet de créer un utilisateur
        /// </summary>
        /// <param name="request">Les informations nécessaires pour créer un nouvel utilisateur</param>
        /// <returns>Un ViewModel spécifiant qque l'utilisateur a bien été créé</returns>
        [AllowAnonymous]
        [HttpPost("SignUp")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Crée un nouvel utilisateur")]
        [SwaggerResponse(StatusCodes.Status200OK, "L'utilisateur a bien été créé", typeof(SuccessViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Les informations de l'utilisateur sont invalide ou l'utilisateur existe déjà", typeof(ErrorViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Les informations transmises sont invalide", typeof(ErrorViewModel))]
        public async Task<ActionResult<SuccessViewModel>> SignUp(UserSignUpDto request)
        {
            
            if (!ModelState.IsValid) return BadRequest(new ErrorViewModel("Informations invalide"));

            var exist = _userManager.Users
                .FirstOrDefault(u => u.UserName.Equals($"{request.FirstName} {request.LastName}"));
            if (exist != null) return BadRequest(new ErrorViewModel("Nom d'utilisateur déjà existant")); 
            var user = new User
            {
                UserName = $"{request.FirstName} {request.LastName}",
                Email = request.Email,
                Periods = new List<Period>()
            };
            
            var result = request.Password != string.Empty 
                ? await _userManager.CreateAsync(user, request.Password)
                : await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ErrorViewModel(string.Join(" | ", result.Errors.Select(e => e.Code))));
            await _userManager.AddToRoleAsync(user, "Member");
            
            _mailService.SendMail(new AddAccountMail("Bienvenue sur Vacancy Pro !", user.Email, null,new []{user.UserName}));

            return Ok(new SuccessViewModel("Compte créé avec succès"));
        }
        
        /// <summary>
        /// Route (POST) permettant de connecter un utilisateur
        /// </summary>
        /// <param name="request">Les information nécessaires pour authentifier un utilisateur</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SignIn")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Connecte un utilisateur")]
        [SwaggerResponse(StatusCodes.Status200OK, "L'utilisateur a bien été connecté", typeof(SuccessViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Les informations de connexion sont invalide", typeof(ErrorViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "L'utilisateur n'existe pas", typeof(ErrorViewModel))]
        public async Task<ActionResult<UserViewModel>> SingIn(UserSignInDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorViewModel("Informations invalide"));

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return BadRequest(new ErrorViewModel("Email invalide"));

            if (request.Password != String.Empty)
            {
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result) return BadRequest(new ErrorViewModel("Mot de passe invalide"));
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            user.Periods = _context.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.Periods).ThenInclude(p => p.Place)
                .Include(u => u.Periods).ThenInclude(p => p.ListActivity)
                .Include(u => u.Periods).ThenInclude(p => p.Creator)
                .FirstOrDefault()!.Periods;

            user.Periods = _context.Periods.Where(p => p.Creator.Id == user.Id)
                .Include(p => p.ListActivity).ThenInclude(a => a.Place)
                .Include(p => p.Place)
                //.Include(p => p.PeriodPlace.Id == _context.PeriodPlaces.First(place => place.Id == p.PeriodPlace.Id).Id)
                .Include(p => p.ListUser)
                .ToList();
            
            return Ok(new UserViewModel(user, isAdmin, Token.CreateToken(user, _userManager, _config)));
        }
        
        /// <summary>
        /// Route (PUT) qui permet de modifier le nom d'utilisateur de l'utilisateur connecté
        /// </summary>
        /// <param name="email"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /*public async Task<ActionResult<SuccessViewModel>> EditUser(string email, [FromBody] UsernameDto request)
        {
            
            return Ok(new SuccessViewModel("Nom d'utilisateur modifié avec succès"));
        }*/
    
    }
}

