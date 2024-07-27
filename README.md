
## API C# Vertical Slice Architecture
## Estrutura do Projeto

- `Infrastructure/Database/`: Configurações de banco de dados e mapeamentos do Entity Framework.
- `Domain/`: Entidades de domínio.
- `Endpoints/`: Definições dos endpoints da API.
- `Extensions/`: Métodos de extensão para configuração e Entity Framework.
- `Feature/`: Implementações das funcionalidades relacionadas a artigos.
- `Migrations/`: Migrações de banco de dados do Entity Framework.
- `Program.cs`: Ponto de entrada da aplicação.


<div>
    <h2>Features</h2>
    <ul>
        <li>Entity Framework (code first)</li>               
        <li>MediatR</li>
        <li>JWT auth</li>
        <li>Automapper</li>
        <li>Autofac</li>
        <li>Fluent Validation</li>
        <li>Swagger</li>
        <li>Integration Tests</li>
        <ul>
        <li>In-memory DB</li>
        <li>XUnit</li>
        <li>Shouldly</li>
        </ul>
    </ul>        
</div>

## Configuração e Instalação

### Clonando o Repositório
Clone o repositório usando: https://github.com/gfmaurila/poc.vertical.slices-full-stack-react.net8.git

### Configurando o Docker e Docker Compose
```
docker-compose up --build
```

- Backend: http://localhost:5075/swagger/index.html
- Frontend: http://localhost/

### Configurando React

```
cd C:\Work\poc.vertical.slices-full-stack-react.net8
```

```
npm install
```

```
yarn install
```

```
npm start
```

```
yarn start
```

- Acesso: http://localhost/ - Falta iniciar o projeto


### SQL Server

```
Add-Migration Inicial -Context EFSqlServerContext
```

```
Update-Database -Context EFSqlServerContext
```


## Youtube
- ......

## Autor

- Guilherme Figueiras Maurila

## 📫 Como me encontrar
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/channel/UCjy19AugQHIhyE0Nv558jcQ)
[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila)](https://www.linkedin.com/in/guilherme-maurila)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)

📧 Email: gfmaurila@gmail.com


