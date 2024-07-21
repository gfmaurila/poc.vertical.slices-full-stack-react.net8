using Microsoft.EntityFrameworkCore;
using poc.admin.Domain.User;
using poc.admin.Feature.Users.GetArticle;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.ValueObjects;

namespace poc.admin.Infrastructure.Database.Repositories;

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

    public async Task<UserQueryModel> GetByIdAsync(Guid id)
    {
        var entity = await _context.User.AsNoTracking()
                                   .Where(u => u.Id == id)
                                   .FirstOrDefaultAsync();
        if (entity is not null)
            return MapperModelToEntity(entity);

        return null;
    }

    #region Mapper
    private UserQueryModel MapperModelToEntity(UserEntity entity)
    {
        return new UserQueryModel()
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Gender = entity.Gender,
            Email = entity.Email.Address,
            Phone = entity.Phone.Phone,
            RoleUserAuth = entity.RoleUserAuth,
            DateOfBirth = entity.DateOfBirth
        };
    }

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
                //Password = entityItem.Password,
                DateOfBirth = entityItem.DateOfBirth
            });
        }
        return model;
    }
    #endregion

}
