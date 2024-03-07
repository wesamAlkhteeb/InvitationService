using InvitationQueryService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.Abstractions
{
    public interface IPermissionRepository
    {
        public Task<List<PermissionEntity>> GetAllPermissions(int page);
        public Task<string> IsExistsPermission(int id);
    }
}
