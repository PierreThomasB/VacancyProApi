using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Models.ViewModels
{
    public class UserViewModel
    {

        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public string Token { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public List<Period> Periods { get; set; } 

        public UserViewModel(User user,bool isAdmin, string token)
        {
            this.Id = user.Id;
            this.Username = user.UserName;
            this.Email = user.Email;
            this.Token = token;
            IsAdmin = isAdmin;
        }
    }
}

