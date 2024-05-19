﻿using clipboard_project.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

public class CustomAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var principal = httpContext.User;

        // Получаем токен из заголовка Authorization
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthOptions.KEY);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out var validatedToken);

            // Получаем ID пользователя из токена
            var userId = (validatedToken as JwtSecurityToken)?.Claims.FirstOrDefault(c => c.Type == AuthOptions.UserIdClaimType)?.Value;

            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Добавляем ID пользователя в контекст HTTP
            httpContext.Items["userId"] = userId;
        }
        catch (Exception)
        {
            // Если валидация токена не удалась, возвращаем ошибку аутентификации
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}