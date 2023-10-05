using System.Security.Claims;

namespace VacancyProAPI.Services.UserService;

public class UserService : IUserService
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string? GetUserFromToken()
    {
        throw new NotImplementedException();
    }

    public bool IsTokenValid()
    {
        throw new NotImplementedException();
    }

    public bool IsUserAdmin()
    {
        throw new NotImplementedException();
    }
}