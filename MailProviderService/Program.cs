namespace MailProviderService;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!IsPortSet())
            throw new Exception("PORT Environment variable not set!");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static bool IsPortSet()
        => Environment.GetEnvironmentVariable("PORT") is not null;
}