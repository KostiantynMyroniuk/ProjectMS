using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yarp.ReverseProxy.Transforms;

namespace Web.BFF.Extensions
{
    public static class Extensions
    {
        public static void AddConfigurations(this IHostApplicationBuilder builder)
        {
            builder.Services.AddServiceDiscovery();

            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .AddTransforms(transformBuilderContext =>
                {
                    transformBuilderContext.AddRequestTransform(async transformContext =>
                    {
                        var token = transformContext.HttpContext.Request.Cookies["jwt"];

                        if (!string.IsNullOrEmpty(token))
                        {
                            transformContext.ProxyRequest.Headers.Authorization =
                                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        }
                    });
                })
                .AddServiceDiscoveryDestinationResolver();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                            ?? throw new InvalidOperationException("JWT Key is not configured")))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["jwt"];

                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
        }
    }
}
