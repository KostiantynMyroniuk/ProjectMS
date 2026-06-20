using Microsoft.Extensions.Azure;

namespace File.API.Extensions
{
    public static class Extension
    {
        public static void AddConfiguration(this IHostApplicationBuilder builder)
        {
            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(builder.Configuration["BLOBS_CONNECTIONSTRING"]);
            });
        }
    }
}
