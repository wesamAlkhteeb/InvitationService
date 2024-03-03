using InvitationQueryService.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.QuerySide.Leave
{
    public class LeaveInvitationQuery:IRequest<bool>
    {
        public int Id { get; set; }
        public required string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime dateTime { get; set; }
        public required InfoModel Data { get; set; }
    }
}
