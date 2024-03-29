﻿using APIGateway.Services;

namespace APIGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo { Title = "APIGateway", Version = "v1" });
            });

            services.AddHttpClient<IMailCollectorService, MailCollectorServiceV1>();
            services.AddHttpClient<IMailProviderService, MailProviderServiceV1>();

            services.AddSingleton<IMailCollectorService, MailCollectorServiceV1>();
            services.AddSingleton<IMailProviderService, MailProviderServiceV1>();
        }

        public void Configure(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
