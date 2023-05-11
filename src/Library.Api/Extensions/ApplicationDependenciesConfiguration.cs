using Library.Api.Profiles;
using Library.BusinessLogic.Services;
using Library.DataAccess.Extensions;
using Library.DataAccess.SeedData;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Extensions;

public static class ApplicationDependenciesConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDatabase(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
            builder.Services
            .AddRepositories()
            .AddAutoMapper(typeof(BookProfile))
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IBookService, BookService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IEditionHouseService, EditionHouseService>();
                
        return builder;
    }

    public static async  void SeedData(this WebApplication app)
    {
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

        using var scope = serviceScopeFactory.CreateScope();
        var serviceRole = scope.ServiceProvider.GetRequiredService<SeedRoles>();
        await serviceRole.seedRolesUsers();

        var serviceGenre = scope.ServiceProvider.GetRequiredService<SeedGenresData>();
        serviceGenre.SeedGenre();

        var servicePosition = scope.ServiceProvider.GetRequiredService<SeedPositionData>();
        servicePosition.SeedPositions();

        var serviceDataSexe = scope.ServiceProvider.GetRequiredService<SeedDataAdministrator>();
        await serviceDataSexe.AddAdministrator();
    }
}
