namespace VacancyProAPI.Services.UserService;

public interface IUserService
{
    string? GetUserIdFromToken();
    bool IsTokenValid();
    bool IsUserAdmin();
}