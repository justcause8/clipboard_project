﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace clipboard_project.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        public const string KEY = "mysupersecret_secretkey!1234567890123456";
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static string UserIdClaimType = "userId"; // Добавляем параметр для хранения ID пользователя

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}