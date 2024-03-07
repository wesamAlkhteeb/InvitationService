using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Domain.Models.Response
{
    public class UsersInSubscriptionResponseModel
    {
        public required int UserId { get; set; }
        public required int Id { get; set; }
        public required string Status { get; set; }
    }
}
