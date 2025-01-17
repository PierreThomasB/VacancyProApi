﻿using Microsoft.AspNetCore.Identity;

namespace VacancyProAPI.Models.DbModels
{
    public class User : IdentityUser, IEqualityComparer<User>
    {

        public List<Period> Periods = new();
        public List<Notification> Notifications = new();
        public bool Equals(User? x, User? y)
        {
            if (x == null || y == null)return false;
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

