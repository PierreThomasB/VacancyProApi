using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;
using VacancyProAPI.Models.DTOs;
using VacancyProAPI.Models.ViewModels;
using VacancyProAPI.Services.UserService;

namespace VacancyProAPI.Controllers
{
    /// <summary>
    /// Controller qui permet de gérer les utilisateurs
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        /// <summary>
        /// Constructeur du controller qui permet de gérer les utilisateurs
        /// </summary>
        /// <param name="context">Objet permettant d'intéragir avec la base de données</param>
        /// <param name="userManager">Service permettant de gérer l'utilisateur connecté</param>
        /// <param name="config">Objet contenant la configuration du système</param>
        /// <param name="userService">Service permettant d'intéragir avec l'utilisateur connecté</param>
        public UserController(DatabaseContext context, UserManager<User> userManager, IConfiguration config, IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _userService = userService;
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
            if (!ModelState.IsValid) return BadRequest("Informations invalides");

            var user = new User
            {
                UserName = $"{request.FirstName} {request.LastName}",
                Email = request.Email
            };
            
            var result = request.Password != string.Empty 
                ? await _userManager.CreateAsync(user, request.Password)
                : await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ErrorViewModel(string.Join(" | ", result.Errors.Select(e => e.Code))));

            await _userManager.AddToRoleAsync(user, "Member");

            return Ok(new SuccessViewModel("Compte créé avec succès"));
        }
    
    }
}

