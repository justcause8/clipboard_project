using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using clipboard_project.Models; // Импортируем пространство имен для доступа к AuthOptions

namespace clipboard_project
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавляем аутентификацию через JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Указываем, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // Строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,

                        // Указываем, будет ли валидироваться потребитель токена
                        ValidateAudience = true,
                        // Установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // Указываем, будет ли валидироваться время существования
                        ValidateLifetime = true,

                        // Устанавливаем ключ безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // Валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // Добавляем Swagger
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            // Используем аутентификацию и авторизацию
            app.UseAuthentication();
            app.UseAuthorization();

            // Используем Swagger и Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
