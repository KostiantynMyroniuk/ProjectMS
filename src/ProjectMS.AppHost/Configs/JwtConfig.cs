using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectMS.AppHost.Configs
{
    public static class JwtConfig
    {
        public static IResourceBuilder<ProjectResource> WithJwtConfig(
            this IResourceBuilder<ProjectResource> builder,
            IResourceBuilder<ParameterResource> key,
            IResourceBuilder<ParameterResource> issuer,
            IResourceBuilder<ParameterResource> audience) =>
            builder
                .WithEnvironment("Jwt__Key", key)
                .WithEnvironment("Jwt__Issuer", issuer)
                .WithEnvironment("Jwt__Audience", audience);
    }
}
