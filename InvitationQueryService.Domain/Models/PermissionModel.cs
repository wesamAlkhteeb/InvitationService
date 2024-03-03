using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Domain.Models
{
    public class PermissionModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
