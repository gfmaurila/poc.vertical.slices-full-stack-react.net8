using API.Admin.Feature.Users.CreateUser;
using API.Admin.Tests.Integration.Features.Users.Data;
using Bogus;
using poc.core.api.net8.Enumerado;

namespace API.Admin.Tests.Integration.Features.Users.Fakes;

public static class CreateUserCommandFake
{
    public static CreateUserCommand CreateUserCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new CreateUserCommand()
        {
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Gender = EGender.Male,
            Notification = ENotificationType.WhatsApp,
            Password = "@G21r03a1985",
            ConfirmPassword = "@G21r03a1985",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = faker.Person.Email,
            Phone = faker.Person.Phone,
            RoleUserAuth = UserRepo.RoleUserAuth()
        };
        return command;
    }

    public static CreateUserCommand CreateUserInvalidCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new CreateUserCommand()
        {
            FirstName = "",
            LastName = "",
            Gender = EGender.Male,
            Notification = ENotificationType.WhatsApp,
            Password = "",
            ConfirmPassword = "",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = "",
            Phone = "",
            RoleUserAuth = UserRepo.RoleUserAuth()
        };
        return command;
    }

    public static CreateUserCommand CreateUserExistingDataCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new CreateUserCommand()
        {
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Gender = EGender.Male,
            Notification = ENotificationType.WhatsApp,
            Password = "@G21r03a1985",
            ConfirmPassword = "@G21r03a1985",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = "fulanodetal@teste.com.br",
            Phone = faker.Person.Phone,
            RoleUserAuth = UserRepo.RoleUserAuth()
        };
        return command;
    }
}
