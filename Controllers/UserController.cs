using Microsoft.AspNetCore.Mvc;

namespace VacancyProAPI.Controllers;

public class UserController : ControllerBase
{
    
    
    public string[] Get()
    {
        return new string[]
        {
            "Hello",
            "World"
        };
    }
    
}