using Microsoft.AspNetCore.Mvc;
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

        if ( await _context.Places.FindAsync(placeObj.Id) == null)
        {
            Place place = new Place
            {
                Id = placeObj.Id,
                Name = placeObj.Name
            };
            place.UrlPhoto = await GetPhotoUrl(place.Id);

            _context.Add(place);
            await _context.SaveChangesAsync();

            return place;
        }

        return (await _context.Places.FindAsync(placeObj.Id))!;

    }



    private async Task<string> GetPhotoId(string idPlace)
    {
        const  string URL = "https://maps.googleapis.com/maps/api/place/details/json?key=AIzaSyAeX0rGP22Zfco3WbT44TFHbKxqmPmIK_s&placeid=";
        
        
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(URL+idPlace);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string photoUrl = await httpResponseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(photoUrl);
                   
                }

                return "Non trouvé ";
            }  catch (HttpRequestException e)
            {
                return "Non trouvé";
            }
        }
    }
    
    private async Task<string> GetPhotoUrl( string idPhoto)
    {
     const string URL = "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&key=AIzaSyAeX0rGP22Zfco3WbT44TFHbKxqmPmIK_s&photo_reference=";

        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(URL+idPhoto);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string photoUrl = await httpResponseMessage.Content.ReadAsStringAsync();
                    return photoUrl;
                }

                return "Non trouvé ";
            }  catch (HttpRequestException e)
            {
                return "Non trouvé";
            }
        }
        
    }

    
    
    
    
}