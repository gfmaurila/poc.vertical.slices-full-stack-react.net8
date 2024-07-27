using API.Admin.Feature.Auth.Login;
using API.Admin.Feature.Auth.ResetPassword;
using Bogus;

namespace API.Admin.Tests.Integration.Features.Auth.Fakes;

public static class AuthFake
{
    public static AuthResetPasswordCommand AuthResetPasswordInvalidCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new AuthResetPasswordCommand()
        {
            Email = "testedeleteteste.com.br"
        };
        return command;
    }

    public static AuthResetPasswordCommand AuthResetPasswordCommand()
    {
        var faker = new Faker("pt_BR");

        var command = new AuthResetPasswordCommand()
        {
            Email = "testedelete@teste.com.br"
        };
        return command;
    }

    public static AuthCommand GetAuthAsync()
    {
        var faker = new Faker("pt_BR");

        var command = new AuthCommand()
        {
            Email = "auth@auth.com.br",
            Password = "Test123$"
        };
        return command;
    }

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
