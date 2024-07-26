namespace API.Admin.Tests.Integration.Utilities;

public class CustomWebApplicationFactoryFixture : IDisposable
{
    public CustomWebApplicationFactory<Program> Factory { get; }

    public CustomWebApplicationFactoryFixture()
    {
        // Defina useInMemoryDatabase conforme necessário
        Factory = new CustomWebApplicationFactory<Program>();
    }

    public void Dispose()
    {
        Factory.Dispose();
        // Apagar o arquivo de banco de dados após os testes
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Integration", "Data", "testing.db");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
