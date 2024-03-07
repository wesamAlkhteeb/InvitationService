using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Infrastructure.Repository
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
    }
}
