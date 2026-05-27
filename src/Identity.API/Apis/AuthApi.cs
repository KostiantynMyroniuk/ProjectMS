using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Apis
{
    public static class AuthApi
    {
        public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder app)
        {
            app.MapPost("/register", Register);
            app.MapPost("/login", Login);

            return app;
        }

        public record RegisterRequest(string userName, string email, string password);

        public static async Task<IResult> Register(
            RegisterRequest request,
            IJwtService jwtService,
            UserManager<ApplicationUser> userManager,
            HttpContext httpContext)
        {
            var user = new ApplicationUser { UserName = request.userName, Email = request.email };

            var result = await userManager.CreateAsync(user, request.password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            var token = jwtService.GenerateToken(user);

            httpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // for development
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Results.Ok();
        }

        public record LoginRequest(string email, string password);

        public static async Task<IResult> Login(
            LoginRequest request,
            IJwtService jwtService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            HttpContext httpContext)
        {
            var user = await userManager.FindByEmailAsync(request.email);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, request.password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Results.Unauthorized();
            }

            var token = jwtService.GenerateToken(user);

            httpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // for development 
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Results.Ok();
        }
    }
}
