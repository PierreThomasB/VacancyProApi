using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
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
using DateDto = VacancyProAPI.Models.DTOs.DateDto;

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

        [HttpGet]
        [Authorize(Roles = "Member,Admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Récupère les information de l'utilisateur connecté")]
        public async Task<ActionResult<UserViewModel>> WhoAmI()
        {
            var id = _userService.GetUserIdFromToken();
            if (id == null) return NotFound(new ErrorViewModel("Aucun utilisateur associé à ce token"));

            var user = await _userManager.FindByIdAsync(id);
            
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            user.Periods = _context.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.Periods).ThenInclude(p => p.Place)
                /*
                .Include(u => u.Periods).ThenInclude(p => p.ListActivity)
                .Include(u => u.Periods).ThenInclude(p => p.Creator)
                */
                .FirstOrDefault()!.Periods;
            return Ok(new UserViewModel(user, isAdmin, Token.CreateToken(user, _userManager, _config)));
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
        [AllowAnonymous]
        [HttpPost("InVacation")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Récupère la liste des utilisateurs en vacances")]
        [SwaggerResponse(StatusCodes.Status200OK, "La liste des utilisateurs en vacances a bien été récupérée", typeof(Dictionary<string, int>))]
        public async Task<ActionResult<Dictionary<string, int>>> GetCountUsersByPlaces(DateDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorViewModel("Données invalides"));
            }
            Dictionary<string, int> result = new Dictionary<string, int>();
            var activePeriods = await _context.Periods
                .Include(p => p.Place)
                .Where(p => p.BeginDate <= request.Date && request.Date <= p.EndDate)
                .ToListAsync();
            foreach (var period in activePeriods)
            {
                result.Add(period.Place!.Name.Split(",").Last().Trim(), 1);
            }
            return Ok(result);
        }

        /// <summary>
        /// Route (GET) qui permet de récupérer le nombre d'utilisateur
        /// </summary>
        /// <returns>le nombre d'utilisateur</returns>
        [AllowAnonymous]
        [HttpGet("Count")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Récupère le nombre d'utilisateurs")]
        [SwaggerResponse(StatusCodes.Status200OK, "Le nombre d'utilisateurs a bien été récupéré", typeof(int))]
        public ActionResult<int> GetUsersCount() =>Ok(_context.Users.Count());
        
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

            /*var exist = _userManager.Users
                .FirstOrDefault(u => u.UserName.Equals($"{request.FirstName} {request.LastName}"));*/
            var exist = _userManager.Users.FirstOrDefault(u => u.Email.Equals(request.Email));
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
        public async Task<ActionResult<UserViewModel>> SignIn(UserSignInDto request)
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
                /*
                .Include(u => u.Periods).ThenInclude(p => p.ListActivity)
                .Include(u => u.Periods).ThenInclude(p => p.Creator)
                */
                .FirstOrDefault()!.Periods;

            /*
            user.Periods = _context.Periods.Where(p => p.Creator.Id == user.Id)
                /*
                .Include(p => p.ListActivity).ThenInclude(a => a.Place)
                .Include(p => p.Place)
                //.Include(p => p.PeriodPlace.Id == _context.PeriodPlaces.First(place => place.Id == p.PeriodPlace.Id).Id)
                .Include(p => p.ListUser)
                
                .ToList();
            */
            
            return Ok(new UserViewModel(user, isAdmin, Token.CreateToken(user, _userManager, _config)));
        }

        /// <summary>
        /// Route (POST) qui permet de connecter un utilisateur via son token Google
        /// </summary>
        /// <param name="request">Le token fourni par Google</param>
        /// <returns>Les informations de l'utilisateur (sous forme de ViewModel) qui souhaite se connecter via Google</returns>
        [AllowAnonymous]
        [HttpPost("Google")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Connecte un utilisateur via son token Google")]
        [SwaggerResponse(StatusCodes.Status200OK, "L'utilisateur a bien été connecté", typeof(UserViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Le token Google est invalide", typeof(ErrorViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "L'utilisateur n'existe pas", typeof(ErrorViewModel))]
        public async Task<ActionResult<UserViewModel>> Google(OAuthDto request)
        {
            if (!ModelState.IsValid) return BadRequest(new ErrorViewModel("Informations invalide"));

            try
            {
                var payload = GoogleJsonWebSignature
                    .ValidateAsync(request.Credentials, new GoogleJsonWebSignature.ValidationSettings()).Result;

                var user = _userManager.FindByEmailAsync(payload.Email).Result;
                if (user == null) return NotFound(new ErrorViewModel(payload.Email));
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                user.Periods = _context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.Periods).ThenInclude(p => p.Place)
                    /*
                    .Include(u => u.Periods).ThenInclude(p => p.ListActivity)
                    .Include(u => u.Periods).ThenInclude(p => p.Creator)
                    */
                    .FirstOrDefault()!.Periods;
                return Ok(new UserViewModel(user, isAdmin, Token.CreateToken(user, _userManager, _config)));
            }
            catch (Exception)
            {
                return BadRequest(new ErrorViewModel("Requête invalide"));
            }
            
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

