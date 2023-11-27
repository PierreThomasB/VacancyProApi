using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using VacancyProAPI.Models;
using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Controllers;


public class PlaceController 
{
    private readonly DatabaseContext _context;
    


    public PlaceController(DatabaseContext context) 
    {
        this._context = context;
    }



    public async Task<Place> AddPlace(Place placeObj)
    {
        if (await _context.Places.FindAsync(placeObj.Id) == null)
        {
            Place place = new Place
            {
                Id = placeObj.Id,
                Name = placeObj.Name
            };
            string photoId = await GetPhotoId(placeObj.Id);
            place.UrlPhoto = photoId;

            _context.Add(place);
            await _context.SaveChangesAsync();

            return place;
        }

        return (await _context.Places.FindAsync(placeObj.Id))!;

    }



    private async Task<string> GetPhotoId(string idPlace)
    {
        const string url = "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyAeX0rGP22Zfco3WbT44TFHbKxqmPmIK_s&placeid=";
        
        
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url+idPlace);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string photoUrl = await httpResponseMessage.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(photoUrl);
                    return (string)json["result"]!["photos"]![0]!["photo_reference"]!;
                }

                return "Non trouvé ";
            }  catch (HttpRequestException e)
            {
                return "Non trouvé";
            }
        }
    }
    

    
    
    
}