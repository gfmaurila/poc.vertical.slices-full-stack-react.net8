using Bogus;
using poc.admin.Domain.User;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Extensions;
using poc.core.api.net8.User;
using poc.core.api.net8.ValueObjects;

namespace poc.admin.tests.Fakes;

internal static class UserFake
{
    public static UserEntity Insert()
    {
        var faker = new Faker("pt_BR");

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
            new List<string>
            {
                    ERoleUserAuth.USER.ToString(),
                    ERoleUserAuth.CREATE_USER.ToString(),
                    ERoleUserAuth.UPDATE_USER.ToString(),
                    ERoleUserAuth.DELETE_USER.ToString(),
                    ERoleUserAuth.GET_USER.ToString(),
                    ERoleUserAuth.GET_BY_ID_USER.ToString(),

                    ERoleUserAuth.NOTIFICATION.ToString(),
                    ERoleUserAuth.CREATE_NOTIFICATION.ToString(),
                    ERoleUserAuth.DELETE_NOTIFICATION.ToString(),
                    ERoleUserAuth.GET_NOTIFICATION.ToString(),

                    ERoleUserAuth.REGION.ToString(),
                    ERoleUserAuth.COUNTRI.ToString(),
                    ERoleUserAuth.DEPARTMENT.ToString(),
                    ERoleUserAuth.EMPLOYEE.ToString(),
                    ERoleUserAuth.JOB.ToString(),
                    ERoleUserAuth.JOB_HISTORY.ToString(),
                    ERoleUserAuth.LOCATION.ToString(),

                    ERoleUserAuth.MKT_POST.ToString(),
            },
            new DateTime(1990, 1, 1));

        return newUser;
    }
}
