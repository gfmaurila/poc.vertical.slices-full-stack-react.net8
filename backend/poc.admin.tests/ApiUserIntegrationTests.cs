﻿using NUnit.Framework;
using poc.admin.Feature.Users.GetArticle;
using System.Net;
using System.Net.Http.Json;

namespace poc.admin.tests;

public class ApiCatalogoIntegrationTests
{
    [Test]
    public async Task GET_ALL_USER()
    {
        await using var application = new AdminApiApplication();

        await UserMockData.CreateUser(application, true);
        var url = "/api/user";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var categorias = await client.GetFromJsonAsync<List<GetUserQuery>>(url);

        Xunit.Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        //Assert.IsNotNull(categorias);
        //Assert.IsTrue(categorias.Count == 2);
    }
}
