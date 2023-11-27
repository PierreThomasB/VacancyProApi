using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models.ViewModels;

namespace VacancyProAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class MeteoController : ControllerBase
{

    
    private const string Url =
        "http://api.weatherstack.com/current?access_key=e673e42fd6f4e270fd86ae218fbc7d07&query=";
    
    [HttpGet("GetMeteo")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "L'utilisateur n'est pas connecté ou son token est invalide")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Le lieux de météo n'existe pas", typeof(ErrorViewModel))]


    public async Task<IActionResult> GetMeteoByLieu(string lieu)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(Url + lieu);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    return Ok(apiResponse);
                }

                return NotFound("La vile n'a pas été trouvée" + httpResponseMessage.StatusCode);
            }
            catch (HttpRequestException e)
            {
                return BadRequest("Erreur dans la requête");
            }
        }
        
    }
    
    
}