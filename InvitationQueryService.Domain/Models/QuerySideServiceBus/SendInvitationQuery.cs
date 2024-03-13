using InvitationQueryService.Domain.Models;
using MediatR;

namespace InvitationQueryService.Models.QuerySideServiceBus
{
    public class SendInvitationQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public required string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime DateTime { get; set; }
        public required DataInfoModel Data { get; set; }
    }
}
