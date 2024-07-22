using Bogus;
using Microsoft.Extensions.DependencyInjection;
using poc.admin.Domain.User;
using poc.admin.Infrastructure.Database;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Extensions;
using poc.core.api.net8.ValueObjects;

namespace poc.admin.tests;

public class UserMockData
{
    public static async Task CreateUser(AdminApiApplication application, bool create)
    {
        // https://github.com/macoratti/CatalogoApi_TestesIntegracao

        using var scope = application.Services.CreateScope();

        var provider = scope.ServiceProvider;
        using var dbContext = provider.GetRequiredService<EFSqlServerContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (create)
        {
            var faker = new Faker();

            var email = new Email(faker.Person.Email);
            var phone = new PhoneNumber(faker.Person.Phone);
            var password = Password.ComputeSha256Hash("@G21r03a1985");

            var newUser = new UserEntity(
                faker.Person.FirstName,
                faker.Person.LastName,
                EGender.Male,
                ENotificationType.WhatsApp,
                email,
                phone,
                password,
                new List<string> { "USER", "CREATE_USER", "UPDATE_USER", "DELETE_USER", "GET_USER", "GET_BY_ID_USER", "NOTIFICATION", "CREATE_NOTIFICATION", "DELETE_NOTIFICATION", "GET_NOTIFICATION", "REGION", "COUNTRI", "DEPARTMENT", "EMPLOYEE", "JOB", "JOB_HISTORY", "LOCATION", "MKT_POST" },
                new DateTime(1990, 1, 1));

            await dbContext.User.AddAsync(newUser);
            await dbContext.SaveChangesAsync();

            //await dbContext.Categorias.AddAsync(new Categoria
            //{ Nome = "Categoria 1", Descricao = "Descricao Categoria 1" });

            //await dbContext.Categorias.AddAsync(new Categoria
            //{ Nome = "Categoria 2", Descricao = "Descricao Categoria 2" });

            //await dbContext.SaveChangesAsync();
        }
    }
}
