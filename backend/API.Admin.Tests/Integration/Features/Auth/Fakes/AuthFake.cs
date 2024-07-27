using API.Admin.Feature.Auth;
using Bogus;

namespace API.Admin.Tests.Integration.Features.Auth.Fakes;

public static class AuthFake
{
    public static AuthCommand AuthCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new AuthCommand()
        {
            Email = "testedelete@teste.com.br",
            Password = "Test123$"
        };
        return command;
    }

    public static AuthCommand AuthInvalidCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new AuthCommand()
        {
            Email = "testedeleteteste.com.br",
            Password = "Test123$"
        };
        return command;
    }
}
