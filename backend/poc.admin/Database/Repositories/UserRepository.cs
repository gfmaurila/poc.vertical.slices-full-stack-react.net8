using Microsoft.EntityFrameworkCore;
using poc.admin.Database.Repositories.Interfaces;
using poc.admin.Domain.User;
using poc.admin.Feature.Users.GetArticle;
using poc.core.api.net8.ValueObjects;
using poc.vertical.slices.net8.Database;

namespace poc.admin.Database.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    private readonly EFSqlServerContext _context;
    public UserRepository(EFSqlServerContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
        => await _context.User.AsNoTracking().AnyAsync(entity => entity.Email.Address == email.Address);

    public async Task<bool> ExistsByEmailAsync(Email email, Guid currentId)
        => await _context.User.AsNoTracking().AnyAsync(entity => entity.Email.Address == email.Address && entity.Id != currentId);

    public async Task<List<UserQueryModel>> GetAllAsync()
        => MapperModelToEntity(await _context.User.AsNoTracking().ToListAsync());

    #region Mapper
    private List<UserQueryModel> MapperModelToEntity(List<UserEntity> entity)
    {
        var model = new List<UserQueryModel>();
        foreach (var entityItem in entity)
        {
            model.Add(new UserQueryModel
            {
                FirstName = entityItem.FirstName,
                LastName = entityItem.LastName,
                Gender = entityItem.Gender,
                Email = entityItem.Email.Address,
                Phone = entityItem.Phone.Phone,
                RoleUserAuth = entityItem.RoleUserAuth,
                Password = entityItem.Password,
                DateOfBirth = entityItem.DateOfBirth
            });
        }
        return model;
    }
    #endregion

}
