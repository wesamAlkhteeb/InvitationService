using InvitationQueryService.Domain.Models;
using MediatR;

namespace InvitationQueryService.Models.QuerySideServiceBus
{
    public class RejectInvitationQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string AggregateId { get; set; }
        public int Sequence { get; set; }
        public DateTime DateTime { get; set; }
        public required InfoModel Data { get; set; }
    }
}
