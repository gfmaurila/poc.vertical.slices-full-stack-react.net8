using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace poc.admin.tests.User.Data;


public class UserRepo
{
    public async Task<List<UserEntityFake>> Get()
    {
        using IDbConnection db = new SqlConnection(AdminDb.ConnectionString());
        var produtos = await db.QueryAsync<UserEntityFake>(SQL_GET);
        var lista = produtos.ToList();
        return lista;
    }

    public async Task<UserEntityFake> Get(int id)
    {
        using IDbConnection db = new SqlConnection(AdminDb.ConnectionString());
        var produto = await db.QueryFirstOrDefaultAsync<UserEntityFake>(SQL_GET_ID, new { Id = id });
        return produto;
    }

    public async Task<UserEntityFake> Post(UserEntityFake entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        using IDbConnection db = new SqlConnection(AdminDb.ConnectionString());

        var id = await db.QuerySingleAsync<Guid>(SQL_POST, entity);
        entity.Id = id;

        return entity;
    }

    public async Task<UserEntityFake> Put(UserEntityFake entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        using IDbConnection db = new SqlConnection(AdminDb.ConnectionString());

        await db.ExecuteAsync(SQL_PUT, entity);
        return entity;
    }

    public async Task<UserEntityFake> Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        using IDbConnection db = new SqlConnection(AdminDb.ConnectionString());

        await db.ExecuteAsync(SQL_DELETE, new { Id = id });

        return entity;
    }

    #region SQL
    private const string SQL_POST = @" INSERT INTO [dbo].[User] (FirstName, LastName, Gender, Notification, DateOfBirth, Email, Phone, Password, RoleUserAuth)
                                       VALUES (@FirstName, @LastName, @Gender, @Notification, @DateOfBirth, @Email, @Phone, @Password, @RoleUserAuth);
                                       SELECT CAST(SCOPE_IDENTITY() as uniqueidentifier);";
    private const string SQL_PUT = @" UPDATE [dbo].[User]
                                      SET FirstName = @FirstName, LastName = @LastName, Gender = @Gender, Notification = @Notification,
                                          DateOfBirth = @DateOfBirth, Email = @Email, Phone = @Phone, Password = @Password, RoleUserAuth = @RoleUserAuth
                                      WHERE Id = @Id;";
    private const string SQL_DELETE = @"DELETE FROM [dbo].[User] WHERE Id = @Id;";
    private const string SQL_GET = @"SELECT * FROM [dbo].[User]";
    private const string SQL_GET_ID = @"SELECT * FROM [dbo].[User] WHERE Id = @Id";

    #endregion
}
