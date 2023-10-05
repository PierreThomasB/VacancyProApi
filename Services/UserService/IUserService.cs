namespace VacancyProAPI.Services.UserService;

public interface IUserService
{
    string? GetUserFromToken();
    bool IsTokenValid();
    bool IsUserAdmin();
}