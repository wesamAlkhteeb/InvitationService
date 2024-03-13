using InvitationQueryService.Domain.Models;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.Join
{
    public class JoinInvitationQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime DateTime { get; set; }
        public DataInfoModel Data { get; set; }
    }
}
