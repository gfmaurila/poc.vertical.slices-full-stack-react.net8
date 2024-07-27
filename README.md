
## API C# Vertical Slice Architecture

<div>
    <h2>Estrutura do Projeto</h2>
    <ul>
        <li>`Infrastructure/Database/`: Configurações de banco de dados e mapeamentos do Entity Framework.</li>               
        <li>`Domain/`: Entidades de domínio.</li>
        <li>`Endpoints/`: Definições dos endpoints da API.</li>
        <li>`Extensions/`: Métodos de extensão para configuração e Entity Framework.</li>
        <li>`Feature/`: Implementações das funcionalidades relacionadas a artigos.</li>
        <li>`Migrations/`: Migrações de banco de dados do Entity Framework.</li>
        <li>`Program.cs`: Ponto de entrada da aplicação.</li>
    </ul>    
</div>

<div>
    <h2>Vertical Slice Architecture</h2>
    <ul>
        <li>Event Sourcing</li>               
        <li>Repository Pattern</li>
        <li>Resut Pattern</li>
        <li>Domain Events</li>
    </ul>    
</div>

<div>
    <h2>Features</h2>
    <ul>
        <li>EASP.NET Core 8.0: Framework para desenvolvimento da Microsoft.</li>
        <li>Entity Framework</li>               
        <li>MediatR</li>
        <li>Mapster</li>
        <li>JWT auth</li>
        <li>Carter</li>
        <li>Ardalis Result</li>
        <li>Fluent Validation</li>
        <li>Swagger</li>
        <li>Serilog</li>
        <li>MongoDB</li>
        <li>Redis</li>
        <li>SQL Server</li>
        <li>Serilog</li>
        <li>RabbitMQ</li>
        <li>Kafka</li>
        <li>Docker & Docker Compose</li>
        <li>Integration Tests</li>
            <ul>
                <li>In-memory DB</li>
                <li>Sqlite</li>
                <li>Sql Server</li>
                <li>XUnit</li>
                <li>NUnit</li>
                <li>Moq</li>
                <li>Bogus</li>
            </ul>
        </ul>    
</div>

<div>
    <h2>Frontend - Ainda não foi concluido</h2>
    <ul>
        <li>React</li>
    </ul>    
</div>

<div>
    <h2>Configuração e Instalação</h2>
    <table>
        <tr>
            <td>Clone o repositório usando:</td>
            <td>https://github.com/gfmaurila/poc.vertical.slices-full-stack-react.net8.git</td>
        </tr>
        <tr>
            <td>Configurando o Docker e Docker Compose</td>
            <td>docker-compose up --build</td>
        </tr>
    </table>    
</div>

<div>
    <h2>Configurando projeto</h2>
    <table>
        <tr>
            <td>Frontend:</td>
            <td>http://localhost/</td>
        </tr>
        <tr>
            <td>Backend:</td>
            <td>http://localhost:5075/swagger/index.html</td>
        </tr>
        <tr>
            <td>Pasta:</td>
            <td>cd C:\Work\poc.vertical.slices-full-stack-react.net8</td>
        </tr>
        <tr>
            <td>Rodando a aplicação</td>
            <td>docker-compose up --build</td>
        </tr>
        <tr>
            <td>React</td>
            <td>
                <ul>
                    <li>npm install ou yarn install</li>
                    <li>npm install</li>
                    <li>npm start ou yarn start</li>
                </ul>
            </td>
        </tr>
        <tr>
            <td>SQL Server</td>
            <td>
                <ul>
                    <li>Add-Migration Inicial -Context EFSqlServerContext</li>
                    <li>Update-Database -Context EFSqlServerContext</li>
                </ul>
            </td>
        </tr>
    </table>    
</div>


<div>
    <h2>Swagger</h2>
    ```
    curl -X 'GET' \
    'https://localhost:44375/api/v1/User/2e4ad093-5908-45a8-8e94-ae1b2a6101d5' \
    -H 'accept: application/json' \
    -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6ImdmbWF1cmlsYUBnbWFpbC5jb20iLCJpZCI6IjhhOGNhY2JlLTI2NDUtNDA5MC1hYzgwLTQwNTAyMTRkNGRlOSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJVU0VSIiwiQ1JFQVRFX1VTRVIiLCJVUERBVEVfVVNFUiIsIkRFTEVURV9VU0VSIiwiR0VUX1VTRVIiLCJHRVRfQllfSURfVVNFUiIsIk5PVElGSUNBVElPTiIsIkNSRUFURV9OT1RJRklDQVRJT04iLCJERUxFVEVfTk9USUZJQ0FUSU9OIiwiR0VUX05PVElGSUNBVElPTiIsIlJFR0lPTiIsIkNPVU5UUkkiLCJERVBBUlRNRU5UIiwiRU1QTE9ZRUUiLCJKT0IiLCJKT0JfSElTVE9SWSIsIkxPQ0FUSU9OIiwiTUtUX1BPU1QiXSwiZXhwIjoxNzIxMjgwMjA3LCJpc3MiOiJKd3RBcGlBdXRoIiwiYXVkIjoiSnd0QXBpQXV0aCJ9.XQX5mkAxlMo8R29MOvuSiPEmRY29ANHz-OdwlL9-R1M'
    ```   
</div>

## Youtube
- ......

## Autor

- Guilherme Figueiras Maurila

## 📫 Como me encontrar
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/channel/UCjy19AugQHIhyE0Nv558jcQ)
[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila)](https://www.linkedin.com/in/guilherme-maurila)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)

📧 Email: gfmaurila@gmail.com


