using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models.ViewModels;
using System.Text.Json;


namespace VacancyProAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class MeteoController : ControllerBase
{
    private readonly ILogger <MeteoController> _logger;

    public MeteoController(ILogger<MeteoController> logger)
    {
        _logger = logger;
    }

    private const string MeteoUrl =
        "http://api.weatherstack.com/current?access_key=e673e42fd6f4e270fd86ae218fbc7d07&query=";
    
    [HttpGet("Meteo")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Le lieux de météo n'existe pas", typeof(ErrorViewModel))]
    [SwaggerOperation(Summary = "Permet de recupérer la météo d'un lieux  ")]
    [Produces("application/json")]
    public async Task<IActionResult> GetMeteoByLieu(string lieu)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(MeteoUrl + lieu);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogInformation("Météo récupérée");
                    return Ok(apiResponse);
                }
                _logger.LogError("Météo non trouvée");
                return NotFound("La vile n'a pas été trouvée" + httpResponseMessage.StatusCode);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError("Erreur dans la requête");
                return BadRequest("Erreur dans la requête");
            }
        }
        
    }
    
    
}