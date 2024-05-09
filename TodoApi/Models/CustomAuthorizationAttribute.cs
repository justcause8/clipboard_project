using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using clipboard_project.Models;
using System.IdentityModel.Tokens.Jwt;

public class CustomAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] allowedUsers;

    public CustomAuthorizationAttribute(params string[] users)
    {
        this.allowedUsers = users;
    }

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

            // Получаем имя пользователя из токена
            var userName = (validatedToken as JwtSecurityToken)?.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            // Проверяем, имеет ли пользователь право на доступ к ресурсу
            if (!IsUserAllowed(userName))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
        catch (Exception)
        {
            // Если валидация токена не удалась, возвращаем ошибку аутентификации
            context.Result = new UnauthorizedResult();
            return;
        }
    }

    private bool IsUserAllowed(string userName)
    {
        // Если список разрешенных пользователей пуст или не задан, пропускаем всех
        if (allowedUsers == null || !allowedUsers.Any())
        {
            return true;
        }

        // Проверяем, есть ли имя пользователя в списке разрешенных
        return allowedUsers.Contains(userName);
    }
}