using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Application.Exceptions;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace InvitationCommandService.Infrastructure.Repository
{
    
    public class PermissionRepository : IPermissionRepository
    {
        private readonly InvitationDbContext database;

        public PermissionRepository(InvitationDbContext database)
        {
            this.database = database;
        }
        public async Task<List<PermissionEntity>> GetAllPermissions(int page)
        {
            int skip = (page - 1) * Constants.countItemInPage;
            return await database.Permissions
                .Skip(skip).Take(Constants.countItemInPage)
                .ToListAsync();
        }

        public async Task<string> IsExistsPermission(int id)
        {
            string? name = await database.Permissions.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(name))
            {
                throw new NotFoundException("Permisson not found.");
            }
            return name;
        }
    }
}
